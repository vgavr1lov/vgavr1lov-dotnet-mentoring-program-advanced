using AutoMapper;
using ECommerce.Catalog.Application.Common.Exceptions;
using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Application.Products.Contracts;
using MediatR;

namespace ECommerce.Catalog.Application.Products.Queries.GetProduct;

public record GetProductQuery(long Id) : IRequest<ProductModel>;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductModel> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var product = await _context.Product
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == query.Id, cancellationToken);

        if (product == null)
            throw new NotFoundException("Product not found");

        var response = _mapper.Map<ProductModel>(product);

        return response;
    }
}
