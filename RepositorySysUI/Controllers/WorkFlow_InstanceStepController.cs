using Common;
using IBLL;
using Models.DTO;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepositorySysUI.Controllers
{
   
    public class WorkFlow_InstanceStepController : Controller
    {
        private IWorkFlow_InstanceStepBLL  _workFlow_InstanceStepBLL;
        public WorkFlow_InstanceStepController(IWorkFlow_InstanceStepBLL workFlow_InstanceStepBLL)
        {
            _workFlow_InstanceStepBLL = workFlow_InstanceStepBLL;
        }

        #region 获取列表
        // GET: WorkFlow_InstanceStep
        public ActionResult ListView()
        {
            return View();
        }
        
        public ActionResult WorkFlow_InstanceStepList(int page, int limit, int status = 0)
        {
            var userId = HttpContext.Request.Cookies["UserId"];
            //数量总条数
            int count;
            //调用BLL的获取用户方法
            List<GetWorkFlow_InstanceStepDTO> list = _workFlow_InstanceStepBLL.WorkFlow_InstanceStepList(page, limit, userId.Value, status, out count);
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

        #region 编辑申请步骤
        /// <summary>
        /// 编辑申请步骤
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateView()
        {
            return View();
        }
        #endregion

        #region 审核工作流步骤
        [HttpPost]
        public ActionResult UpdateWorkFlow_InstanceStep(string Id, string reviewReason, int outNum, WorkFlow_InstanceStepEnums reviewStatus)
        {
            ReturnResult result = new ReturnResult();
            var userId = HttpContext.Request.Cookies["UserId"];
            if (string.IsNullOrWhiteSpace(Id))
            {
                result.Msg = "工作流步骤不能为空!";
                return new JsonHelper(result);
            }
            
            if (outNum<=0)
            {
                result.Msg = "申请耗材数量不能为空!";
                return new JsonHelper(result);
            }
            if (reviewStatus!=WorkFlow_InstanceStepEnums.同意&&reviewStatus!= WorkFlow_InstanceStepEnums.驳回)
            {
                result.Msg = "状态错误!";
                return new JsonHelper(result);
            }
            string msg;
            result.IsSuccess = _workFlow_InstanceStepBLL.UpdateWorkFlow_InstanceStep(Id, reviewReason,userId.Value, outNum, reviewStatus,out msg);
            result.Msg = msg;
            result.Code = result.IsSuccess ? 200 : result.Code;
            return new JsonHelper(result);
        }
        #endregion

    }
}