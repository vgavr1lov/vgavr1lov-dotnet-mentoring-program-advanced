using AutoMapper;
using ECommerce.Catalog.Application.Categories.Contracts;
using ECommerce.Catalog.Application.Common.Interfaces;
using MediatR;

namespace ECommerce.Catalog.Application.Categories.Queries.GetAllCategories;

public record GetAllCategoriesQuery() : IRequest<List<CategoryModel>>;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CategoryModel>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await _context.Category
            .AsNoTracking()
            .ToListAsync();

        var response = _mapper.Map<List<CategoryModel>>(categories);

        return response;
    }
}
