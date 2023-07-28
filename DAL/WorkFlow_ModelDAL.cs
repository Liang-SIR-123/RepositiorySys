using IDAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public class WorkFlow_ModelDAL:BaseDeleteDAL<WorkFlow_Model> ,IWorkFlow_ModelDAL
    {
        private RepositorySysData _repositorySys;

        public WorkFlow_ModelDAL(RepositorySysData repositorySys) : base(repositorySys)
        {
            _repositorySys = repositorySys;
        }
    }
}
