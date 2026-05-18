using AutoMapper;
using ECommerce.Cart.Application.Carts.Contracts;
using ECommerce.Cart.Application.Carts.Queries.GetItems;
using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Application.Tests.TestDataBuilders;
using ECommerce.Cart.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ECommerce.Cart.Application.Tests.UnitTests;

public class GetItemsQueryHandlerUnitTests
{
    private const long SampleCartId = 1;

    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly IMapper _mapper;
    private readonly GetItemsQueryHandler _sut;

    public GetItemsQueryHandlerUnitTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();

        var loggerFactory = LoggerFactory.Create(builder => { });
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }, loggerFactory);
        _mapper = mapperConfig.CreateMapper();

        _sut = new GetItemsQueryHandler(_cartRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_WhenItemsExist_ReturnItems()
    {
        // Arrange
        var item = new SampleCartItemBuilder()
            .WithRandomId()
            .Build();

        var cart = new ShoppingCart(SampleCartId);
        cart.AddItem(item);

        _cartRepositoryMock
            .Setup(x => x.GetCartByIdAsync(SampleCartId, CancellationToken.None))
            .ReturnsAsync(cart);

        var query = new GetItemsQuery(SampleCartId);

        var expectedResult = _mapper.Map<List<ItemModel>>(cart.Items);

        // Act
        var actualResult = await _sut.Handle(query, CancellationToken.None);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}
