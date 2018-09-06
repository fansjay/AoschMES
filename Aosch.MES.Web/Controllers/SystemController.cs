using Aosch.MES.Common;
using Aosch.MES.Model;
using Aosch.MES.Service;
using Aosch.MES.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Aosch.MES.Web.Controllers
{

    [LoginFilterAcctribute]
    public class SystemController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private RoleService roleService = new RoleService();
        private LogService logService = new LogService();
        private EmployeeService employeeService = new EmployeeService();
        private AccountService accountService = new AccountService();
        // GET: System
        public ActionResult Index()
        {
            logger.Info($"用户【{CookieHelper.GetCurrentAccount().Username}】:登入主系统！");
            return View();
        }


        public ActionResult Roles()
        {
            ViewBag.RoleLevelList = new List<SelectListItem>() {new SelectListItem() { Value = "10", Text = "超级管理员" },new SelectListItem() { Value = "100", Text = "系统管理" }, new SelectListItem() { Value = "200", Text = "公司管理" },new SelectListItem() { Value = "300", Text = "普通员工",Selected=true } };
            
            return View();
        }

     

        #region 系统用户管理 
        public ActionResult Account()
        {
            //角色
            var Roles = roleService.LoadEntities(a => a.ID > 0).ToList();
            var rolelist = new List<SelectListItem>();
            foreach (var item in Roles)
            {
                rolelist.Add(new SelectListItem() { Value = item.ID.ToString(), Text = item.RoleName });
            }
            //ViewBag.RoleSelectList = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "禁用" }, new SelectListItem() { Value = "1", Text = "启用", Selected = true } };
            ViewBag.RoleSelectList = rolelist;

            //帐号
            var usernames = employeeService.LoadEntities(a => a.ID > 0).ToList();
            var usernamelist = new List<SelectListItem>();
            foreach (var item in usernames)
            {
                usernamelist.Add(new SelectListItem() {Text=item.NickName,Value=item.EmployeeName });
            }
            ViewBag.EmployeeSelectList = usernamelist;

            //是否启用
           ViewBag.StatusSelectList = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "禁用" }, new SelectListItem() { Value = "1", Text = "启用", Selected = true } };


            return View();
        }

        public ActionResult GetAllAccountJson(int? limit, int? offset, string order)
        {
            if (string.IsNullOrEmpty(order)) order = "asc";
            int TotalCount = 0;
            var AllAccounts = accountService.LoadEntities((int)offset, (int)limit, out TotalCount, (r => r.ID > 0), (r => new { r.ID }), "asc".Equals(order) ? false : true);
            return Json(new { total = TotalCount, rows = AllAccounts }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddAccount([Bind(Exclude = "Repassword")] Account account)
        {
            Regex PasswordRegex = new Regex("^[a-zA-Z0-9]{6,12}$");


            //密码不为空
            if (!string.IsNullOrEmpty(account.Password))
            {
                if (!PasswordRegex.IsMatch(account.Password))
                {
                    return Json(new { Status = "ERROR", Message = "密码必须为6~12有效字母和数字组合" });
                }
            }
            else
            {
                account.Password = "123456";
            }
            account.Password =EncryptUtil.Base64(account.Password);
            account.HomePage = account.HomePage ?? "";
            var no = 0;
            int.TryParse(Request["ICnumber"], out no);
            account.ICNumber = accountService.LoadEntities(a => a.ID > 0).OrderByDescending(a => a.ICNumber).First().ICNumber + 1;
            account.Description = account.Description ?? "";
            account.RegisterTime = DateTime.Now;
            account.EmployeeID = employeeService.LoadEntities(e => e.EmployeeName ==account.Username).FirstOrDefault().ID;           
            if (accountService.AddEntity(account) != null)
            {
                var emp = employeeService.LoadEntities(a => a.EmployeeName == account.Username).FirstOrDefault();
                var role = roleService.LoadEntities(a => a.ID == account.RoleID).FirstOrDefault();
                logger.Warn($"用户【{CookieHelper.GetCurrentAccount().Username}】 添加系统帐号为{account.Username},姓名为{emp.NickName} 角色为{role.RoleName}，的{employeeService.GetDepartment(emp.DepartmentID).DepartmentName} 的系统帐号！\n");
                return Json(new { Status = "OK" });
            }
         
            return Json(new { Status = "ERROR" });
        }

        [HttpPost]
        [HandlerPermission(false)]
        public ActionResult DeleteAccount(int ID)
        {
            if (accountService.ExistEntity(ID))
            {
                if (accountService.DeleteEntityByID(ID))
                {
                    logger.Info($"用户{CookieHelper.GetCurrentAccount().Username}：删除了ID为{ID}的系统登录帐户！");
                    return Json(new { Status = "OK" });
                }
                else
                {
                    return Json(new { Status = "ERROR", Message = "帐户删除失败，请刷新后重新尝试！" });
                }
            }

            return Json(new { Status = "ERROR",Message="帐户已经删除，或者不存在。请刷新后重新尝试！" });
        }

        public ActionResult UpdateAccount(Account AccountModel)
        {

            AccountModel.Description = AccountModel.Description ?? "";
            AccountModel.HomePage = AccountModel.HomePage ?? "";
            if (ModelState.IsValid)
            {
                if (accountService.UpdateEntity(AccountModel))
                {
                    logger.Info($"用户{CookieHelper.GetCurrentAccount().Username}：更新了{AccountModel.Username}的个人信息！");
                    return Json(new { Status = "OK" });
                }
                else
                {
                    return Json(new { Status = "ERROR", Message = "帐户更新失败，请刷新后重新尝试！" });
                }
            }
            return Json(new { Status = "ERROR", Message = "帐户已经删除，或者不存在。请刷新后重新尝试！" });
        }

        #endregion

        #region 角色管理
        [HttpPost]
        public ActionResult AddRole()
        {
            var AddRole = new Role();
            AddRole.RoleLevel = Request["RoleLevel"] == null ? 0 : int.Parse(Request["RoleLevel"]);
            AddRole.RoleName = string.IsNullOrEmpty(Request["RoleName"]) ? "" : Request["RoleName"];
            AddRole.Description = string.IsNullOrEmpty(Request["Description"]) ? "" : Request["Description"];

            var ResultRole = roleService.AddEntity(AddRole);
            if (ResultRole != null)
            {
                logger.Info($"用户【{CookieHelper.GetCurrentAccount().Username}】：成功添加了角色ID为{AddRole.ID},角色名为:{AddRole.RoleLevel}的角色！");
                return Json(new { Status = "OK", RoleModel = AddRole });
            }
            return Json(new { Status = "ERROR" });
        }

        [HttpPost]
        public ActionResult UpdateRole(Role RoleModel)
        {
            RoleModel.Description = string.IsNullOrEmpty(RoleModel.Description) ? "" : RoleModel.Description;
            if (ModelState.IsValid)
            {
                if (roleService.UpdateEntity(RoleModel))
                {
                    logger.Info($"用户【{CookieHelper.GetCurrentAccount().Username}】：修改了角色ID为{RoleModel.ID},角色名为:{RoleModel.RoleLevel}的角色！");
                    return Json(new { Status = "OK" });
                }
            }
            return Json(new { Status = "ERROR" });
        }

        [HttpPost]
        [HandlerPermission(false)]
        public ActionResult DeleteRole(int ID)
        {
            if (roleService.ExistEntity(ID))
            {
                var role = roleService.LoadEntities(a => a.ID == ID).FirstOrDefault();
                if (roleService.DeleteEntity(role))
                {
                    //LoggerHelper.Log(Server.MapPath($"/Log/{DateTime.Now.ToString("yyyyMMdd")}.log"), LogType.Warning, $"用户 删除角色ID为{role.ID}角色名称为{role.RoleName}角色等级为{role.RoleLevel}的角色！\n");
                    logger.Warn($"用户 【{CookieHelper.GetCurrentAccount().Username}】 删除角色ID为{role.ID}角色名称为{role.RoleName}角色等级为{role.RoleLevel}的角色！\n");
                    return Json(new { Status = "OK" });
                }
            }
            return Json(new { Status = "Error" });
        }


        public ActionResult GetAllRoleJson(int? limit, int? offset, string order)
        {
            if (string.IsNullOrEmpty(order)) order = "asc";
            int TotalCount = 0;
            var AllRole = roleService.LoadEntities((int)offset, (int)limit, out TotalCount, (r => r.ID > 0), (r => new { r.ID }), "asc".Equals(order) ? false : true);
            return Json(new { total = TotalCount, rows = AllRole }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllRoleSelectJson()
        {
            return Json(roleService.LoadEntities(a => a.ID > 0).ToList());
        }


        /// <summary>
        /// 加载权限action列表 【ActionURL表】
        /// </summary>
        /// <returns></returns>

        public ActionResult GetRolePermissionListJson()
        {
            var list = roleService.GetActionURLs();
            return Json(list);
        }

        /// <summary>
        /// 为角色分配权限
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerPermission(false)]
        public ActionResult AssignPermissions(int RoleID, int[] ActionIDs)
        {
            var IDsList = ActionIDs.ToList();
            IDsList.AddRange(roleService.GetAllTopActionURLIDS());
            ActionIDs = IDsList.ToArray();
            int InsertCount = ActionIDs.Length;
            int InsertedCount = 0;
            if (roleService.ExistEntity(RoleID))
            {
                //删除以前的权限映射
                roleService.DeleteRolePermissions(RoleID);
                foreach (int ActionURLID in ActionIDs)
                {
                    if (roleService.InsertRolePermissions(RoleID, ActionURLID))
                    {
                        InsertedCount++;
                    }
                }
            }
            if (InsertedCount == InsertCount)
            {
                logger.Warn($"用户 【{CookieHelper.GetCurrentAccount().Username}】给角色ID为{RoleID}的角色分配了权限");
                return Json(new { Status = "OK" });
            }
            return Json(new { Status = "ERROR", ErrorMessage = $"计划添加权限{InsertCount}个，实际添加{InsertedCount}个" });
        }


        [HttpPost]
        public ActionResult GetParentIDs()
        {
            return Json(roleService.GetAllTopActionURLIDS());
        }
        public ActionResult GetOptionHtml(int CurrentRoleID)
        {
            return Content(roleService.GetAllOptionHTML(CurrentRoleID));
        }

        #endregion


        #region 日志管理
        public ActionResult Logs()
        {
            return View();
        }

        public ActionResult GetAllLogJson(int? limit, int? offset, string order)
        {
            if (string.IsNullOrEmpty(order)) order = "asc";
            int TotalCount = 0;
            var AllLogs = logService.LoadEntities((int)offset, (int)limit, out TotalCount, (r => r.ID > 0), (r => new { r.ID }), "asc".Equals(order) ? false : true);
            return Json(new { total = TotalCount, rows = AllLogs }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [HandlerPermission(false)]
        public ActionResult DeleteLog(int ID)
        {
            if (logService.ExistEntity(ID))
            {
                var log = logService.LoadEntities(a => a.ID == ID).FirstOrDefault();
                if (logService.DeleteEntity(log))
                {
                    logger.Warn($"用户{CookieHelper.GetCurrentAccount().Username}-> 删除日志ID为{log.ID},日志内容为{log.Message}，的{log.Level}类型日志！\n");
                    return Json(new { Status = "OK" });
                }
            }
            return Json(new { Status = "Error" });
        }
        [HttpPost]
        [HandlerPermission(false)]
        public ActionResult DeleteLogs(int [] IDs)
        {
            int IDSCount = IDs.Length;
            int DeletedIDCount = 0;
            foreach (var ID in IDs)
            {
                if (logService.ExistEntity(ID))
                {
                    var log = logService.LoadEntities(a => a.ID == ID).FirstOrDefault();
                    if (logService.DeleteEntity(log))
                    {
                        logger.Warn($"用户{CookieHelper.GetCurrentAccount().Username}-> 删除日志ID为{log.ID},日志内容为{log.Message}，的{log.Level}类型日志！\n");
                        DeletedIDCount++;
                    }
                }
            }

            if (IDSCount == DeletedIDCount)
            {
                return Json(new { Status = "OK", Message = $"成功删除{DeletedIDCount}个日志！" });
            }
            return Json(new { Status = "Error",Message=$"成功删除{DeletedIDCount}个,其中{IDSCount-DeletedIDCount}个删除失败！" });
        }

        #endregion


        #region 员工管理
        public ActionResult Employees()
        {

            ViewBag.StatusList = new List<SelectListItem>() { new SelectListItem() { Value = "false", Text = "禁用" }, new SelectListItem() { Value = "true", Text = "启用",Selected=true} };
            ViewBag.SexList = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "女" }, new SelectListItem() { Value = "1", Text = "男"},new SelectListItem() { Value = "2", Text = "保密", Selected = true } };
            var DepartmentList = new List<SelectListItem>();
            var departments= employeeService.GetDepartments();
            foreach (var item in departments)
            {
                DepartmentList.Add(new SelectListItem() {Text=item.DepartmentName,Value=item.ID.ToString() });
            }
            ViewBag.DepartmentsList = DepartmentList;

            var Positions = employeeService.GetPositions();
            var PositionList = new List<SelectListItem>();
            foreach (var item in Positions)
            {
                PositionList.Add(new SelectListItem() {Text=item.PositionName,Value=item.ID.ToString() });
            }
            ViewBag.PositionsList = PositionList;
            logger.Warn($"用户 【{CookieHelper.GetCurrentAccount().Username}】查看了员工列表！");
            return View();
        }

        public ActionResult GetAllEmployeeJson(int? limit, int? offset, string order,string NickName)
        {
            if (string.IsNullOrEmpty(order)) order = "asc";
            int TotalCount = 0;
            var employees = employeeService.LoadEntities((int)offset, (int)limit, out TotalCount, (r => r.ID > 0&&r.NickName.Contains(NickName)), (r => new { r.ID }), "asc".Equals(order) ? false : true);
            return Json(new { total = TotalCount, rows = employees }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult GetAllDepartmentsJson()
        {
            return Json(employeeService.GetDepartments());
        }

        [HttpPost]
        public ActionResult GetAllPositionJson()
        {
            return Json(employeeService.GetPositions());
        }

        [HttpPost]
        public ActionResult GetDepartmentNameByID(int DepartmentID)
        {
           var d= employeeService.GetDepartment(DepartmentID);
            if (d != null)
            {
                return Content(d.DepartmentName);
            }
            return Content("");
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee employee)
        {
            employee.Address = employee.Address ?? "";
            employee.Description = employee.Description??"";
            employee.IDCardNumber =employee.IDCardNumber??"";
            employee.RecordAccount = CookieHelper.GetCurrentAccount().Username;
            employee.FireDate = DateTime.Parse("1900-01-01");
            if (employeeService.AddEntity(employee) != null)
            {
                logger.Info($"用户 【{CookieHelper.GetCurrentAccount().Username}】 添加一个员工,姓名为：{employee.NickName};");
                return Json(new { Status = "OK", Message = "" });
            }
            return Json(new { Status = "Error",Message="" });
        }

        public ActionResult GetEmployeeDetailByID(int EmployeeID)
        {
            return Json(employeeService.LoadEntities(a=>a.ID==EmployeeID).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult UpdateEmployee(Employee EmployeeModel)
        {
            EmployeeModel.Address= EmployeeModel.Address??"";
            EmployeeModel.Description =EmployeeModel.Description??"";
            EmployeeModel.IDCardNumber =EmployeeModel.IDCardNumber??"";
            EmployeeModel.RecordAccount = CookieHelper.GetCurrentAccount().Username;
            EmployeeModel.FireDate = DateTime.Parse("1900-01-01");
            if (employeeService.UpdateEntity(EmployeeModel))
            {
                logger.Warn($"用户 【{CookieHelper.GetCurrentAccount().Username}】修改了员工{EmployeeModel.NickName}的信息！");
                return Json(new { Status = "OK", Message = "" });
            }
            return Json(new { Status = "Error", Message = "" });
        }

        [HttpPost]
        [HandlerPermission(false)]
        public ActionResult DeleteEmployee(int ID)
        {
            if (employeeService.ExistEntity(ID))
            {
                if (employeeService.DeleteEmployeeByID(ID))
                {
                    logger.Warn($"用户 【{CookieHelper.GetCurrentAccount().Username}】 删除员工ID为{ID}的员工！");
                    return Json(new { Status = "OK", Message = "" });
                }
            }
            return Json(new { Status = "Error", Message = "" });
        }

        [HttpPost]
        [HandlerPermission(false)]
        public ActionResult DeleteEmployees(int []IDs)
        {
            int IDSCount = IDs.Length;
            int DeletedIDCount = 0;
            foreach (var ID in IDs)
            {
                if (employeeService.ExistEntity(ID))
                {
                    var employee = employeeService.LoadEntities(a => a.ID == ID).FirstOrDefault();
                    if (employeeService.DeleteEntity(employee))
                    {
                        logger.Warn($"用户{CookieHelper.GetCurrentAccount().Username}-> 删除员工ID为{employee.ID},姓名为{employee.NickName}，的{employeeService.GetDepartment(employee.DepartmentID)}员工！\n");
                        DeletedIDCount++;
                    }
                }
            }

            if (IDSCount == DeletedIDCount)
            {
                return Json(new { Status = "OK", Message = $"成功删除{DeletedIDCount}个员工！" });
            }
            return Json(new { Status = "Error", Message = $"成功删除{DeletedIDCount}个员工,其中{IDSCount - DeletedIDCount}个删除失败！" });
        }

        #endregion


        #region 文件下载
        public FileResult DownloadFileTemplate(string FileName,string FileType)
        {
            string FileExtention = ".xlsx";
            switch (FileType)
            {
                case "EXCEL":
                    FileExtention = ".xlsx";
                    break;
                case "TXT":
                    FileExtention = ".txt";
                    break;
                case "POWERPOINT":
                    FileExtention = ".pptx";
                    break;
                case "DOCUMENT":
                    FileExtention = ".docx";
                    break;
                case "PDF":
                    FileExtention = ".pdf";
                    break;
                default:
                    break;
            }
            string FilePath = Server.MapPath("~/Download/" + FileName + FileExtention);
            logger.Warn($"用户 【{CookieHelper.GetCurrentAccount().Username}】下载了文件名为{FileName}类型为{FileType}的文件！");
            return File(FilePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.Now.Ticks}{FileName}{FileExtention}");
           
        }
        #endregion

        #region 系统配置

      
        public ActionResult Config()
        {
            return View();
        }
        #endregion
    }
}