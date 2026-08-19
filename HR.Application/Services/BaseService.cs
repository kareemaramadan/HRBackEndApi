using HR.Application.Interfaces;
using HR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Services
{
    public class BaseService<T>(IBaseRepository<T> repository) : IBaseService<T>
        where T : class
    {

        private readonly IBaseRepository<T> _repository = repository;

        public async Task<T> CreateAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return entity;
        }

        public async Task<int> CUDUsingStoredProcedureAsync(string spName, Dictionary<string, object> parameters)
        {
           return await _repository.CUDUsingStoredProcedureAsync(spName, parameters);
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<List<T>> GetStoredProcedureAsync(string spName)
        {
            return await _repository.GetStoredProcedureAsync(spName);
        }
    }
}
