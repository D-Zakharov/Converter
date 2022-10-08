using Converter.Domain.DB;
using Converter.Domain.DB.Models;
using Converter.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Converter.WebService.Services;

internal abstract class AbstractQueueManager : IQueueManager
{
    private readonly IRequestData _requestDataHandler;
    private readonly IDataStorage _inputFilesStorage;

    public AbstractQueueManager(IRequestData requestDataHandler, IDataStorage inputFilesStorage)
    {
        _requestDataHandler = requestDataHandler;
        _inputFilesStorage = inputFilesStorage;
    }

    public async Task SendToConversionQueue(Guid idempotencyKey, IFormFile content)
    {
        ConvertRequest? requestData = _requestDataHandler.SaveInfo(idempotencyKey);
        if (requestData is not null)
        {
            try
            {
                using (var fileStream = content.OpenReadStream())
                {
                    var fileId = await _inputFilesStorage.SaveFile(FileTypes.Input, fileStream, Path.GetExtension(content.FileName));
                    _requestDataHandler.UpdateInfo(requestData, inputFileId: fileId, outputFileId: null);
                    SendToQueue(idempotencyKey);
                }
            }
            catch
            {
                _requestDataHandler.RemoveInfo(requestData);
                throw;
            }
        }
    }

    private protected abstract void SendToQueue(Guid idempotencyKey);
}
