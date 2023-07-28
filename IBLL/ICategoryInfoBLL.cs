using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
  public  interface ICategoryInfoBLL
    {
        List<GetCategoryInfoDTO> getCategoryInfos(int page, int limit, string CategoryName, out int count);
        bool CreateCategoryInfo(Category entity, out string msg);
        object GetCategoryInfos(string Id);
        bool UpdateCategoryInfo(Category category, out string msg);
        bool DeleteCategory(string Id);
        bool DeleteCategorys(List<string> Ids);
    }
}
