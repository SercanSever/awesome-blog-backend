using Blog.CORE.Utilities.Results;
using Blog.Service.Dto;

namespace Blog.Service.Abstract
{
   public interface ICategoryService
   {
      Task<IDataResult<List<CategoryDto>>> GetAllAsync();
      Task<IDataResult<CategoryDto>> AddAsync(CategoryDto categoryDto);
      Task<IDataResult<CategoryDto>> GetById(int id);
      Task<IResult> HardDeleteAsync(CategoryDto categoryDto);
      Task<IResult> SoftDeleteAsync(CategoryDto categoryDto);
      Task<IDataResult<CategoryDto>> UpdateAsync(CategoryDto categoryDto);
      Task<IDataResult<List<CategoryDto>>> GetCategoriesByArticleId(int articleId);
      Task<IDataResult<CategoryDto>> GetByName(string categoryName);
   }
}