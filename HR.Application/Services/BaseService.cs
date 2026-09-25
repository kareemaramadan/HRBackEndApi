using HR.Application.Helpers;
using HR.Application.Interfaces;
using HR.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace HR.Application.Services
{
    public class BaseService<T> ( IBaseRepository<T> _repository ) : IBaseService<T>
        where T : class
    {

        //Main CRUD Operations
        //=====================

        public async Task<(IEnumerable<T>?, bool IsSuccess)> GetAllAsync ( )
        {
            var items = await _repository.GetAllAsync ( );
            if ( items.Count() == 0 )
                return (null, false);

            return (items, true);
        }
        public async Task<(IEnumerable<T>?, bool IsSuccess)> GetByConditionAsync ( Expression<Func<T, bool>> criteria )
        {
            var items = await _repository.GetByConditionAsync ( criteria );
            if ( items.Count() == 0 )
                return (null, false);

            return (items, true);
        }
        public async Task<(T? entity, bool IsSuccess)> CreateAsync ( T entity )
        {
            var createdEntity = await _repository.CreateAsync ( entity );
            if ( createdEntity != null )
                return (createdEntity, true);
            return (null, false);
        }
        public async Task<(T? entity, bool IsSuccess)> CreateAsync ( T entity, HttpRequestType httpRequest, Expression<Func<T, bool>> checkCriteria )
        {
            bool IsExist = await IsExistAsync ( checkCriteria, httpRequest );

            return ( !IsExist ) ? (await _repository.CreateAsync ( entity ), true) : (null, false);
        }
        public async Task<(T, bool IsSuccess)> UpdateAsync ( T entity )
        {
            var updatedItem = await _repository.UpdateAsync ( entity );
            if ( updatedItem == null )
                return (null, false);

            return (updatedItem, true);
        }
        public async Task<(T, bool IsSuccess)> UpdateAsync ( T entity, Expression<Func<T, bool>> criteria )
        {
            var (result, IsSuccess) = await _repository.UpdateAsync ( entity, criteria );
            return (result, IsSuccess);
        }
        public async Task<int> DeleteAsync ( Expression<Func<T, bool>> criteria )
        {
            return await _repository.DeleteAsync ( criteria );
        }

        //Extra Functions for count and checking
        //=======================================

        public async Task<bool> IsExistAsync ( Expression<Func<T, bool>> criteria, HttpRequestType httpRequest )
        {
            switch ( httpRequest )
            {
                case HttpRequestType.Post:
                case HttpRequestType.Put:
                case HttpRequestType.Delete:
                case HttpRequestType.Get:
                    {
                        return await CountAsync ( criteria ) > 0 ? true : false;
                    }

                default:
                    return false;
            }
        }
        public async Task<int> CountAsync ( )
        {
            return await _repository.CountAsync ( );
        }
        public async Task<int> CountAsync ( Expression<Func<T, bool>> criteria )
        {
            return await _repository.CountAsync ( criteria );
        }
        public async Task<(IEnumerable<T>?, bool IsSuccess)> FindAsync ( Expression<Func<T, bool>> criteria )
        {
            var items = await _repository.FindAsync ( criteria );
            return ( items.Count() > 0 ) ? (items, true) : (null, false);
        }

        //Using StoredProcedures
        //=======================
        public async Task<IEnumerable<T>> GetUsingStoredProcedureAsync ( string spName, Dictionary<string, object> parameters )
        {
            return await _repository.GetUsingStoredProcedureAsync ( spName, parameters );
        }
        public async Task<int> CUDUsingStoredProcedureAsync ( string spName, Dictionary<string, object> parameters, Expression<Func<T, bool>> checkCriteria, HttpRequestType httpRequest )
        {
            bool IsExist = await IsExistAsync ( checkCriteria, httpRequest );
            return ( ( IsExist && httpRequest != HttpRequestType.Post ) || ( !IsExist && httpRequest == HttpRequestType.Post ) )
                ? await _repository.CUDUsingStoredProcedureAsync ( spName, parameters, httpRequest )
                : 0;
        }

    }
}
