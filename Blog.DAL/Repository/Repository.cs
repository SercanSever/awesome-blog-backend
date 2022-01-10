using System.Linq.Expressions;
using Blog.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repository
{
   public class Repository : IRepository
   {
      private readonly DbContext _context;

      public Repository(DbContext context)
      {
         _context = context;
      }

      public virtual T Add<T>(T entity) where T : class, new()
      {
         _context.Set<T>().Add(entity);
         _context.SaveChanges();
         return entity;
      }

      public virtual async Task<T> AddAsync<T>(T entity) where T : class, new()
      {
         await _context.Set<T>().AddAsync(entity);
         await _context.SaveChangesAsync();
         return entity;
      }

      public virtual IEnumerable<T> AddRange<T>(IEnumerable<T> entities) where T : class, new()
      {
         _context.Set<T>().AddRange(entities);
         _context.SaveChanges();
         return entities;
      }

      public virtual async Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> entities) where T : class, new()
      {
         await _context.Set<T>().AddRangeAsync(entities);
         await _context.SaveChangesAsync();
         return entities;
      }

      public virtual T Get<T>(Expression<Func<T, bool>> filter) where T : class, new()
      {
         return _context.Set<T>().FirstOrDefault(filter);
      }

      public virtual List<T> GetAll<T>(Expression<Func<T, bool>> filter = null) where T : class, new()
      {
         return filter == null
         ? _context.Set<T>().ToList()
         : _context.Set<T>().Where(filter).ToList();
      }

      public virtual async Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>> filter = null) where T : class, new()
      {
         return filter == null
         ? await _context.Set<T>().ToListAsync()
         : await _context.Set<T>().Where(filter).ToListAsync();
      }

      public Task<T> GetAsync<T>(Expression<Func<T, bool>> filter) where T : class, new()
      {
         return _context.Set<T>().FirstOrDefaultAsync(filter);
      }

      public virtual void HardDelete<T>(T entity) where T : class, new()
      {
         _context.Set<T>().Remove(entity);
         _context.SaveChanges();
      }

      public virtual async Task HardDeleteAsync<T>(T entity) where T : class, new()
      {
         _context.Set<T>().Remove(entity);
         await _context.SaveChangesAsync();
      }

      public virtual IEnumerable<T> HardDeleteRange<T>(IEnumerable<T> entities) where T : class, new()
      {
         _context.Set<T>().RemoveRange(entities);
         _context.SaveChanges();
         return entities;
      }

      public virtual async Task<IEnumerable<T>> HardDeleteRangeAsync<T>(IEnumerable<T> entities) where T : class, new()
      {
         _context.Set<T>().RemoveRange(entities);
         await _context.SaveChangesAsync();
         return entities;
      }

      public virtual T Replace<T>(T entity) where T : class, new()
      {
         _context.Entry(entity).State = EntityState.Modified;
         _context.SaveChanges();
         return entity;
      }

      public virtual async Task<T> ReplaceAsync<T>(T entity) where T : class, new()
      {
         _context.Entry(entity).State = EntityState.Modified;
         await _context.SaveChangesAsync();
         return entity;
      }

      public virtual void SoftDelete<T>(T entity) where T : class, ISoftDeleteEntity, new()
      {
         entity.IsDeleted = !entity.IsDeleted;
         Replace<T>(entity);
      }

      public virtual async Task SoftDeleteAsync<T>(T entity) where T : class, ISoftDeleteEntity, new()
      {
         entity.IsDeleted = !entity.IsDeleted;
         await ReplaceAsync<T>(entity);
      }

      public virtual T Update<T>(T entity) where T : class, new()
      {
         _context.Set<T>().Update(entity);
         _context.SaveChanges();
         return entity;
      }

      public virtual async Task<T> UpdateAsync<T>(T entity) where T : class, new()
      {
         _context.Set<T>().Update(entity);
         await _context.SaveChangesAsync();
         return entity;
      }

      public virtual void UpdateMatchEntity<T>(T updateEntity, T setEntity) where T : class, new()
      {
         if (updateEntity == null)
            throw new ArgumentNullException(nameof(updateEntity));
         if (setEntity == null)
            throw new ArgumentNullException(nameof(setEntity));

         _context.Entry<T>(updateEntity).CurrentValues.SetValues(setEntity);

         foreach (var property in _context.Entry<T>(setEntity).Properties)
         {
            if (property.CurrentValue == null)
            {
               _context.Entry<T>(updateEntity).Property(property.Metadata.Name).IsModified = false;
            }
         }
         _context.SaveChanges();
      }

      public virtual async Task UpdateMatchEntityAsync<T>(T updateEntity, T setEntity) where T : class, new()
      {
         if (updateEntity == null)
            throw new ArgumentNullException(nameof(updateEntity));
         if (setEntity == null)
            throw new ArgumentNullException(nameof(setEntity));

         _context.Entry<T>(updateEntity).CurrentValues.SetValues(setEntity);

         foreach (var property in _context.Entry<T>(setEntity).Properties)
         {
            if (property.CurrentValue == null)
            {
               _context.Entry<T>(updateEntity).Property(property.Metadata.Name).IsModified = false;
            }
         }
         await _context.SaveChangesAsync();
      }

      public virtual IEnumerable<T> UpdateRange<T>(IEnumerable<T> entities) where T : class, new()
      {
         _context.Set<T>().UpdateRange(entities);
         _context.SaveChanges();
         return entities;
      }

      public virtual async Task<IEnumerable<T>> UpdateRangeAsync<T>(IEnumerable<T> entities) where T : class, new()
      {
         _context.Set<T>().UpdateRange(entities);
         await _context.SaveChangesAsync();
         return entities;
      }
   }
}