using System;
using System.Collections.Generic;
using System.Text;
using ShopWebAPI.DAL.Models;

namespace ShopWebAPI.BL.Services.Interfaices
{
    public interface IProductService
    {
        List<Product> GetProducts();

        Product GetProductById(Guid productId);

        bool UpdateProduct(Product updateProduct);

        bool DeleteProduct(Guid productId);
    }
}
