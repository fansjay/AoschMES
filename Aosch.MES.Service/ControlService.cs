using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Aosch.MES.IService;
using Aosch.MES.Model;

namespace Aosch.MES.Service
{
    public class AccountService : BaseService<Account>, IAccountService
    {
        public bool ExistEntity(int ID)
        {
            return CurrentDBSession.AccountDal.LoadEntities(a => a.ID ==ID).Count() > 0;
        }

        public Account Login(string Username, string Password)
        {
            Account account = new Account();
            var accounts= CurrentDBSession.AccountDal.LoadEntities(a => a.Username == Username && a.Password == Password&&a.Status);
            foreach (var item in accounts)
            {
                if (item != null)
                {
                    return item;
                }
            }
            return null;

        }

        #region 特征方法（特有的方法）

        #endregion
        public override void SetCurrentDAL()
        {
            CurrentDAL = this.CurrentDBSession.AccountDal;
        }

        public bool DeleteEntityByID(int ID)
        {
           return CurrentDBSession.ExcuteSQL("DELETE Account WHERE ID=@ID", new SqlParameter("@ID", ID)) > 0;
        }
    }

    public class RoleService : BaseService<Role>, IRoleService
    {
        public bool ExistEntity(int  ID)
        {
            return CurrentDBSession.RoleDal.LoadEntities(a => a.ID == ID).Count() > 0;
        }

        public List<ActionURL> GetActionURLs(string Filter="")
        {
            string SQLString = "SELECT * FROM ActionURL"+(string.IsNullOrEmpty(Filter)?"":" WHERE "+Filter);
            return CurrentDBSession.ExcuteQuery<ActionURL>(SQLString);
        }

        public bool DeleteRolePermissions(int RoleID)
        {
            string SQLString = "DELETE RoleActionURL_Mapping WHERE RoleID=@RoleID";
            return CurrentDBSession.ExcuteSQL(SQLString, new System.Data.SqlClient.SqlParameter("@RoleID", RoleID))>0;
        }


        public List<ActionURLViewModel> GetNavigation(string AccountUsername)
        {
            string SQLString = "SELECT * FROM ActionURL WHERE ID IN (SELECT ActionURLID FROM RoleActionURL_Mapping WHERE RoleID=(select RoleID from Account where Username=@Username )) AND Showable=1";
            var Lists = CurrentDBSession.ExcuteQuery<ActionURLViewModel>(SQLString, new System.Data.SqlClient.SqlParameter("@Username", AccountUsername));




            var MenuLists = new List<ActionURLViewModel>();
            //RootLists:
            //1   系统设置        0       fa fa-flash 1   1   1
            //5   员工管理        0       fa fa-flash 2   1   1

            var RootLists = Lists.FindAll(a => a.ParentID == 0&&a.IsRoot==true);
            foreach (var FirstLevelitem in RootLists)
            {
                var FirstLevelMenu = new ActionURLViewModel();
                FirstLevelMenu.ID = FirstLevelitem.ID;
                FirstLevelMenu.MenuName = FirstLevelitem.MenuName;
                FirstLevelMenu.Icon = FirstLevelitem.Icon;
                FirstLevelMenu.URL = FirstLevelitem.URL;

                //含有子菜单(2级菜单)
                var SecondLevelMenuLists = Lists.FindAll(a => a.ParentID == FirstLevelMenu.ID&&a.IsRoot==true);
                FirstLevelMenu.Childrens = SecondLevelMenuLists;

                foreach (var Seconditem in SecondLevelMenuLists)
                {
                    var SecondLevelMenu = new ActionURLViewModel();
                    SecondLevelMenu.ID = Seconditem.ID;
                    SecondLevelMenu.MenuName = Seconditem.MenuName;
                    SecondLevelMenu.Icon = Seconditem.Icon;
                    SecondLevelMenu.URL = Seconditem.URL;

                    //含有子菜单三级菜单
                    var ThirdLevelMenuLists= Lists.FindAll(a => a.ParentID == SecondLevelMenu.ID&&a.IsRoot==false);
                  

                    if (ThirdLevelMenuLists.Count>0)
                    {
                        foreach (var Thirditem in ThirdLevelMenuLists)
                        {
                            var ThirdLevelMenu = new ActionURLViewModel();
                            ThirdLevelMenu.ID = Thirditem.ID;
                            ThirdLevelMenu.MenuName = Thirditem.MenuName;
                            ThirdLevelMenu.Icon = Thirditem.Icon;
                            ThirdLevelMenu.URL = Thirditem.URL;
                        }
                        Seconditem.Childrens = ThirdLevelMenuLists;

                    }
                }

                MenuLists.Add(FirstLevelMenu);
            }
             
            return MenuLists;
        }
        #region 特征方法（特有的方法）

        #endregion
        public override void SetCurrentDAL()
        {
            CurrentDAL = this.CurrentDBSession.RoleDal;
        }


        /// <summary>
        /// 添加角色权限对应
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="ActionURLID"></param>
        /// <returns></returns>
        public bool InsertRolePermissions(int RoleID, int ActionURLID)
        {
            string SQLString = "INSERT INTO RoleActionURL_Mapping(RoleID,ActionURLID) VALUES(@RoleID,@ActionURLID)";
            
            return CurrentDBSession.ExcuteSQL(SQLString, new SqlParameter[] { new SqlParameter("@RoleID", RoleID), new SqlParameter("@ActionURLID", ActionURLID) }) > 0;
        }

        public List<int> GetAllTopActionURLIDS()
        {
            string SQLString = "SELECT ID FROM ActionURL WHERE ParentID=0";
            return CurrentDBSession.ExcuteQuery<int>(SQLString);
        }

        public string GetAllOptionHTML(int CurrentRoleID)
        {
            var HtmlString = "";
#if false
              <option value="2" data-section="系统设置" selected="selected">主题设置</option>
              <option value="3" data-section="系统设置">日志管理</option>
              <option value="4" data-section="系统设置">角色权限</option>


              <option value="6" data-section="系统设置/日志管理">系统日志</option>
              <option value="7" data-section="系统设置/日志管理">操作日志</option>
              <option value="8" data-section="员工管理" selected="selected">员工添加</option>
#endif
            string SQLString = "SELECT * FROM ActionURL ";
            var ActionURLs= CurrentDBSession.ExcuteQuery<ActionURL>(SQLString);
            foreach (var item in ActionURLs)
            {
                //是否选中
                string str = $"SELECT * FROM RoleActionURL_Mapping WHERE RoleID={CurrentRoleID} AND ActionURLID ={item.ID} ";
                string selected = CurrentDBSession.ExcuteQuery<RoleActionURL_Mapping>(str).Count > 0 ? "selected=\"selected\"" : "";

                //二级菜单
                if (item.ParentID!= 0&&item.IsRoot&&item.Showable)
                {
                    //根菜单名称
                    var RootName = ActionURLs.Where(a => a.ID == item.ParentID).FirstOrDefault().MenuName;

                    HtmlString += $" <option value=\"{item.ID}\" data-section=\"{RootName}\" {selected}>{item.MenuName}</option>\n";
                }
                else if(item.ParentID!=0&&item.IsRoot==false&&item.Showable) //三级菜单
                {
                    //--------------------------------------------------系统设置------------------------------------------------------------------------日志管理--------------------------------------
                    var RootName = ActionURLs.Where(a => a.ID == (ActionURLs.Where(B => B.ID == item.ParentID).FirstOrDefault().ParentID)).FirstOrDefault().MenuName + "/" + ActionURLs.Where(a => a.ID == item.ParentID).FirstOrDefault().MenuName;
                    HtmlString += $" <option value=\"{item.ID}\" data-section=\"{RootName}\" {selected}>{item.MenuName}</option>\n";
                }
            }
            return HtmlString;
        }
    }

    public class LogService : BaseService<Log>, ILogService
    {
        public bool ExistEntity(int ID)
        {
            return CurrentDBSession.LogDal.LoadEntities(a => a.ID == ID).Count() > 0;
        }

        public override void SetCurrentDAL()
        {
            CurrentDAL = this.CurrentDBSession.LogDal;
        }
    }

    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        public bool DeleteEmployeeByID(int EmpID)
        {
           return  CurrentDBSession.ExcuteSQL("Delete Employee where ID=@ID", new SqlParameter("@ID", EmpID))>0;
        }

        public bool ExistEntity(int ID)
        {
            return CurrentDBSession.EmployeeDal.LoadEntities(a => a.ID == ID).Count() > 0;
        }

        public Department GetDepartment(int ID)
        {
          return   CurrentDBSession.ExcuteQuery<Department>($"SELECT * FROM Department WHERE ID=@ID", new SqlParameter("@ID", ID)).FirstOrDefault();
        }

        public List<Department> GetDepartments(bool IncludeDisabled=true)
        {

            string Sqlstring = "";
            if (IncludeDisabled)
            {
                Sqlstring = "SELECT * FROM Department";
            }
            else
            {
                Sqlstring = "select * from Department where Status=1";
            }
            return CurrentDBSession.ExcuteQuery<Department>(Sqlstring);
        }

        public List<Position> GetPositions(bool IncludeDisabled = true)
        {
            string Sqlstring = "";
            if (IncludeDisabled)
            {
                Sqlstring = "SELECT * FROM Position";
            }
            else
            {
                Sqlstring = "select * from Position where Status=1";
            }
            return CurrentDBSession.ExcuteQuery<Position>(Sqlstring);
        }

        public override void SetCurrentDAL()
        {
            CurrentDAL = this.CurrentDBSession.EmployeeDal;
        }

     
    }
}
