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
   public class CategoryManager : ICategoryService
   {
      private readonly IRepository _repository;
      private readonly IMapper _mapper;
      private readonly ICacheService _cacheService;
      private readonly ILogger<CategoryManager> _logger;
      public CategoryManager(IRepository repository, IMapper mapper, ILogger<CategoryManager> logger, ICacheService cacheService)
      {
         _repository = repository;
         _mapper = mapper;
         _logger = logger;
         _cacheService = cacheService;
      }
      public async Task<IDataResult<List<CategoryDto>>> GetAllAsync()
      {
         //try
         try
         {
            _logger.LogInformation("Called : GetAllAsync");
            return await _cacheService.GetOrCreate<IDataResult<List<CategoryDto>>>(CacheKeys.CategoryGetAll.ToString(), async category =>
            {
               var categories = await _repository.GetAllAsync<Category>();
               var mappedCategory = _mapper.Map<List<CategoryDto>>(categories);

               BusinessRules.Run(NullCheck(mappedCategory));

               return new SuccessDataResult<List<CategoryDto>>(mappedCategory, Messages.Listed);
            });
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : GetAllAsync");
            throw;
         }

      }
      public async Task<IDataResult<CategoryDto>> GetById(int id)
      {
         try
         {
            _logger.LogInformation("Called : GetById");
            var category = await _repository.GetAsync<Category>(x => x.CategoryId == id);
            var mappedCategory = _mapper.Map<CategoryDto>(category);

            BusinessRules.Run(NullCheck(mappedCategory));

            return new SuccessDataResult<CategoryDto>(mappedCategory, Messages.Listed);
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : GetById");
            throw;
         }

      }
      public async Task<IDataResult<CategoryDto>> AddAsync(CategoryDto categoryDto)
      {
         try
         {
            BusinessRules.Run(NullCheck(categoryDto));

            var mappedCategory = _mapper.Map<Category>(categoryDto);
            categoryDto.UploadDate = DateTime.Now;
            await _repository.AddAsync(mappedCategory);

            _logger.LogInformation("Called : AddAsync");
            _cacheService.Remove(CacheKeys.CategoryGetAll.ToString(), CacheKeys.CategoryGet.ToString());

            return new SuccessDataResult<CategoryDto>(categoryDto, Messages.Added);

         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : AddAsync");
            return new ErrorDataResult<CategoryDto>(categoryDto, exception.Message);
         }
      }
      public async Task<IDataResult<CategoryDto>> UpdateAsync(CategoryDto categoryDto)
      {
         try
         {
            BusinessRules.Run(NullCheck(categoryDto));

            var mappedCategory = _mapper.Map<Category>(categoryDto);
            mappedCategory.UpdateDate = DateTime.Now;
            var category = await _repository.UpdateAsync<Category>(mappedCategory);

            _logger.LogInformation("Called : UpdateAsync");
            _cacheService.Remove(CacheKeys.CategoryGetAll.ToString(), CacheKeys.CategoryGet.ToString());

            return new SuccessDataResult<CategoryDto>(categoryDto, Messages.Updated);
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : UpdateAsync");
            return new ErrorDataResult<CategoryDto>(categoryDto, exception.Message);
         }

      }
      public async Task<IResult> HardDeleteAsync(int categoryId)
      {
         try
         {
            var category = await _repository.GetAsync<Category>(x => x.CategoryId == categoryId);
            var articleCategory = await _repository.GetAllAsync<ArticleCategory>(x => x.CategoryId == categoryId);

            BusinessRules.Run(NullCheck(category));

            foreach (var ac in articleCategory)
            {
               await _repository.HardDeleteAsync<ArticleCategory>(ac);
            }
            await _repository.HardDeleteAsync<Category>(category);

            _logger.LogInformation("Called : HardDeleteAsync");
            _cacheService.Remove(CacheKeys.CategoryGetAll.ToString());

            return new SuccessResult(Messages.HardDeleted);
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : HardDeleteAsync");
            return new ErrorResult(exception.Message);
         }

      }
      public async Task<IResult> SoftDeleteAsync(int categoryId)
      {
         try
         {
            var category = await _repository.GetAsync<Category>(x => x.CategoryId == categoryId);

            BusinessRules.Run(NullCheck(category));

            await _repository.SoftDeleteAsync<Category>(category);

            _logger.LogInformation("Called : SoftDeleteAsync");
            _cacheService.Remove(CacheKeys.CategoryGetAll.ToString());

            return new SuccessResult(Messages.SoftDeleted);
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : SoftDeleteAsync");
            return new ErrorResult(exception.Message);
         }
      }
      public async Task<IDataResult<List<CategoryDto>>> GetCategoriesByArticleId(int articleId)
      {
         try
         {
            _logger.LogInformation("Called : GetCategoriesByArticleId");
            using (var context = new BlogContext())
            {
               var categories = await context.ArticleCategories.Include(x => x.Article).Where(x => x.ArticleId == articleId).Select(x => x.Category).ToListAsync();
               var mappedCategories = _mapper.Map<List<CategoryDto>>(categories);
               BusinessRules.Run(NullCheck(mappedCategories));
               return new SuccessDataResult<List<CategoryDto>>(mappedCategories, Messages.Listed);
            }
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : GetCategoriesByArticleId");
            throw;
         }
      }
      public async Task<IDataResult<CategoryDto>> GetByName(string categoryName)
      {
         try
         {
            _logger.LogInformation("Called : GetByName");
            var entity = await _repository.GetAsync<Category>(x => x.Name == categoryName);
            var mappedArticle = _mapper.Map<CategoryDto>(entity);

            BusinessRules.Run(NullCheck(entity));

            return new SuccessDataResult<CategoryDto>(mappedArticle, Messages.Listed);
         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : GetByName");
            throw;
         }
      }
      public async Task<IDataResult<List<string>>> GetAllCategoryNamesAsync()
      {
         try
         {
            _logger.LogInformation("Called : GetAllCategoryNamesAsync");

            var categories = await _repository.GetAllAsync<Category>();
            var mappedCategory = _mapper.Map<List<CategoryDto>>(categories);
            var result = mappedCategory.Select(x => x.Name).ToList();

            BusinessRules.Run(NullCheck(mappedCategory));

            return new SuccessDataResult<List<string>>(result, Messages.Listed);

         }
         catch (Exception exception)
         {
            _logger.LogError(exception, "Error : GetAllCategoryNamesAsync");
            throw;
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