using Blog.Service.Abstract;
using Blog.Service.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class ArticleCategoriesController : ControllerBase
   {
      private readonly IArticleCategoryService _articleCategoriesService;
      private readonly IArticleService _articleService;

      public ArticleCategoriesController(IArticleCategoryService articleCategoriesService, IArticleService articleService)
      {
         _articleCategoriesService = articleCategoriesService;
         _articleService = articleService;
      }
      [HttpGet("getAll")]
      public async Task<IActionResult> GetAllArticleCategories()
      {
         var articleCategories = await _articleCategoriesService.GetAllAsync();
         if (!articleCategories.Success)
            return BadRequest();

         return Ok(articleCategories);
      }
      [HttpGet("getById")]
      public async Task<IActionResult> GetAllArticleCategories(int articleCategoryId)
      {
         var articleCategories = await _articleCategoriesService.GetByIdAsync(articleCategoryId);
         if (!articleCategories.Success)
            return BadRequest();

         return Ok(articleCategories);
      }
      [HttpPost("addCategory")]
      public async Task<IActionResult> AddCategory(int articleId, List<string> categoryNames)
      {
         if (articleId == 0)
         {
            var lastArticle = _articleService.GetLastArticleId();
            var article = await _articleCategoriesService.AddCategoryAsync(lastArticle.Result.Data, categoryNames);
            if (!article.Success)
               return BadRequest(article);
            return Ok(article);
         }
         return Ok();
      }

   }
}