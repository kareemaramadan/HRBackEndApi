using HR.Application.Helpers;
using HR.Application.Response;
using HR.Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;

namespace HR.Application.Interfaces
{
 public interface IBaseRepository<T> where T : class
 {
  Task<IEnumerable<T>> GetAllAsync ( );
  Task<IEnumerable<T>> GetAllAsync ( Expression<Func<T,bool>> criteria );
  Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
  Task<T> CreateAsync ( T entity );
  Task<T> UpdateAsync ( T entity,Expression<Func<T,bool>> criteria );
  Task<T> UpdateAsync ( T entity );
  Task<int> DeleteAsync ( Expression<Func<T,bool>> criteria );
  Task<int> DeleteAsync ( T entity );
  Task<int> CUDUsingStoredProcedureAsync ( string spName,Dictionary<string,object> parameters);
  Task<IEnumerable<T>> GetUsingStoredProcedureAsync ( string spName,Dictionary<string,object> parameters );
  Task<IEnumerable<T>> FindAsync ( Expression<Func<T,bool>> criteria );
  Task<T> FindItemAsync ( Expression<Func<T,bool>> criteria );
  Task<int> CountAsync ( );
  Task<int> CountAsync ( Expression<Func<T,bool>> criteria );
 }
}
