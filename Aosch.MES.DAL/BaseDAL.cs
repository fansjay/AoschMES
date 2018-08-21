using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Aosch.MES.DAL
{
    public class BaseDAL<T> where T:class,new()
    {
       public   DbContext dbcontext = DBContextFactory.CreateDBContext();
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> WhereLamd)
        {
            return dbcontext.Set<T>().Where(WhereLamd);
        }
        public IQueryable<T> LoadEntities<S>(int PageSize, int PageIndex, out int TotalCount, Expression<Func<T, bool>> WhereLamd, Expression<Func<T, S>> OrderByLamd, bool IsDESC)
        {
            var TempList = dbcontext.Set<T>().Where(WhereLamd);
            TotalCount = TempList.Count();
            if (IsDESC)//降序
            {
                //TempList = TempList.OrderByDescending<T, S>(OrderByLamd).Skip<T>(PageIndex * (PageSize - 1)).Take<T>(PageSize);
                TempList = TempList.OrderByDescending<T, S>(OrderByLamd).Skip<T>(PageSize).Take<T>(PageIndex);
            }
            else
            {
                //TempList = TempList.OrderBy<T,S>(OrderByLamd).Skip<T>(PageIndex * (PageSize - 1)).Take<T>(PageSize);
                TempList = TempList.OrderBy<T, S>(OrderByLamd).Skip<T>(PageSize).Take<T>(PageIndex);
            }
            return TempList;

        }

        public T AddEntity(T model)
        {
            dbcontext.Set<T>().Add(model);
            return model;
        }

        public bool DeleteEntity(T model)
        {
            dbcontext.Entry<T>(model).State = EntityState.Deleted;
            return true;
           // return dbcontext.SaveChanges()>0;
        }

        public bool UpdateEntity(T model)
        {
            dbcontext.Entry<T>(model).State = EntityState.Modified;
            return true;
            //return dbcontext.SaveChanges() > 0;
        }

        public bool ExistEntity(T model)
        {
            dbcontext.Entry<T>(model).State = EntityState.Modified;
            return true;
        }
    }
}
