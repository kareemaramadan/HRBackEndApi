using HR.Application.Interfaces;
using HR.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HR.Infrastructure.Repository
{
    internal class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _Dbcontext;
        public BaseRepository(AppDbContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _Dbcontext.Set<T>().AddAsync(entity);
            await _Dbcontext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _Dbcontext.Set<T>().AddRangeAsync(entities);
            await _Dbcontext.SaveChangesAsync();
            return entities;
        }

        public void DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindAsync(Expression<Func<T, bool>> predicate, string[]? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByNameAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }

}
