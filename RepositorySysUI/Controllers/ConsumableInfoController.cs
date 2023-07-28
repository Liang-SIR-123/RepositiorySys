using BLL;
using Common;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace RepositorySysUI.Controllers
{
    public class ConsumableInfoController : Controller
    {
        private RepositorySysData _repositorySys;
        private ConsumableInfoBLL _consumableInfoBLL;
        private CategoryInfoBLL _categoryInfoBLL;
        public ConsumableInfoController(RepositorySysData repositorySys, ConsumableInfoBLL consumableInfoBLL, CategoryInfoBLL categoryInfoBLL)
        {
            _repositorySys = repositorySys;
            _consumableInfoBLL = consumableInfoBLL;
            _categoryInfoBLL = categoryInfoBLL;
        }
        // GET: ConsumableInfo
        #region 获取耗材显示数据
        public ActionResult ListView()
        {
            return View();
        }
        public ActionResult GetConsumableInfo(int page, int limit, string ConsumableName)
        {
            int count;
            List<GetConsumableInfoDTO> list = _consumableInfoBLL.GetConsumableInfo(page, limit, ConsumableName, out count);
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

        #region 添加耗材信息
        public ActionResult CreateConsumableView()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateConsumable([Form] ConsumableInfo consumable)
        {
            string msg;
            bool isSuccess = _consumableInfoBLL.CreateConsumable(consumable, out msg);
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

        #region 获取下拉列表耗材类型的信息
        [HttpGet]
        public ActionResult CategoryOption(string Id)
        {


            var list = _consumableInfoBLL.GetCategoryInfos(Id);
            ReturnResult result = new ReturnResult();
            result.Msg = list == null ? "获取失败！" : "获取成功！";
            if (list != null)
            {
                result.IsSuccess = true;
                result.Code = 200;
                result.Data = list;
                return new JsonHelper(result);
            }


            return new JsonHelper(result);
        }

        #endregion

        #region 修改耗材信息
        public ActionResult UpdateConsumableView()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateConsumable([Form] ConsumableInfo consumable)
        {
            ReturnResult result = new ReturnResult();
            string msg;
            bool isSuccess = _consumableInfoBLL.UpdateConsumable(consumable, out msg);
            result.Msg = msg;
            if (isSuccess)
            {
                result.IsSuccess = isSuccess;
                result.Code = 200;
            }
            return new JsonHelper(result);
        }

        #endregion

        #region 根据Id软删除
        [HttpPost]
        public ActionResult DeleteConsumable(string Id)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(Id))
            {
                result.Msg = "Id不能为空";
                return new JsonHelper(result);
            }
            bool isSuccess = _consumableInfoBLL.DeleteConsumable(Id);
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
        public ActionResult DeleteConsumables(List<string> ids)
        {
            ReturnResult result = new ReturnResult();
            if (ids == null || ids.Count == 0)
            {
                result.Msg = "Ids不能为空";
                return new JsonHelper(result);
            }
            bool isSuccess = _consumableInfoBLL.DeleteConsumables(ids);
            if (isSuccess)
            {

                result.Msg = "删除耗材成功！";
                result.Code = 200;
            }
            return new JsonHelper(result);
        }

        #endregion

        #region 入库Excel导入的接口Upload
        public ActionResult Upload(HttpPostedFileBase file)
        {

            ReturnResult result = new ReturnResult();
            //获取用户Id
            var userId = HttpContext.Request.Cookies["UserId"];
            if (userId == null || string.IsNullOrWhiteSpace(userId.Value))
            {
                result.Msg = "上传的用户不存在！";
                return new JsonHelper(result);
            }
            if (file == null)
            {
                result.Msg = "文件不能为空！";
                return new JsonHelper(result);
            }
            //获取文件名
            string fileName = file.FileName;
            string extension = Path.GetExtension(fileName);//引入IO，获取文件后缀
            if (extension == ".xls" || extension == ".xlsx")
            {
                //获取传递进来文件的文件流
                Stream stream = file.InputStream;
                string msg;
                //调用BLL上传方法
                bool IsSuccess = _consumableInfoBLL.Upload(stream, extension, userId.Value, out msg);
                result.Msg = msg;
                result.IsSuccess = IsSuccess;
                if (IsSuccess )
                {
                    result.Code = 200;
                   
                }
                return new JsonHelper(result);
            }
            else
            {
                result.Msg = "上传的文件只能是Excel格式";
                return new JsonHelper(result);
            }


        }

        #endregion

        #region 导出
        public ActionResult GetDownLoad()
        {
            string downloadFileName;
            Stream stream = _consumableInfoBLL.GetDownLoad(out downloadFileName);
            //返回内容类型
            return File(stream, "application/octet-stream",downloadFileName);
        }
        #endregion
    }
}