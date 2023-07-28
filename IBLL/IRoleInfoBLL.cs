using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    public interface IRoleInfoBLL
    {
        List<GetRoleInfo> getRoleInfos(int page, int limit, string departmentName, out int count);
        bool CreateRole(RoleInfo role, out string msg);
        object GetRole(string Id);
        bool UpdateRoleInfo(RoleInfo role, out string msg);
        bool DeleteRole(string Id, out string msg);

        bool DeleteRoles(List<string> Ids, out string msg);
        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        List<string> GetR_UserInfo_RoleInfo(string roleId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        bool BindUserInfo(List<string> userId, string roleId);
        List<string> GetR_RoleInfo_MenuInfo(string roleId);

        bool BindMenuInfo(List<string> userIds, string roleId);
    }
}
