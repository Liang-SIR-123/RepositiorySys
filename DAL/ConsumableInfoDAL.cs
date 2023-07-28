using IDAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public  class ConsumableInfoDAL:BaseDeleteDAL<ConsumableInfo>, IConsumableInfoDAL
    {
        private RepositorySysData _repositorySys;
        public ConsumableInfoDAL(
            RepositorySysData RepositorySys
            ) : base(RepositorySys)
        {
            //RepositorySys = new RepositorySysData();
            _repositorySys = RepositorySys;
        }
    }
}
