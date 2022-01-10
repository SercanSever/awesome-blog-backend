using Microsoft.Extensions.Caching.Memory;

namespace Blog.CORE.CrossCuttingConcerns.Cache.Microsoft
{
   public class MemoryCacheManager : ICacheService
   {
      private readonly IMemoryCache _memoryCache;

      public MemoryCacheManager(IMemoryCache memoryCache)
      {
         _memoryCache = memoryCache;
      }
      public T Get<T>(string key)
      {
         return _memoryCache.Get<T>(key);
      }
      public object Get(string key)
      {
         return _memoryCache.Get(key);
      }

      public bool IsAdd(string key)
      {
         return _memoryCache.TryGetValue(key, out _);
      }

      public void Remove(params string[] keys)
      {
         foreach (string key in keys)
            _memoryCache.Remove(key);
      }
      public void Set(string key, object value, MemoryCacheEntryOptions options = null)
      {
         _memoryCache.Set(key, value, options);
      }
      public async Task<T> GetOrCreate<T>(string key, Func<object, Task<T>> value)
      {
         return await _memoryCache.GetOrCreateAsync<T>(key, value);
      }
   }
}