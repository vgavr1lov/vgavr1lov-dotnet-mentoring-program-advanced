namespace ECommerce.Messaging.RabbitMq.Configuration;

public class RabbitMqConfiguration
{
    public string HostName { get; set; } = "localhost";
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool AutomaticRecoveryEnabled { get; set; } = true;
    public bool TopologyRecoveryEnabled { get; set; } = true;
    public TimeSpan NetworkRecoveryInterval { get; set; } = TimeSpan.FromSeconds(5);
}
