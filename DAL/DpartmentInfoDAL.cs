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
    public class DpartmentInfoDAL : IDpartmentInfo
    {
        private RepositorySysData _repositorySys;
        public DpartmentInfoDAL(
            RepositorySysData RepositorySys
            )
        {
            //RepositorySys = new RepositorySysData();
            _repositorySys = RepositorySys;
        }
        public DbSet<DepartmentInfo> GetDpartmentInfo()
        {
            return _repositorySys.DepartmentInfo;
        }
    }
}
