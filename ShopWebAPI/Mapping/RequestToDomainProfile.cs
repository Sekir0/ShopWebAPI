using AutoMapper;
using ShopWebApi.Contracts.V1.Requests.Pagination;
using ShopWebAPI.DAL.Domain;


namespace ShopWebAPI.Mapping
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
        }
    }
}
