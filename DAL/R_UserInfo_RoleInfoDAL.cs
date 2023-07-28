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
    public class R_UserInfo_RoleInfoDAL : BaseDAL<R_UserInfo_RoleInfo>, IR_UserInfo_RoleInfoDAL
    {
        private RepositorySysData _repositorySys;

        public R_UserInfo_RoleInfoDAL( RepositorySysData repositorySys ) : base(repositorySys)
        {
            _repositorySys = repositorySys;
        }

    }
}
