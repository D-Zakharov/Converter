using System.Text;
using Converter.ConverterService.Services.Converters;
using Converter.Domain.Config;
using Converter.Domain.DB.Models;
using Converter.Domain.Entities.Exceptions;
using Converter.Domain.Services;
using Converter.Domain.Services.Factories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Converter.ConverterService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRabbitConnectionManager _rabbitConnectionManager;
        private readonly IDataStorageFactory _dataStorageFactory;
        private readonly IConverter _converter;
        private readonly IRequestDataFactory _requestDataFactory;

        public Worker(ILogger<Worker> logger, IRabbitConnectionManager rabbitConnectionManager, IDataStorageFactory dataStorageFactory, 
            IConverter converter, IRequestDataFactory requestDataFactory)
        {
            _logger = logger;
            _rabbitConnectionManager = rabbitConnectionManager;
            _dataStorageFactory = dataStorageFactory;
            _converter = converter;
            _requestDataFactory = requestDataFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Converter service start at {time:dd.MM.yyyy HH:mm}", DateTime.Now);
            using (var model = _rabbitConnectionManager.CreateModel())
            {
                model.DeclareDurableQueue(_rabbitConnectionManager.QueueName);

                var consumer = new AsyncEventingBasicConsumer(model);
                consumer.Received += ExecTask;

                model.BasicConsume(queue: _rabbitConnectionManager.QueueName,
                                     autoAck: true,
                                     consumer: consumer);

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }

        private async Task ExecTask(object consumerModel, BasicDeliverEventArgs ea)
        {
            var consumer = (AsyncEventingBasicConsumer)consumerModel;

            Guid? requestGuid = null;
            try
            {
                requestGuid = new(ea.Body.ToArray());
                _logger.LogDebug("Try to convert request {requestGuid}", requestGuid);

                var convertRequestHandler = _requestDataFactory.GetRequestHandler();
                var request = convertRequestHandler.GetInfo(requestGuid.Value);

                if (request.InputFileId is null)
                    throw new InputFileIsMissingException(requestGuid.Value);

                var (dataStorage, inputStoredFile) = _dataStorageFactory.GetDataStorageImplementation(request.InputFileId.Value);

                string resultPath = await _converter.Convert(dataStorage.GetFileUrl(inputStoredFile));

                int outputFileId = await dataStorage.SaveFile(FileTypes.Output, resultPath);

                dataStorage.DeleteFile(inputStoredFile);
                convertRequestHandler.UpdateInfo(request, null, outputFileId);
            }
            catch (CriticalConverterException ex)
            {
                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                if (requestGuid is not null)
                {
                    consumer.Model.SendItemToQueue(_rabbitConnectionManager.QueueName, requestGuid.Value.ToByteArray());
                }
            }
        }
    }
}