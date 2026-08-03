namespace ECommerce.Common.Contracts.Events;

public class ProductUpdatedIntegrationEvent
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageAltText { get; set; }
    public required long CategoryId { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
}
