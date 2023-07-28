using IDAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public  class WorkFlow_InstanceDAL: BaseDAL<WorkFlow_Instance>, IWorkFlow_InstanceDAL
    {
        private RepositorySysData _repositorySys;
        
       public WorkFlow_InstanceDAL(RepositorySysData repositorySys):base(repositorySys)
        {
            _repositorySys = repositorySys;
        }
    }
}
