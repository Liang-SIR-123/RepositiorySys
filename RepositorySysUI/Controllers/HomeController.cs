using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepositorySysUI.Controllers
{
    [MyFilter]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            //var username = HttpContext.Session["UserName"];
            //if (username == null)
            //{
            //    return RedirectToAction("");
            //}

            return View();
        }

        
       
    }
}