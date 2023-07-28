﻿using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
  public  interface IRoleInfoDAL:IBaseDeleteDAL<RoleInfo>
    {
       
            /// <summary>
            /// 获取所有部门表
            /// </summary>
            /// <returns></returns>
        DbSet<RoleInfo> GetEntity();

    }
}
