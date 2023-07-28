using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class GetWorkFlow_InstanceStepDTO
    {
        
       public string Id { get; set; }
        /// <summary>
        /// 工作流实例Id
        /// </summary>
        public string InstanceId { get; set; }
        /// <summary>
        /// 申请实例
        /// </summary>
        public string InstanceName { get; set; }
        /// <summary>
        /// 审核人Id
        /// </summary>

        public string ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        /// <summary>
        /// 审核理由
        /// </summary>
        
        public string ReviewReason { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int ReviewStatus { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ReviewTime { get; set; }
        /// <summary>
        /// 上一个步骤Id
        /// </summary>
       
        public string BeforeStepId { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 申请物品
        /// </summary>
        public string ReviewGoods { get; set; }
        /// <summary>
        /// 申请数量
        /// </summary>
        public int ReviewNum { get; set; }
        /// <summary>
        /// 申请人Id
        /// </summary>
        public string applyId { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public string applyIdName { get; set; }
        public string Reason { get; set; }
    }
}
