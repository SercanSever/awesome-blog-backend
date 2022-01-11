using Blog.CORE.CrossCuttingConcerns.Cache.Microsoft;
using Blog.Service.Abstract;
using Blog.Service.Dto;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class ArticlesController : ControllerBase
   {
      private readonly IArticleService _articleService;
      private readonly IValidator<ArticleDto> _articleValidator;

      public ArticlesController(IArticleService articleService, IValidator<ArticleDto> articleValidator)
      {
         _articleService = articleService;
         _articleValidator = articleValidator;
      }
      [HttpGet("getAll")]
      public async Task<IActionResult> GetAllArticles()
      {
         var articles = await _articleService.GetAllAsync();
         if (!articles.Success)
            return BadRequest();

         return Ok(articles);
      }
      [HttpGet("get")]
      public async Task<IActionResult> GetById(int id)
      {
         var article = await _articleService.GetById(id);
         if (!article.Success)
            return BadRequest(article);

         return Ok(article);
      }
      [HttpGet("getbyName")]
      public async Task<IActionResult> GetByName(string articleName)
      {
         var article = await _articleService.GetByName(articleName);
         if (!article.Success)
            return BadRequest(article);

         return Ok(article);
      }
      [HttpPost("add")]
      public async Task<IActionResult> AddArticle(ArticleDto articleDto)
      {
         var validationResult = await _articleValidator.ValidateAsync(articleDto);
         if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

         var article = await _articleService.AddAsync(articleDto);
         if (!article.Success)
            return BadRequest(article);

         return Ok(article);
      }
      [HttpDelete("hardDelete")]
      public async Task<IActionResult> HardDelete(ArticleDto articleDto)
      {
         var article = await _articleService.HardDeleteAsync(articleDto);
         if (!article.Success)
            return BadRequest(article);

         return Ok(article);
      }
      [HttpDelete("softDelete")]
      public async Task<IActionResult> SoftDelete(ArticleDto articleDto)
      {
         var article = await _articleService.SoftDeleteAsync(articleDto);
         if (!article.Success)
            return BadRequest(article);

         return Ok(article);
      }
      [HttpPut("update")]
      public async Task<IActionResult> UpdateArticle(ArticleDto articleDto)
      {
         var article = await _articleService.UpdateAsync(articleDto);
         if (!article.Success)
            return BadRequest(article);

         return Ok(article);
      }
      [HttpGet("getbycategory")]
      public async Task<IActionResult> GetArticlesByCategoryId(int categoryId)
      {
         var article = await _articleService.GetArticlesByCategoryIdAsync(categoryId);
         if (!article.Success)
            return BadRequest(article);

         return Ok(article);
      }
      [HttpPost("addCategory")]
      public IActionResult AddCategory(int articleId, int categoryId)
      {
         // var article = _articleService.AddCategory(articleId, categoryId);
         // if (!article.Success)
         //    return BadRequest(article);

         // return Ok(article);
         return Ok();
      }
      [HttpGet("url")]
      public async Task<IActionResult> GetArticlesByCategoryId(string articleUrl)
      {
         var article = await _articleService.GetByUrl(articleUrl);
         if (!article.Success)
            return BadRequest(article);

         return Ok(article);
      }
   }
}