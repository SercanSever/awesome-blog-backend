using Microsoft.Extensions.Caching.Memory;

namespace Blog.CORE.CrossCuttingConcerns.Cache.Microsoft
{
   public interface ICacheService
   {
      T Get<T>(string key);
      object Get(string key);
      void Set(string key, object value, MemoryCacheEntryOptions options = null);
      bool IsAdd(string key);
      void Remove(params string[] keys);
      Task<T> GetOrCreate<T>(string key, Func<object, Task<T>> value);
   }
}