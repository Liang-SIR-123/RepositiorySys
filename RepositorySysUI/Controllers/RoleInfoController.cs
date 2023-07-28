using Common;
using IBLL;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace RepositorySysUI.Controllers
{
    [MyFilter]
    public class RoleInfoController : Controller
    {
        private IRoleInfoBLL _RoleInfoBLL;
        private IUserInfoBLL _userInfoBLL;
        private IMenuInfoBLL _menuInfoBLL;
        public RoleInfoController(IRoleInfoBLL RoleInfoBLL, IUserInfoBLL userInfoBLL, IMenuInfoBLL menuInfoBLL)
        {
            _RoleInfoBLL = RoleInfoBLL;
            _userInfoBLL = userInfoBLL;
            _menuInfoBLL = menuInfoBLL;
        }
        // GET: RoleInfo
        #region 显示角色信息
        public ActionResult ListView()
        {
            return View();
        }

        public ActionResult GetRoleInfo(int page, int limit, string roleName)
        {
            int count;
            List<GetRoleInfo> list = _RoleInfoBLL.getRoleInfos(page, limit, roleName, out count);
            ReturnResult result = new ReturnResult()
            {
                Code = 0,
                Data = list,
                IsSuccess = true,
                Count = count,
                Msg = "获取成功"
            };
            return new JsonHelper(result);
        }

        #endregion

        #region 添加
        public ActionResult CreateRoleView()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateRole([Form] RoleInfo role)
        {
            string msg;
            ReturnResult result = new ReturnResult();
            result.IsSuccess = _RoleInfoBLL.CreateRole(role, out msg);
            if (result.IsSuccess)
            {
                result.Msg = msg;
                result.Code = 200;
                return new JsonHelper(result);
            }
            result.Msg = msg;
            return new JsonHelper(result);

        }
        #endregion

        #region 修改角色
        public ActionResult UpdateRoleView()
        {
            return View();
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateRole(string Id)
        {
            ReturnResult result = new ReturnResult();
            var role = _RoleInfoBLL.GetRole(Id);
            result.Msg = "获取成功！";
            result.Code = 200;
            result.IsSuccess = true;
            result.Data = role;
            return new JsonHelper(result);
        }
        [HttpPost]
        public ActionResult UpdateRoleInfo([Form] RoleInfo role)
        {
            string msg;
            ReturnResult result = new ReturnResult();
            result.IsSuccess = _RoleInfoBLL.UpdateRoleInfo(role, out msg);
            if (result.IsSuccess)
            {
                result.Msg = msg;
                result.Code = 200;
                return new JsonHelper(result);
            }
            result.Msg = msg;
            return new JsonHelper(result);

        }
        #endregion

        #region 用户绑定角色
        public ActionResult UserBindRoleView()
        {
            return View();
        }

        #endregion

        #region 根据Id软删除角色
        [HttpPost]
        public ActionResult DeleteRole(string Id)
        {
            ReturnResult result = new ReturnResult();
            string msg;
            result.IsSuccess = _RoleInfoBLL.DeleteRole(Id, out msg);
            if (result.IsSuccess)
            {
                result.Msg = msg;
                result.Code = 200;
                return new JsonHelper(result);
            }
            return new JsonHelper(result);
        }

        #endregion

        #region 根据Id批量软删除角色
        [HttpPost]
        public ActionResult DeleteRoles(List<string> Ids)
        {

            ReturnResult result = new ReturnResult();
            string msg;
            if (Ids == null || Ids.Count == 0)
            {
                result.Msg = "你还没有选择Id!";
                return new JsonHelper(result);
            }
            result.IsSuccess = _RoleInfoBLL.DeleteRoles(Ids, out msg);
            if (result.IsSuccess)
            {
                result.Msg = msg;
                result.Code = 200;
                return new JsonHelper(result);
            }
            return new JsonHelper(result);
        }

        #endregion
        #region 获取用户信息
        [HttpGet]
        public ActionResult GetUserInfoOptions(string roleId)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(roleId))
            {
                result.Msg = "Id不能为空！";
                return new JsonHelper(result);
            }
            //获取用户列表
            List<GetUserInfoDTO> options = _userInfoBLL.GetUserInfo();
            List<string> userIds = _RoleInfoBLL.GetR_UserInfo_RoleInfo(roleId);
            result.Data = new
            {
                options,
                userIds
            };
            result.Code = 200;
            result.IsSuccess = true;
            result.Msg = "获取成功！";
            return new JsonHelper(result);

        }
        #endregion
        #region 绑定用户角色的方法
        /// <summary>
        /// 绑定用户角色的方法
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BindUser(List<String> userIds, string roleId)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(roleId))
            {
                result.Msg = "角色Id不能为空！";
                return new JsonHelper(result);
            };
            result.IsSuccess = _RoleInfoBLL.BindUserInfo(userIds, roleId);
            result.Code = 200;
            result.Msg = "成功";
            return new JsonHelper(result);
        }
        #endregion

        #region 获取菜单信息
        public ActionResult MenuBindRoleView()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetMenuInfoOptions(string roleId)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(roleId))
            {
                result.Msg = "Id不能为空！";
                return new JsonHelper(result);
            }
            //获取菜单列表
            var options = _menuInfoBLL.getMenu();
            List<string> menuIds = _RoleInfoBLL.GetR_RoleInfo_MenuInfo(roleId);
            result.Data = new
            {
                options,
                menuIds
            };
            result.Code = 200;
            result.IsSuccess = true;
            result.Msg = "获取成功！";
            return new JsonHelper(result);
        }

        #endregion

        #region 绑定菜单的方法
        [HttpPost]
        public ActionResult BindMenu(List<string> menuIds,string roleId)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(roleId))
            {
                result.Msg = "角色Id不能为空！";
                return new JsonHelper(result);
            };
            result.IsSuccess = _RoleInfoBLL.BindMenuInfo(menuIds, roleId);
            result.Code = 200;
            result.Msg = "成功";
            return new JsonHelper(result);
        }

        #endregion
    }
}