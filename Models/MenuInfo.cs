﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 菜单表
    /// </summary>
  public  class MenuInfo:BaseDeleteEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        [MaxLength(16)]
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(32)]
        public string Desciption { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        
        public int Level { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        //访问地址
        [MaxLength(128)]
        public string Href { get; set; }
        //父菜单Id
        [MaxLength(36)]

        public string ParentId { get; set; }
        /// <summary>
        /// 图标样式
        /// </summary>
        [MaxLength(32)]
        public string Icon { get; set; }
        /// <summary>
        /// 目标
        /// </summary>
        [MaxLength(16)]
        public string Target { get; set; }
    }
}
