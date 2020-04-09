using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using ShopWebAPI.Contracts.V1.Requests;
using ShopWebAPI.Contracts.V1.Responses;

namespace ShopWebApi.SDK
{
    [Headers("Authorization: Bearer")]
    public interface IShopWebApi
    {
        [Get("/api/v1/products")]
        Task<ApiResponse<List<ProductResponse>>> GetAllAsynk();

        [Get("/api/v1/products/{productId}")]
        Task<ApiResponse<ProductResponse>> GetAsynk(Guid productId);

        [Post("/api/vq/products")]
        Task<ApiResponse<ProductResponse>> CreateAsynk([Body] CreateProductRequest createProductRequest);

        [Post("/api/vq/products/{productId}")]
        Task<ApiResponse<ProductResponse>> UpdateAsynk(Guid productId, [Body] UpdateProductRequest createProductRequest);

        [Post("/api/vq/products/{productId}")]
        Task<ApiResponse<ProductResponse>> DeleteAsynk(Guid productId);
    }
}
