using IDAL;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public  class MenuInfoDAL:BaseDeleteDAL<MenuInfo>,IMenuInfoDAL
    {
       
            private RepositorySysData _repositorySys;

            public MenuInfoDAL(RepositorySysData repositorySys) : base(repositorySys)
            {
                _repositorySys = repositorySys;
            }
        public DbSet<MenuInfo> GetMenu()
        {
            return _repositorySys.MenuInfo;
        }

    }
}
