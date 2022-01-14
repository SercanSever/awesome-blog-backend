using Blog.Service.Abstract;
using Microsoft.AspNetCore.Mvc;


namespace Blog.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class ImagesController : ControllerBase
   {
      private readonly IHttpContextAccessor _httpContextAccessor;
      private readonly IImageService _imageService;

      public ImagesController(IHttpContextAccessor httpContextAccessor, IImageService imageService)
      {
         _httpContextAccessor = httpContextAccessor;
         _imageService = imageService;
      }
      [HttpPost("UploadImage")]
      public async Task<object> UploadImage()
      {
         var file = _httpContextAccessor.HttpContext?.Request.Form.Files[0];
         var imageUrl = _imageService.Upload(file);

         if (imageUrl == null)
            return await Task.FromResult(new { error = new { message = "Error uploading file" } });

         return Task.FromResult(new { url = imageUrl });



      }
   }
}