using BLL;
using Common;
using IBLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepositorySysUI.Controllers
{
    public class AccountController : Controller
    {
        // private UserInfoBLL _userInfoBLL;
        private IUserInfoBLL _userInfoBLL;
        public AccountController(IUserInfoBLL userInfo)
        {
            //userInfoBLL = new UserInfoBLL();
            _userInfoBLL = userInfo;
        }
        // GET: Account
        public ActionResult LoginView()
        {
            return View();
        }
        /// <summary>
        /// 登录的接口
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lgion(string account,string password)
        {
           /* ReturnResult test = new ReturnResult();
            //把.net对象转为json字符串
            string jsonStr = JsonConvert.SerializeObject(test);
            //把json字符串转为.net对象，转换时需要有明确的对象类型
            ReturnResult ret = JsonConvert.DeserializeObject<ReturnResult>(jsonStr);
*/



            ReturnResult result = new ReturnResult();
            //判断账号是否为null或者""
            if (string.IsNullOrWhiteSpace(account))
            {
                result.Msg = "账号不能为空";
                //return Json(result);
                return new JsonHelper(result);
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                result.Msg = "密码不能为空";
                //return Json(result);
                return new JsonHelper(result);
            }
            string msg;
            string userName;
            string userId;
            //调用登录业务逻辑
            bool isSuccess= _userInfoBLL.Login(account,password,out msg,out userName,out userId);
            //把提示消息赋值给结果对象Msg属性
            result.Msg = msg;
            if (isSuccess)
            {
                result.IsSuccess = isSuccess;
                result.Code = 200;
                result.Data = userName;
                //return Json(result);
                //把信息存到Session
               /* HttpContext.Session["UserName"] = userName;
                HttpContext.Session["UserId"] = userId;*/
                //存cookie
                HttpCookie cookie = new HttpCookie("UserId", userId);
                //在浏览器存储的时间
                cookie.Expires = DateTime.Now.AddDays(100);//设置过期时间为100天后过期
                //向请求方响应
                Response.Cookies.Add(cookie);
                return new JsonHelper(result);
            }
            else
            {
                result.Code = 500;
                return new JsonHelper(result);
                // return Json(result);
            }

        }
    }
}