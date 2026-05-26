using ECommerce.Catalog.Application.Categories.Contracts;
using ECommerce.Catalog.Application.Common.Exceptions;
using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.ValueObjects;
using MediatR;

namespace ECommerce.Catalog.Application.Categories.Commands.AddCategory;

public record AddCategoryCommand(CreateCategoryRequest CategoryRequest) : IRequest<long>;

public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, long>
{
    private readonly IApplicationDbContext _context;

    public AddCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        var isExistingCategory = await _context.Product
            .AnyAsync(p => p.Id == command.CategoryRequest.Id, cancellationToken);

        if (isExistingCategory)
            throw new ConflictException($"Category with id {command.CategoryRequest.Id} already exists.");

        var category = new Category(
        command.CategoryRequest.Id,
        command.CategoryRequest.Name,
        string.IsNullOrWhiteSpace(command.CategoryRequest.ImageUrl)
            ? null
            : new Image(command.CategoryRequest.ImageUrl, command.CategoryRequest.ImageAltText),
        command.CategoryRequest.ParentCategoryId);

        await _context.Category.AddAsync(category);
        await _context.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}