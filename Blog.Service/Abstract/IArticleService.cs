using Blog.CORE.Utilities.Results;
using Blog.DATA.Entity;
using Blog.Service.Dto;

namespace Blog.Service.Abstract
{
   public interface IArticleService
   {
      Task<IDataResult<List<ArticleDto>>> GetAllAsync();
      Task<IDataResult<ArticleDto>> AddAsync(ArticleDto articleDto);
      Task<IDataResult<ArticleDto>> GetById(int id);
      Task<IResult> HardDeleteAsync(int articleId);
      Task<IResult> SoftDeleteAsync(int articleId);
      Task<IDataResult<ArticleDto>> UpdateAsync(ArticleDto articleDto);
      Task<IDataResult<List<ArticleDto>>> GetArticlesByCategoryIdAsync(int categoryId);
      Task<IDataResult<List<ArticleDto>>> GetByName(string articleName);
      Task<IDataResult<ArticleDto>> GetByUrl(string url);
      Task<IDataResult<int>> GetLastArticleId();
   }
}