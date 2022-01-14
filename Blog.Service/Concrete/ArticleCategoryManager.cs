using AutoMapper;
using Blog.CORE.CrossCuttingConcerns.Cache.Microsoft;
using Blog.CORE.Utilities.Business;
using Blog.CORE.Utilities.Results;
using Blog.DAL.Context;
using Blog.DAL.Repository;
using Blog.DATA.Entity;
using Blog.Service.Abstract;
using Blog.Service.Constants;
using Blog.Service.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Concrete
{
   public class ArticleCategoryManager : IArticleCategoryService
   {
      private readonly IRepository _repository;
      private readonly IMapper _mapper;
      private readonly ICacheService _cacheService;
      private readonly ILogger<ArticleCategoryManager> _logger;
      private readonly ICategoryService _categoryService;
      private readonly IArticleService _articeService;
      public ArticleCategoryManager(IRepository repository, IMapper mapper, ILogger<ArticleCategoryManager> logger, ICacheService cacheService, ICategoryService categoryService, IArticleService articleService)
      {
         _repository = repository;
         _mapper = mapper;
         _logger = logger;
         _cacheService = cacheService;
         _categoryService = categoryService;
         _articeService = articleService;
      }
      public async Task<IDataResult<ArticleCategoryDto>> AddAsync(ArticleCategoryDto articleCategoryDto)
      {

         try
         {
            BusinessRules.Run(NullCheck(articleCategoryDto));
            var mappedCategory = _mapper.Map<ArticleCategoryDto>(articleCategoryDto);
            await _repository.AddAsync<ArticleCategoryDto>(mappedCategory);
         }
         catch (Exception e)
         {
            return new ErrorDataResult<ArticleCategoryDto>(articleCategoryDto, e.Message);
         }
         return new SuccessDataResult<ArticleCategoryDto>(articleCategoryDto, Messages.Added);
      }
      public async Task<IDataResult<List<ArticleCategoryDto>>> GetAllAsync()
      {
         var articleCategories = await _repository.GetAllAsync<ArticleCategory>();
         var mappedCategory = _mapper.Map<List<ArticleCategoryDto>>(articleCategories);

         BusinessRules.Run(NullCheck(mappedCategory));
         return new SuccessDataResult<List<ArticleCategoryDto>>(mappedCategory, Messages.Listed);
      }
      public async Task<IDataResult<ArticleCategoryDto>> GetByIdAsync(int id)
      {
         var category = await _repository.GetAsync<ArticleCategory>(x => x.ArticleCategoryId == id);
         BusinessRules.Run(NullCheck(category));

         var mappedCategory = _mapper.Map<ArticleCategoryDto>(category);
         return new SuccessDataResult<ArticleCategoryDto>(mappedCategory, Messages.Listed);
      }
      public async Task<IResult> AddCategoryAsync(int articleId, List<string> categoryNames)
      {
         try
         {

            foreach (var categoryName in categoryNames)
            {
               var categoryId = _categoryService.GetByName(categoryName).Result.Data.CategoryId;
               var articleCategories = new ArticleCategory();
               articleCategories.ArticleId = articleId;
               articleCategories.CategoryId = categoryId;
               await _repository.AddAsync(articleCategories);
            }

            _logger.LogInformation("Called : AddCategoryAsync");
            _cacheService.Remove(CacheKeys.ArticleGetAll.ToString(), CacheKeys.ArticleGet.ToString());

            return new SuccessResult(Messages.Added);
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : AddCategoryAsync");
            return new ErrorResult(exception.Message);
         }
      }



      //Logics
      private IResult NullCheck(object value)
      {
         if (value == null)
         {
            return new ErrorResult(Messages.NullOrEmpty);
         }
         return new SuccessResult(Messages.Success);
      }
   }
}