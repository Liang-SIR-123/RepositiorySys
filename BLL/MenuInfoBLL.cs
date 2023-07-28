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
    public class MenuInfoBLL : IMenuInfoBLL
    {
        private RepositorySysData _repositorySys;
        private MenuInfoDAL _menuInfoDAL;
        private IR_UserInfo_RoleInfoDAL _UserInfo_RoleInfo;
        private IR_RoleInfo_MenuInfoDAL _RoleInfo_MenuInfoDAL;
        public MenuInfoBLL(RepositorySysData repositorySys, MenuInfoDAL menuInfoDAL,
            IR_UserInfo_RoleInfoDAL UserInfo_RoleInfo, 
            IR_RoleInfo_MenuInfoDAL RoleInfo_MenuInfoDAL)
        {
            _repositorySys = repositorySys;
            _menuInfoDAL = menuInfoDAL;
            _UserInfo_RoleInfo= UserInfo_RoleInfo;
            _RoleInfo_MenuInfoDAL = RoleInfo_MenuInfoDAL;
        }
        #region 查询部门列表
        public List<GetMenuInfo> GetMenuInfo(int page, int limit, string title, out int count)
        {

            //var MenuInfoList = _menuInfoDAL.GetEntity().Where(u => u.IsDelete == false).ToList();
            var MenuInfoList = from M in _repositorySys.MenuInfo.Where(u => u.IsDelete == false)
                               join m in _repositorySys.MenuInfo.Where(u => u.IsDelete == false)
                               on M.ParentId equals m.Id
                               into Menu
                               from Mm in Menu.DefaultIfEmpty()
                               select new GetMenuInfo
                               {
                                   Id = M.Id,
                                   CreateTime = M.CreateTime,
                                   Title = M.Title,
                                   Description = M.Desrciption,
                                   Target = M.Target,
                                   Sort = M.Sort,
                                   ParentId = Mm.Id,
                                   Level = M.Level,
                                   Icon = M.Icon,
                                   Href = M.Href,
                                   ParentName = Mm.Title,

                               };
            //模糊查询姓名
            if (!string.IsNullOrWhiteSpace(title))
            {
                MenuInfoList = MenuInfoList.Where(u => u.Title.Contains(title));
            }
            count = MenuInfoList.Count();//返回总数
                                         //分页
            var listPage = MenuInfoList.OrderByDescending(u => u.CreateTime)
                .Skip(limit * (page - 1))
                .Take(limit)
                .ToList();
            /* List<GetMenuInfo> list = new List<GetMenuInfo>();
             foreach (var item in listPage)
             {

                 GetMenuInfo data = new GetMenuInfo()
                 {
                     Id = item.Id,
                     CreateTime = item.CreateTime,
                     Title = item.Title,
                     Description = item.Desrciption,
                     Target = item.Target,
                     Sort = item.Sort,
                     ParentId = item.ParentId,
                     Level = item.Level,
                     Icon = item.Icon,
                     Href = item.Href

                 };
                 list.Add(data);//;把单个对象添加到返回集合中
             };
             return list;*/

            return listPage;
        }

        #endregion

        #region 添加菜单
        public object GetSelectOptions(string id)
        {

            var MenuInfoList = _menuInfoDAL.GetEntity().Where(u => u.IsDelete == false)
                .Select(u => new { title = u.Title, value = u.Id })
                .ToList();
            if (!string.IsNullOrWhiteSpace(id))
            {
                var list = _menuInfoDAL.GetEntityById(id);
                return new { MenuInfoList, list };
            }

            return MenuInfoList;
        }
        public object getMenu()
        {
            var list = _menuInfoDAL.GetEntity().Where(m => !m.IsDelete)
                .Select(M => new
                {
                    value = M.Id,
                    title = M.Title
                })
                .ToList();
            return list;
        }

        public bool CreateMenuInfo(MenuInfo menu, out string msg)
        {
            
            if (string.IsNullOrWhiteSpace(menu.Title))
            {
                msg = "名称不能为空！";
                return false;
            }
           

            MenuInfo department2 = _menuInfoDAL.GetEntity().FirstOrDefault(d => d.Title == menu.Title);
            if (department2 != null)
            {
                msg = "该名称已存在！";
                return false;
            }
            menu.Id = Guid.NewGuid().ToString();
            menu.CreateTime = DateTime.Now;
            //entity.LeaderId= _repositorySys.UserInfo.Where(u=>u.DepartmentId==entity.)
            bool isSuccess = _menuInfoDAL.CreateEntity(menu);
            msg = isSuccess ? $"添加{menu.Title}成功！" : "添加失败！";
            return isSuccess;
        }

        #endregion

        #region 修改菜单
        public bool EditMenuInfo(MenuInfo menu, out string msg)
        {
            MenuInfo menulist = _menuInfoDAL.GetEntityById(menu.Id);
            if (menulist == null)
            {
                msg = "id无效";
                return false;
            }
          
            if (menu.Title == null)
            {
                msg = "名称不能为空！";
                return false;
            }
           

            menulist.Desrciption = menu.Desrciption;
            menulist.Level = menu.Level;
            menulist.Title = menu.Title;
            menulist.Sort = menu.Sort;
            menulist.ParentId = menu.ParentId;
            menulist.Href = menu.Href;
            menulist.Icon = menu.Icon;
            menulist.Target = menu.Target;


            bool isSuccess = _menuInfoDAL.UpdateEntity(menulist);
            msg = isSuccess ? "修改成功！" : "修改失败";
            return isSuccess;
        }

        #endregion

        #region 根据Id删除菜单
        public bool DeleteMenuInfo(string Id)
        {
            MenuInfo menu = _menuInfoDAL.GetEntityById(Id);
            if (menu == null)
            {
                return false;
            }
            menu.IsDelete = true;
            menu.DeleteTime = DateTime.Now;
            return _menuInfoDAL.UpdateEntity(menu);
        }

        #endregion

        #region 根据Ids批量删除
        public bool DeleteMenuInfos(List<string> Ids)
        {
            var menu = _menuInfoDAL.GetMenu().Where(u => Ids.Contains(u.Id)).ToList();
            foreach (var item in Ids)
            {
                MenuInfo menulist = menu.FirstOrDefault(u => u.Id == item);
                //DepartmentInfo departmentlist = _dpartmentInfoDAL.GetEntityById(item);
                if (menulist == null)
                {
                    continue;
                }
                menulist.IsDelete = true;
                menulist.DeleteTime = DateTime.Now;
                _menuInfoDAL.UpdateEntity(menulist);
            }
            return true;
        }

        #endregion

        #region 根据用户Id,获取菜单
        /// <summary>
        /// 根据用户Id,获取菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<HomeMenuDTO> GetMenus(string userId)
         {
            List<string> rolist = _UserInfo_RoleInfo.GetEntity().Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).ToList();

            List<string> menuIds = _RoleInfo_MenuInfoDAL.GetEntity().Where(rm => rolist.Contains(rm.RoleId)).Select(rm=>rm.MenuId).ToList();

            List<MenuInfo> allMenus = _menuInfoDAL.GetEntity().Where(m => menuIds.Contains(m.Id)&& m.IsDelete==false).OrderBy(m => m.Sort).ToList();

            List<HomeMenuDTO> topMenus = allMenus.Where(a => a.Level == 1).OrderBy(a => a.Sort).Select(a => new HomeMenuDTO()
            {
                Id = a.Id,
                Title = a.Title,
                Href = a.Href,
                Target = a.Target,
                Icon = a.Icon
            }).ToList();
            //通过遍历查询顶级菜单的子菜单
            /*  foreach (var item in topMenus)
              {
                  List<HomeMenuDTO> child = allMenus.Where(a => a.ParentId == item.Id).Select(a=>new HomeMenuDTO() {
                      Id = a.Id,
                      Title = a.Title,
                      Href = a.Href,
                      Target = a.Target,
                      Icon = a.Icon
                  }).OrderBy(a=>a.Sort).ToList();
                  item.Child = child;
              };*/
            //调用递归
            GetChildMenus(topMenus,allMenus);
            return topMenus;
        }
        //构造GetChildMenus（）方法
        public void GetChildMenus(List<HomeMenuDTO> parentMenus, List<MenuInfo> allmenus)
        {
            foreach (var item in parentMenus)
            {
                List<HomeMenuDTO> child = allmenus.Where(a => a.ParentId == item.Id).Select(a => new HomeMenuDTO()
                {
                    Id = a.Id,
                    Title = a.Title,
                    Href = a.Href,
                    Target = a.Target,
                    Icon = a.Icon
                }).OrderBy(a => a.Sort).ToList();
                //递归循环子菜单
                GetChildMenus(child, allmenus);
                item.Child = child;
               
            };
        }
        #endregion
    }
}
