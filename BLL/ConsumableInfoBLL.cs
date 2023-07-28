using DAL;
using IBLL;
using Models;
using Models.DTO;
using Models.Enums;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ConsumableInfoBLL : IConsumableInfoBLL
    {
        private RepositorySysData _repositorySys;
        private ConsumableInfoDAL _consumableInfoDAL;
        private CategoryInfoDAL _categoryInfoDAL;
        public ConsumableInfoBLL(RepositorySysData repositorySys, ConsumableInfoDAL consumableInfoDAL, CategoryInfoDAL categoryInfoDAL)
        {
            _repositorySys = repositorySys;
            _consumableInfoDAL = consumableInfoDAL;
            _categoryInfoDAL = categoryInfoDAL;
        }

        #region 获取耗材详细信息
        public List<GetConsumableInfoDTO> GetConsumableInfo(int page, int limit, string ConsumableName, out int count)
        {
            //Linq 语句查询
            var data = from u in _repositorySys.ConsumableInfo.Where(u => u.IsDelete == false)
                       join d in _repositorySys.Category.Where(d => d.IsDelete == false)
                       on u.CategoryId equals d.Id
                       into temub
                       from dd in temub.DefaultIfEmpty()


                       select new GetConsumableInfoDTO
                       {
                           Id = u.Id,
                           ConsumableName = u.ConsumableName,
                           Description = u.Description,
                           CreateTime = u.CreateTime,
                           CategoryId = u.CategoryId,
                           CategoryName = dd.CategoryName,
                           Money = u.Money,
                           Num = u.Num,
                           Specificatin = u.Specificatin,
                           Unit = u.Unit,
                           WarningNum = u.WarningNum,

                           //这个Linq查询语句相当于左右链接链表查询,会显示null
                       };


            //模糊查询姓名
            if (!string.IsNullOrWhiteSpace(ConsumableName))
            {
                data = data.Where(u => u.ConsumableName.Contains(ConsumableName));
            }
            count = data.Count();//返回总数
                                 //分页
            var listPage = data.OrderByDescending(u => u.CreateTime)
                .Skip(limit * (page - 1))
                .Take(limit)
                .ToList();

            return listPage;
        }
        #endregion

        #region 添加耗材信息
        public bool CreateConsumable(ConsumableInfo entity, out string msg)
        {
            if (string.IsNullOrWhiteSpace(entity.ConsumableName))
            {
                msg = "耗材名称不能为空！";
                return false;
            }


            /*Category department = _categoryInfoDAL.GetEntity().FirstOrDefault(d => d.CategoryName == entity.CategoryName);
            if (department != null)
            {
                msg = "该已存在！";
                return false;
            }*/
            entity.Id = Guid.NewGuid().ToString();
            entity.CreateTime = DateTime.Now;

            bool isSuccess = _consumableInfoDAL.CreateEntity(entity);
            msg = isSuccess ? $"添加{entity.ConsumableName}成功！" : "添加失败！";
            return isSuccess;
        }

        #endregion

        #region 获取下拉列表信息
        public object GetCategoryInfos(string Id)
        {
            
            if (string.IsNullOrWhiteSpace(Id))
            {
                var category2 = _categoryInfoDAL.GetEntity().Where(c => !c.IsDelete).Select(c=>new {value=c.Id,title=c.CategoryName }).ToList();
                var consumable2 = _consumableInfoDAL.GetEntity().Where(c => !c.IsDelete) .Select(c => new { value = c.Id, title = c.ConsumableName }).ToList();
                return new
                {
                    category2,
                    consumable2
                };
            }
            var category = _categoryInfoDAL.GetEntity().Where(c => !c.IsDelete).ToList();

            var consumable = _consumableInfoDAL.GetEntityById(Id);
            return new
            {
                category,
                consumable
            };
            
        }
        #endregion

        #region 更新耗材信息
        public bool UpdateConsumable(ConsumableInfo consumable, out string msg)
        {
            ConsumableInfo cate = _consumableInfoDAL.GetEntityById(consumable.Id);
            if (cate == null)
            {
                msg = "id无效";
                return false;
            }

            if (consumable.ConsumableName == null)
            {
                msg = "名称不能为空！";
                return false;
            }
            cate.Description = consumable.Description;
            cate.ConsumableName = consumable.ConsumableName;
            cate.CategoryId = consumable.CategoryId;
            cate.Money = consumable.Money;
            cate.Unit = consumable.Unit;
            cate.Specificatin = consumable.Specificatin;
            cate.WarningNum = consumable.WarningNum;



            bool isSuccess = _consumableInfoDAL.UpdateEntity(cate);
            msg = isSuccess ? "修改成功！" : "修改失败";
            return isSuccess;
        }

        #endregion

        #region 根据Id软删除
        public bool DeleteConsumable(string Id)
        {
            ConsumableInfo consumable = _consumableInfoDAL.GetEntityById(Id);
            if (consumable == null)
            {
                return false;
            }
            consumable.IsDelete = true;
            consumable.DeleteTime = DateTime.Now;
            return _consumableInfoDAL.UpdateEntity(consumable);
        }
        #endregion

        #region 根据Id批量软删除
        public bool DeleteConsumables(List<string> ids)
        {
            var consumable = _consumableInfoDAL.GetEntity().Where(u => ids.Contains(u.Id)).ToList();
            foreach (var item in ids)
            {
                ConsumableInfo consumablelist = consumable.FirstOrDefault(u => u.Id == item);
                //DepartmentInfo departmentlist = _dpartmentInfoDAL.GetEntityById(item);
                if (consumablelist == null)
                {
                    continue;
                }
                consumablelist.IsDelete = true;
                consumablelist.DeleteTime = DateTime.Now;
                _consumableInfoDAL.UpdateEntity(consumablelist);
            }
            return true;
        }
        #endregion

        #region 入库Excel导入的接口Upload
        /// <summary>
        /// 入库Excel导入的接口Upload
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="extension">后缀名</param>
        /// <param name="userId">用户Id</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool Upload(Stream stream, string extension, string userId, out string msg)
        {
            //安装NOPI库
            //创建工作簿
            IWorkbook book = null;
            if (extension.Equals(".xls"))
            {
                //把文件流写入工作簿
                book = new HSSFWorkbook(stream);
            }
            else
            {
                book = new XSSFWorkbook(stream);
            }
            stream.Close();
            stream.Dispose();

            //获取第一个工作簿的sheet
            ISheet sheet = book.GetSheetAt(0);//索引从0开始
            //先获取sheet有多少行
            int rowNum = sheet.LastRowNum + 1;


            //打开事务
            using (var transaction = _repositorySys.Database.BeginTransaction())
            {
                try
                {
                    for (int i = 1; i < rowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);//遍历获取的每一行
                        ICell cell = row.GetCell(0);//每一行的第一列
                        string value = cell.ToString();//获取商品名称
                        ICell cell2 = row.GetCell(2);//每一行的第三列
                        string value2 = cell2.ToString();//获取实际购买数量
                        int num = Convert.ToInt32(value2);
                        bool b = int.TryParse(value2, out num);
                        if (b == false)
                        {
                            transaction.Rollback();//事务回滚
                            msg = $"第{i + 1}行耗材实际购买数量有误！";
                            return false;
                        }
                        //查询该商品在数据库中的数据
                        ConsumableInfo consumable = _consumableInfoDAL.GetEntity().FirstOrDefault(c => !c.IsDelete && c.ConsumableName == value);
                        if (consumable == null)
                        {
                            transaction.Rollback();
                            msg = $"第{i + 1}行耗材有误！{value}";
                            return false;
                        }
                        ConsumableRecord record = new ConsumableRecord()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ConsumableId = consumable.Id,
                            CreateTime = DateTime.Now,
                            Creator = userId,
                            Num = num,
                            Type = (int)ConsumableRecordTypeEnums.入库,
                        };
                        //添加到耗材记录表数据库
                        _repositorySys.ConsumableRecord.Add(record);
                        //保存更改
                        bool IsSuccess = _repositorySys.SaveChanges() > 0;
                        if (IsSuccess == false)
                        {
                            transaction.Rollback();
                            msg = $"第{i + 1}行添加耗材失败！";
                            return false;
                        }
                        //更新耗材信息表库存
                        consumable.Num += num;
                        _repositorySys.Entry(consumable).State = System.Data.Entity.EntityState.Modified;//把实体改成修改状态
                        IsSuccess = _repositorySys.SaveChanges() > 0;
                        if (IsSuccess == false)
                        {
                            transaction.Rollback();
                            msg = $"第{i + 1}行耗材信息表更新失败！";
                            return false;
                        }
                    }
                    //提交事务
                    transaction.Commit();
                    msg = "入库成功！";
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    msg = "出错了！" + ex.Message;
                    return false;
                }

            }
        }

        #endregion

        #region 出入库记录
        /// <summary>
        /// 出入库记录
        /// </summary>
        /// <param name="downloadFileName">下载的文件名称</param>
        /// <returns></returns>
        public Stream GetDownLoad(out string downloadFileName)
        {
            //查询要导出的数据
            var datas = (from cr in _repositorySys.ConsumableRecord
                         join c in _repositorySys.ConsumableInfo.Where(c => !c.IsDelete)
                         on cr.ConsumableId equals c.Id
                         into tempc
                         from aa in tempc.DefaultIfEmpty()

                         join u in _repositorySys.UserInfo
                         on cr.Creator equals u.Id
                         into tempu
                         from uu in tempu.DefaultIfEmpty()

                         select new
                         {
                             aa.ConsumableName,
                             aa.Specificatin,
                             Type = cr.Type == (int)ConsumableRecordTypeEnums.入库 ? "入库" : "出库",
                             cr.Num,
                             CreateTime = cr.CreateTime,
                             uu.UserName
                         }).ToList();

            //对文件操作
            string path = Directory.GetCurrentDirectory();
            string fileName = "出库记录" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx";
            //拼接文件完整路径
            string filePath = Path.Combine(path, fileName);
            //创建excel对象
            IWorkbook book = null;
            string extension = Path.GetExtension(fileName);
            //操作文件
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                //根据excel后缀判断版本，实例化不同的对象
                if (extension.Equals(".xls"))
                {
                    book = new HSSFWorkbook();
                }
                else
                {
                    book = new XSSFWorkbook();
                }
                //创表
                ISheet sheet = book.CreateSheet("sheet1");
                #region 创建表头
                IRow row = sheet.CreateRow(0);
                string[] title =
                {
                    "耗材名称",
                    "耗材规格",
                    "出入库类型",
                    "出入库数量",
                    "出入库时间",
                    "操作人"
                };
                for (int i = 0; i < title.Length; i++)
                {
                    ICell cell = row.CreateCell(i);//第一列
                    cell.SetCellValue(title[i]);
                }
                #endregion
                #region 创建表体
                for (int i = 0; i < datas.Count; i++)
                {
                    var data = datas[i];//获取数据
                    IRow tempRow2 = sheet.CreateRow(i + 1);
                    //第1列
                    ICell tempCell = tempRow2.CreateCell(0);
                    tempCell.SetCellValue(data.ConsumableName);
                    //第2列
                    ICell tempCel2 = tempRow2.CreateCell(1);
                    tempCel2.SetCellValue(data.Specificatin);
                    //第3列
                    ICell tempCel3 = tempRow2.CreateCell(2);
                    tempCel3.SetCellValue(data.Type);
                    //第4列
                    ICell tempCel4 = tempRow2.CreateCell(3);
                    tempCel4.SetCellValue(data.Num);
                    //第5列
                    ICell tempCel5 = tempRow2.CreateCell(4);
                    tempCel5.SetCellValue(data.CreateTime.ToString("yyyy-MM-dd hh:mm:ss"));
                    //第6列
                    ICell tempCel6 = tempRow2.CreateCell(5);
                    tempCel6.SetCellValue(data.UserName);
                    #endregion


                }


                //把excel写入文件内
                book.Write(fs);
                //重新打开
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                downloadFileName = fileName;//文件名
                return fileStream; //文件




            }
            #endregion

        }
    }
}
