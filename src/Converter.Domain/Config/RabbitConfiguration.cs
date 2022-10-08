using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Converter.Domain.Config;

public class RabbitConfiguration
{
    public string Host { get; set; } = default!;
    public string QueueName { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Pass { get; set; } = default!;
    public int ConsumerDispatchConcurrency { get; set; } = 1;


    public IConnectionFactory CreateConnectionFactory()
    {
        return new ConnectionFactory()
        {
            HostName = Host,
            UserName = Username,
            Password = Pass,
            DispatchConsumersAsync = true, 
            ConsumerDispatchConcurrency = ConsumerDispatchConcurrency
        };
    }
}
