using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopWebAPI.DAL.Models;
using ShopWebAPI.Contracts.V1;
using ShopWebAPI.Contracts.V1.Requests;
using ShopWebAPI.Contracts.V1.Responses;

namespace ShopWebAPI.Controllers.V1
{
    public class ProductController : Controller
    {
        private List<ProductModels> _products;

        public ProductController()
        {
            _products = new List<ProductModels>();
            for(var i = 0;i < 5; i++)
            {
                _products.Add(new ProductModels{ Id = Guid.NewGuid().ToString()});
            }
        }
        [HttpGet(ApiRoutes.Products.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_products);
        }

        [HttpPost(ApiRoutes.Products.Create)]
        public IActionResult Create([FromBody] CreateProductRequest productRequest)
        {
            var product = new ProductModels { Id = productRequest.Id };

            if (string.IsNullOrEmpty(product.Id))
                product.Id = Guid.NewGuid().ToString();

            _products.Add(product);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Products.Get.Replace("{postId}", product.Id);

            var response = new ProductResponse { Id = product.Id };
            return Created(locationUrl, product);


        }  
    }
}
