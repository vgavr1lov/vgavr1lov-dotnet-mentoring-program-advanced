using AutoMapper;
using ECommerce.Catalog.Application.Categories.Queries.GetCategory;
using ECommerce.Catalog.Application.Tests.TestDataBuilders;
using ECommerce.Catalog.Infrastructure.Data.Context;
using Microsoft.Extensions.Logging;

namespace ECommerce.Catalog.Application.Tests.IntegrationTests;

public class GetCategoryQueryHandlerIntegrationTests : IClassFixture<TestSqlServerFixture>
{
    private readonly TestSqlServerFixture _fixture;
    private readonly ApplicationDbContext _context;
    private readonly GetCategoryQueryHandler _sut;

    public GetCategoryQueryHandlerIntegrationTests(TestSqlServerFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CreateContext();

        var loggerFactory = LoggerFactory.Create(builder => { });
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }, loggerFactory);
        var mapper = mapperConfig.CreateMapper();

        _sut = new GetCategoryQueryHandler(_context, mapper);
    }

    [Fact]
    public async Task Handle_WhenCategoryIsAddedThenRequestedById_ShouldReturnCategory()
    {
        // Arrange
        var category = new SampleCategoryBuilder()
            .WithRandomId()
            .Build();

        await _context.Category.AddAsync(category);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        var query = new GetCategoryQuery(category.Id);
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(category.Id, result.Id);
    }
}
