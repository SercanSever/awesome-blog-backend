using Microsoft.AspNetCore.Builder;
namespace Blog.CORE.Extensions
{
   public static class ExceptionMiddlewareExtensions
   {
      public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
      {
         app.UseMiddleware<ExceptionMiddleware>();
      }
   }
}