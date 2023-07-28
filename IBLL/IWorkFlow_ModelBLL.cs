using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    public interface IWorkFlow_ModelBLL
    {
        List<GetWorkFlow_ModelDTO> GetWorkFlow_Model(int page, int limit, string Title, out int count);
        bool CreateWorkFlow(WorkFlow_Model entity, out string msg);
        object getWorkFlow(string Id);
        bool UpdateWorkFlow(WorkFlow_Model workFlow, out string msg);
        bool DeleteWorkFlow(string Id);
        bool DeleteWorkFlows(List<string> Ids);
    }
}
