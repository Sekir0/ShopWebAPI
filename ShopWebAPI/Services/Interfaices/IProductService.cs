using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopWebAPI.DAL.Domain;

namespace ShopWebAPI.Services.Interfaices
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsynk(PaginationFilter paginationFilter = null);
        Task<Product> GetProductByIdAsynk(Guid productId);
        Task<bool> UpdateProductAsynk(Product updateProduct);
        Task<bool> DeleteProductAsynk(Guid productId);
        Task<bool> CreateProdutAsynk(Product product);
        Task<bool> UserOwnProductAsynk(Guid productId, string UserId);


        Task<List<Category>> GetAllCategorysAsynk();
        Task<bool> CreateCategoryAsynk(Category category);
        Task<Category> GetCategoryByNameAsynk(string categoryName);
        Task<bool> DeleteCategory(string categoryName);
    }
}
