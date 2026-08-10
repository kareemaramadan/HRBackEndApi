using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Interfaces
{
    internal interface IBaseService<T, TDto> where T : class where TDto : class
    {

    }
}
