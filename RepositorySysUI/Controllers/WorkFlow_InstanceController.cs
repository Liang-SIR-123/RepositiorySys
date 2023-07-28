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
    public class WorkFlow_InstanceController : Controller
    {

        private IWorkFlow_ModelBLL _workFlow_ModelBLL;
        private IConsumableInfoBLL _consumableInfoBLL;
        private IWorkFlow_InstanceBLL _workFlow_InstanceBLL;
        public WorkFlow_InstanceController(IWorkFlow_ModelBLL workFlow_ModelBLL, IConsumableInfoBLL consumableInfoBLL, IWorkFlow_InstanceBLL workFlow_InstanceBLL)
        {
            _workFlow_ModelBLL = workFlow_ModelBLL;
            _consumableInfoBLL = consumableInfoBLL;
            _workFlow_InstanceBLL = workFlow_InstanceBLL;
        }
        // GET: WorkFlow_Instance
        public ActionResult ListView()
        {
            return View();
        }
        public ActionResult CreateWorkFlow_InstanceView()
        {
            return View();
        }

        #region 获取工作流和物品流下拉数据的接口

        [HttpGet]
        public ActionResult GetOption()
        {
            string id = "";
            ReturnResult result = new ReturnResult();
            var modellist = _workFlow_ModelBLL.getWorkFlow(id);
            var consumablelist = _consumableInfoBLL.GetCategoryInfos(id);
            result.Data = new {
                modellist,
                consumablelist
            };
            result.Code = 200;
            result.IsSuccess = true;
            return new JsonHelper(result);

        }
        #endregion

        #region 提交申领
        [HttpPost]
        public ActionResult CreateWorkFlow_Instance([Form] WorkFlow_Instance workFlow)
        {
            ReturnResult result = new ReturnResult();
            string msg;
            var userId = HttpContext.Request.Cookies["UserId"];
            if (userId == null)
            {
                result.Msg = "用户id不能为空！";
                return new JsonHelper(result);
            }
            if (string.IsNullOrWhiteSpace(workFlow.ModelId))
            {
                result.Msg = "工作流模板不能为空！";
                return new JsonHelper(result);
            }
            if (string.IsNullOrWhiteSpace(workFlow.OutGoodsId))
            {
                result.Msg = "物品不能为空！";
                return new JsonHelper(result);
            }
            if (string.IsNullOrWhiteSpace(workFlow.Reason))
            {
                result.Msg = "申请理由不能为空！";
                return new JsonHelper(result);
            }
            if (workFlow.OutNum<=0)
            {
                result.Msg = "申请数量不能为空！";
                return new JsonHelper(result);
            }

            bool IsSuccess = _workFlow_InstanceBLL.CreateWorkFlow_Instance(workFlow, userId.Value,out msg);
            result.IsSuccess = IsSuccess;
            result.Msg = msg;
            if (result.IsSuccess)
            {
                result.Code = 200;
            }
                return new JsonHelper(result);
        }
        #endregion

        #region 工作流实例列表
        
        public ActionResult workFlow_InstancesList(int page, int limit,int status=0)
        {
            var userId = HttpContext.Request.Cookies["UserId"];
            //数量总条数
            int count;
            //调用BLL的获取用户方法
            List<GetWorkFlow_InstanceDTO> list = _workFlow_InstanceBLL.workFlow_InstancesList(page, limit,userId.Value,status, out count);
            //返回结果信息

            ReturnResult result = new ReturnResult()
            {
                Code = 0,
                Msg = "获取成功",
                Data = list,
                IsSuccess = true,
                Count = count

            };
            //返回结果
            return new JsonHelper(result);
        }

        #endregion

        #region 作废处理
        [HttpPost]
        public ActionResult UpdateGetWorkFlow_Instance(string Id)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(Id))
            {
                result.Msg = "实例Id不能为空！";
                return new JsonHelper(result);
            }
            string msg;
            result.IsSuccess = _workFlow_InstanceBLL.UpdateGetWorkFlow_Instance(Id,out msg);
            result.Msg = msg;
            result.Code = result.IsSuccess == true ? 200:result.Code;
            return new JsonHelper(result);

        }
        #endregion
    }
}