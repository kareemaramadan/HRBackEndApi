using HR.Application.Helpers;
using HR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;

namespace HR.Application.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> criteria);
        Task<T> AddAsync(T entity);
        void UpdateByConditionAsync(T entity, Expression<Func<T, bool>> criteria);
        Task DeleteByConditionAsync(Expression<Func<T, bool>> criteria);
        Task<object> CRUDUsingStoredProcedureAsync(string spName, Dictionary<string, object> parameters, HttpRequestType httpRequest);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> criteria);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);
    }
}
