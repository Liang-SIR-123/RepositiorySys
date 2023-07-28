using IDAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public  class R_RoleInfo_MenuInfoDAL:BaseDAL<R_RoleInfo_MenuInfo>, IR_RoleInfo_MenuInfoDAL
    {
        private RepositorySysData _repositorySys;

        public R_RoleInfo_MenuInfoDAL(RepositorySysData repositorySys) : base(repositorySys)
        {
            _repositorySys = repositorySys;
        }
    }
}
