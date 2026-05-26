using AutoMapper;
using ECommerce.Cart.Application.Carts.Contracts;
using ECommerce.Cart.Domain.Entities;

namespace ECommerce.Cart.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CartItem, ItemModel>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image != null ? src.Image.Url : null))
            .ForMember(dest => dest.ImageAltText, opt => opt.MapFrom(src => src.Image != null ? src.Image.AltText : null))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
    }
}
