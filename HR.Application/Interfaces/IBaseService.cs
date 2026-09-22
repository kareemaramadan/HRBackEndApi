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
        //Main CRUD Operations
        //=====================
        
        Task<(IEnumerable<T>?, bool IsSuccess)> GetAllAsync();
        Task<(IEnumerable<T>?, bool IsSuccess)> GetByConditionAsync(Expression<Func<T, bool>> criteria);
        Task<(T? entity, bool IsSuccess)> CreateAsync(T entity);
        Task<(T? entity, bool IsSuccess)> CreateAsync(T entity, HttpRequestType httpRequest, Expression<Func<T, bool>> checkCriteria);
        Task<(T, bool IsSuccess)> UpdateAsync ( T entity);
        Task<(T, bool IsSuccess)> UpdateAsync(T entity, Expression<Func<T, bool>> criteria);
        Task<int> DeleteAsync(Expression<Func<T, bool>> criteria);

        //Extra Functions for count and checking
        //=======================================

        Task<bool> IsExistAsync(Expression<Func<T, bool>> criteria,HttpRequestType httpRequest);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);
        Task<(IEnumerable<T>?, bool IsSuccess)> FindAsync(Expression<Func<T, bool>> criteria);

        //Using StoredProcedures
        //=======================

        Task<IEnumerable<T>> GetUsingStoredProcedureAsync(string spName, Dictionary<string, object> parameters);
        Task<int> CUDUsingStoredProcedureAsync(string spName, Dictionary<string, object> parameters, Expression<Func<T, bool>> checkCriteria, HttpRequestType httpRequest);

    }
}
