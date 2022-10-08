using Converter.Domain.Config;
using Converter.Domain.DB;
using Converter.Domain.Services;

namespace Converter.WebService.Services
{
    internal class RabbitQueueManager : AbstractQueueManager
    {
        private readonly IRabbitConnectionManager _rabbitConnectionManager;

        public RabbitQueueManager(IRequestData requestDataHandler, IDataStorage inputFilesStorage, IRabbitConnectionManager rabbitConnectionManager)
            : base(requestDataHandler, inputFilesStorage)
        {
            _rabbitConnectionManager = rabbitConnectionManager;
        }

        private protected override void SendToQueue(Guid idempotencyKey)
        {
            using (var model = _rabbitConnectionManager.CreateModel())
            {
                model.DeclareDurableQueue(_rabbitConnectionManager.QueueName);

                model.SendItemToQueue(_rabbitConnectionManager.QueueName, idempotencyKey.ToByteArray());
            }
        }
    }
}
