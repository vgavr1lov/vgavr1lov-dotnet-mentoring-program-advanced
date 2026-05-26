using AutoMapper;
using ECommerce.Catalog.Application.Categories.Contracts;
using ECommerce.Catalog.Application.Common.Exceptions;
using ECommerce.Catalog.Application.Common.Interfaces;
using MediatR;

namespace ECommerce.Catalog.Application.Categories.Queries.GetCategory;

public record GetCategoryQuery(long Id) : IRequest<CategoryModel>;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CategoryModel> Handle(GetCategoryQuery query, CancellationToken cancellationToken)
    {
        var category = await _context.Category
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == query.Id, cancellationToken);

        if (category == null)
            throw new NotFoundException("Category not found");

        var response = _mapper.Map<CategoryModel>(category);

        return response;
    }
}