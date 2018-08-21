
using Aosch.MES.DALFactory;
using Aosch.MES.IDAL;
using System;
using System.Linq;
using System.Linq.Expressions;
namespace Aosch.MES.Service
{
    public abstract class BaseService<T> where T : class, new()
    {
        public IDBSession CurrentDBSession { get { return DBSessionFactory.CreateDBSession(); } }
        public IDAL.IBaseDAL<T> CurrentDAL { set; get; }
        public abstract void SetCurrentDAL();//子类如果要使用或者继承此类一定要实现此方法
        public BaseService()
        {
            SetCurrentDAL();
        }
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> WhereLamd)
        {
            return CurrentDAL.LoadEntities(WhereLamd);
        }
        public IQueryable<T> LoadEntities<S>(int PageSize, int PageIndex, out int TotalCount, Expression<Func<T, bool>> WhereLamd, Expression<Func<T, S>> OrderByLamd, bool IsDESC)
        {
            return CurrentDAL.LoadEntities(PageSize, PageIndex, out TotalCount, WhereLamd, OrderByLamd, IsDESC);
        }

       public   T AddEntity(T model)
        {
             CurrentDAL.AddEntity(model);
             if (CurrentDBSession.SaveChanges())
             {
                 return model;
             }
             return null;
        }

        public bool DeleteEntity(T model)
        {
            CurrentDAL.DeleteEntity(model);
            return CurrentDBSession.SaveChanges();
        }

        public bool UpdateEntity(T model)
        {
            CurrentDAL.UpdateEntity(model);
            return CurrentDBSession.SaveChanges();
        }
    }
}
