using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
 public  class GetWorkFlow_InstanceDTO
    {

        public string Id { get; set; }
        /// <summary>
        /// 工作流模板Id
        /// </summary>
        /// 

        public string ModelId { get; set; }
        public string ModelName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        ///  描述
        /// </summary>
        
        public string Description { get; set; }
        /// <summary>
        /// 申请理由
        /// </summary>
       
        public string Reason { get; set; }
        /// <summary>
        /// 添加人Id
        /// </summary>
        
        public string Creator { get; set; }
        /// <summary>
        /// 出库数量
        /// </summary>
        public int OutNum { get; set; }
        /// <summary>
        /// 出库物资
        /// </summary>
      
        public string OutGoodsId { get; set; }
        public string OutGoodsName { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
