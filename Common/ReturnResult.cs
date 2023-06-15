using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 返回结果封装实体类
    /// </summary>
 public   class ReturnResult
    {
        public int Code { get; set; } = 501; //参数错误
        public string Msg { get; set; } = "失败"; 
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
        public int Count { get; set; } = 0; //数据总数
    }
}
