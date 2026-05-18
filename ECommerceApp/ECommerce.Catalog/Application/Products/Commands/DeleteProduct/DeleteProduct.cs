using ECommerce.Catalog.Application.Common.Exceptions;
using ECommerce.Catalog.Application.Common.Interfaces;
using MediatR;

namespace ECommerce.Catalog.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(long Id) : IRequest;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _context.Product
            .FirstOrDefaultAsync(c => c.Id == command.Id);

        if (product is null)
            throw new NotFoundException("Product not found."); ;

        _context.Product.Remove(product);

        await _context.SaveChangesAsync(cancellationToken);
    }
}