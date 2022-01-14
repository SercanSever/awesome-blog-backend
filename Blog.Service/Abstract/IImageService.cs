using Blog.CORE.Utilities.Results;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Abstract
{
   public interface IImageService
   {
      Task<IDataResult<string>> Upload(IFormFile file);
   }
}