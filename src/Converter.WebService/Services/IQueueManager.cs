namespace Converter.WebService.Services
{
    public interface IQueueManager
    {
        Task SendToConversionQueue(Guid idempotencyKey, IFormFile content);
    }
}
