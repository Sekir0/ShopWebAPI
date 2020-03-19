using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShopWebAPI.BL.Services.Interfaices;
using ShopWebAPI.DAL.Models;

namespace ShopWebAPI.BL.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products;

        public ProductService()
        {
            _products = new List<Product>();
            for (var i = 0; i < 5; i++)
            {
                _products.Add(new Product { Id = Guid.NewGuid(), Name = $"Product Name {i}" });
            }
        }

        public bool DeleteProduct(Guid productId)
        {
            var product = GetProductById(productId);


            if (product == null)
                return false;

            _products.Remove(product);
            return true;
        }

        public Product GetProductById(Guid productId)
        {
            return _products.SingleOrDefault(x => x.Id == productId);
        }

        public List<Product> GetProducts()
        {
            return _products;
        }

        public bool UpdateProduct(Product updateProduct)
        {
            var exists = GetProductById(updateProduct.Id) != null;

            if (!exists)
                return false;

            var index = _products.FindIndex(x => x.Id == updateProduct.Id);
            _products[index] = updateProduct;
            return true;
        }
    }
}
