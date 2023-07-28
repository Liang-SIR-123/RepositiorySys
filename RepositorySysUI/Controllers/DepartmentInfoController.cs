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
    public class DepartmentInfoController : Controller
    {
        private IDepartmentInfoBLL _departmentInfo;
        public DepartmentInfoController(IDepartmentInfoBLL departmentInfo)
        {
            _departmentInfo = departmentInfo;
        }
        // GET: DepartmentInfo
        public ActionResult ListView()
        {
            return View();
        }
        
        #region 查询列表
        public ActionResult GetDepartmentInfo(int page, int limit, string departmentName)
        {
            int count;
            List<GetDepartmentInfo> list = _departmentInfo.getDepartmentInfos(page, limit, departmentName, out count);
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

        #region 添加部门
        public ActionResult CreateDpartmentView()
        {
            return View();
        }
        public ActionResult CreateDepartment([Form] DepartmentInfo department)
        {
            string msg;
            bool isSuccess = _departmentInfo.CreateDepartmentInfo(department, out msg);
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


        #region 下拉框数据
        public ActionResult GetSelect()
        {
            ReturnResult result = new ReturnResult();
            var data = _departmentInfo.GetSelectOptions();
            result.Code = 200;
            result.Msg = "获取成功！";
            result.IsSuccess = true;
            result.Data = data;
            return new JsonHelper(result);

        }
        #endregion

        #region 根据id软删除
        [HttpPost]
        public ActionResult DeleteDepartment(string Id)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(Id))
            {
                result.Msg = "Id不能为空";
                return new JsonHelper(result);
            }
            bool isSuccess = _departmentInfo.DeleteDepartment(Id);
            if (isSuccess)
            {

                result.Msg = "删除部门成功！";
                result.Code = 200;
            }
            return new JsonHelper(result);
        }
        #region 根据Id批量软删除
        public ActionResult DeleteDepartments(List<string> Ids)
        {
            
            ReturnResult result = new ReturnResult();
            if (Ids == null || Ids.Count == 0)
            {
                result.Msg = "你还没有选择要删除的部门！";
                return new JsonHelper(result);
            }
            bool IsSuccess = _departmentInfo.DeleteDepartments(Ids);
            
            if (IsSuccess==false)
            {
                result.Msg = "删除失败！";
                result.IsSuccess = IsSuccess;
                result.Code = 501;
            }
            result.Msg= "删除成功！";
            result.IsSuccess = IsSuccess;
            result.Code = 200;
            return new JsonHelper(result);
        }
        #endregion


        #endregion
        #region 根据Id获取部门详情
        public ActionResult UpdateDepartment()
        {
            return View();
        }
        /// <summary>
        /// 显示修改信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateDepartmentInfos(string Id)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrWhiteSpace(Id))
            {
                result.Msg = "Id不能为空";
                return new JsonHelper(result);
            }
            var department = _departmentInfo.UpdateDepartment(Id);
            if (department == null)
            {
                result.Msg = "未能获取到部门信息！";
            }
            else
            {
                var select = _departmentInfo.GetSelectOptions();
                result.Code = 200;
                result.Msg = "查询成功！";
                result.Data = new
                {
                    department,
                    select
                };
            }
            return new JsonHelper(result);
        }
        #endregion
        #region 更新部门
        public ActionResult UpdateDepartmentInfo([Form] DepartmentInfo department)
        {
            ReturnResult result = new ReturnResult();
            string msg;
            bool isSuccess = _departmentInfo.UpdateDepartments(department,out msg);
            result.Msg = msg;
            if (isSuccess)
            {
                result.IsSuccess = isSuccess;
                result.Code = 200;
            }
            return new JsonHelper(result);
        }
        #endregion
    }
}