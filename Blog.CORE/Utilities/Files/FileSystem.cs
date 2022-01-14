using Microsoft.AspNetCore.Http;

namespace Blog.CORE.Utilities.Files
{
   public class FileSystem : IFileSystem
   {
      public async Task<string> Add(IFormFile file, string path)
      {
         if (file.Length > 0)
         {
            using (var stream = new FileStream(path, FileMode.Create))
            {
               await file.CopyToAsync(stream);
            }
         }
         return path;
      }
   }
}