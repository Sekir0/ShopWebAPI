using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShopWebAPI.Services.Interfaices;
using ShopWebAPI.DAL.Models;
using ShopWebAPI.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShopWebAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly DataContext _dataContext;

        public ProductService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> DeleteProductAsynk(Guid productId)
        {
            var product = await GetProductByIdAsynk(productId);

            if (product == null)
                return false;

            _dataContext.Products.Remove(product);

            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> CreateProdutAsynk(Product product)
        {
            await _dataContext.Products.AddAsync(product);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<Product> GetProductByIdAsynk(Guid productId)
        {
            return await _dataContext.Products.SingleOrDefaultAsync(x => x.Id == productId);
        }

        public async Task<List<Product>> GetProductsAsynk()
        {
            return await _dataContext.Products.ToListAsync();
        }

        public async Task<bool> UpdateProductAsynk(Product updateProduct)
        {
            _dataContext.Products.Update(updateProduct);
            var update = await _dataContext.SaveChangesAsync();
            return update > 0;
        }

        public async Task<bool> UserOwnProductAsynk(Guid productId, string UserId)
        {
            var product = await _dataContext.Products.AsNoTracking().SingleOrDefaultAsync(x => x.Id == productId);

            if(product == null)
            {
                return false;
            }

            if(product.UserId != UserId)
            {
                return false;
            }

            return true;
        }
    }
}
