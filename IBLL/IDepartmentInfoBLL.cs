using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
   public interface IDepartmentInfoBLL
    {
        List<GetDepartmentInfo> getDepartmentInfos(int page, int limit, string departmentName, out int count);
        bool CreateDepartmentInfo(DepartmentInfo entity, out string msg);
        /// <summary>
        /// 返回父级部门和领导信息
        /// </summary>
        /// <returns></returns>
        object GetSelectOptions();
        bool DeleteDepartment(string Id);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool DeleteDepartments(List<string> ids);
        /// <summary>
        /// 根据Id获取部门详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        DepartmentInfo UpdateDepartment(string Id);
        bool UpdateDepartments(DepartmentInfo department,out string msg);
    }
}
