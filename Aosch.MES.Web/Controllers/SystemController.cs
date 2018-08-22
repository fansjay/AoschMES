﻿using Aosch.MES.Common;
using Aosch.MES.Model;
using Aosch.MES.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Aosch.MES.Web.Controllers
{
    public class SystemController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private RoleService roleService = new RoleService();
        private LogService logService = new LogService();
        private EmployeeService employeeService = new EmployeeService();
        // GET: System
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RolePermission()
        {
            return View();
        }

        public ActionResult Roles()
        {


            ViewBag.RoleLevelList = new List<SelectListItem>() {new SelectListItem() { Value = "10", Text = "超级管理员" },new SelectListItem() { Value = "100", Text = "系统管理" }, new SelectListItem() { Value = "200", Text = "公司管理" },new SelectListItem() { Value = "300", Text = "普通员工",Selected=true } };
            
            return View();
        }

        public ActionResult GetAllRoleJson(int? limit, int? offset, string order)
        {
            if (string.IsNullOrEmpty(order)) order = "asc";
            int TotalCount = 0;
            var AllRole = roleService.LoadEntities((int)offset, (int)limit, out TotalCount, (r => r.ID > 0), (r => new { r.ID }), "asc".Equals(order) ? false : true);
            return Json(new { total = TotalCount, rows = AllRole }, JsonRequestBehavior.AllowGet);
        }

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
                logger.Info($"用户{CookieHelper.GetCurrentAccount().Username}：成功添加了角色ID为{AddRole.ID},角色名为:{AddRole.RoleLevel}的角色！");
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
                    logger.Info($"用户{CookieHelper.GetCurrentAccount().Username}：修改了角色ID为{RoleModel.ID},角色名为:{RoleModel.RoleLevel}的角色！");
                    return Json(new { Status = "OK" });
                }
            }
            return Json(new { Status = "ERROR" });
        }

        [HttpPost]
        public ActionResult DeleteRole(int ID)
        {
            if (roleService.ExistEntity(ID))
            {
                var role = roleService.LoadEntities(a => a.ID == ID).FirstOrDefault();
                if (roleService.DeleteEntity(role))
                {
                    //LoggerHelper.Log(Server.MapPath($"/Log/{DateTime.Now.ToString("yyyyMMdd")}.log"), LogType.Warning, $"用户 删除角色ID为{role.ID}角色名称为{role.RoleName}角色等级为{role.RoleLevel}的角色！\n");
                    logger.Warn($"用户 {CookieHelper.GetCurrentAccount().Username} 删除角色ID为{role.ID}角色名称为{role.RoleName}角色等级为{role.RoleLevel}的角色！\n");
                    return Json(new { Status = "OK" });
                }
            }
            return Json(new { Status = "Error" });
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
            if (InsertedCount == InsertCount) { return Json(new { Status = "OK" }); }
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
            ViewBag.StatusList = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "禁用" }, new SelectListItem() { Value = "1", Text = "启用",Selected=true} };
            ViewBag.SexList = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "女" }, new SelectListItem() { Value = "1", Text = "男", Selected = true } };
            return View();
        }

        public ActionResult GetAllEmployeeJson(int? limit, int? offset, string order)
        {
            if (string.IsNullOrEmpty(order)) order = "asc";
            int TotalCount = 0;
            var employees = employeeService.LoadEntities((int)offset, (int)limit, out TotalCount, (r => r.ID > 0), (r => new { r.ID }), "asc".Equals(order) ? false : true);
            return Json(new { total = TotalCount, rows = employees }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEmployee(Employee employee)
        {
            if (employeeService.AddEntity(employee) != null)
            {
                return Json(new { Status = "OK", Message = "" });
            }
            return Json(new { Status = "Error",Message="" });
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
            return File(FilePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.Now.Ticks}{FileName}{FileExtention}");
        }
        #endregion

    }
}