using System.Linq.Expressions;

namespace Blog.DAL.Repository
{
   public interface IRepository
   {
      T Get<T>(Expression<Func<T, bool>> filter) where T : class, new();
      List<T> GetAll<T>(Expression<Func<T, bool>> filter = null) where T : class, new();
      T Add<T>(T entity) where T : class, new();
      T Update<T>(T entity) where T : class, new();
      void UpdateMatchEntity<T>(T updateEntity, T setEntity) where T : class, new();
      void HardDelete<T>(T entity) where T : class, new();
      void SoftDelete<T>(T entity) where T : class, ISoftDeleteEntity, new();
      IEnumerable<T> AddRange<T>(IEnumerable<T> entities) where T : class, new();
      IEnumerable<T> UpdateRange<T>(IEnumerable<T> entities) where T : class, new();
      IEnumerable<T> HardDeleteRange<T>(IEnumerable<T> entities) where T : class, new();
      T Replace<T>(T entity) where T : class, new();

      Task<T> GetAsync<T>(Expression<Func<T, bool>> filter) where T : class, new();
      Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>> filter = null) where T : class, new();
      Task<T> AddAsync<T>(T entity) where T : class, new();
      Task<T> UpdateAsync<T>(T entity) where T : class, new();
      Task UpdateMatchEntityAsync<T>(T updateEntity, T setEntity) where T : class, new();
      Task HardDeleteAsync<T>(T entity) where T : class, new();
      Task SoftDeleteAsync<T>(T entity) where T : class, ISoftDeleteEntity, new();
      Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> entities) where T : class, new();
      Task<IEnumerable<T>> UpdateRangeAsync<T>(IEnumerable<T> entities) where T : class, new();
      Task<IEnumerable<T>> HardDeleteRangeAsync<T>(IEnumerable<T> entities) where T : class, new();
      Task<T> ReplaceAsync<T>(T entity) where T : class, new();
   }
}