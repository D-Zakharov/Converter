using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Converter.Domain.Config;

public static class ExtensionMethods
{
    public static void DeclareDurableQueue(this IModel model, string queueName)
    {
        model.QueueDeclare(queue: queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public static void SendItemToQueue(this IModel model, string queueName, ReadOnlyMemory<byte> body)
    {
        var properties = model.CreateBasicProperties();
        properties.Persistent = true;

        model.BasicPublish(exchange: "",
                            routingKey: queueName,
                            basicProperties: properties,
                            body: body);
    }
}
