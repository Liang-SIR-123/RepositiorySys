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
  public  class RoleInfoDAL : BaseDeleteDAL<RoleInfo>, IRoleInfoDAL
    {
       
            private RepositorySysData _repositorySys;
            public RoleInfoDAL(
                RepositorySysData RepositorySys
                ) : base(RepositorySys)
            {
                //RepositorySys = new RepositorySysData();
                _repositorySys = RepositorySys;
            }
            public DbSet<RoleInfo> GetRoleInfo()
            {
                return _repositorySys.RoleInfo;
            }
        
    }
}
