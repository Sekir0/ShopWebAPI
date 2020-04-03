using Microsoft.AspNetCore.Mvc;
using ShopWebAPI.DAL.Contracts;
using ShopWebAPI.DAL.Contracts.V1.Requests.Categorys;
using ShopWebAPI.DAL.Domain;
using ShopWebAPI.Extensions;
using ShopWebAPI.Services.Interfaices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWebAPI.Controllers.V1
{
    public class CategoryController : Controller
    {
        private readonly IProductService _productService;

        public CategoryController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet(ApiRoutes.Categorys.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productService.GetAllCategorysAsynk());
        }

        [HttpGet(ApiRoutes.Categorys.GetByName)]
        public async Task<IActionResult> GetByName([FromRoute] string categoryName)
        {
            var category = await _productService.GetCategoryByNameAsynk(categoryName);
            if(category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost(ApiRoutes.Categorys.Create)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var newCategory = new Category
            {
                Name = request.CategoryName,
                CreatorId = HttpContext.GetUserId(),
                CreatedOn = DateTime.UtcNow
            };

            var created = await _productService.CreateCategoryAsynk(newCategory);
            if (!created)
                return BadRequest(new { error = "Unable to create category" });

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Categorys.GetByName.Replace("{categoryName}", newCategory.Name);
            return Created(locationUrl, newCategory);
        }

    }
}
