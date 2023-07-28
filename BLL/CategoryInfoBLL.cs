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
public    class CategoryInfoBLL: ICategoryInfoBLL
    {
        private RepositorySysData _repositorySys;
        private CategoryInfoDAL _categoryInfoDAL;
        public CategoryInfoBLL(RepositorySysData repositorySys, CategoryInfoDAL categoryInfoDAL)
        {
            _repositorySys = repositorySys;
            _categoryInfoDAL = categoryInfoDAL;
        }

        #region 获取耗材详细信息
        public List<GetCategoryInfoDTO> getCategoryInfos(int page, int limit, string CategoryName, out int count)
        {
            //用户表
            var CategoryInfoList = _categoryInfoDAL.GetEntity().Where(u => u.IsDelete == false).ToList();


            //模糊查询姓名
            if (!string.IsNullOrWhiteSpace(CategoryName))
            {
                CategoryInfoList = CategoryInfoList.Where(u => u.CategoryName.Contains(CategoryName)).ToList();
            }
            count = CategoryInfoList.Count();//返回总数
                                             //分页
            var listPage = CategoryInfoList.OrderByDescending(u => u.CreateTime)
                .Skip(limit * (page - 1))
                .Take(limit)
                .ToList();
            List<GetCategoryInfoDTO> list = new List<GetCategoryInfoDTO>();
            //部门表
            var departmentList = _categoryInfoDAL.GetEntity().ToList();
            foreach (var item in listPage)
            {

                GetCategoryInfoDTO data = new GetCategoryInfoDTO()
                {
                    Id = item.Id,

                    CategoryName = item.CategoryName,
                    Description = item.Description
                };
                list.Add(data);//;把单个对象添加到返回集合中
            };

            return list;
        }
        #endregion


        #region 添加耗材
        public bool CreateCategoryInfo(Category entity, out string msg)
        {
            if (string.IsNullOrWhiteSpace(entity.CategoryName))
            {
                msg = "耗材名称不能为空！";
                return false;
            }
            

            /*Category department = _categoryInfoDAL.GetEntity().FirstOrDefault(d => d.CategoryName == entity.CategoryName);
            if (department != null)
            {
                msg = "该已存在！";
                return false;
            }*/
            entity.Id = Guid.NewGuid().ToString();
            entity.CreateTime = DateTime.Now;
            //entity.LeaderId= _repositorySys.UserInfo.Where(u=>u.DepartmentId==entity.)
            bool isSuccess = _categoryInfoDAL.CreateEntity(entity);
            msg = isSuccess ? $"添加{entity.CategoryName}成功！" : "添加失败！";
            return isSuccess;
        }
        #endregion

        #region 获取耗材信息

        public object GetCategoryInfos(string Id)
        {
            if (!string.IsNullOrWhiteSpace(Id))
            {
                var  category1 = _categoryInfoDAL.GetEntityById(Id);

                return category1;
            }
            var  category = _repositorySys.Category.Where(c=>!c.IsDelete);

            return category;
        }
        #endregion

        #region 更新耗材信息
        public bool UpdateCategoryInfo(Category category,out string msg)
        {
            Category cate = _categoryInfoDAL.GetEntityById(category.Id);
            if (cate == null)
            {
                msg = "id无效";
                return false;
            }
            if (category.Description == null)
            {
                msg = "描述不能为空！";
                return false;
            }
            if (category.CategoryName == null)
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


            cate.Description = category.Description;
            cate.CategoryName = category.CategoryName;
            

            bool isSuccess = _categoryInfoDAL.UpdateEntity(cate);
            msg = isSuccess ? "修改成功！" : "修改失败";
            return isSuccess;
        }
        #endregion

        #region 根据id软删除
        public bool DeleteCategory(string Id)
        {
            Category category = _categoryInfoDAL.GetEntityById(Id);
            if (category == null)
            {
                return false;
            }
            category.IsDelete = true;
            category.DeleteTime = DateTime.Now;
            return _categoryInfoDAL.UpdateEntity(category);
        }
        #endregion

        #region MyRegion
        public bool DeleteCategorys(List<string> Ids)
        {
            var category = _categoryInfoDAL.GetEntity().Where(u => Ids.Contains(u.Id)).ToList();
            foreach (var item in Ids)
            {
                Category categorylist = category.FirstOrDefault(u => u.Id == item);
              
                if (categorylist == null)
                {
                    continue;
                }
                categorylist.IsDelete = true;
                categorylist.DeleteTime = DateTime.Now;
                _categoryInfoDAL.UpdateEntity(categorylist);
            }
            return true;
        }

        #endregion
    }
}
