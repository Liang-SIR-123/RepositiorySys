using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
  public interface IMenuInfoBLL
    {
        List<GetMenuInfo> GetMenuInfo(int page, int limit, string title, out int count);
        object GetSelectOptions(string id);
        //object getMenu(string id);
        bool CreateMenuInfo(MenuInfo menu, out string msg);
        bool EditMenuInfo(MenuInfo menu, out string msg);

        bool DeleteMenuInfo(string Id);
         bool DeleteMenuInfos(List<string> Ids);
        object getMenu();
        /// <summary>
        /// 根据用户Id,获取菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<HomeMenuDTO> GetMenus(string userId);
    }
}
