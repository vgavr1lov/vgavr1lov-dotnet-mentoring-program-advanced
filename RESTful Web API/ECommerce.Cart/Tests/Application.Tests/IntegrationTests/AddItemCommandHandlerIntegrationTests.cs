using ECommerce.Cart.Application.Carts.Commands.AddItem;
using ECommerce.Cart.Application.Carts.Contracts;
using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Application.Tests.TestDataBuilders;
using ECommerce.Cart.Infrastructure.Repositories;

namespace ECommerce.Cart.Application.Tests.IntegrationTests;

public class AddItemCommandHandlerIntegrationTests : IDisposable
{
    private const long SampleCartId = 1;

    private readonly AddItemCommandHandler _sut;
    private readonly string _dbPath;
    private readonly ICartRepository _cartTestRepository;

    public AddItemCommandHandlerIntegrationTests()
    {
        _dbPath = Path.Combine(
            Path.GetTempPath(),
            $"cart_test_{Guid.NewGuid():N}.db");

        _cartTestRepository = new CartRepository(_dbPath);
        _sut = new AddItemCommandHandler(_cartTestRepository);
    }

    public void Dispose()
    {
        try
        {
            if (File.Exists(_dbPath))
                File.Delete(_dbPath);
        }
        catch
        {
        }
    }

    [Fact]
    public async Task Handle_WhenItemIsAdded_ShouldPersistItem()
    {
        // Arrange
        var item = new SampleCartItemBuilder()
            .WithRandomId()
            .Build();

        var request = new AddItemRequest
        {
            CartId = SampleCartId,
            ItemId = item.Id,
            Name = item.Name,
            ImageUrl = item.Image?.Url,
            ImageAltText = item.Image?.AltText,
            Amount = item.Price.Amount,
            Currency = item.Price.Currency,
            Quantity = item.Quantity
        };

        var command = new AddItemCommand(request);

        // Act
        await _sut.Handle(command, CancellationToken.None);
        var result = await _cartTestRepository.GetCartByIdAsync(SampleCartId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(result!.Items, x => x.Id == item.Id);
    }
}
