using ECommerce.Catalog.Application.Common.Exceptions;
using ECommerce.Catalog.Application.Common.Interfaces;
using MediatR;

namespace ECommerce.Catalog.Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(long Id) : IRequest;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _context.Category
            .FirstOrDefaultAsync(c => c.Id == command.Id);

        if (category is null)
            throw new NotFoundException("Category not found");

        _context.Category.Remove(category);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
