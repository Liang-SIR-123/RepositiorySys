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
    public class DepartmentInfoDAL : BaseDeleteDAL<DepartmentInfo>, IDpartmentInfo
    {
        private RepositorySysData _repositorySys;
        public DepartmentInfoDAL(
            RepositorySysData RepositorySys
            ):base(RepositorySys)
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
