using RabbitMQ.Client;

namespace ECommerce.Messaging.RabbitMq.Connection;

public interface IRabbitMqConnectionManager
{
    void Dispose();
    Task<IConnection?> GetConnectionAsync();
    Task<IChannel?> CreateChannelAsync();
}
