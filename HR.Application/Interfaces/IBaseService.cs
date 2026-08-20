using HR.Application.Dtos.LookUpDtos.Country;
using HR.Application.Helpers;
using HR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HR.Application.Interfaces
{
    public interface IBaseService<T>
        where T : class
    {
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);
        Task<T> CreateAsync(T entity);
        Task<T> CreateAsync(T entity, HttpRequestType httpRequest, Expression<Func<T, bool>> checkCriteria);

        Task<int> CUDUsingStoredProcedureAsync(string spName, Dictionary<string, object> parameters, Expression<Func<T, bool>> checkCriteria, HttpRequestType httpRequest);
        Task<IEnumerable<T>> GetUsingStoredProcedureAsync(string spName, Dictionary<string, object> parameters);
        
        Task DeleteAsync(Expression<Func<T, bool>> criteria);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> criteria);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> criteria);
        Task <bool> IsExistAsync(Expression<Func<T, bool>> criteria,HttpRequestType httpRequest);
        Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> criteria);
    }
}
