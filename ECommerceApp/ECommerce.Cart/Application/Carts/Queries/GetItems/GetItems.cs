using AutoMapper;
using ECommerce.Cart.Application.Carts.Contracts;
using ECommerce.Cart.Application.Common.Interfaces;
using MediatR;

namespace ECommerce.Cart.Application.Carts.Queries.GetItems;

public record GetItemsQuery(long CartId) : IRequest<List<ItemModel>>;

public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, List<ItemModel>?>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public GetItemsQueryHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<List<ItemModel>?> Handle(GetItemsQuery query, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetCartByIdAsync(query.CartId, cancellationToken);
        var items = cart?.Items;

        if (cart is null)
            return null;

        var response = _mapper.Map<List<ItemModel>>(cart.Items);

        return response;
    }
}
