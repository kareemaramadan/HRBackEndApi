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
    }
}
