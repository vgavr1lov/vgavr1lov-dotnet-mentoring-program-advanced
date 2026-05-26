using ECommerce.Catalog.Application.Common.Exceptions;
using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Application.Products.Contracts;
using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.ValueObjects;
using MediatR;

namespace ECommerce.Catalog.Application.Products.Commands.AddProduct;

public record AddProductCommand(CreateProductRequest ProductRequest) : IRequest<long>;

public class AddProductCommandHandler : IRequestHandler<AddProductCommand, long>
{
    private readonly IApplicationDbContext _context;

    public AddProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(AddProductCommand command, CancellationToken cancellationToken)
    {
        var isExistingProduct = await _context.Product
            .AnyAsync(p => p.Id == command.ProductRequest.Id, cancellationToken);

        if (isExistingProduct)
            throw new ConflictException($"Product with id {command.ProductRequest.Id} already exists.");
    

       var product = new Product(
        command.ProductRequest.Id,
        command.ProductRequest.Name,
        command.ProductRequest.Description,
        string.IsNullOrWhiteSpace(command.ProductRequest.ImageUrl)
            ? null
            : new Image(command.ProductRequest.ImageUrl, command.ProductRequest.ImageAltText),
        command.ProductRequest.CategoryId,
        new Money(command.ProductRequest.Amount, command.ProductRequest.Currency),
        command.ProductRequest.Quantity);

        await _context.Product.AddAsync(product);
        await _context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}