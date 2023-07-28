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
    public class UserInfoController : Controller
    {
        private IUserInfoBLL _userInfoBLL;
        private IDepartmentInfoBLL _departmentInfoBLL;
        public UserInfoController(IUserInfoBLL userInfo, IDepartmentInfoBLL departmentInfoBLL)
        {
            //userInfoBLL = new UserInfoBLL();
            _userInfoBLL = userInfo;
            _departmentInfoBLL = departmentInfoBLL;
        }
        // GET: UserInfo
        public ActionResult ListView()
        {
            return View();
        }
        public ActionResult CreateUserLiew()
        {
            return View();
        }
        //修改资料
        public ActionResult UserSettingView()
        {
            return View();
        }
        #region 用户列表获取的方法
        /// <summary>
        /// 用户列表获取的方法
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="account"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserInfos(int page, int limit, string account, string userName)
        {
            //数量总条数
            int count;
            //调用BLL的获取用户方法
            List<GetUserInfoDTO> list = _userInfoBLL.GetUserInfos(page, limit, userName, account, out count);
            //返回结果信息

            ReturnResult result = new ReturnResult()
            {
                Code = 0,
                Msg = "获取成功",
                Data = list,
                IsSuccess = true,
                Count = count

            };
            //返回结果
            return new JsonHelper(result);

        }
        #endregion
        #region 添加用户的方法
        [HttpPost]
        public ActionResult CreateUserInfo([Form] UserInfo user)
        {
            string msg;

            bool isSuccess = _userInfoBLL.CreateUserInfo(user, out msg);
            //返回结果封装
            ReturnResult result = new ReturnResult();
            result.Msg = msg;
            result.IsSuccess = isSuccess;
            if (isSuccess)
            {
                result.Code = 200;
            }
            return new JsonHelper(result);
        }
        #endregion

        #region 软删除的接口
        [HttpPost]
        public ActionResult DeleteUserInfo(string Id)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(Id))
            {
                result.Msg = "Id不能为空";
                return new JsonHelper(result);
            }
            bool isSuccess = _userInfoBLL.DeleteUserInfo(Id);
            if (isSuccess)
            {

                result.Msg = "删除用户成功！";
                result.Code = 200;
            }
            return new JsonHelper(result);
        }
        #endregion
        #region 批量软删除
        [HttpPost]
        public ActionResult DeleteUsers(List<string> ids)
        {
            ReturnResult result = new ReturnResult();
            if (ids == null || ids.Count == 0)
            {
                result.Msg = "你还没有选择要删除的用户";
                return new JsonHelper(result);
            }
            bool isSuccess = _userInfoBLL.DeleteUserInfo(ids);
            if (isSuccess)
            {
                result.Msg = "删除成功！";
                result.Code = 200;
            }
            else
            {
                result.Msg = "删除失败！";

            }
            return new JsonHelper(result);
        }
        #endregion
        #region 编辑更新信息
        public ActionResult UpdateUserInfoView()
        {
            return View();
        }
        #endregion

        #region 更新方法
        [HttpPost]
        public ActionResult UpdateUser([Form] UserInfo user)
        {
            ReturnResult result = new ReturnResult();
            string msg;
            bool isSuccess = _userInfoBLL.UpdateUserInfo(user, out msg);
            result.Msg = msg;
            if (isSuccess)
            {
                result.Code = 200;
                result.IsSuccess = isSuccess;
            }
            return new JsonHelper(result);
        }
        #endregion

        #region 获取下拉框的部门信息
        [HttpGet]
        public ActionResult GetDepartmentSelect()
        {
            ReturnResult result = new ReturnResult();
            var selectlist = _userInfoBLL.GetDepartmentSelect();
            result.Code = 200;
            result.IsSuccess = true;
            result.Msg = "获取成功";
            result.Data = selectlist;
            return new JsonHelper(result);
        }
        #endregion
        #region 修改个人资料
        [HttpGet]
        public ActionResult GetUserfile(string Id)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(Id))
            {
                result.Msg = "用户Id不能为空！";
                return new JsonHelper(result);
            }
            var Info = _userInfoBLL.GetUserFile(Id);
            var selectlist = _userInfoBLL.GetDepartmentSelect();
            result.Code = 200;
            result.IsSuccess = true;
            result.Msg = "获取成功";
            result.Data = new
            {
                Info,
                selectlist
            };
            return new JsonHelper(result);
        }
        [HttpGet]
        public ActionResult GetUserFiles(string Id)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(Id))
            {
                result.Msg = "用户Id不能为空！";
                return new JsonHelper(result);
            }
            var Info = _userInfoBLL.GetUserFiles(Id);
            var selectlist = _departmentInfoBLL.GetSelectOptions();
            result.Code = 200;
            result.IsSuccess = true;
            result.Msg = "获取成功";
            result.Data = new
            {
                Info,
                selectlist
            };
            return new JsonHelper(result);
        }
        #endregion

        #region 修改密码
        public ActionResult UpdatePassWord()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdatePwd(string id, string oldPwd, string newPwd, string againPwd)
        {
            ReturnResult result = new ReturnResult();
            string msg;
            bool IsSuccess = _userInfoBLL.UpdatePWD(id, oldPwd, newPwd, againPwd, out msg);
            result.Msg =msg;
            if (IsSuccess)
            {
                result.Code = 200;
                result.IsSuccess = IsSuccess;
            }
            return new JsonHelper(result);
        }
        #endregion

    }
}