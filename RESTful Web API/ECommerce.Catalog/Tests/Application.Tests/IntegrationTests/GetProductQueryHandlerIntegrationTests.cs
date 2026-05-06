using AutoMapper;
using ECommerce.Catalog.Application.Products.Queries.GetProduct;
using ECommerce.Catalog.Application.Tests.TestDataBuilders;
using ECommerce.Catalog.Infrastructure.Data.Context;
using Microsoft.Extensions.Logging;

namespace ECommerce.Catalog.Application.Tests.IntegrationTests;

public class GetProductQueryHandlerIntegrationTests : IClassFixture<TestSqlServerFixture>
{
    private readonly TestSqlServerFixture _fixture;
    private readonly ApplicationDbContext _context;
    private readonly GetProductQueryHandler _sut;

    public GetProductQueryHandlerIntegrationTests(TestSqlServerFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CreateContext();

        var loggerFactory = LoggerFactory.Create(builder => { });
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }, loggerFactory);
        var mapper = mapperConfig.CreateMapper();

        _sut = new GetProductQueryHandler(_context, mapper);
    }

    [Fact]
    public async Task Handle_WhenProductIsAddedThenRequestedById_ShouldReturnProduct()
    {
        // Arrange
        var category = new SampleCategoryBuilder()
            .WithRandomId()
            .Build();

        var product = new SampleProductBuilder()
            .WithRandomId()
            .WithCategoryId(category.Id)
            .Build();

        await _context.Category.AddAsync(category);
        await _context.SaveChangesAsync(CancellationToken.None);

        await _context.Product.AddAsync(product);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        var query = new GetProductQuery(product.Id);
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(product.Id, result.Id);
    }
}
