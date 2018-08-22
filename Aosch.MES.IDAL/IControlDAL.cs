

using Aosch.MES.Model;
using System.Collections.Generic;

namespace Aosch.MES.IDAL
{
    public  interface IAccountDAL:IBaseDAL<Account>
    {
        //这里面实现特有的方法
     
    }
    public interface IRoleDAL : IBaseDAL<Role>
    {
    }

    public interface ILogDAL : IBaseDAL<Log> { }
    public interface IEmployeeDAL : IBaseDAL<Employee> { }


}
