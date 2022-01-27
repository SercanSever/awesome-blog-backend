using System.Security.Cryptography.X509Certificates;
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
   public class ArticleManager : IArticleService
   {
      private readonly IRepository _repository;
      private readonly IMapper _mapper;
      private readonly ICacheService _cacheService;
      private readonly ICategoryService _categoryService;
      private readonly ILogger<ArticleManager> _logger;
      public ArticleManager(IRepository repository, IMapper mapper, ICacheService cacheService, ILogger<ArticleManager> logger, ICategoryService categoryService)
      {
         _repository = repository;
         _mapper = mapper;
         _cacheService = cacheService;
         _logger = logger;
         _categoryService = categoryService;

      }
      public async Task<IDataResult<List<ArticleDto>>> GetAllAsync()
      {
         try
         {
            _logger.LogInformation("Called : GetAllAsync");
            return await _cacheService.GetOrCreate<IDataResult<List<ArticleDto>>>(CacheKeys.ArticleGetAll.ToString(), async article =>
            {
               var articles = await _repository.GetAllAsync<Article>();
               var mappedArticle = _mapper.Map<List<ArticleDto>>(articles);
               foreach (var mArticle in mappedArticle)
               {
                  mArticle.CategoryNames = _categoryService.GetCategoriesByArticleId(mArticle.ArticleId).Result.Data.Select(x => x.Name).ToList();
               }
               BusinessRules.Run(NullCheck(mappedArticle));

               return new SuccessDataResult<List<ArticleDto>>(mappedArticle, Messages.Listed);
            });
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : GetAllAsync");
            throw;
         }
      }
      public async Task<IDataResult<ArticleDto>> GetById(int id)
      {
         try
         {
            _logger.LogInformation("Called : GetById");
            var entity = await _repository.GetAsync<Article>(x => x.ArticleId == id);
            var mappedArticle = _mapper.Map<ArticleDto>(entity);
            mappedArticle.CategoryNames = _categoryService.GetCategoriesByArticleId(mappedArticle.ArticleId).Result.Data.Select(x => x.Name).ToList();

            BusinessRules.Run(NullCheck(entity));

            return new SuccessDataResult<ArticleDto>(mappedArticle, Messages.Listed);
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : GetById");
            throw;
         }

      }
      public async Task<IDataResult<int>> GetLastArticleId()
      {
         try
         {
            _logger.LogInformation("Called : GetLastArticleId");
            var entity = await _repository.GetAllAsync<Article>();
            var lastArticle = entity.OrderByDescending(x => x.ArticleId).Take(1).First();

            BusinessRules.Run(NullCheck(lastArticle));

            return new SuccessDataResult<int>(lastArticle.ArticleId, Messages.Listed);
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : GetLastArticleId");
            throw;
         }

      }
      public async Task<IDataResult<ArticleDto>> AddAsync(ArticleDto articleDto)
      {
         try
         {
            BusinessRules.Run(NullCheck(articleDto), ConvertNameToUrl(articleDto));

            var mappedArticle = _mapper.Map<Article>(articleDto);
            articleDto.UploadDate = DateTime.Now;

            await _repository.AddAsync(mappedArticle);

            _logger.LogInformation("Called : AddAsync");
            _cacheService.Remove(CacheKeys.ArticleGetAll.ToString(), CacheKeys.ArticleGet.ToString());

            return new SuccessDataResult<ArticleDto>(articleDto, Messages.Added);
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : AddAsync");
            return new ErrorDataResult<ArticleDto>(articleDto, exception.Message);
         }

      }
      public async Task<IDataResult<ArticleDto>> UpdateAsync(ArticleDto articleDto)
      {
         try
         {
            BusinessRules.Run(NullCheck(articleDto));

            var mappedArticle = _mapper.Map<Article>(articleDto);
            mappedArticle.UpdateDate = DateTime.Now;
            var article = await _repository.UpdateAsync<Article>(mappedArticle);

            _logger.LogInformation("Called : UpdateAsync");
            _cacheService.Remove(CacheKeys.ArticleGetAll.ToString(), CacheKeys.ArticleGet.ToString());

            return new SuccessDataResult<ArticleDto>(articleDto, Messages.Updated);
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : UpdateAsync");
            return new ErrorDataResult<ArticleDto>(articleDto, exception.Message);
         }

      }
      public async Task<IResult> HardDeleteAsync(int articleId)
      {
         try
         {
            var article = await _repository.GetAsync<Article>(x => x.ArticleId == articleId);
            var articleCategory = await _repository.GetAllAsync<ArticleCategory>(x => x.ArticleId == articleId);

            BusinessRules.Run(NullCheck(article));

            foreach (var ac in articleCategory)
            {
               await _repository.HardDeleteAsync<ArticleCategory>(ac);
            }
            await _repository.HardDeleteAsync<Article>(article);

            _logger.LogInformation("Called : HardDeleteAsync");
            _cacheService.Remove(CacheKeys.ArticleGetAll.ToString(), CacheKeys.ArticleGet.ToString());

            return new SuccessResult(Messages.HardDeleted);
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : HardDeleteAsync");
            return new ErrorResult(exception.Message);
         }

      }
      public async Task<IResult> SoftDeleteAsync(int articleId)
      {
         try
         {
            var article = await _repository.GetAsync<Article>(x => x.ArticleId == articleId);
            BusinessRules.Run(NullCheck(article));

            await _repository.SoftDeleteAsync<Article>(article);

            _logger.LogInformation("Called : SoftDeleteAsync");
            _cacheService.Remove(CacheKeys.ArticleGetAll.ToString(), CacheKeys.ArticleGet.ToString());

            return new SuccessResult(Messages.SoftDeleted);
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : SoftDeleteAsync");
            return new ErrorResult(exception.Message);
         }
      }
      public async Task<IDataResult<List<ArticleDto>>> GetArticlesByCategoryIdAsync(int categoryId)
      {
         try
         {
            _logger.LogInformation("Called : GetArticlesByCategoryId");
            using (var context = new BlogContext())
            {
               var articles = await context.ArticleCategories.Include(x => x.Article).Where(x => x.CategoryId == categoryId).Select(x => x.Article).ToListAsync();
               var mappedArticle = _mapper.Map<List<ArticleDto>>(articles);
               foreach (var mArticle in mappedArticle)
               {
                  mArticle.CategoryNames = _categoryService.GetCategoriesByArticleId(mArticle.ArticleId).Result.Data.Select(x => x.Name).ToList();
               }
               BusinessRules.Run(NullCheck(mappedArticle));
               return new SuccessDataResult<List<ArticleDto>>(mappedArticle, Messages.Listed);
            }
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : GetArticlesByCategoryId");
            throw;
         }
      }
      public async Task<IDataResult<List<ArticleDto>>> GetArticlesByCategoryNameAsync(string categoryName)
      {
         try
         {
            _logger.LogInformation("Called : GetArticlesByCategoryNameAsync");
            using (var context = new BlogContext())
            {
               var category = await _categoryService.GetByName(categoryName);
               var articles = await context.ArticleCategories.Include(x => x.Article).Where(x => x.CategoryId == category.Data.CategoryId).Select(x => x.Article).ToListAsync();
               var mappedArticle = _mapper.Map<List<ArticleDto>>(articles);
               foreach (var mArticle in mappedArticle)
               {
                  mArticle.CategoryNames = _categoryService.GetCategoriesByArticleId(mArticle.ArticleId).Result.Data.Select(x => x.Name).ToList();
               }
               BusinessRules.Run(NullCheck(mappedArticle));
               return new SuccessDataResult<List<ArticleDto>>(mappedArticle, Messages.Listed);
            }
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : GetArticlesByCategoryNameAsync");
            throw;
         }
      }
      public async Task<IDataResult<List<ArticleDto>>> GetByName(string articleName)
      {
         try
         {
            _logger.LogInformation("Called : GetByName");
            var entity = await _repository.GetAllAsync<Article>(x => x.Name.Contains(articleName));
            var mappedArticle = _mapper.Map<List<ArticleDto>>(entity);
            foreach (var mArticle in mappedArticle)
            {
               mArticle.CategoryNames = _categoryService.GetCategoriesByArticleId(mArticle.ArticleId).Result.Data.Select(x => x.Name).ToList();
            }

            BusinessRules.Run(NullCheck(entity));

            return new SuccessDataResult<List<ArticleDto>>(mappedArticle, Messages.Listed);
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : GetById");
            throw;
         }
      }
      public async Task<IDataResult<ArticleDto>> GetByUrl(string url)
      {
         try
         {
            _logger.LogInformation("Called : GetByUrl");
            var entity = await _repository.GetAsync<Article>(x => x.NameUrl == url);
            var mappedArticle = _mapper.Map<ArticleDto>(entity);
            mappedArticle.CategoryNames = _categoryService.GetCategoriesByArticleId(mappedArticle.ArticleId).Result.Data.Select(x => x.Name).ToList();

            BusinessRules.Run(NullCheck(entity));

            return new SuccessDataResult<ArticleDto>(mappedArticle, Messages.Listed);
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : GetByUrl");
            throw;
         }
      }





      //Logics
      private IResult NullCheck(object value)
      {
         if (value == null)
            return new ErrorResult(Messages.NullOrEmpty);

         return new SuccessResult(Messages.Success);
      }

      private IResult ConvertNameToUrl(ArticleDto articleDto)
      {
         articleDto.NameUrl = articleDto.Name.Replace(" ", "-").ToLower();
         return new SuccessResult(Messages.Success);
      }


   }
}