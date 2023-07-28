using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
  public  interface IConsumableInfoBLL
    {
        List<GetConsumableInfoDTO> GetConsumableInfo(int page, int limit, string CategoryName, out int count);
        bool CreateConsumable(ConsumableInfo entity, out string msg);
        object GetCategoryInfos(string Id);
        bool UpdateConsumable(ConsumableInfo consumable, out string msg);
        bool DeleteConsumable(string Id);
        bool DeleteConsumables(List<string> ids);
        bool Upload(Stream stream, string extension, string userId, out string msg);

        /// <summary>
        /// 出入库记录
        /// </summary>
        /// <param name="downloadFileName">下载的文件名称</param>
        /// <returns></returns>
        Stream GetDownLoad(out string downloadFileName);
    }
}
