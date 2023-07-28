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
    public class WorkFlow_ModelController : Controller
    {
        private WorkFlow_ModelBLL  _workFlow_ModelBLL;
        public WorkFlow_ModelController( WorkFlow_ModelBLL workFlow_ModelBLL)
        {
            _workFlow_ModelBLL = workFlow_ModelBLL;
        }
        // GET: WorkFlow_Model
       
        #region 获取工作流模板数据
        public ActionResult ListView()
        {
            return View();
        }
        public ActionResult GetWorkFlow_Model(int page, int limit, string title)
        {
            int count;
            List<GetWorkFlow_ModelDTO> list = _workFlow_ModelBLL.GetWorkFlow_Model(page, limit, title, out count);
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

        #region 创建工作流模板
        public ActionResult CreateWorkFlowView()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateWorkFlow([Form] WorkFlow_Model workFlow)
        {
            string msg;
            bool isSuccess = _workFlow_ModelBLL.CreateWorkFlow(workFlow, out msg);
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

        #region 更新工作流模板
        public ActionResult UpdateWorkFlowView()
        {
            return View();
        }
        [HttpGet]
        public ActionResult getWorkFlow(string Id)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(Id))
            {
                result.Msg = "Id不能为空";
                return new JsonHelper(result);
            }
            var department = _workFlow_ModelBLL.getWorkFlow(Id);
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
        public ActionResult UpdateWorkFlow([Form] WorkFlow_Model workFlow)
        {
            ReturnResult result = new ReturnResult();
            string msg;
            bool isSuccess = _workFlow_ModelBLL.UpdateWorkFlow(workFlow, out msg);
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
        public ActionResult DeleteWorkFlow(string Id)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(Id))
            {
                result.Msg = "Id不能为空";
                return new JsonHelper(result);
            }
            bool isSuccess = _workFlow_ModelBLL.DeleteWorkFlow(Id);
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
        public ActionResult DeleteWorkFlows(List<string> Ids)
        {

            ReturnResult result = new ReturnResult();
            if (Ids == null || Ids.Count == 0)
            {
                result.Msg = "你还没有选择要删除的模板！";
                return new JsonHelper(result);
            }
            bool IsSuccess = _workFlow_ModelBLL.DeleteWorkFlows(Ids);

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
