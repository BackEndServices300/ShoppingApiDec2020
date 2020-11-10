using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.VisualBasic.CompilerServices;
using ShoppingApi.Data;
using ShoppingApi.Models.Products;
using ShoppingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile(PricingConfiguration config)
        {
            CreateMap<Product, ProductSummaryItem>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice * config.Markup));

            CreateMap<Product, GetProductDetailsResponse>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice * config.Markup));

            CreateMap<PostProductRequest, Product>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(s => s.UnitPrice.Value))
                .ForMember(dest => dest.InInventory, opt => opt.MapFrom(_ => true));
                
                
        }
    }
}
