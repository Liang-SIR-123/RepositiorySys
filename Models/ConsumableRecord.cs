using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 耗材记录表
    /// </summary>
   public class ConsumableRecord:BaseEntity
    {
        /// <summary>
        /// 耗材Id
        /// </summary>
        [MaxLength(36)]
        public string ConsumableId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 添加入Id
        /// </summary>
        [MaxLength(36)]
        public string Creator { get; set; }
    }
}
