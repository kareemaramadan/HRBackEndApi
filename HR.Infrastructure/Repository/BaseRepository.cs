using HR.Application.Interfaces;
using HR.Domain.Models;
using HR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HR.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _Dbcontext;
        protected readonly DbSet<T> _dbSet;
        public BaseRepository(AppDbContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
            _dbSet = _Dbcontext.Set<T>();   
        }

        public async Task AddAsync(T entity)
        {
           await _dbSet.AddAsync(entity);
        }

        public Task<bool> SaveChangesAsync()
        {
            return _Dbcontext.SaveChangesAsync().ContinueWith(task => task.Result > 0);
        }

        //public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        //{
        //    await _Dbcontext.Set<T>().AddRangeAsync(entities);
        //    await _Dbcontext.SaveChangesAsync();
        //    return entities;
        //}

        //public void DeleteAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public void DeleteRangeAsync(IEnumerable<T> entities)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<T> FindAsync(Expression<Func<T, bool>> predicate, string[]? includes = null)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IEnumerable<T>> GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<T> GetByIdAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<T> GetByNameAsync(Expression<Func<T, bool>> predicate)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UpdateAsync(T entity)
        //{
        //    throw new NotImplementedException();
        //}
    }

}
