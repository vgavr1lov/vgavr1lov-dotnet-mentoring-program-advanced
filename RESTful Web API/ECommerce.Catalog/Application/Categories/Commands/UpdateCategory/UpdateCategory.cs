using ECommerce.Catalog.Application.Categories.Contracts;
using ECommerce.Catalog.Application.Common.Exceptions;
using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.ValueObjects;
using MediatR;

namespace ECommerce.Catalog.Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(UpdateCategoryRequest CategoryRequest) : IRequest;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var categoryToUpdate = await _context.Category
            .FirstOrDefaultAsync(c => c.Id == command.CategoryRequest.Id);

        if (categoryToUpdate is null)
            throw new NotFoundException("Category not found");

        var category = new Category(
            command.CategoryRequest.Id,
            command.CategoryRequest.Name,
            string.IsNullOrWhiteSpace(command.CategoryRequest.ImageUrl)
                ? null
                : new Image(command.CategoryRequest.ImageUrl, command.CategoryRequest.ImageAltText),
            command.CategoryRequest.ParentCategoryId);

        _context.Category.Entry(categoryToUpdate).CurrentValues.SetValues(category);

        await _context.SaveChangesAsync(cancellationToken);
    }
}