using Microsoft.AspNetCore.Mvc;


namespace Blog.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class ImagesController : ControllerBase
   {
      private readonly IHttpContextAccessor _httpContextAccessor;

      public ImagesController(IHttpContextAccessor httpContextAccessor)
      {
         _httpContextAccessor = httpContextAccessor;
      }
      [HttpPost("UploadImage")]
      public async Task<object> UploadImage()
      {
         var file = _httpContextAccessor.HttpContext.Request.Form.Files[0];
         var directory = Directory.GetCurrentDirectory();
         var path = directory + "/Images/";
         int total;
         try
         {
            total = HttpContext.Request.Form.Files.Count;
         }
         catch (System.Exception exception)
         {
            return await Task.FromResult(new { error = new { message = "Error uploading file" } });
         }
         if (total == 0)
         {
            return await Task.FromResult(new { error = new { message = "No file has sent" } });
         }
         if (!Directory.Exists(path))
         {
            Directory.CreateDirectory(path);
         }

         string fileName = file.FileName;

         if (string.IsNullOrEmpty(fileName))
         {
            return await Task.FromResult(new { error = new { message = "Filename invalid" } });
         }

         string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
         string newPath = path + newFileName;
         if (file.Length > 0)
         {
            using (var stream = new FileStream(newPath, FileMode.Create))
            {
               file.CopyTo(stream);
            }
         }

         string imageUrl = "https://localhost:7217" + path + newFileName;

         return Task.FromResult(new { url = imageUrl });




      }
   }
}