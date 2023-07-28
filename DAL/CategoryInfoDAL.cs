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
  public  class CategoryInfoDAL: BaseDeleteDAL<Category>,ICategoryInfoDAL
    {
        private RepositorySysData _repositorySys;
        public CategoryInfoDAL(
            RepositorySysData RepositorySys
            ) : base(RepositorySys)
        {
            //RepositorySys = new RepositorySysData();
            _repositorySys = RepositorySys;
        }
        /*public DbSet<Category> GetCategoryInfo()
        {
            return _repositorySys.Category;
        }*/

    }
}
