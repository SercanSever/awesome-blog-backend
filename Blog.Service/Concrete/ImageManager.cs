using Blog.CORE.Utilities.Files;
using Blog.CORE.Utilities.Results;
using Blog.Service.Abstract;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Concrete
{
   public class ImageManager : IImageService
   {
      public async Task<IDataResult<string>> Upload(IFormFile file)
      {
         var path = await new FileSystem().Add(file, CreateNewPath(file));
         return new SuccessDataResult<string>("https://localhost:7217" + path);
      }
      private string CreateNewPath(IFormFile file)
      {

         var directory = Directory.GetCurrentDirectory();
         var path = directory + "/Images/";
         if (!Directory.Exists(path))
         {
            Directory.CreateDirectory(path);
         }
         var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
         return Path.Combine(path, fileName);
      }
   }
}