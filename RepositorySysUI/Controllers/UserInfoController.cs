using Common;
using IBLL;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepositorySysUI.Controllers
{
    public class UserInfoController : Controller
    {
        private IUserInfoBLL _userInfoBLL;
        public UserInfoController(IUserInfoBLL userInfo)
        {
            //userInfoBLL = new UserInfoBLL();
            _userInfoBLL = userInfo;
        }
        // GET: UserInfo
        public ActionResult ListView()
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

    }
}