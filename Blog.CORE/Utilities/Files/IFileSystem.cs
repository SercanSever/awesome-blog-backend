using Microsoft.AspNetCore.Http;

namespace Blog.CORE.Utilities.Files
{
   public interface IFileSystem
   {
      Task<string> Add(IFormFile file, string path);

   }
}