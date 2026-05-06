using ECommerce.Cart.Application.Carts.Commands.RemoveItem;
using ECommerce.Cart.Application.Carts.Contracts;
using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Application.Tests.TestDataBuilders;
using ECommerce.Cart.Domain.Entities;
using ECommerce.Cart.Infrastructure.Repositories;

namespace ECommerce.Cart.Application.Tests.IntegrationTests;

public class RemoveItemCommandHandlerIntegrationTests : IDisposable
{
    private const long SampleCartId = 1;

    private readonly RemoveItemCommandHandler _sut;
    private readonly string _dbPath;
    private readonly ICartRepository _cartTestRepository;

    public RemoveItemCommandHandlerIntegrationTests()
    {
        _dbPath = Path.Combine(
            Path.GetTempPath(),
            $"cart_test_{Guid.NewGuid():N}.db");

        _cartTestRepository = new CartRepository(_dbPath);
        _sut = new RemoveItemCommandHandler(_cartTestRepository);
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
    public async Task Handle_WhenItemExists__ShouldDeleteItem()
    {
        // Arrange
        var item = new SampleCartItemBuilder()
            .WithRandomId()
            .Build();

        var cart = new ShoppingCart(SampleCartId);
        cart.AddItem(item);

        await _cartTestRepository.SaveCartAsync(cart, CancellationToken.None);

        var request = new RemoveItemRequest
        {
            CartId = SampleCartId,
            ItemId = item.Id,
        };

        var command = new RemoveItemCommand(request);

        // Act
        await _sut.Handle(command, CancellationToken.None);
        var result = await _cartTestRepository.GetCartByIdAsync(SampleCartId, CancellationToken.None);

        // Assert
        Assert.DoesNotContain(result!.Items, x => x.Id == item.Id);
    }
}