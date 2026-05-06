using AutoMapper;
using ECommerce.Cart.Application.Carts.Queries.GetItems;
using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Application.Tests.TestDataBuilders;
using ECommerce.Cart.Domain.Entities;
using ECommerce.Cart.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace ECommerce.Cart.Application.Tests.IntegrationTests;

public class GetItemsQueryHandlerIntegrationTests : IDisposable
{
    private const long SampleCartId = 1;

    private readonly IMapper _mapper;
    private readonly GetItemsQueryHandler _sut;
    private readonly string _dbPath;
    private readonly ICartRepository _cartTestRepository;

    public GetItemsQueryHandlerIntegrationTests()
    {
        _dbPath = Path.Combine(
            Path.GetTempPath(),
            $"cart_test_{Guid.NewGuid():N}.db");

        _cartTestRepository = new CartRepository(_dbPath);

        var loggerFactory = LoggerFactory.Create(builder => { });
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }, loggerFactory);
        _mapper = mapperConfig.CreateMapper();

        _sut = new GetItemsQueryHandler(_cartTestRepository, _mapper);
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
    public async Task Handle_WhenItemExists_ShouldReturnItem()
    {
        // Arrange
        var item = new SampleCartItemBuilder()
            .WithRandomId()
            .Build();

        var cart = new ShoppingCart(SampleCartId);
        cart.AddItem(item);

        await _cartTestRepository.SaveCartAsync(cart, CancellationToken.None);

        var query = new GetItemsQuery(cart.Id);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(result, x => x.Id == item.Id);
    }
}
