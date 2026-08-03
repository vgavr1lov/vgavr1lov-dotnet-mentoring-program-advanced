using ECommerce.Messaging.RabbitMq.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ECommerce.Messaging.RabbitMq.Connection;

public class RabbitMqConnectionManager : IRabbitMqConnectionManager
{
    private IConnection? _connection;
    private readonly IConnectionFactory _factory;

    private readonly SemaphoreSlim _lockCreate = new(1, 1);

    public RabbitMqConnectionManager(IOptions<RabbitMqConfiguration> rabbitMqConfiguration)
    {
        var config = rabbitMqConfiguration.Value;

        if (!string.IsNullOrEmpty(config.Username))
        {
            _factory = new ConnectionFactory
            {
                HostName = config.HostName,
                UserName = config.Username,
                Password = config.Password,
                AutomaticRecoveryEnabled = config.AutomaticRecoveryEnabled,
                TopologyRecoveryEnabled = config.TopologyRecoveryEnabled,
                NetworkRecoveryInterval = config.NetworkRecoveryInterval,
            };
        }
        else
        {
            _factory = new ConnectionFactory { HostName = config.HostName };
        }
    }

    public async Task<IConnection?> GetConnectionAsync()
    {
        if (_connection is not null)
            return _connection;

        await _lockCreate.WaitAsync();
        try
        {
            if (_connection is null)
                _connection = await _factory.CreateConnectionAsync();
        }
        catch
        {
            _connection = null;
            return _connection;
        }
        finally
        {
            _lockCreate.Release();
        }

        return _connection;
    }

    public async Task<IChannel?> CreateChannelAsync()
    {
        var connection = await GetConnectionAsync();

        if (connection is null)
            return null;

        return await connection.CreateChannelAsync();
    }

    public void Shutdown()
    {
        _connection?.Dispose();
        _lockCreate.Dispose();
    }
}
