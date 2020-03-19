using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopWebAPI.DAL.Models;
using ShopWebAPI.Contracts.V1;
using ShopWebAPI.Contracts.V1.Requests;
using ShopWebAPI.Contracts.V1.Responses;
using ShopWebAPI.BL.Services.Interfaices;

namespace ShopWebAPI.Controllers.V1
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet(ApiRoutes.Products.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_productService.GetProducts());
        }

        [HttpGet(ApiRoutes.Products.Get)]
        public IActionResult Get([FromRoute]Guid productId)
        {
            var product = _productService.GetProductById(productId);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPut(ApiRoutes.Products.Update)]
        public IActionResult Update([FromRoute]Guid productId, [FromBody] UpdateProductRequest request)
        {
            var product = new Product
            {
                Id = productId,
                Name = request.Name
            };

            var update = _productService.UpdateProduct(product);

            if(update)
                return Ok(product);

            return NotFound();

        }

        [HttpDelete(ApiRoutes.Products.Delete)]
        public IActionResult Delete([FromRoute]Guid productId)
        {
            var delete = _productService.DeleteProduct(productId);

            if (delete)
                return NoContent();

            return NotFound();
        }

        [HttpPost(ApiRoutes.Products.Create)]
        public IActionResult Create([FromBody] CreateProductRequest productRequest)
        {
            var product = new Product { Id = productRequest.Id };

            if (string.IsNullOrEmpty(
                product.Id.ToString()))
                product.Id = Guid.NewGuid();

            _productService.GetProducts().Add(product);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Products.Get.Replace("{postId}", product.Id.ToString());

            var response = new ProductResponse { Id = product.Id };
            return Created(locationUrl, response);
        }  
    }
}
