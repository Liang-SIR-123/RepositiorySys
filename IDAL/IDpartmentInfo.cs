using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
   public interface IDpartmentInfo:IBaseDeleteDAL<DepartmentInfo>
    {
        /// <summary>
        /// 获取所有部门表
        /// </summary>
        /// <returns></returns>
        DbSet<DepartmentInfo> GetDpartmentInfo();
    }
}
