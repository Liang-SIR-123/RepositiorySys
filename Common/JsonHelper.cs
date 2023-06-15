using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common
{
    /// <summary>
    /// 指定返回Json帮助类
    /// </summary>
    public class JsonHelper : JsonResult
    {
        public new object Data { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">传值</param>
        public JsonHelper(object data)
        {
            this.Data = data;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("请求的HttpContext为空");
            }
            //获取响应对象
            var response = context.HttpContext.Response;
            //设置相应对象的类型
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
            //内容编码判断
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            //
            var camel = new JsonSerializerSettings
            {
                //把驼峰命名改为小写开头，第二个单词开始首字母大写
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString="yyyy-MM-dd HH:mm:ss"
            };
            response.Write(JsonConvert.SerializeObject(Data, camel));


        }


    }
}
