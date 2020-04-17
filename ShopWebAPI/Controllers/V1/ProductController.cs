using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ShopWebAPI.DAL.Domain;
using ShopWebAPI.Contracts;
using ShopWebAPI.Contracts.V1.Requests;
using ShopWebAPI.Contracts.V1.Responses;
using ShopWebAPI.Services.Interfaices;
using ShopWebAPI.Extensions;
using ShopWebAPI.Cache;
using AutoMapper;
using ShopWebApi.Contracts.V1.Requests.Pagination;
using ShopWebApi.Contracts.V1.Responses.Pagination;
using ShopWebAPI.Helpers;

namespace ShopWebAPI.Controllers.V1
{
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public ProductController(IProductService productService, IMapper mapper, IUriService uriService)
        {
            _productService = productService;
            _mapper = mapper;
            _uriService = uriService;
        }

        
        [HttpGet(ApiRoutes.Products.GetAll)]
        [Cached(600)]
        public async Task<IActionResult> GetAllAsynk([FromQuery]PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var posts = await _productService.GetProductsAsynk(pagination);
            var productResponse = _mapper.Map<List<ProductResponse>>(posts);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<ProductResponse>(productResponse));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, productResponse);
            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.Products.GetById)]
        [Cached(600)]
        public async Task<IActionResult> GetAsynk([FromRoute]Guid productId)
        {
            var product = await _productService.GetProductByIdAsynk(productId);

            if (product == null)
                return NotFound();

            return Ok(new Response<ProductResponse>(_mapper.Map<ProductResponse>(product)));
        }

        //[Authorize(Roles = "Admin")]
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
                return Ok(new Response<ProductResponse>(_mapper.Map<ProductResponse>(product)));

            return NotFound();

        }

        //[Authorize(Roles = "Admin")]
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

        //[Authorize(Roles = "Admin")]
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

            var locationUri = _uriService.GetProductUri(product.Id.ToString());
            return Created(locationUri, new Response<ProductResponse>(_mapper.Map<ProductResponse>(product)));
        }  
    }
}
