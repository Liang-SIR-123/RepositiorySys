using DAL;
using IBLL;
using IDAL;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RoleInfoBLL : IRoleInfoBLL
    {
        private RepositorySysData _repositorySys;
        private IRoleInfoDAL _RoleInfoDAL;
        private IR_UserInfo_RoleInfoDAL _R_UserInfo_RoleInfoDAL;//用户角色表数据访问
        private IR_RoleInfo_MenuInfoDAL _R_RoleInfo_MenuInfoDAL;
        public RoleInfoBLL(RepositorySysData repositorySys, IRoleInfoDAL RoleInfoDAL, IR_UserInfo_RoleInfoDAL R_UserInfo_RoleInfoDAL, IR_RoleInfo_MenuInfoDAL R_RoleInfo_MenuInfoDAL)
        {
            _repositorySys = repositorySys;
            _RoleInfoDAL = RoleInfoDAL;
            _R_UserInfo_RoleInfoDAL = R_UserInfo_RoleInfoDAL;
            _R_RoleInfo_MenuInfoDAL=R_RoleInfo_MenuInfoDAL;
        }
        #region 角色查询
        public List<GetRoleInfo> getRoleInfos(int page, int limit, string RoleName, out int count)
        {
            //用户表
            var RoleInfoList = _RoleInfoDAL.GetEntity().Where(u => u.IsDelete == false).ToList();


            //模糊查询姓名
            if (!string.IsNullOrWhiteSpace(RoleName))
            {
                RoleInfoList = RoleInfoList.Where(u => u.RoleName.Contains(RoleName)).ToList();
            }
            count = RoleInfoList.Count();//返回总数
                                         //分页
            var listPage = RoleInfoList.OrderByDescending(u => u.CreateTime)
                .Skip(limit * (page - 1))
                .Take(limit)
                .ToList();
            List<GetRoleInfo> list = new List<GetRoleInfo>();
            //部门表
            var departmentList = _RoleInfoDAL.GetEntity().ToList();
            foreach (var item in listPage)
            {

                GetRoleInfo data = new GetRoleInfo()
                {
                    Id = item.Id,
                    CreateTime = item.CreateTime,
                    RoleName = item.RoleName,
                    Description = item.Desciption
                };
                list.Add(data);//;把单个对象添加到返回集合中
            };

            return list;
        }
        #endregion

        #region 添加角色
        public bool CreateRole(RoleInfo role,out string msg)
        {
            if (string.IsNullOrWhiteSpace(role.Desciption))
            {
                msg = "描述不能为空";
                return false;
            }
            if (string.IsNullOrWhiteSpace(role.RoleName))
            {
                msg = "角色名不能为空";
                return false;
            }
            var roles = _RoleInfoDAL.GetEntity().Where(r => !r.IsDelete).ToList();
            RoleInfo roless = roles.FirstOrDefault(r => r.RoleName == role.RoleName);
            if (roless != null)
            {
                msg = "角色名已存在！";
                return false;
            }
            role.CreateTime = DateTime.Now;
            role.Id = Guid.NewGuid().ToString();
            bool IsSuccess = _RoleInfoDAL.CreateEntity(role);
            msg = IsSuccess ? $"添加{role.RoleName}成功！" : "添加失败！";
            return IsSuccess;
        }
        #endregion

        #region 修改角色
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public object GetRole(string Id)
        {
            RoleInfo role= _RoleInfoDAL.GetEntityById(Id);
            return role ;
        }
        /// <summary>
        /// 对角色进行修改
        /// </summary>
        /// <param name="role"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UpdateRoleInfo(RoleInfo role, out string msg)
        {
            RoleInfo Role = _RoleInfoDAL.GetEntityById(role.Id);
            if (string.IsNullOrWhiteSpace(role.Desciption))
            {
                msg = "描述不能为空";
                return false;
            }
            if (string.IsNullOrWhiteSpace(role.RoleName))
            {
                msg = "角色名不能为空";
                return false;
            }
            if (Role.RoleName != role.RoleName)
            {
                RoleInfo roles = _RoleInfoDAL.GetEntity().FirstOrDefault(r => r.RoleName == role.RoleName);
                if (roles != null)
                {
                    msg = "角色名已存在！";
                    return false;
                }
            }
            Role.RoleName = role.RoleName;
            Role.Desciption = role.Desciption;
            bool isSuccess = _RoleInfoDAL.UpdateEntity(Role);
            msg = isSuccess ? "修改成功" : "修改失败";
            return isSuccess;
        }
        #endregion

        #region 根据Id软删除角色
        public bool DeleteRole(string Id, out string msg)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                msg = "Id不能为空！";
                return false;
            }
            RoleInfo role = _RoleInfoDAL.GetEntityById(Id);

            role.IsDelete = true;
            role.DeleteTime = DateTime.Now;
            bool IsSuccess = _RoleInfoDAL.UpdateEntity(role);
            msg = IsSuccess ? $"删除{role.RoleName}成功！" : "删除失败！";
            return IsSuccess;
        }
        #endregion

        #region 根据Id批量软删除角色
        public bool DeleteRoles(List< string> Ids, out string msg)
        {
            var role = _RoleInfoDAL.GetEntity().Where(r => Ids.Contains(r.Id)).ToList();
            foreach (var item in Ids)
            {
                RoleInfo Role = _RoleInfoDAL.GetEntityById(item);
                if (Role == null)
                {
                    continue;
                }
                Role.IsDelete = true;
                Role.DeleteTime = DateTime.Now;
                 _RoleInfoDAL.UpdateEntity(Role);
            }
            msg = "删除成功！";
            return true;
        }
        #endregion

        #region 获取用户已经绑定
        public List<string> GetR_UserInfo_RoleInfo(string roleId)
        {
            //查询当前角色已经绑定的用户Id
            List<string> UserId = _R_UserInfo_RoleInfoDAL.GetEntity()
                                                        .Where(x => x.RoleId == roleId)
                                                        .Select(x => x.UserId)
                                                        .ToList();

            return UserId;

        }


        #endregion

        #region 绑定用户角色的方法、解绑的方法
        public bool BindUserInfo(List<string> userIds, string roleId)
        {
            List<R_UserInfo_RoleInfo> BindUsered = _R_UserInfo_RoleInfoDAL.GetEntity().Where(u => u.RoleId == roleId).ToList();
            #region 解绑
            foreach (var item in BindUsered)
            {
                bool ishas = userIds==null? false: userIds.Any(x => x == item.UserId);
                if (!ishas)
                {
                    _R_UserInfo_RoleInfoDAL.DeleteFntity(item);
                }

            }
            #endregion

            #region 绑定
            if (userIds == null)
            {
                return false;
            }
            //List<R_UserInfo_RoleInfo> BindUsered = _R_UserInfo_RoleInfoDAL.GetEntity().Where(u => u.RoleId == roleId).ToList();
            foreach (var item in userIds)
            {
                // var user = BindUsered.FirstOrDefault(u => u.RoleId==roleId);
                bool isHas = BindUsered.Any(x => x.UserId == item);
                if (!isHas)
                {
                    R_UserInfo_RoleInfo entity = new R_UserInfo_RoleInfo()
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoleId = roleId,
                        UserId = item,
                        CreateTime = DateTime.Now
                    };
                    _R_UserInfo_RoleInfoDAL.CreateEntity(entity);
                }
            }
            return true;
            #endregion

        }
        #endregion

        #region 查询R_RoleInfo_MenuInfo表
        public List<string> GetR_RoleInfo_MenuInfo(string roleId)
        {
            //查询当前角色已经绑定的用户Id
            List<string> MenuId = _R_RoleInfo_MenuInfoDAL.GetEntity()
                                                        .Where(x => x.RoleId == roleId)
                                                        .Select(x => x.MenuId)
                                                        .ToList();

            return MenuId;

        }
        #endregion

        #region 菜单的绑定、解绑
        public bool BindMenuInfo(List<string> menuIds, string roleId)
        {
            List<R_RoleInfo_MenuInfo> Bindmenu = _R_RoleInfo_MenuInfoDAL.GetEntity().Where(u => u.RoleId == roleId).ToList();
            #region 解绑
            foreach (var item in Bindmenu)
            {
                bool ishas = menuIds == null ? false : menuIds.Any(x => x == item.MenuId);
                if (!ishas)
                {
                    _R_RoleInfo_MenuInfoDAL.DeleteFntity(item);
                }

            }
            #endregion

            #region 绑定
            if (menuIds == null)
            {
                return false;
            }
            //List<R_UserInfo_RoleInfo> BindUsered = _R_UserInfo_RoleInfoDAL.GetEntity().Where(u => u.RoleId == roleId).ToList();
            foreach (var item in menuIds)
            {
                // var user = BindUsered.FirstOrDefault(u => u.RoleId==roleId);
                bool isHas = Bindmenu.Any(x => x.MenuId == item);
                if (!isHas)
                {
                    R_RoleInfo_MenuInfo entity = new R_RoleInfo_MenuInfo()
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoleId = roleId,
                        MenuId = item,
                        CreateTime = DateTime.Now
                    };
                    _R_RoleInfo_MenuInfoDAL.CreateEntity(entity);
                }
            }
            return true;
            #endregion

        }
        #endregion


    }
}
