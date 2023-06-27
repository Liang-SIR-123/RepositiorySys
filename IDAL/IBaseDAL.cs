using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    /// <summary>
    /// 基础数据访问层接口
    /// </summary>
  public  interface IBaseDAL<T> where T:BaseEntity
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity">添加的实体</param>
        /// <returns></returns>
        bool CreateEntity(T entity);
        /// <summary>
        /// 根据Id删除实体
        /// </summary>
        /// <param name="Id">实体id</param>
        /// <returns></returns>
        bool DeleteEntity(string Id);
        /// <summary>
        /// 根据实体进行删除
        /// </summary>
        /// <param name="entity"> 删除的实体</param>
        /// <returns></returns>
        bool DeleteFntity(T entity);
        /// <summary>
        /// 查询整表数据
        /// </summary>
        /// <returns></returns>
        DbSet<T> GetEntity();
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">更新的实体</param>
        /// <returns></returns>
        bool UpdateEntity(T entity);

    }
}
