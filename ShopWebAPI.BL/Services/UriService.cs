using Microsoft.AspNetCore.WebUtilities;
using ShopWebApi.Contracts.V1.Requests.Pagination;
using ShopWebAPI.Contracts;
using ShopWebAPI.BL.Services.Interfaices;
using System;


namespace ShopWebAPI.BL.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetProductUri(string postId)
        {
            return new Uri(_baseUri + ApiRoutes.Products.GetAll.Replace("{postId}", postId));
        }

        public Uri GetAllProductsUri(PaginationQuery pagination = null)
        {
            var uri = new Uri(_baseUri);

            if (pagination == null)
            {
                return uri;
            }

            var modifiedUri = QueryHelpers.AddQueryString(_baseUri, "pageNumber", pagination.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());

            return new Uri(modifiedUri);
        }
    }
}
