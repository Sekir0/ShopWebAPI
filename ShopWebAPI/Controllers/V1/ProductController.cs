﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopWebAPI.DAL.Models;
using ShopWebAPI.DAL.Contracts;
using ShopWebAPI.DAL.Contracts.V1.Requests;
using ShopWebAPI.DAL.Contracts.V1.Responses;
using ShopWebAPI.Services.Interfaices;
using ShopWebAPI.Extensions;

namespace ShopWebAPI.Controllers.V1
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet(ApiRoutes.Products.GetAllAsynk)]
        public async Task<IActionResult> GetAllAsynk()
        {
            return Ok(await _productService.GetProductsAsynk());
        }

        [HttpGet(ApiRoutes.Products.GetAsynk)]
        public async Task<IActionResult> GetAsynk([FromRoute]Guid productId)
        {
            var product = await _productService.GetProductByIdAsynk(productId);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPut(ApiRoutes.Products.UpdateAsynk)]
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
                return Ok(product);

            return NotFound();

        }

        [HttpDelete(ApiRoutes.Products.DeleteAsynk)]
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

        [HttpPost(ApiRoutes.Products.CreateAsynk)]
        public async Task<IActionResult> CreateProductAsynk([FromBody] CreateProductRequest productRequest)
        {
            var product = new Product
            {
                Name = productRequest.Name,
                Description = productRequest.Description,
                Quantity = productRequest.Quantity,
                Price = productRequest.Price,
                Url = productRequest.Url,
                UserId = HttpContext.GetUserId()
            };

            await _productService.CreateProdutAsynk(product);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Products.GetAsynk.Replace("{postId}", product.Id.ToString());

            var response = new ProductResponse { Id = product.Id };
            return Created(locationUrl, response);
        }  
    }
}
