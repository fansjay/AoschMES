using Aosch.MES.Common;
using Aosch.MES.Model;
using Aosch.MES.Service;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Aosch.MES.Web.Controllers
{
    public class SystemController : Controller
    {
        public RoleService roleService = new RoleService();
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
            AddRole.RoleLevel =Request["RoleLevel"]==null?0:int.Parse(Request["RoleLevel"]);
            AddRole.RoleName = string.IsNullOrEmpty(Request["RoleName"])?"": Request["RoleName"];
            AddRole.Description= string.IsNullOrEmpty(Request["Description"]) ? "" : Request["Description"];

            var ResultRole = roleService.AddEntity(AddRole);
            if(ResultRole!=null)
            {
                return Json(new { Status = "OK", RoleModel = AddRole });
            }
            return Json(new {Status="ERROR" });
        }
        
        [HttpPost]
        public ActionResult UpdateRole(Role RoleModel)
        {
            RoleModel.Description=string.IsNullOrEmpty(RoleModel.Description) ? "" : RoleModel.Description;
            if (ModelState.IsValid)
            {
                if (roleService.UpdateEntity(RoleModel))
                {
                    return Json(new { Status = "OK" });
                }
            }
            return Json(new {Status="ERROR" });
        }

        [HttpPost]
        public ActionResult DeleteRole(int ID)
        {
            if (roleService.ExistEntity(ID))
            {
                var role = roleService.LoadEntities(a => a.ID == ID).FirstOrDefault();
               if (roleService.DeleteEntity(role))
                {
                    LoggerHelper.Log(Server.MapPath($"/Log/{DateTime.Now.ToString("yyyyMMdd")}.log"), LogType.Warning, $"用户 删除角色ID为{role.ID}角色名称为{role.RoleName}角色等级为{role.RoleLevel}的角色！\n");
                    return Json(new { Status = "OK" });
                }
            }
            return Json(new { Status="Error"});
        }

        /// <summary>
        /// 加载权限action列表 【ActionURL表】
        /// </summary>
        /// <returns></returns>

        public ActionResult GetRolePermissionListJson()
        {
            var list= roleService.GetActionURLs();
            return Json(list);
        }

        /// <summary>
        /// 为角色分配权限
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AssignPermissions(int RoleID,int[] ActionIDs)
        {
            var IDsList=  ActionIDs.ToList();
            IDsList.AddRange(roleService.GetAllTopActionURLIDS());
            ActionIDs=IDsList.ToArray();
            int InsertCount = ActionIDs.Length;
            int InsertedCount = 0;
            if(roleService.ExistEntity(RoleID))
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
            return Json(new { Status = "ERROR",ErrorMessage=$"需要添加权限{InsertCount}个，实际添加{InsertedCount}个"});
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

    }
}