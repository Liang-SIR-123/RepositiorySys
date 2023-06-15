using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
 public interface IUserInfoDAL
    {
        /// <summary>
        /// 获取所有用户表
        /// </summary>
        /// <returns></returns>
        DbSet<UserInfo> GetUserInfos();
    }
}
