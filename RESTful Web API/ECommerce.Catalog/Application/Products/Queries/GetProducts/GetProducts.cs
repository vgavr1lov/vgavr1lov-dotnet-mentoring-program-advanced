using AutoMapper;
using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Application.Products.Contracts;
using MediatR;

namespace ECommerce.Catalog.Application.Products.Queries.GetProducts;

public record GetProductsQuery(GetProductsRequest request) : IRequest<List<ProductModel>>;

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
            .Where(p => !query.request.CategoryId.HasValue || p.CategoryId == query.request.CategoryId)
            .Skip(query.request.PageNumber * query.request.PageSize)
            .Take(query.request.PageSize)
            .ToListAsync();

        var response = _mapper.Map<List<ProductModel>>(products);

        return response;
    }
}