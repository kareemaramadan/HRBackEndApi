using HR.Application.Dtos.LookUpDtos.Country;
using HR.Application.Helpers;
using HR.Application.Response;
using HR.Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
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

  Task<ApiResponse<IEnumerable<T>>> GetAllAsync ( );
  Task<ApiResponse<IEnumerable<T>>> GetAllAsync ( Expression<Func<T,bool>> criteria );
  Task<ApiResponse<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
  Task<ApiResponse<T>> CreateAsync ( T entity );
  Task<ApiResponse<IEnumerable<T>>> CreateAsyncAndGetAll ( T entity );
  Task<ApiResponse<T>> CreateAsync ( T entity,Expression<Func<T,bool>> checkCriteria );
  Task<ApiResponse<IEnumerable<T>>> CreateAsyncAndGetAll ( T entity,Expression<Func<T,bool>> checkCriteria, Expression<Func<T, bool>> getAllCriteria);
  Task<ApiResponse<T>> UpdateAsync ( T entity );
  Task<ApiResponse<T>> UpdateAsync ( T entity,Expression<Func<T,bool>> criteria );
  Task<ApiResponse<int>> DeleteAsync ( T entity );
  Task<ApiResponse<int>> DeleteAsync ( Expression<Func<T,bool>> criteria );


  //Extra Functions for count and checking
  //=======================================

  Task<ApiResponse<bool>> IsExistAsync ( Expression<Func<T,bool>> criteria);
  Task<ApiResponse<int>> CountAsync ( );
  Task<ApiResponse<int>> CountAsync ( Expression<Func<T,bool>> criteria );
  Task<ApiResponse<T>> FindItemAsync ( Expression<Func<T,bool>> criteria );
  Task<ApiResponse<IEnumerable<T>>> FindAsync ( Expression<Func<T,bool>> criteria );

  //Using StoredProcedures
  //=======================

  Task<ApiResponse<IEnumerable<T>>> GetUsingStoredProcedureAsync ( string spName,Dictionary<string,object> parameters );
  Task<ApiResponse<int>> CUDUsingStoredProcedureAsync ( string spName,Dictionary<string,object> parameters,Expression<Func<T,bool>> checkCriteria);

 }
}
