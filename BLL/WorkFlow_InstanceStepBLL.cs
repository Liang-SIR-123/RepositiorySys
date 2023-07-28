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
    public class WorkFlow_InstanceStepBLL : IWorkFlow_InstanceStepBLL
    {
        private RepositorySysData _repositorySys;
        //private IWorkFlow_InstanceStepDAL _workFlow_InstanceStepDAL
        public WorkFlow_InstanceStepBLL(RepositorySysData repositorySys)
        {
            _repositorySys = repositorySys;
        }

        #region 列表
        public List<GetWorkFlow_InstanceStepDTO> WorkFlow_InstanceStepList(int page, int limit, string userId, int status, out int count)
        {
            //Linq 语句查询
            var data = from ws in _repositorySys.WorkFlow_InstanceStep.Where(wi => wi.ReviewerId == userId && wi.ReviewStatus != 4)
                       join u in _repositorySys.WorkFlow_Instance //实例表
                       on ws.InstanceId equals u.Id
                       into tempdu
                       from du in tempdu.DefaultIfEmpty()

                       join c in _repositorySys.ConsumableInfo //耗材表
                       on du.OutGoodsId equals c.Id
                       into tempduc
                       from duc in tempduc.DefaultIfEmpty()

                       join ui in _repositorySys.UserInfo.Where(wm => !wm.IsDelete) //用户表
                       on du.Creator equals ui.Id
                       into tempdui
                       from dui in tempdui.DefaultIfEmpty()

                       join m in _repositorySys.WorkFlow_Model.Where(wm => !wm.IsDelete) //模板
                       on du.ModelId equals m.Id
                       into tempdm
                       from dm in tempdm.DefaultIfEmpty()

                       join u2 in _repositorySys.UserInfo.Where(wm => !wm.IsDelete) //用户
                      on ws.ReviewerId equals u2.Id
                      into tempdwsu
                       from wsu in tempdwsu.DefaultIfEmpty()

                           //实例化
                       select new GetWorkFlow_InstanceStepDTO
                       {
                           InstanceId = du.Id,
                           InstanceName = dm.Title,
                           ReviewerId = wsu.Id,
                           ReviewerName = wsu.UserName,
                           ReviewStatus = ws.ReviewStatus,
                           ReviewReason = ws.ReviewReason,
                           ReviewTime = ws.ReviewTime,
                           ReviewNum = du.OutNum,
                           ReviewGoods = duc.ConsumableName,
                           Id = ws.Id,
                           CreateTime = ws.CreateTime,
                           applyIdName = dui.UserName,
                           applyId = dui.Id,
                           BeforeStepId = ws.BeforeStepId,
                           Reason = du.Reason
                           //这个Linq查询语句相当于左右链接链表查询,会显示null
                       };
            if (status > 0)
            {
                data = data.Where(x => x.ReviewStatus == status);
            }

            count = data.Count();
            //分页
            var listPage = data.OrderByDescending(d => d.CreateTime).Skip(limit * (page - 1)).Take(limit).ToList();
            return listPage;
        }
        #endregion

        public bool UpdateWorkFlow_InstanceStep(string Id, string reviewReason, string userId, int outNum, WorkFlow_InstanceStepEnums reviewStatus, out string msg)
        {

            using (var trancsation = _repositorySys.Database.BeginTransaction())
            {
                try
                {
                    #region 
                    //先处理自己要审核的那条工作流步骤
                    //获取步骤数据
                    WorkFlow_InstanceStep workFlow_ = _repositorySys.WorkFlow_InstanceStep.FirstOrDefault(x => x.Id == Id);
                    if (workFlow_ == null)
                    {
                        trancsation.Rollback();
                        msg = "未找到该工作步骤";
                        return false;
                    }
                    if (workFlow_.ReviewerId != userId)
                    {
                        trancsation.Rollback();
                        msg = "你没有权限审核";
                        return false;
                    }
                    workFlow_.ReviewReason = reviewReason;//审核理由
                    workFlow_.ReviewStatus = (int)reviewStatus;//审核状态
                    workFlow_.ReviewTime = DateTime.Now;//审核时间
                    _repositorySys.Entry(workFlow_).State = System.Data.Entity.EntityState.Modified;
                    bool isSuccess = _repositorySys.SaveChanges() > 0;
                    if (isSuccess == false)
                    {
                        trancsation.Rollback();
                        msg = "审核失败！";
                        return false;
                    }
                    #endregion

                    List<string> roleName = (from ur in _repositorySys.R_UserInfo_RoleInfo.Where(x => x.UserId == userId)
                                             join r in _repositorySys.RoleInfo
                                             on ur.RoleId equals r.Id
                                             select r.RoleName).ToList();
                    if (roleName.Contains("部门经理"))
                    {
                        if (reviewStatus == WorkFlow_InstanceStepEnums.同意)
                        {
                            List<string> checkUserIds = (from r in _repositorySys.RoleInfo.Where(x => x.RoleName == "仓库管理员")
                                                         join ur in _repositorySys.R_UserInfo_RoleInfo
                                                         on r.Id equals ur.RoleId
                                                         select ur.UserId).ToList();
                            //先处理第一个仓库管理员
                            string checkUserId = checkUserIds.FirstOrDefault();
                            if (string.IsNullOrEmpty(checkUserId))
                            {
                                msg = "找不到仓库管理员";
                                trancsation.Rollback();
                                return false;
                            }
                            WorkFlow_InstanceStep _InstanceStep = new WorkFlow_InstanceStep()
                            {
                                Id = Guid.NewGuid().ToString(),
                                BeforeStepId = Id,//上一个步骤
                                InstanceId = workFlow_.InstanceId,
                                CreateTime = DateTime.Now,
                                
                                ReviewerId = checkUserId,
                                ReviewStatus = (int)WorkFlow_InstanceStepEnums.审核中
                            };
                            _repositorySys.WorkFlow_InstanceStep.Add(_InstanceStep);
                            isSuccess = _repositorySys.SaveChanges() > 0;
                            if (isSuccess == false)
                            {
                                trancsation.Rollback();
                                msg = "仓库管理员创建步骤失败！";
                                return false;
                            }

                        }
                        else if (reviewStatus == WorkFlow_InstanceStepEnums.驳回)
                        {
                            //第一步，查询员工Id，根据步骤记录的实例id查询实例表
                            WorkFlow_Instance _Instance = _repositorySys.WorkFlow_Instance.FirstOrDefault(x => x.Id == workFlow_.InstanceId);
                            if (_Instance == null)
                            {
                                trancsation.Rollback();
                                msg = "查询不到工作流实例";
                                return false;
                            }
                            //第二步，给普通员工创建工作流步骤
                            WorkFlow_InstanceStep instanceStep = new WorkFlow_InstanceStep()
                            {
                                Id = Guid.NewGuid().ToString(),
                                BeforeStepId = Id,//上一个步骤id
                                InstanceId = workFlow_.InstanceId,//实例id
                                CreateTime = DateTime.Now,
                                ReviewReason= workFlow_.ReviewReason,
                                ReviewerId = _Instance.Creator,//申请人id(申请人)
                                ReviewStatus = (int)WorkFlow_InstanceStepEnums.审核中,

                            };
                            _repositorySys.WorkFlow_InstanceStep.Add(instanceStep);
                            isSuccess = _repositorySys.SaveChanges() > 0;
                            if (isSuccess == false)
                            {
                                trancsation.Rollback();
                                msg = "驳回失败！";
                                return false;
                            }

                        }
                        else
                        {
                            trancsation.Rollback();
                            msg = "操作错误！";
                            return false;
                        }
                    }
                    else if (roleName.Contains("仓库管理员"))
                    {

                        if (reviewStatus == WorkFlow_InstanceStepEnums.同意)
                        {
                            //第一步，结束工作流实例
                            WorkFlow_Instance _Instance = _repositorySys.WorkFlow_Instance.FirstOrDefault(x => x.Id == workFlow_.InstanceId);

                            if (_Instance == null)
                            {
                                trancsation.Rollback();
                                msg = "找不到工作流实例";
                                return false;
                            }
                            _Instance.Status = (int)WorkFlow_InstanceStatusEnums.结束;//修改工作流实例
                            _repositorySys.Entry(_Instance).State = System.Data.Entity.EntityState.Modified;
                            isSuccess = _repositorySys.SaveChanges() > 0;
                            if (isSuccess == false)
                            {
                                trancsation.Rollback();
                                msg = "结束工作流实例失败";
                                return false;
                            }
                            //第二步，减少耗材库存
                            ConsumableInfo consumable = _repositorySys.ConsumableInfo.FirstOrDefault(x => x.Id == _Instance.OutGoodsId);
                            if (consumable == null)
                            {
                                trancsation.Rollback();
                                msg = "耗材不存在！";
                                return false;
                            }
                            //判断库存是否充足
                            if (consumable.Num - _Instance.OutNum < 0)
                            {
                                trancsation.Rollback();
                                msg = "耗材库存不足！";
                                return false;
                            }
                            //扣库存
                            consumable.Num -= _Instance.OutNum;
                            _repositorySys.Entry(consumable).State = System.Data.Entity.EntityState.Modified;
                            isSuccess = _repositorySys.SaveChanges() > 0;
                            if (isSuccess == false)
                            {
                                trancsation.Rollback();
                                msg = "库存耗材减少失败！";
                                return false;
                            }
                            //新增耗材记录表（出库信息）
                            ConsumableRecord record = new ConsumableRecord()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ConsumableId = consumable.Id,
                                CreateTime = DateTime.Now,
                                Creator = userId,
                                Num = _Instance.OutNum,
                                Type = (int)ConsumableRecordTypeEnums.出库

                            };
                            _repositorySys.ConsumableRecord.Add(record);
                            isSuccess = _repositorySys.SaveChanges() > 0;
                            if (isSuccess == false)
                            {
                                trancsation.Rollback();
                                msg = "创建出库失败！";
                                return false;
                            }
                        }
                        else if (reviewStatus == WorkFlow_InstanceStepEnums.驳回)
                        {
                            //找到上一个步骤的审核人是谁
                            WorkFlow_InstanceStep oldwork = _repositorySys.WorkFlow_InstanceStep.FirstOrDefault(x => x.Id == workFlow_.BeforeStepId);
                            if (oldwork == null)
                            {
                                trancsation.Rollback();
                                msg = "无法找到上一个步骤！";
                                return false;
                            }
                            //给上一级步骤的审核人创建一条新的工作步骤，通知部门经理重新审核
                            WorkFlow_InstanceStep newwork = new WorkFlow_InstanceStep()
                            {
                                Id = Guid.NewGuid().ToString(),
                                BeforeStepId = Id,//上一个步骤Id
                                CreateTime = DateTime.Now,
                                InstanceId = workFlow_.InstanceId,//实例的id
                                ReviewerId = oldwork.ReviewerId,//审核人Id（申请人）
                                ReviewReason = oldwork.ReviewReason,
                                ReviewStatus = (int)WorkFlow_InstanceStepEnums.审核中
                            };
                            _repositorySys.WorkFlow_InstanceStep.Add(newwork);
                            isSuccess = _repositorySys.SaveChanges() > 0;
                            if (isSuccess == false)
                            {
                                trancsation.Rollback();
                                msg = "驳回失败！";
                                return false;
                            }
                        }
                        else
                        {
                            trancsation.Rollback();
                            msg = "你操作错误！";
                            return false;
                        }
                    }
                    else if (roleName.Contains("普通员工"))
                    {
                        //第一步
                        WorkFlow_Instance flow_Instance = _repositorySys.WorkFlow_Instance.FirstOrDefault(x => x.Id == workFlow_.InstanceId);
                        if (flow_Instance == null)
                        {
                            trancsation.Rollback();
                            msg = "找不到工作流实例！";
                            return false;
                        }
                        flow_Instance.OutNum = outNum;
                        _repositorySys.Entry(flow_Instance).State = System.Data.Entity.EntityState.Modified;
                        isSuccess = _repositorySys.SaveChanges() > 0;
                        if (isSuccess == false)
                        {
                            trancsation.Rollback();
                            msg = "修改工作流实例申请耗材数量失败！";
                            return false;
                        }
                        //第二步，添加到新的工作流步骤给部门经理（通知他重新审核）
                        WorkFlow_InstanceStep oldwork = _repositorySys.WorkFlow_InstanceStep.FirstOrDefault(x => x.Id == workFlow_.BeforeStepId);
                        if (oldwork == null)
                        {
                            trancsation.Rollback();
                            msg = "找不到上一个工作流步骤！";
                            return false;
                        }
                        //新建工作流步骤给部门经理
                        WorkFlow_InstanceStep newwork = new WorkFlow_InstanceStep()
                        {
                            Id = Guid.NewGuid().ToString(),
                            BeforeStepId = Id,//上一个步骤Id
                            CreateTime = DateTime.Now,
                            InstanceId = workFlow_.InstanceId,//实例的id
                            ReviewerId = oldwork.ReviewerId,//审核人Id（申请人）
                            ReviewStatus = (int)WorkFlow_InstanceStepEnums.审核中
                        };
                        _repositorySys.WorkFlow_InstanceStep.Add(newwork);
                        isSuccess = _repositorySys.SaveChanges() > 0;
                        if (isSuccess == false)
                        {
                            trancsation.Rollback();
                            msg = "审核失败！（修改工作流步骤失败）";
                            return false;
                        }
                    }
                    else
                    {
                        trancsation.Rollback();
                        msg = "你操作错误！";
                        return false;
                    }
                    trancsation.Commit();
                    msg = "审核成功！";
                    return true;

                }
                catch (Exception ex)
                {

                    msg=ex.Message;
                    trancsation.Rollback();
                    return false;
                }




            }
        }
    }
}
