using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopWebAPI.DAL.Domain;
using ShopWebAPI.DAL.Contracts;
using ShopWebAPI.DAL.Contracts.V1.Requests;
using ShopWebAPI.DAL.Contracts.V1.Responses;
using ShopWebAPI.Services.Interfaices;
using ShopWebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ShopWebAPI.Controllers.V1
{
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        
        [HttpGet(ApiRoutes.Products.GetAll)]
        public async Task<IActionResult> GetAllAsynk()
        {
            var products = await _productService.GetProductsAsynk();
            var productResponse = products.Select(product => new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                Price = product.Price,
                Url = product.Url,
                UserId = product.UserId
            });
            return Ok(productResponse);
        }

        [HttpGet(ApiRoutes.Products.GetById)]
        public async Task<IActionResult> GetAsynk([FromRoute]Guid productId)
        {
            var product = await _productService.GetProductByIdAsynk(productId);

            if (product == null)
                return NotFound();

            return Ok(new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                Price = product.Price,
                Url = product.Url,
                UserId = product.UserId
            });
        }

        [Authorize(Roles = "Admin")]
        [Authorize(Policy = "MustWorkForAdmin")]
        [HttpPut(ApiRoutes.Products.Update)]
        public async Task<IActionResult> UpdateAsynk([FromRoute]Guid productId, [FromBody] UpdateProductRequest request)
        {
            var userOwnProduct = await _productService.UserOwnProductAsynk(productId, HttpContext.GetUserId());

            if (!userOwnProduct)
            {
                return BadRequest(new { error = "You do not have access to this product." });
            }

            var product = await _productService.GetProductByIdAsynk(productId);
            product.Name = request.Name;
            product.Description = request.Description;
            product.Quantity = request.Quantity;
            product.Url = request.Url;
            product.Price = request.Price;

            var update = await _productService.UpdateProductAsynk(product);

            if(update)
                return Ok(new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Quantity = product.Quantity,
                    Price = product.Price,
                    Url = product.Url,
                    UserId = product.UserId
                });

            return NotFound();

        }

        [Authorize(Roles = "Admin")]
        [Authorize(Policy = "MustWorkForAdmin")]
        [HttpDelete(ApiRoutes.Products.Delete)]
        public async Task<IActionResult> Delete([FromRoute]Guid productId)
        {
            var userOwnProduct = await _productService.UserOwnProductAsynk(productId, HttpContext.GetUserId());

            if (!userOwnProduct)
            {
                return BadRequest(new { error = "You do not have access to this product." });
            }

            var delete = await _productService.DeleteProductAsynk(productId);

            if (delete)
                return NoContent();

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [Authorize(Policy = "MustWorkForAdmin")]
        [HttpPost(ApiRoutes.Products.Create)]
        public async Task<IActionResult> CreateProductAsynk([FromBody] CreateProductRequest productRequest)
        {
            var newProductId = Guid.NewGuid();
            var product = new Product
            {
                Id = newProductId,
                Name = productRequest.Name,
                Description = productRequest.Description,
                Quantity = productRequest.Quantity,
                Price = productRequest.Price,
                Url = productRequest.Url,
                UserId = HttpContext.GetUserId(),
                Categorys = productRequest.Categorys.Select(x => new ProductCategory { ProductId = newProductId, CategoryName = x }).ToList()
            };

            await _productService.CreateProdutAsynk(product);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Products.GetById.Replace("{postId}", product.Id.ToString());

            var response = new ProductResponse 
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                Price = product.Price,
                Url = product.Url,
                UserId = product.UserId
            };
            return Created(locationUrl, response);
        }  
    }
}
