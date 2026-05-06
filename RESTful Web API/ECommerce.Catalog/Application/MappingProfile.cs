using AutoMapper;
using ECommerce.Catalog.Application.Categories.Contracts;
using ECommerce.Catalog.Application.Products.Contracts;
using ECommerce.Catalog.Domain.Entities;

namespace ECommerce.Catalog.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryModel>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image != null ? src.Image.Url : null))
            .ForMember(dest => dest.ImageAltText, opt => opt.MapFrom(src => src.Image != null ? src.Image.AltText : null));

        CreateMap<Product, ProductModel>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image != null ? src.Image.Url : null))
            .ForMember(dest => dest.ImageAltText, opt => opt.MapFrom(src => src.Image != null ? src.Image.AltText : null))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Amount));
    }
}
