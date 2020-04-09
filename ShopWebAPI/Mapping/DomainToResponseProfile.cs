using System.Linq;
using AutoMapper;
using ShopWebAPI.Contracts.V1.Responses;
using ShopWebAPI.Contracts.V1.Responses.Categorys;
using ShopWebAPI.DAL.Domain;

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
