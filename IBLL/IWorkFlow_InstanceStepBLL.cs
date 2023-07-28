using Models.DTO;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
   public interface IWorkFlow_InstanceStepBLL
    {
        List<GetWorkFlow_InstanceStepDTO> WorkFlow_InstanceStepList(int page, int limit, string userId, int status, out int count);
        bool UpdateWorkFlow_InstanceStep(string Id, string reviewReason, string userId, int outNum, WorkFlow_InstanceStepEnums reviewStatus ,out string msg);

        
    }
}
