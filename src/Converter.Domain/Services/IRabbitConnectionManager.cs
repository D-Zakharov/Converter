using RabbitMQ.Client;

namespace Converter.Domain.Services
{
    public interface IRabbitConnectionManager
    {
        string QueueName { get; }

        IModel CreateModel();
    }
}