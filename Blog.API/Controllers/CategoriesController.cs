using Blog.Service.Abstract;
using Blog.Service.Dto;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class CategoriesController : ControllerBase
   {
      private readonly ICategoryService _categoryService;
      private readonly IValidator<CategoryDto> _categoryValidator;
      public CategoriesController(ICategoryService categoryService, IValidator<CategoryDto> categoryValidator)
      {
         _categoryService = categoryService;
         _categoryValidator = categoryValidator;
      }
      [HttpGet("getAll")]
      public async Task<IActionResult> GetAllCategories()
      {
         var categories = await _categoryService.GetAllAsync();
         if (!categories.Success)
            return BadRequest();

         return Ok(categories);
      }
      [HttpGet("getAllNames")]
      public async Task<IActionResult> GetAllCategoryNames()
      {
         var categories = await _categoryService.GetAllCategoryNamesAsync();
         if (!categories.Success)
            return BadRequest();

         return Ok(categories);
      }
      [HttpGet("get")]
      public async Task<IActionResult> GetById(int categoryId)
      {
         var category = await _categoryService.GetById(categoryId);
         if (!category.Success)
            return BadRequest(category);

         return Ok(category);
      }
      [HttpPost("add")]
      public async Task<IActionResult> AddCategory(CategoryDto categoryDto)
      {
         var validationResult = _categoryValidator.Validate(categoryDto);
         if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

         var category = await _categoryService.AddAsync(categoryDto);
         if (!category.Success)
            return BadRequest(category);

         return Ok(category);
      }
      [HttpDelete("hardDelete")]
      public async Task<IActionResult> HardDelete(int categoryId)
      {
         var category = await _categoryService.HardDeleteAsync(categoryId);
         if (!category.Success)
            return BadRequest(category);

         return Ok(category);
      }
      [HttpDelete("softDelete")]
      public async Task<IActionResult> SoftDelete(int categoryId)
      {
         var category = await _categoryService.SoftDeleteAsync(categoryId);
         if (!category.Success)
            return BadRequest(category);

         return Ok(category);
      }
      [HttpPut("update")]
      public async Task<IActionResult> UpdateCategory(CategoryDto categoryDto)
      {
         var category = await _categoryService.UpdateAsync(categoryDto);
         if (!category.Success)
            return BadRequest(category);

         return Ok(category);
      }
      [HttpGet("getByArticleId")]
      public async Task<IActionResult> GetByArticleId(int articleId)
      {
         var categories = await _categoryService.GetCategoriesByArticleId(articleId);
         if (!categories.Success)
            return BadRequest(categories);

         return Ok(categories);
      }
      [HttpGet("getByName")]
      public async Task<IActionResult> GetByArticleName(string categoryName)
      {
         var categories = await _categoryService.GetByName(categoryName);
         if (!categories.Success)
            return BadRequest(categories);

         return Ok(categories);
      }


   }
}