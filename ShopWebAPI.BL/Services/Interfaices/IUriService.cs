using ShopWebApi.Contracts.V1.Requests.Pagination;
using System;

namespace ShopWebAPI.BL.Services.Interfaices
{
    public interface IUriService
    {
        Uri GetProductUri(string postId);

        Uri GetAllProductsUri(PaginationQuery pagination = null);
    }
}
