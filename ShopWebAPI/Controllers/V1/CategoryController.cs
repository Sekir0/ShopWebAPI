using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWebAPI.Contracts;
using ShopWebAPI.Contracts.V1.Requests.Categorys;
using ShopWebAPI.Contracts.V1.Responses.Categorys;
using ShopWebAPI.DAL.Domain;
using ShopWebAPI.BL.Extensions;
using ShopWebAPI.BL.Services.Interfaices;


namespace ShopWebAPI.Controllers.V1
{
    public class CategoryController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public CategoryController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Categorys.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var categorys = await _productService.GetAllCategorysAsynk();
            return Ok(_mapper.Map<List<CategoryResponse>>(categorys));
        }

        [HttpGet(ApiRoutes.Categorys.GetByName)]
        public async Task<IActionResult> GetByName([FromRoute] string categoryName)
        {
            var category = await _productService.GetCategoryByNameAsynk(categoryName);
            if(category == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CategoryResponse>(category));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Authorize(Roles = "Admin")]
        [Authorize(Policy = "MustWorkForAdmin")]
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
            return Created(locationUrl, _mapper.Map<CategoryResponse>(newCategory));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Authorize(Roles = "Admin")]
        [Authorize(Policy = "MustWorkForAdmin")]
        [HttpDelete(ApiRoutes.Categorys.Delete)]
        public async Task<IActionResult> Delete([FromRoute] string categoryName)
        {
            var deleted = await _productService.DeleteCategory(categoryName);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}
