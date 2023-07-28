using IDAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public  class WorkFlow_InstanceStepDAL:BaseDAL<WorkFlow_InstanceStep>, IWorkFlow_InstanceStepDAL
    {
        private RepositorySysData _repositorySys;
        public WorkFlow_InstanceStepDAL(RepositorySysData repositorySys):base(repositorySys)
        {
            _repositorySys = repositorySys;
        }
    }
}
