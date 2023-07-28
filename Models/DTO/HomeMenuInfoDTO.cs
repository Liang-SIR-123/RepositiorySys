using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
  public  class HomeMenuInfoDTO
    {
        public HomeMenuInfoDTO()
        {

        }
        public HomeMenuInfoDTO (List<HomeMenuDTO> menus)
        {
            var homeMenuInfoDTO = this.MenuInfo.FirstOrDefault();
            if (homeMenuInfoDTO != null)
            {
                homeMenuInfoDTO.Child = menus;
            }
        }
        /// <summary>
        /// Home
        /// </summary>
        public HomeMenuDTO HomeInfo { get; set; } = new HomeMenuDTO()
        {
            //初始化值
            Title = "首页",
            Href = "../layuimini/page/welcome-1.html?t=1"
        };


        /// <summary>
        /// logo
        /// </summary>
        public HomeMenuDTO LogoInfo { get; set; } = new HomeMenuDTO()
        {
            Title = "物资管理系统",
            Image = "../layuimini/images/logo.png"
        };

        /// <summary>
        /// 权限菜单树
        /// </summary>
        public List<HomeMenuDTO> MenuInfo { get; set; } = new List<HomeMenuDTO>()
        {
            new HomeMenuDTO()
            {
                Title="常规",
                Icon="",
                Href="",
                Target="_self",

            }
        };
    }
}
