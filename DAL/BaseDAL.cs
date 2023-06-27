using IDAL;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    /// 所有数据访问层的基类
    /// </summary>
    public class BaseDAL<T> : IBaseDAL<T> where T : BaseEntity
    {
        RepositorySysData _repositorySys;
        public BaseDAL(RepositorySysData repositorySys)
        {
            _repositorySys = repositorySys;
        }
        /// <summary>
        /// 新增实体对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool CreateEntity(T entity)
        {
            _repositorySys.Set<T>().Add(entity);
            return _repositorySys.SaveChanges() > 0;
        }
        /// <summary>
        /// 根据Id删除实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteEntity(string Id)
        {
            T entity = _repositorySys.Set<T>().FirstOrDefault(u=>u.Id==Id);//先根据Id查有没有这个实体
            if (entity == null)//如果没有实体
            {
                return false; //返回删除失败 false
            }
            else
            {
                _repositorySys.Set<T>().Remove(entity); //把实体删除
                return _repositorySys.SaveChanges() > 0; //返回删除结果

            }
        }
        /// <summary>
        /// 根据实体进行删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteFntity(T entity)
        {
            if (entity == null)//如果没有实体
            {
                return false; //返回删除失败 false
            }
            else
            {
                _repositorySys.Set<T>().Remove(entity); //把实体删除
                return _repositorySys.SaveChanges() > 0; //返回删除结果

            }
        }
        /// <summary>
        /// 查询表的所有数据
        /// </summary>
        /// <returns></returns>
        public DbSet<T> GetEntity()
        {
            return _repositorySys.Set<T>();
        }
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateEntity(T entity)
        {
            _repositorySys.Entry(entity).State = EntityState.Modified;
            return _repositorySys.SaveChanges() > 0;
        }
    }
}
