using DAL;
using IBLL;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// 部门表的业务逻辑层
    /// </summary>
    public class DepartmentInfoBLL : IDepartmentInfoBLL
    {
        private RepositorySysData _repositorySys;
        private DepartmentInfoDAL _dpartmentInfoDAL;
        public DepartmentInfoBLL(RepositorySysData repositorySys, DepartmentInfoDAL dpartmentInfoDAL)
        {
            _repositorySys = repositorySys;
            _dpartmentInfoDAL = dpartmentInfoDAL;
        }
        #region 查询部门列表
        public List<GetDepartmentInfo> getDepartmentInfos(int page, int limit, string departmentName, out int count)
        {
            //Linq 语句查询
            var data = from d in _repositorySys.DepartmentInfo.Where(d => d.IsDelete == false)
                       join u in _repositorySys.UserInfo.Where(u => u.IsDelete == false)
                       on d.LeaderId equals u.Id
                       into temub
                       from ub in temub.DefaultIfEmpty()

                       join d2 in _repositorySys.DepartmentInfo.Where(d2 => d2.IsDelete == false)
                       on d.ParentId equals d2.Id
                       into temdd
                       from dd in temdd.DefaultIfEmpty()

                           //实例化
                       select new GetDepartmentInfo
                       {
                           Id = d.Id,
                           DepartmentName = d.DepartmentName,
                           Description = d.Description,
                           CreateTime = d.CreateTime,
                           LeaderId = ub.Id,
                           LeaderName = ub.UserName,
                           ParentId = dd.ParentId,
                           ParentName = dd.DepartmentName
                           //这个Linq查询语句相当于左右链接链表查询,会显示null
                       };

            /* var data = from d in _repositorySys.DepartmentInfo.Where(d => d.IsDelete == false)

                            //实例化
                        select new GetDepartmentInfo
                        {
                            Id = d.Id,
                            DepartmentName = d.DepartmentName,
                            Description = d.Description,
                            CreateTime = d.CreateTime,
                            LeaderId = _repositorySys.UserInfo.FirstOrDefault(u=>u.Id==d.LeaderId).Id,
                            LeaderName = _repositorySys.UserInfo.FirstOrDefault(u => u.Id == d.LeaderId).UserName,
                            ParentId = d.ParentId,
                            ParentName = d.DepartmentName

                        };*/
            if (!string.IsNullOrWhiteSpace(departmentName))
            {
                data = data.Where(d => d.DepartmentName.Contains(departmentName));
            }
            count = data.Count();
            //分页
            var listPage = data.OrderByDescending(d => d.CreateTime).Skip(limit * (page - 1)).Take(limit).ToList();
            return listPage;
        }
        #endregion
        #region 添加部门
        public bool CreateDepartmentInfo(DepartmentInfo entity, out string msg)
        {
            if (string.IsNullOrWhiteSpace(entity.DepartmentName))
            {
                msg = "用户姓名不能为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(entity.Description))
            {
                msg = "部门描述不能为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(entity.LeaderId))
            {
                msg = "主管Id不能为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(entity.DepartmentName))
            {
                msg = "父部门Id不能为空！";
                return false;
            }
            DepartmentInfo department = _dpartmentInfoDAL.GetEntity().FirstOrDefault(d => d.DepartmentName == entity.DepartmentName);
            if (department != null)
            {
                msg = "该部门已存在！";
                return false;
            }
            entity.Id = Guid.NewGuid().ToString();
            entity.CreateTime = DateTime.Now;
            //entity.LeaderId= _repositorySys.UserInfo.Where(u=>u.DepartmentId==entity.)
            bool isSuccess = _dpartmentInfoDAL.CreateEntity(entity);
            msg = isSuccess ? $"添加{entity.DepartmentName}成功！" : "添加失败！";
            return isSuccess;
        }
        #endregion
        #region Selete父级部门和领导信息下拉数据
        /// <summary>
        /// 返回数据下拉框
        /// </summary>
        /// <returns></returns>
        public object GetSelectOptions()
        {
            //先查父级部门数据
            var parentSelect = _repositorySys.DepartmentInfo.Where(d => !d.IsDelete)
                                                            .Select(d => new //查询并赋值
                                                            {
                                                                value = d.Id,
                                                                title = d.DepartmentName
                                                            }).ToList();

            //再查领导信息
            var leaderSelect = _repositorySys.UserInfo.Where(u => !u.IsDelete)
                                                        .Select(u => new
                                                        {
                                                            value = u.Id,
                                                            title = u.UserName
                                                        }).ToList();
                                                        
            //最后返回数据
            var data = new
            {
                parentSelect,
                leaderSelect
            };
            return data;
            //简写实例对象
            /*return new
            {
                parentSelect,
                leaderSelect
            };*/
        }
        #endregion

        #region 根据id软删除
        public bool DeleteDepartment(string Id)
        {
            DepartmentInfo department = _dpartmentInfoDAL.GetEntityById(Id);
            if (department == null)
            {
                return false;
            }
            department.IsDelete = true;
            department.DeleteTime = DateTime.Now;
            return _dpartmentInfoDAL.UpdateEntity(department);
        }
        #endregion
        #region 根据Id获取部门详情
        /// <summary>
        /// 根据Id获取部门详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
       public DepartmentInfo UpdateDepartment(string Id)
        {
            return _dpartmentInfoDAL.GetEntityById(Id);
        }
        #endregion
        #region 根据Id批量软删除
        public bool DeleteDepartments(List<string> ids )
        {
            var department = _dpartmentInfoDAL.GetDpartmentInfo().Where(u => ids.Contains(u.Id)).ToList();
            foreach (var item in ids)
            {
                DepartmentInfo userlist = department.FirstOrDefault(u => u.Id == item);
                //DepartmentInfo departmentlist = _dpartmentInfoDAL.GetEntityById(item);
                if (userlist == null)
                {
                    continue;
                }
                userlist.IsDelete = true;
                userlist.DeleteTime = DateTime.Now;
                _dpartmentInfoDAL.UpdateEntity(userlist);
            }
            return true;
        }
        #endregion

        #region 更新部门
        public bool UpdateDepartments(DepartmentInfo department ,out string msg)
        {
            DepartmentInfo depart = _dpartmentInfoDAL.GetEntityById(department.Id);
            if (depart == null)
            {
                msg = "部门id无效";
                return false;
            }
            if (department.Description == null)
            {
                msg = "描述不能为空！";
                return false;
            }
            if (department.DepartmentName == null)
            {
                msg = "部门名称不能为空！";
                return false;
            }
            if (depart.DepartmentName != department.DepartmentName)
            {
                DepartmentInfo departments = _dpartmentInfoDAL.GetEntity().FirstOrDefault(d => d.DepartmentName == department.DepartmentName);
                if (departments != null)
                {
                    msg = "已存在该部门！";
                    return false;
                }
            }
           
            if (department.LeaderId == null)
            {
                msg = "主管人不能为空！";
                return false;
            }
            if (department.ParentId == null)
            {
                msg = "父级部门不能为空！";
                return false;
            }
            depart.Description = department.Description;
            depart.DepartmentName = department.DepartmentName;
            depart.LeaderId = department.LeaderId;
            depart.ParentId = department.ParentId;

            bool isSuccess = _dpartmentInfoDAL.UpdateEntity(depart);
            msg = isSuccess ? "修改成功！" : "修改失败";
            return isSuccess;
        }
        #endregion


    }
}
