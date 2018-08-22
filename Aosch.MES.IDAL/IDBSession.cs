using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Aosch.MES.IDAL
{
    public interface IDBSession
    {
        DbContext dbcontext { get; }

        bool SaveChanges();
        int ExcuteSQL(string sql, params SqlParameter[] pars);

        List<T> ExcuteQuery<T>(string sql, params SqlParameter[] pars);



        IAccountDAL AccountDal { set; get; }
        IRoleDAL RoleDal { set; get; }
        //IWareHouseDAL WareHouseDal { set; get; }
        //IAccountRole_MappingDAL AccountRole_MappingDal{set;get;}
        //IDepartmentDAL DepartmentDal { set; get; }
        ILogDAL LogDal { set; get; }
        IEmployeeDAL EmployeeDal { set; get; }
    }
}
