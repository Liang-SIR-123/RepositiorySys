using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepositorySysUI.Controllers
{
    public class HomeController : Controller
    {
        private RepositorySysData db = new RepositorySysData();
        public ActionResult Index()
        {
            var data = db.UserInfo.ToList();
            return View();
        }

        
       
    }
}