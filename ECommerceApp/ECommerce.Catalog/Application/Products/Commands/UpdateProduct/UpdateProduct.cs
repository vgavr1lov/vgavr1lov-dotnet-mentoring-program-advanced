using ECommerce.Catalog.Application.Common.Exceptions;
using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Application.Products.Contracts;
using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.ValueObjects;
using MediatR;

namespace ECommerce.Catalog.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(UpdateProductRequest ProductRequest) : IRequest;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var image = string.IsNullOrWhiteSpace(command.ProductRequest.ImageUrl)
            ? null
            : new Image(command.ProductRequest.ImageUrl, command.ProductRequest.ImageAltText);

        var product = new Product(
        command.ProductRequest.Id,
        command.ProductRequest.Name,
        command.ProductRequest.Description,
        image,
        command.ProductRequest.CategoryId,
        new Money(command.ProductRequest.Amount, command.ProductRequest.Currency),
        command.ProductRequest.Quantity);

        var exists = await _context.Product
            .AsNoTracking()
            .AnyAsync(c => c.Id == product.Id);

        if (!exists)
            throw new NotFoundException("Product not found.");

        _context.Product.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
