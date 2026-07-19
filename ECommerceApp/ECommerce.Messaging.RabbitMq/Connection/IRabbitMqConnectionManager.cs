using RabbitMQ.Client;

namespace ECommerce.Messaging.RabbitMq.Connection;

public interface IRabbitMqConnectionManager
{
    void Shutdown();
    Task<IConnection?> GetConnectionAsync();
    Task<IChannel?> CreateChannelAsync();
}
