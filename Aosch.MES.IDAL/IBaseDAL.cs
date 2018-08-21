using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Aosch.MES.IDAL
{
    public   interface IBaseDAL<T> where T:class,new() 
    {
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> WhereLamd);
        IQueryable<T> LoadEntities<S>(int PageSize, int PageIndex, out int TotalCount, Expression<Func<T, bool>> WhereLamd, Expression<Func<T, S>> OrderByLamd, bool IsDESC);
        T AddEntity(T model);
        bool DeleteEntity(T model);
        bool UpdateEntity(T model);

        bool ExistEntity(T model);
    }
}
