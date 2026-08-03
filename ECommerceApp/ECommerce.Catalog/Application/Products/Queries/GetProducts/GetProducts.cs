using AutoMapper;
using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Application.Products.Contracts;
using MediatR;

namespace ECommerce.Catalog.Application.Products.Queries.GetProducts;

public record GetProductsQuery(GetProductsRequest Request) : IRequest<List<ProductModel>>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductModel>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _context.Product
            .AsNoTracking()
            .Where(p => !query.Request.CategoryId.HasValue || p.CategoryId == query.Request.CategoryId)
            .Skip(query.Request.PageNumber * query.Request.PageSize)
            .Take(query.Request.PageSize)
            .ToListAsync();

        return _mapper.Map<List<ProductModel>>(products);
    }
}
