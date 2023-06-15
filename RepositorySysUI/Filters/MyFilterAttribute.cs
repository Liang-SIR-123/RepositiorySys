using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepositorySysUI
{
    /// <summary>
    /// 我的自定义筛选器
    /// </summary>
    public class MyFilterAttribute:ActionFilterAttribute
    {
        /// <summary>
        /// 在执行操作之前
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //获取session中存在的UserId
            //var userId = filterContext.HttpContext.Session["UserId"];
            //cookie
            var userId2 = filterContext.HttpContext.Request.Cookies["UserId"];
            //判断userId是否为空
            if (userId2 == null)
            {
                var result = filterContext.Result;
                //为空则设置返回的结果为登录页面
                filterContext.Result = new RedirectResult("/Account/LoginView");
            }
        }
        /// <summary>
        /// 在执行操作之后
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}