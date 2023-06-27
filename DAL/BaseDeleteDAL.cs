using IDAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    /// 带有软删除的数据访问层基类
    /// </summary>
    public class BaseDeleteDAL<T> : BaseDAL<T>, IBaseDeleteDAL<T> where T : BaseDeleteEntity
    {
        RepositorySysData _repository;
        public BaseDeleteDAL(RepositorySysData repository):base(repository)
        {
            this._repository=repository;
        }
        /// <summary>
        /// 根据Id删除实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T GetEntityById(string Id)
        {
            return _repository.Set<T>().FirstOrDefault(u => u.Id == Id);
        }
    }
}
