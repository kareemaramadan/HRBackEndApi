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
    public class BaseService<T>(IBaseRepository<T> repository) : IBaseService<T>
        where T : class
    {

        private readonly IBaseRepository<T> _repository = repository;

        public async Task<int> CountAsync()
        {
           return await _repository.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
        {
            return await _repository.CountAsync(criteria);
        }

        public async Task<T> CreateAsync(T entity)
        {
            return await _repository.CreateAsync(entity);
        }

        public async Task<T> CreateAsync(T entity, HttpRequestType httpRequest, Expression<Func<T, bool>> checkCriteria)
        {
            bool IsExist = await IsExistAsync(checkCriteria, httpRequest);

            return (!IsExist) ? await _repository.CreateAsync(entity) : throw new ArgumentException("This item is already exists.");
        }





        public async Task<IEnumerable<T>> GetUsingStoredProcedureAsync(string spName, Dictionary<string, object> parameters)
        {
            return await _repository.GetUsingStoredProcedureAsync(spName, parameters);
        }

        public async Task<int> CUDUsingStoredProcedureAsync(string spName, Dictionary<string, object> parameters,Expression<Func<T, bool>> checkCriteria, HttpRequestType httpRequest)
        {
            bool IsExist = await IsExistAsync(checkCriteria, httpRequest);
            return ((IsExist && httpRequest != HttpRequestType.Post) || (!IsExist && httpRequest == HttpRequestType.Post))
                ? await _repository.CUDUsingStoredProcedureAsync(spName, parameters, httpRequest)
                : 0;
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> criteria)
        {
           await _repository.DeleteAsync(criteria);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> criteria)
        {
           return await _repository.FindAsync(criteria);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> criteria)
        {
            return await _repository.GetAsync(criteria);
        }

        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> criteria, HttpRequestType httpRequest)
        {
            switch (httpRequest)
            {
                case HttpRequestType.Post:
                case HttpRequestType.Put:
                case HttpRequestType.Delete:
                case HttpRequestType.Get:
                    {
                        return await CountAsync(criteria) > 0 ? true : false;
                    }

                default:
                    throw new ArgumentException("There is no data for this criteria.");
            }
        }

        public async Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> criteria)
        {
            return await _repository.UpdateAsync(entity, criteria);
        }

    }
}
