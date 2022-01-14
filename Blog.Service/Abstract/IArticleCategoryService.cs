using Blog.CORE.Utilities.Results;
using Blog.Service.Dto;

namespace Blog.Service.Abstract
{
   public interface IArticleCategoryService
   {
      Task<IDataResult<List<ArticleCategoryDto>>> GetAllAsync();
      Task<IDataResult<ArticleCategoryDto>> AddAsync(ArticleCategoryDto articleCategoryDto);
      Task<IDataResult<ArticleCategoryDto>> GetByIdAsync(int id);
      Task<IResult> AddCategoryAsync(int articleId, List<string> categoryNames);
   }
}