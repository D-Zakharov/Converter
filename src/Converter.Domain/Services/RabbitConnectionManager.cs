using Converter.Domain.Config;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Converter.Domain.Services;

public sealed class RabbitConnectionManager : IDisposable, IRabbitConnectionManager
{
    private readonly RabbitConfiguration _rabbitConfiguration;
    private readonly IConnectionFactory _rabbitConnectionFactory;
    private readonly IConnection _rabbitConnection;

    public string QueueName => _rabbitConfiguration.QueueName;

    public RabbitConnectionManager(IOptions<RabbitConfiguration> options)
    {
        _rabbitConfiguration = options.Value;
        _rabbitConnectionFactory = _rabbitConfiguration.CreateConnectionFactory();
        _rabbitConnection = _rabbitConnectionFactory.CreateConnection();
    }

    public void Dispose()
    {
        if (_rabbitConnection != null)
            _rabbitConnection.Dispose();
    }

    public IModel CreateModel()
    {
        return _rabbitConnection.CreateModel();
    }
}
