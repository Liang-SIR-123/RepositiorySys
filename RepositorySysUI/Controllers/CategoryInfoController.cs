using BLL;
using Common;
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
    public class CategoryInfoController : Controller
    {
        private RepositorySysData _repositorySys;
        private CategoryInfoBLL _categoryInfoBLL;
        public CategoryInfoController(RepositorySysData repositorySys, CategoryInfoBLL categoryInfoBLL)
        {
            _repositorySys = repositorySys;
            _categoryInfoBLL = categoryInfoBLL;
        }
        // GET: CategoryInfo
        #region 获取耗材类别数据
        public ActionResult ListView()
        {
            return View();
        }
        public ActionResult GetCategoryInfo(int page, int limit, string CategoryName)
        {
            int count;
            List<GetCategoryInfoDTO> list = _categoryInfoBLL.getCategoryInfos(page, limit, CategoryName, out count);
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

        #region 创建耗材类别
        public ActionResult CreateCategoryView()
        {
            return View();
        }
       [HttpPost]
        public ActionResult CreateCategory([Form] Category category)
        {
            string msg;
            bool isSuccess = _categoryInfoBLL.CreateCategoryInfo(category, out msg);
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

        #region 更新耗材
        public ActionResult UpdateCategoryView()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetCategoryInfos(string Id)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(Id))
            {
                result.Msg = "Id不能为空";
                return new JsonHelper(result);
            }
            var department = _categoryInfoBLL.GetCategoryInfos(Id);
            if (department == null)
            {
                result.Msg = "未能获取到耗材信息！";
            }
            else
            {
                result.Code = 200;
                result.Msg = "查询成功！";
                result.Data = department;
            }
            return new JsonHelper(result);
        }
        [HttpPost]
        public ActionResult UpdateCategoryInfo([Form] Category category)
        {
            ReturnResult result = new ReturnResult();
            string msg;
            bool isSuccess = _categoryInfoBLL.UpdateCategoryInfo(category, out msg);
            result.Msg = msg;
            if (isSuccess)
            {
                result.IsSuccess = isSuccess;
                result.Code = 200;
            }
            return new JsonHelper(result);
        }
        #endregion

        #region 根据id软删除
        [HttpPost]
        public ActionResult DeleteCategory(string Id)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(Id))
            {
                result.Msg = "Id不能为空";
                return new JsonHelper(result);
            }
            bool isSuccess = _categoryInfoBLL.DeleteCategory(Id);
            if (isSuccess)
            {

                result.Msg = "删除耗材成功！";
                result.Code = 200;
            }
            return new JsonHelper(result);
        }
        #endregion

        #region 根据Id批量软删除
        [HttpPost]
        public ActionResult DeleteCategorys(List<string> Ids)
        {

            ReturnResult result = new ReturnResult();
            if (Ids == null || Ids.Count == 0)
            {
                result.Msg = "你还没有选择要删除的耗材！";
                return new JsonHelper(result);
            }
            bool IsSuccess = _categoryInfoBLL.DeleteCategorys(Ids);

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

    }
}