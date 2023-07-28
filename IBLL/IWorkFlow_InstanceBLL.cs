using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
 public   interface IWorkFlow_InstanceBLL
    {
        /// <summary>
        /// 申领方法
        /// </summary>
        /// <param name="workFlow"></param>
        /// <param name="userId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool CreateWorkFlow_Instance(WorkFlow_Instance workFlow, string userId, out string msg);
        List<GetWorkFlow_InstanceDTO> workFlow_InstancesList(int page, int limit, string userId, int status, out int count);

        bool UpdateGetWorkFlow_Instance(string Id,out string msg);
    }
}
