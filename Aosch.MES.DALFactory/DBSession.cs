using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Diagnostics;
using Aosch.MES.IDAL;
using Aosch.MES.DAL;
using System.Data.Entity;

namespace Aosch.MES.DALFactory
{
    public class DBSession:IDBSession
    {

        private IAccountDAL _AccountDal;
        public IAccountDAL AccountDal
        {
            set { _AccountDal = value; }
            get
            {
                if (_AccountDal == null)
                {
                    _AccountDal = AbstractFactory.CreateAccountDAL();
                }
                return _AccountDal;
            }

        }
        private IRoleDAL _RoleDal;
        public IRoleDAL RoleDal
        {
            set { _RoleDal = value; }
            get
            {
                if (_RoleDal == null)
                {
                    _RoleDal = AbstractFactory.CreateRoleDAL();
                }
                return _RoleDal;
            }

        }
       

        private ILogDAL _LogDal;
        public ILogDAL LogDal
        {
            set { _LogDal = value; }
            get
            {
                if (_LogDal == null)
                {
                    _LogDal = AbstractFactory.CreateLogDAL();
                }
                return _LogDal;
            }
        }



        private IEmployeeDAL _EmployeeDal;
        public IEmployeeDAL EmployeeDal
        {
            set { _EmployeeDal = value; }
            get
            {
                if (_EmployeeDal == null)
                {
                    _EmployeeDal = AbstractFactory.CreateEmployeeDAL();
                }
                return _EmployeeDal;
            }
        }




        //private IDepartmentDAL _DepartmentDal;
        //public IDepartmentDAL DepartmentDal
        //{
        //    set { _DepartmentDal = value; }
        //    get
        //    {
        //        if (_DepartmentDal == null)
        //        {
        //            _DepartmentDal = AbstractFactory.CreateDepartmentDAL();
        //        }
        //        return _DepartmentDal;
        //    }

        //}




        public List<T> ExcuteQuery<T>(string sql, params SqlParameter[] pars)
        {
            return dbcontext.Database.SqlQuery<T>(sql, pars).ToList<T>();
        }
      

        public int ExcuteSQL(string sql, params SqlParameter[] pars)
        {
            return dbcontext.Database.ExecuteSqlCommand(sql, pars);
        }
        public DbContext dbcontext
        {
            get { return DBContextFactory.CreateDBContext(); }
        }

       

        public bool SaveChanges()
        {
            try
            {
                return dbcontext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            
        }
    }
}
