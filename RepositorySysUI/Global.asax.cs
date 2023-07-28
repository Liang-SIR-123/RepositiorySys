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
            //���ó�ʼ�����ݿ�
            //InitDB();
        }
        /// <summary>
        /// ��ʼ�������������ݿⷽ��
        /// </summary>
        public static void InitDB()
        {
            //ʵ��������������
            RepositorySysData db = new RepositorySysData();
            //������ݿ��Ƿ����
            if (db.Database.Exists())
            {
                db.Database.Delete();//ɾ�����ݿ�
            }
            db.Database.Create(); //�������ݿ�

            //ʵ����һ����ʼ�˺�
            UserInfo userInfo = new UserInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Account = "admin",
                PassWord = MD5Helper.GetMD5("123"),
                CreateTime=DateTime.Now,
                IsAdmin=true,
                UserName="��ʼ���������û�",
            };
            //��ӵ����ݿ�
            db.UserInfo.Add(userInfo);//��ӵ�UserInfo��ע�⣺ִ����ô��뻹û�ύ�����ݿ⣩
            db.SaveChanges(); //�ύ�����ݿ⣨ע�⣺ִ����ô����������ӵ����ݿ⣩
        }
    }
}
