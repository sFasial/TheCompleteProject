using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TheCompleteProject.Repository.Infrastructure
{
    public interface IBaseRepository <T> where T : class
    {
        #region UNIT OF WORK
        Task<object> UpdateAsync(T entity);
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetByIdAsync(int id);
        void Add(T entity);
        Task<T> AddAsync(T entity);
        Task<List<T>> AddRangeAsync(List<T> entity);
        T Update(T entity);
        void Delete(T entity);
        void DeleteRange(List<T> entity);
        Task<T> GetDefaultAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> Query(bool eager = false);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool eager = false);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        #endregion
    }
}
