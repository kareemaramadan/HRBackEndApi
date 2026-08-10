using System;
using System.Collections.Generic;
using System.Text;
using HR.Application.Interfaces;
using HR.Infrastructure.Context;

namespace HR.Infrastructure.Repository
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        public IBaseRepository<T> GetRepository { get; private set; }

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            GetRepository = new BaseRepository<T>(_dbContext);
        }

        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
