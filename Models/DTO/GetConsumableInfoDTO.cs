using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
   public class GetConsumableInfoDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 耗材类型Id
        /// </summary>
       
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        /// <summary>
        ///  耗材名称
        /// </summary>
       
        public string ConsumableName { get; set; }
        /// <summary>
        /// 耗材规格
        /// </summary>
       
        public string Specificatin { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
       
        public string Unit { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 警告库存
        /// </summary>
        public int WarningNum { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
