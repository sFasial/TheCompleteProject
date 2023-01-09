using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TheCompleteProject.Repository.DatabaseContext;

namespace TheCompleteProject.Repository.Infrastructure
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {

        public readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region GET METHODS
        public async Task<IEnumerable<T>> GetAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).FirstOrDefaultAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool eager = false)
        {
            return await Query(eager).FirstOrDefaultAsync(predicate);
        }
        #endregion

        #region ADDING ENTITY
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<List<T>> AddRangeAsync(List<T> entity)
        {
            await _context.Set<T>().AddRangeAsync(entity);
            return entity;
        }
        #endregion

        #region UPDATE METHODS
        public T Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public async Task<object> UpdateAsync(T entity)
        {
            _context.Update(entity);
            //await _context.SaveChangesAsync();
            return entity;
        }
        #endregion

        #region DELETE METHODS
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(List<T> entity)
        {
            _context.Set<T>().RemoveRange(entity);
        } 
        #endregion
       
        
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (predicate != null)
                query.Where(predicate);
            return await query.CountAsync();
        }

        public IQueryable<T> Query(bool eager = false)
        {
            var query = _context.Set<T>().AsQueryable();
            if (eager)
            {
                foreach (var property in _context.Model.FindEntityType(typeof(T)).GetNavigations())
                    query = query.Include(property.Name);
            }
            return query;
        }

    }
}
