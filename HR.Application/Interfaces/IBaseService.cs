using HR.Application.Dtos.LookUpDtos.Country;
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
        //Task<TDto> GetByNameAsync(Expression<Func<TDto, bool>> predicate);
        //Task<TDto> GetByIdAsync(int id);
        //Task<IEnumerable<TDto>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        //Task<IEnumerable<TDto>> AddRangeAsync(IEnumerable<TDto> entities);
        //Task<TDto> UpdateAsync(TDto entity);
        //void DeleteAsync(int id);
        //void DeleteRangeAsync(IEnumerable<TDto> entities);
        //Task<TDto> FindAsync(Expression<Func<TDto, bool>> predicate, string[]? includes = null);
    }
}
