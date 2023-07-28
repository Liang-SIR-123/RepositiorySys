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
    public class MenuInfoController : Controller
    {

        private IMenuInfoBLL _menuInfoBLL;
        public MenuInfoController(IMenuInfoBLL menuInfoBLL)
        {
            _menuInfoBLL = menuInfoBLL;
        }


        #region 查询列表

        public ActionResult ListView()
        {
            return View();
        }

        public ActionResult GetMenuInfos(int page, int limit, string title)
        {
            int count;
            List<GetMenuInfo> list = _menuInfoBLL.GetMenuInfo(page, limit, title, out count);
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
        // GET: MenuInfo

        #region 添加
        public ActionResult CreateMenuInfoView()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetParentMenu(string id)
        {
            ReturnResult result = new ReturnResult();
            var data = _menuInfoBLL.GetSelectOptions(id);
            result.Code = 200;
            result.Msg = "获取成功！";
            result.IsSuccess = true;
            result.Data = data;
            return new JsonHelper(result);
        }
        [HttpPost]
        public ActionResult CreateMenuInfo([Form] MenuInfo menu)
        {
            string msg;
            bool isSuccess = _menuInfoBLL.CreateMenuInfo(menu, out msg);
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


        #region 修改菜单
        public ActionResult EditMenuInfoView()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditMenuInfo([Form] MenuInfo menu)
        {
            ReturnResult result = new ReturnResult();
            string msg;
            bool isSuccess = _menuInfoBLL.EditMenuInfo(menu, out msg);
            result.Msg = msg;
            if (isSuccess)
            {
                result.IsSuccess = isSuccess;
                result.Code = 200;
            }
            return new JsonHelper(result);
        }
        #endregion

        #region 根据Id删除菜单
        [HttpPost]
        public ActionResult DeleteMenu(string Id)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(Id))
            {
                result.Msg = "Id不能为空";
                return new JsonHelper(result);
            }
            bool isSuccess = _menuInfoBLL.DeleteMenuInfo(Id);
            if (isSuccess)
            {

                result.Msg = "删除部门成功！";
                result.Code = 200;
            }
            return new JsonHelper(result);
        }

        #endregion

        #region 根据Ids批量删除
        [HttpPost]
        public ActionResult DeleteMenus(List<string> Ids)
        {
            ReturnResult result = new ReturnResult();
            if (Ids == null || Ids.Count == 0)
            {
                result.Msg = "你还没有选择要删除的部门！";
                return new JsonHelper(result);
            }
            bool IsSuccess = _menuInfoBLL.DeleteMenuInfos(Ids);

            if (IsSuccess == false)
            {
                result.Msg = "删除失败！";
                result.IsSuccess = IsSuccess;
                result.Code = 501;
            }
            result.Msg = "删除成功！";
            result.IsSuccess = IsSuccess;
            result.Code = 200;
            return new JsonHelper(result);
        }

        #endregion

        #region 根据登录用户获取能访问的菜单
        [HttpGet]
        public ActionResult GetMenus()
        {
            //返回菜单信息
            HomeMenuInfoDTO res = new HomeMenuInfoDTO();
            #region 写死方法
            //写死方法
            /* List<HomeMenuDTO> menulist = new List<HomeMenuDTO>()
             {
                 new HomeMenuDTO()
                 {
                    Title="用户管理",
                 Href="/UserInfo/ListView",
                 Icon="fa fa-user",
                 Target="_self"
                 },
                 new HomeMenuDTO()
                 {
                     Title="系统管理",
                 Href="",
                 Icon="fa fa-cog",
                 Target="_self",
                 Child=new List<HomeMenuDTO>()
                 {
                     new HomeMenuDTO()
                     {
                         Title="角色管理",
                         Href="/RoleInfo/ListView",
                         Icon="fa fa-street-view",
                         Target="_self"
                     }
                 }
                 }

             };*/
            #endregion

            var userId = HttpContext.Request.Cookies["UserId"];
            if (userId == null)
            {
                res = new HomeMenuInfoDTO(new List<HomeMenuDTO>());
                return new JsonHelper(res);
            }
            else
            {
                List<HomeMenuDTO> menulist = _menuInfoBLL.GetMenus(userId.Value);
                res = new HomeMenuInfoDTO(menulist);
            }
           
            return new JsonHelper(res);
        }

        
        #endregion
    }
}