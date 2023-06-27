using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    /// <summary>
    /// 带有删除属性的表继承的接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
  public  interface IBaseDeleteDAL<T> : IBaseDAL<T> where T:BaseDeleteEntity
    {
        /// <summary>
        /// 通过Id获取实体
        /// </summary>
        /// <param name="Id">对象的Id</param>
        /// <returns></returns>
        T GetEntityById(string Id);
    }
}
