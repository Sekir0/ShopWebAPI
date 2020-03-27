using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopWebAPI.DAL.Models;

namespace ShopWebAPI.Services.Interfaices
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsynk();
        Task<Product> GetProductByIdAsynk(Guid productId);
        Task<bool> UpdateProductAsynk(Product updateProduct);
        Task<bool> DeleteProductAsynk(Guid productId);
        Task<bool> CreateProdutAsynk(Product product);
        Task<bool> UserOwnProductAsynk(Guid productId, string UserId);
    }
}
