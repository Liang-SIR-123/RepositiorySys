using IBLL;
using IDAL;
using Models;
using Models.DTO;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class WorkFlow_InstanceBLL : IWorkFlow_InstanceBLL
    {
        private RepositorySysData _RepositorySys;
        private IWorkFlow_InstanceDAL _workFlow_InstanceDAL;
        //private readonly int WorkFlow_Instancenums;

        public WorkFlow_InstanceBLL(RepositorySysData RepositorySys, IWorkFlow_InstanceDAL workFlow_InstanceDAL)
        {
            _RepositorySys = RepositorySys;
            _workFlow_InstanceDAL = workFlow_InstanceDAL;
        }

        #region 申领方法
        /// <summary>
        /// 申领方法
        /// </summary>
        /// <param name="workFlow"></param>
        /// <param name="userId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CreateWorkFlow_Instance(WorkFlow_Instance workFlow, string userId, out string msg)
        {
            //开启回滚
            using (var transaction = _RepositorySys.Database.BeginTransaction())
            {
                try
                {
                    //创建工作流实例
                    WorkFlow_Instance _workFlow = new WorkFlow_Instance()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Description = workFlow.Description,
                        Creator = userId,
                        ModelId = workFlow.ModelId,
                        OutGoodsId = workFlow.OutGoodsId,
                        CreateTime = DateTime.Now,
                        OutNum = workFlow.OutNum,
                        Reason = workFlow.Reason,
                        Status = (int)WorkFlow_InstanceStatusEnums.审核中//枚举类
                    };
                    _RepositorySys.WorkFlow_Instance.Add(_workFlow);//添加到数据库
                    bool s = _RepositorySys.SaveChanges() > 0;//判断受影响的行数
                    if (s == false)
                    {
                        transaction.Rollback();
                        msg = "发起申领失败！";
                        return false;
                    }
                    UserInfo userInfo = _RepositorySys.UserInfo.FirstOrDefault(u => u.Id == userId && u.IsDelete == false);
                    if (userInfo == null || string.IsNullOrWhiteSpace(userInfo.Id))
                    {
                        transaction.Rollback();
                        msg = "当前申请的用户没有部门！";
                        return false;
                    }
                    DepartmentInfo department = _RepositorySys.DepartmentInfo.FirstOrDefault(d => d.Id == userInfo.DepartmentId && d.IsDelete == false);
                    if (department == null || string.IsNullOrWhiteSpace(department.LeaderId))
                    {
                        transaction.Rollback();
                        msg = "当前部门没有领导！";
                        return false;
                    }



                    int count = (from ur in _RepositorySys.R_UserInfo_RoleInfo.Where(r => r.UserId == department.LeaderId)
                                 join r in _RepositorySys.RoleInfo.Where(r => r.RoleName == "部门经理")
                                 on ur.RoleId equals r.Id
                                 select r.RoleName).Count();
                    if (count <= 0)
                    {
                        transaction.Rollback();
                        msg = "所属部门的领导不是部门经理这个角色！";
                        return false;
                    }
                    //实例化工作流步骤
                    WorkFlow_InstanceStep workFlow_InstanceStep = new WorkFlow_InstanceStep()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreateTime = DateTime.Now,
                        InstanceId = _workFlow.Id,
                        ReviewerId = department.LeaderId,
                        ReviewStatus = (int)WorkFlow_InstanceStepEnums.审核中,

                    };
                    _RepositorySys.WorkFlow_InstanceStep.Add(workFlow_InstanceStep);
                    bool b2 = _RepositorySys.SaveChanges() > 0;
                    if (b2 == false)
                    {
                        transaction.Rollback();
                        msg = "工作流步骤发起失败！";
                        return false;
                    }
                    msg = "申请成功！";
                    transaction.Commit();//提交事务
                    return true;


                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    msg = "出错了！" + ex.Message;
                    return false;
                }
            }

        }
        #endregion

        public List<GetWorkFlow_InstanceDTO> workFlow_InstancesList(int page, int limit, string userId, int status, out int count)
        {
            //Linq 语句查询
            var data = from d in _RepositorySys.WorkFlow_Instance.Where(w => w.Creator == userId)
                       join u in _RepositorySys.UserInfo.Where(u => !u.IsDelete)
                       on d.Creator equals u.Id
                       into tempdu
                       from du in tempdu.DefaultIfEmpty()

                       join m in _RepositorySys.WorkFlow_Model.Where(w => !w.IsDelete)
                       on d.ModelId equals m.Id
                       into tempdm
                       from dm in tempdm.DefaultIfEmpty()

                       join c in _RepositorySys.ConsumableInfo.Where(w => !w.IsDelete)
                       on d.OutGoodsId equals c.Id
                       into tempdc
                       from dc in tempdc.DefaultIfEmpty()

                           //实例化
                       select new GetWorkFlow_InstanceDTO
                       {
                           ModelId = dm.Id,
                           ModelName = dm.Title,
                           Description = d.Description,
                           //Status = d.Status

                           Status = d.Status/*==(int)WorkFlow_InstanceStepEnums.审核中?
                           "审核中": d.Status == (int)WorkFlow_InstanceStepEnums.同意 ? 
                           "同意": d.Status == (int)WorkFlow_InstanceStepEnums.驳回 ?
                           "驳回": d.Status == (int)WorkFlow_InstanceStepEnums.作废 ?
                           "作废": d.Status == (int)WorkFlow_InstanceStepEnums.已被他人审核 ?
                           "已被他人审核":""*/,
                           Reason = d.Reason,
                           Creator = du.UserName,
                           OutNum = d.OutNum,
                           OutGoodsId = dc.Id,
                           OutGoodsName = dc.ConsumableName,
                           Id = d.Id,
                           CreateTime = d.CreateTime
                           //这个Linq查询语句相当于左右链接链表查询,会显示null
                       };
            if (status > 0)
            {
                data = data.Where(x => x.Status == status);
            }

            count = data.Count();
            //分页
            var listPage = data.OrderByDescending(d => d.CreateTime).Skip(limit * (page - 1)).Take(limit).ToList();
            return listPage;
        }

        #region 作废处理
        public bool UpdateGetWorkFlow_Instance(string Id,out string msg)
        {
            using (var trancsation = _RepositorySys.Database.BeginTransaction())
            {
                try
                {
                    WorkFlow_Instance oldEntity = _RepositorySys.WorkFlow_Instance.FirstOrDefault(x => x.Id == Id);
                    if (oldEntity == null)
                    {
                        trancsation.Rollback();
                        msg = "查不到该实例";
                        return false;
                    }
                    if (oldEntity.Status != (int)WorkFlow_InstanceStatusEnums.审核中)
                    {
                        trancsation.Rollback();
                        msg = "该实例状态不可作废";
                        return false;
                    }
                    //更改状态
                    oldEntity.Status = (int)WorkFlow_InstanceStatusEnums.拒绝;
                    _RepositorySys.Entry(oldEntity).State = System.Data.Entity.EntityState.Modified;
                    bool IsSuccess = _RepositorySys.SaveChanges() > 0;
                    if (IsSuccess == false)
                    {
                        trancsation.Rollback();
                        msg = "该实例状态更新失败！";
                        return false;
                    }
                    //查询该工作实例的所以工作步骤流
                    List<WorkFlow_InstanceStep> list = _RepositorySys.WorkFlow_InstanceStep.Where(x => x.InstanceId == oldEntity.Id).ToList();
                    foreach (var item in list)
                    {
                        item.ReviewStatus = (int)WorkFlow_InstanceStepEnums.作废;
                        _RepositorySys.Entry(item).State = System.Data.Entity.EntityState.Modified;
                         IsSuccess = _RepositorySys.SaveChanges() > 0;
                        if (IsSuccess == false)
                        {
                            trancsation.Rollback();
                            msg = "该实例状态更新失败！";
                            return false;
                        }
                    }
                    trancsation.Commit();
                    msg = "实例作废成功！";
                    return true;
                }
                catch (Exception ex)
                {
                    trancsation.Rollback();
                    msg = ex.Message;
                    return false;
                }
            }
        }

        #endregion
    }
}
