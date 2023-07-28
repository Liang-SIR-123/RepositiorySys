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
  public  class WorkFlow_ModelBLL: IWorkFlow_ModelBLL
    {
        private RepositorySysData _repositorySys;
        private IWorkFlow_ModelDAL  _workFlow_ModelDAL;
        public WorkFlow_ModelBLL(RepositorySysData repositorySys, IWorkFlow_ModelDAL workFlow_ModelDAL)
        {
            _repositorySys = repositorySys;
            _workFlow_ModelDAL = workFlow_ModelDAL;
        }

        #region 获取工作流模板信息
        public List<GetWorkFlow_ModelDTO> GetWorkFlow_Model(int page, int limit, string Title, out int count)
        {
            //用户表
            var WorkFlowList = _workFlow_ModelDAL.GetEntity().Where(u => u.IsDelete == false).ToList();


            //模糊查询姓名
            if (!string.IsNullOrWhiteSpace(Title))
            {
                WorkFlowList = WorkFlowList.Where(u => u.Title.Contains(Title)).ToList();
            }
            count = WorkFlowList.Count();//返回总数
                                             //分页
            var listPage = WorkFlowList.OrderByDescending(u => u.CreateTime)
                .Skip(limit * (page - 1))
                .Take(limit)
                .ToList();
            List<GetWorkFlow_ModelDTO> list = new List<GetWorkFlow_ModelDTO>();
            //部门表
            var departmentList = _workFlow_ModelDAL.GetEntity().ToList();
            foreach (var item in listPage)
            {

                GetWorkFlow_ModelDTO data = new GetWorkFlow_ModelDTO()
                {
                    Id=item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    CreateTime=item.CreateTime
                    
                };
                list.Add(data);//;把单个对象添加到返回集合中
            };

            return list;
        }
        #endregion


        #region 添加工作流模板
        public bool CreateWorkFlow(WorkFlow_Model entity, out string msg)
        {
            if (string.IsNullOrWhiteSpace(entity.Title))
            {
                msg = "耗材名称不能为空！";
                return false;
            }
            entity.Id = Guid.NewGuid().ToString();
            entity.CreateTime = DateTime.Now;
            //entity.LeaderId= _repositorySys.UserInfo.Where(u=>u.DepartmentId==entity.)
            bool isSuccess = _workFlow_ModelDAL.CreateEntity(entity);
            msg = isSuccess ? $"添加{entity.Title}成功！" : "添加失败！";
            return isSuccess;
        }
        #endregion

        #region 获取工作流模板信息

        public object getWorkFlow(string Id)
        {
            if (!string.IsNullOrWhiteSpace(Id))
            {
                var category1 = _workFlow_ModelDAL.GetEntityById(Id);

                return category1;
            }
            var category = _workFlow_ModelDAL.GetEntity().Where(c => !c.IsDelete).Select(m=>new
            {
                value = m.Id,
                title = m.Title
            }).ToList();

            return category;
        }
        #endregion

        #region 更新工作流模板信息
        public bool UpdateWorkFlow(WorkFlow_Model workFlow, out string msg)
        {
            WorkFlow_Model cate = _workFlow_ModelDAL.GetEntityById(workFlow.Id);
            if (cate == null)
            {
                msg = "id无效";
                return false;
            }
            
            if (workFlow.Title == null)
            {
                msg = "名称不能为空！";
                return false;
            }
            /* if (cate.CategoryName != category.CategoryName)
             {
                 Category categorys = _categoryInfoDAL.GetEntity().FirstOrDefault(d => d.CategoryName == category.CategoryName);
                 if (categorys != null)
                 {
                     msg = "已存在该耗材！";
                     return false;
                 }
             }*/


            cate.Description = workFlow.Description;
            cate.Title = workFlow.Title;


            bool isSuccess = _workFlow_ModelDAL.UpdateEntity(cate);
            msg = isSuccess ? "修改成功！" : "修改失败";
            return isSuccess;
        }
        #endregion

        #region 根据id软删除
        public bool DeleteWorkFlow(string Id)
        {
            WorkFlow_Model WorkFlow = _workFlow_ModelDAL.GetEntityById(Id);
            if (WorkFlow == null)
            {
                return false;
            }
            WorkFlow.IsDelete = true;
            WorkFlow.DeleteTime = DateTime.Now;
            return _workFlow_ModelDAL.UpdateEntity(WorkFlow);
        }
        #endregion

        #region 根据id批量软删除
        public bool DeleteWorkFlows(List<string> Ids)
        {
            var workFlow = _workFlow_ModelDAL.GetEntity().Where(u => Ids.Contains(u.Id)).ToList();
            foreach (var item in Ids)
            {
                WorkFlow_Model workFlowlist = workFlow.FirstOrDefault(u => u.Id == item);

                if (workFlowlist == null)
                {
                    continue;
                }
                workFlowlist.IsDelete = true;
                workFlowlist.DeleteTime = DateTime.Now;
                _workFlow_ModelDAL.UpdateEntity(workFlowlist);
            }
            return true;
        }

        #endregion
    }
}
