using System;
using System.Collections.Generic;
using System.Text;


namespace HR.Application.Interfaces
{
    public interface IUnitOfWork<T>:IDisposable where T : class
    {
       IBaseRepository<T> GetRepository {  get; }

        int Complete();
    }
}
