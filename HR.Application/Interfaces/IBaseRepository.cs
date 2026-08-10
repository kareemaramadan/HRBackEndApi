using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HR.Application.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByNameAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(T entity);
        void DeleteAsync(int id);
        void DeleteRangeAsync(IEnumerable<T> entities);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate, string[] includes = null);
    }
    
}
