
using Aosch.MES.Model;
using System.Collections.Generic;

namespace Aosch.MES.IService
{
    public interface IAccountService:IBaseService<Account>
    {
         Account Login(string Username, string Password);
    }
    public interface IRoleService : IBaseService<Role>
    {
        //导航菜单
        List<ActionURLViewModel> GetNavigation(string AccountUsername);


        //ActionURl列表
        List<ActionURL> GetActionURLs(string Filter="");
        /// <summary>
        /// 删除角色对应的所有URL
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        bool DeleteRolePermissions(int RoleID);

        bool InsertRolePermissions(int RoleID, int ActionURLID);

        List<int> GetAllTopActionURLIDS();

        string GetAllOptionHTML(int CurrentRoleID);
    }

    public interface ILogService : IBaseService<Log> { }
    public interface IEmployeeService : IBaseService<Employee> { }


   
}
