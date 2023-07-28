using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
  public  class GetCategoryInfoDTO
    {
        public string Id { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        
        public string CategoryName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
