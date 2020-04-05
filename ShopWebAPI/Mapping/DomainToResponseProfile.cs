using AutoMapper;
using ShopWebAPI.DAL.Contracts.V1.Responses;
using ShopWebAPI.DAL.Contracts.V1.Responses.Categorys;
using ShopWebAPI.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWebAPI.Mapping
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.Categorys, opt => 
                opt.MapFrom(src => src.Categorys.Select(x => new CategoryResponse { Name = x.CategoryName})));

            CreateMap<Category, CategoryResponse>();
        }
    }
}
