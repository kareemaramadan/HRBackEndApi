using HR.Application.Helpers;
using HR.Application.Interfaces;
using HR.Domain.Models;
using HR.Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;


namespace HR.Infrastructure.Repository
{
 public class BaseRepository<T> : IBaseRepository<T> where T : class
 {
  protected readonly IdentityContext dbContext;
  protected readonly DbSet<T> dbSet;
  public BaseRepository ( IdentityContext _dbContext )
  {
   dbContext=_dbContext;
   dbSet=dbContext.Set<T> ();
  }


  public async Task<T> CreateAsync ( T entity )
  {
   await dbSet.AddAsync (entity);
   await dbContext.SaveChangesAsync ();
   return entity;
  }

  public async Task<int> CountAsync ( )
  {
   return await dbSet.CountAsync ();
  }

  public async Task<int> CountAsync ( Expression<Func<T,bool>> criteria )
  {
   return await dbSet.CountAsync (criteria);
  }
  public async Task<int> CUDUsingStoredProcedureAsync(string spName, Dictionary<string, object> parameters)
  {
   SqlParameter[] sqlParameters = parameters.Select(
       p => new SqlParameter(p.Key.StartsWith('@') ? p.Key : $"@{p.Key}", p.Value ?? DBNull.Value)).ToArray();
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

   return await dbContext.Database.ExecuteSqlAsync(interpolatedQuery);
  }
 

  public async Task<IEnumerable<T>> GetUsingStoredProcedureAsync ( string spName,Dictionary<string,object> parameters )
  {

   SqlParameter [ ] sqlParameters = parameters.Select (
       p => new SqlParameter (p.Key.StartsWith ('@') ? p.Key : $"@{p.Key}",p.Value??DBNull.Value)).ToArray ();

   int parametersCount = sqlParameters.Length;
   string sqlQuery = string.Empty;
   if (parametersCount>0)
   {
    string parameterNames = string.Join (", ",sqlParameters.Select (p => p.ParameterName));
    sqlQuery=$"EXEC {spName} {parameterNames}";
   }
   else
   {
    sqlQuery=$"EXEC {spName}";
   }
   FormattableString interpolatedQuery = FormattableStringFactory.Create (sqlQuery,sqlParameters);
   return await dbSet.FromSqlInterpolated (interpolatedQuery).ToListAsync ();
   return await dbSet.FromSqlRaw (sqlQuery,sqlParameters).ToListAsync ();
  }

  public async Task<int> DeleteAsync ( T entity )
  {
   dbContext.Remove (entity);
   return await dbContext.SaveChangesAsync ();
  }
  public async Task<int> DeleteAsync ( Expression<Func<T,bool>> criteria )
  {
   var entities = await dbSet.Where (criteria).ToListAsync ();
   foreach (var entity in entities)
   {
    dbSet.Remove (entity);
   }
   return await dbContext.SaveChangesAsync ();
  }

  public async Task<IEnumerable<T>> FindAsync ( Expression<Func<T,bool>> criteria )
  {
   IQueryable<T> query = dbSet.Where (criteria);
   return await query.ToListAsync ();
  }

  public async Task<T> FindItemAsync ( Expression<Func<T,bool>> criteria )
  {
   T item = dbSet.Where (criteria).ToList ().First ();
   return item;
  }

  public async Task<IEnumerable<T>> GetAllAsync ( )
  {
   return await dbSet.ToListAsync ();
  }

  public async Task<IEnumerable<T>> GetAllAsync ( Expression<Func<T,bool>> filter )
  {
   IQueryable<T> query = dbSet.Where (filter);
   return await query.ToListAsync ();
  }

  public async Task<IEnumerable<T>> GetAllAsync (Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
  {
   IQueryable<T> query = dbSet;

   if (include != null)
   {
    query = include(query);
   }

   if (filter != null)
   {
    query = query.Where(filter);
   }

   return await query.ToListAsync();
  }

  public async Task<T> UpdateAsync ( T entity,Expression<Func<T,bool>> criteria )
  {
   var existingEntity = await dbSet.FirstOrDefaultAsync (criteria)??throw new NotImplementedException ("the item is not found");
   dbContext.Update (entity);
   await dbContext.SaveChangesAsync ();
   return entity;
  }

  public async Task<T> UpdateAsync ( T entity )
  {
   dbContext.Update (entity);
   await dbContext.SaveChangesAsync ();
   return entity;
  }
 }
}

