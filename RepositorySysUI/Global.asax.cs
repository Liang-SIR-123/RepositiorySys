using Common;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RepositorySysUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //调用初始化数据库
            //InitDB();
        }
        /// <summary>
        /// 初始化触发创建数据库方法
        /// </summary>
        public static void InitDB()
        {
            //实例化数据上下文
            RepositorySysData db = new RepositorySysData();
            //检查数据库是否存在
            if (db.Database.Exists())
            {
                db.Database.Delete();//删除数据库
            }
            db.Database.Create(); //创建数据库

            //实例化一个初始账号
            UserInfo userInfo = new UserInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Account = "admin",
                PassWord = MD5Helper.GetMD5("123"),
                CreateTime=DateTime.Now,
                IsAdmin=true,
                UserName="初始化创建的用户",
            };
            //添加到数据库
            db.UserInfo.Add(userInfo);//添加到UserInfo表（注意：执行完该代码还没提交到数据库）
            db.SaveChanges(); //提交到数据库（注意：执行完该代码才算是添加到数据库）
        }
    }
}
