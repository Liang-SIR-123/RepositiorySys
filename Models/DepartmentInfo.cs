﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 部门表
    /// </summary>
   public class DepartmentInfo:BaseDeleteEntity
    {
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(32)]
        public string Description { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        [MaxLength(16)]

        public string DepartmentName { get; set; }
        /// <summary>
        /// 主管Id
        /// </summary>
        [MaxLength(36)]
        public string LeaderId { get; set; }
        /// <summary>
        /// 父部门Id
        /// </summary>
        [MaxLength(36)]
        public string ParentId { get; set; }
       
    }
}
