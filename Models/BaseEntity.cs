using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
 public class BaseEntity
    {
        /// <summary>
        /// 正常的基类
        /// </summary>
        [Key]
       ///[Required]必填特性
       ///主键Id
       [MaxLength(36)]
        public string Id { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
