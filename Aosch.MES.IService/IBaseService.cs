using Aosch.MES.IDAL;
using System;
using System.Linq;
using System.Linq.Expressions;
namespace Aosch.MES.IService
{
    public interface IBaseService<T> where T:class,new()
    {
        IDBSession CurrentDBSession { get; }
        Aosch.MES.IDAL.IBaseDAL<T> CurrentDAL { set; get; }
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> WhereLamd);
        IQueryable<T> LoadEntities<S>(int PageSize, int PageIndex, out int TotalCount, Expression<Func<T, bool>> WhereLamd, Expression<Func<T, S>> OrderByLamd, bool IsDESC);
        T AddEntity(T model);
        bool DeleteEntity(T model);
        bool UpdateEntity(T model);

        bool ExistEntity(int ID);
    }
}
