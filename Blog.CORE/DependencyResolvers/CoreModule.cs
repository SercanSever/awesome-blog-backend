using Blog.CORE.CrossCuttingConcerns.Cache.Microsoft;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.CORE.DependencyResolvers
{
   public static class CoreModule
   {
      public static IServiceCollection ConfigureCaching(this IServiceCollection services)
      {
         services.AddMemoryCache();
         services.AddScoped<ICacheService, MemoryCacheManager>();
         return services;
      }
   }
}