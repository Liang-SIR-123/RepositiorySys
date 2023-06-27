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
    /// <summary>
    /// 用户表的数据访问层
    /// </summary>
 public   class UserInfoDAL:BaseDeleteDAL<UserInfo>,IUserInfoDAL
    {
        private RepositorySysData _repositorySys;
        public UserInfoDAL( RepositorySysData RepositorySys ):base(RepositorySys)
        {
            //RepositorySys = new RepositorySysData();
            _repositorySys = RepositorySys;
        }
        /// <summary>
        /// 获取用户表所有数据
        /// </summary>
        /// <returns></returns>
        public DbSet<UserInfo> GetUserInfos()
        {
            //RepositorySysData db = new RepositorySysData();
            return _repositorySys.UserInfo;
        }

       



    }
}
