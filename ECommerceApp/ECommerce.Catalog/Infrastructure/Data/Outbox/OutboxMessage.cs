namespace ECommerce.Catalog.Infrastructure.Data.Outbox;

public class OutboxMessage
{
    public required Guid Id { get; set; }
    public required string Type { get; set; }
    public required string Content { get; set; }
    public required DateTime CreatedOn { get; set; }
    public DateTime? NextRetryOn { get; set; }
    public int RetryCount { get; set; }
}
