using AutoMapper;
using ECommerce.Cart.Application.Carts.Contracts;
using ECommerce.Cart.Application.Common.Interfaces;
using MediatR;

namespace ECommerce.Cart.Application.Carts.Queries.GetCartInfo;

public record GetCartInfoQuery(long CartId) : IRequest<CartModel>;

public class GetCartInfoQueryHandler : IRequestHandler<GetCartInfoQuery, CartModel?>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public GetCartInfoQueryHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<CartModel?> Handle(GetCartInfoQuery query, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetCartByIdAsync(query.CartId, cancellationToken);

        if (cart is null)
            return null;

        var response = new CartModel { CartId = cart.Id, Items = _mapper.Map<List<ItemModel>>(cart.Items) };

        return response;
    }
}