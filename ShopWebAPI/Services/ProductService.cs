using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShopWebAPI.Services.Interfaices;
using ShopWebAPI.DAL.Domain;
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
        
        public async Task<bool> CreateProdutAsynk(Product product)
        {
            product.Categorys?.ForEach(x => x.CategoryName = x.CategoryName.ToLower());
            await AddNewCategory(product);
            await _dataContext.Products.AddAsync(product);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<Product> GetProductByIdAsynk(Guid productId)
        {
            return await _dataContext.Products.Include(x => x.Categorys).SingleOrDefaultAsync(x => x.Id == productId);
        }

        public async Task<List<Product>> GetProductsAsynk()
        {
            return await _dataContext.Products.Include(x => x.Categorys).ToListAsync();
        }

        public async Task<bool> UpdateProductAsynk(Product updateProduct)
        {
            updateProduct.Categorys?.ForEach(x => x.CategoryName = x.CategoryName.ToLower());
            await AddNewCategory(updateProduct);

            _dataContext.Products.Update(updateProduct);
            var update = await _dataContext.SaveChangesAsync();
            return update > 0;
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

        public async Task<List<Category>> GetAllCategorysAsynk()
        {
            return await _dataContext.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<bool> CreateCategoryAsynk(Category category)
        {
            category.Name = category.Name.ToLower();
            var existingCategory = await _dataContext.Categories.AsNoTracking().SingleOrDefaultAsync(x => x.Name == category.Name);
            if (existingCategory != null)
                return true;

            await _dataContext.Categories.AddAsync(category);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<Category> GetCategoryByNameAsynk(string categoryName)
        {
            return await _dataContext.Categories.AsNoTracking().SingleOrDefaultAsync(x => x.Name == categoryName.ToLower());
        }

        public async Task<bool> DeleteCategory(string categoryName)
        {
            var catgory = await _dataContext.Categories.AsNoTracking().SingleOrDefaultAsync(x => x.Name == categoryName.ToLower());

            if (catgory == null)
                return true;

            var productCategorys = await _dataContext.ProductCategories.Where(x => x.CategoryName == categoryName.ToLower()).ToListAsync();

            _dataContext.ProductCategories.RemoveRange(productCategorys);
            _dataContext.Categories.Remove(catgory);
            return await _dataContext.SaveChangesAsync() > productCategorys.Count;
        }

        private async Task AddNewCategory(Product product)
        {
            foreach(var category in product.Categorys)
            {
                var existingCategory = await _dataContext.Categories.SingleOrDefaultAsync(x =>
                x.Name == category.CategoryName);
                if(existingCategory != null)
                    continue;

                await _dataContext.Categories.AddAsync(new Category
                { Name = category.CategoryName, CreatedOn = DateTime.UtcNow, CreatorId = product.UserId});
            }
        }
    }
}
