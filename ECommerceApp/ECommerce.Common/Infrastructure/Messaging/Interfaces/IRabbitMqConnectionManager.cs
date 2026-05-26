using RabbitMQ.Client;

namespace ECommerce.Common.Infrastructure.Messaging.Interfaces;

public interface IRabbitMqConnectionManager
{
    void Dispose();
    Task<IConnection?> GetConnectionAsync();
    Task<IChannel?> CreateChannelAsync();
}
