using HR.Application.Interfaces;
using HR.Domain.Models;
using HR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using HR.Application.Helpers;


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

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _Dbcontext.SaveChangesAsync();
            return entity;
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
        {
            return await _dbSet.CountAsync(criteria);
        }

        public async Task<object> CRUDUsingStoredProcedureAsync(string spName, Dictionary<string, object> parameters, HttpRequestType httpRequest)
        {
            SqlParameter[] sqlParameters = parameters.Select(
                p => new SqlParameter(p.Key.StartsWith("@") ? p.Key : $"@{p.Key}", p.Value ?? DBNull.Value)).ToArray();
            string parameterNames = string.Join(", ", sqlParameters.Select(p => p.ParameterName));

            int parametersCount = sqlParameters.Length;
            string sqlQuery = string.Empty;
            if (parametersCount > 0)
            {
                sqlQuery = $"EXEC {spName} {parameterNames}";
            }
            else
            {
                sqlQuery = $"EXEC {spName}";
            }

            FormattableString interpolatedQuery = FormattableStringFactory.Create(sqlQuery, sqlParameters);

            switch(httpRequest)
            {
                case HttpRequestType.Get:

                    return await _Dbcontext.Database.SqlQuery<T>(interpolatedQuery).ToListAsync();

                case HttpRequestType.Post:
                case HttpRequestType.Put:
                case HttpRequestType.Delete:
                    return await _Dbcontext.Database.ExecuteSqlAsync(interpolatedQuery);

                default:
                    throw new ArgumentException("Invalid HTTP request type for CRUD operations.");
            }
        }

        public async Task DeleteByConditionAsync(Expression<Func<T, bool>> criteria)
        {
            var entities = await _dbSet.Where(criteria).ToListAsync();
            foreach (var entity in entities)
            {
                _dbSet.Remove(entity);
            }
            await _Dbcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> criteria)
        {
           IQueryable<T> query = _dbSet.Where(criteria);
           return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = _dbSet.Where(criteria);
            return await query.ToListAsync();
        }

        public void UpdateByConditionAsync(T entity, Expression<Func<T, bool>> criteria)
        {
            var existingEntity = _dbSet.FirstOrDefaultAsync(criteria);
            if (existingEntity != null)
            {
                _Dbcontext.Entry(existingEntity).CurrentValues.SetValues(entity);
                _Dbcontext.SaveChangesAsync();
            }
        }
    }
}
