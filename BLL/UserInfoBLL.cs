using Common;
using DAL;
using IBLL;
using IDAL;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// 用户表的业务逻辑层
    /// </summary>
    public class UserInfoBLL : IUserInfoBLL
    {
        // private UserInfoDAL _UserInfoDAL;
        private IUserInfoDAL _UserInfoDAL;
        private IDpartmentInfo _dpartmentInfoDAL;
        private RepositorySysData _repositorySys;

        public UserInfoBLL(
            IUserInfoDAL userInfoDAL,
            IDpartmentInfo dpartmentInfo,
            RepositorySysData repositorySys
            )
        {
            // UserInfoDAL = new UserInfoDAL();
            _UserInfoDAL = userInfoDAL;
            _dpartmentInfoDAL = dpartmentInfo;
            _repositorySys = repositorySys;
        }
        #region 用户登录业务逻辑
        /// <summary>
        /// 用户登录业务逻辑
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <param name="msg">返回的消息</param>
        /// <param name="userName">返回的用户名</param>
        /// <param name="userId">返回的用户Id</param>
        /// <returns></returns>
        public bool Login(string account, string password, out string msg, out string userName, out string userId)
        {
            //把密码加密MD5
            string newPassword = MD5Helper.GetMD5(password);
            //根据账号密码查询用户
            UserInfo userInfo = _UserInfoDAL.GetUserInfos().FirstOrDefault(u => u.Account == account && u.PassWord == newPassword);

            userName = "";
            userId = "";

            //判断是否存在该用户
            if (userInfo == null)
            {
                msg = "账号或密码！";
                return false;
            }
            else
            {
                msg = "成功！";
                userName = userInfo.UserName;
                userId = userInfo.Id;
                return true;
            }
        }
        #endregion

        /// <summary>
        /// 显示获取的用户信息的方法
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="account"></param>
        /// <param name="userName"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<GetUserInfoDTO> GetUserInfos(int page, int limit, string account, string userName, out int count)
        {
            #region 旧的查询方法
            /*//用户表
            var userInfoList = _UserInfoDAL.GetUserInfos().Where(u => u.IsDelete == false);
            //查找账号相同的
            if (!string.IsNullOrWhiteSpace(account))
            {
                userInfoList = userInfoList.Where(u => u.Account == account);
            }
            //模糊查询姓名
            if (!string.IsNullOrWhiteSpace(userName))
            {
                userInfoList = userInfoList.Where(u => u.UserName.Contains(userName));
            }
            count = userInfoList.Count();//返回总数
                                         //分页
            var listPage = userInfoList.OrderByDescending(u => u.CreateTime)
                .Skip(limit * (page - 1))
                .Take(limit)
                .ToList();
            List<GetUserInfoDTO> list = new List<GetUserInfoDTO>();
            //部门表
            var departmentList = _dpartmentInfoDAL.GetDpartmentInfo().ToList();
            foreach (var item in listPage)
            {
                var det = departmentList.SingleOrDefault(d => d.Id == item.DepartmentId);
                GetUserInfoDTO data = new GetUserInfoDTO()
                {
                    Id = item.Id,
                    Account = item.Account,
                    Email = item.Email,
                    CreateTime = item.CreateTime,
                    DepartmentName = det == null ? "" : det.DepartmentName,
                    PhoneNum = item.PhoneNum,
                    Sex = item.Sex == 0 ? "女" : "男",
                    UserName = item.UserName
                };
                list.Add(data);//;把单个对象添加到返回集合中
            }

            return list;*/
            #endregion
            #region 新的方法 
            //linq方法进行链表
            var data = from u in _repositorySys.UserInfo.Where(U => U.IsDelete == false)
                       join d in _repositorySys.DepartmentInfo.Where(d => d.IsDelete == false)
                       on u.DepartmentId equals d.Id
                       into temUD
                       from UD in temUD.DefaultIfEmpty()
                       select new GetUserInfoDTO
                       {
                           Id = u.Id,
                           Account = u.Account,
                           UserName = u.UserName,
                           Email = u.Email,
                           Sex = u.Sex == 0 ? "女" : "男",
                           DepartmentName = UD.DepartmentName == null ? "" : UD.DepartmentName,
                           DepartmentId=u.DepartmentId,
                           PhoneNum = u.PhoneNum,
                           CreateTime = u.CreateTime
                       };
            //判断用户姓名
            if (!string.IsNullOrWhiteSpace(userName))
            {
                data = data.Where(u => u.UserName.Contains(userName));
            }
            //判断用户账号
            if (!string.IsNullOrWhiteSpace(account))
            {
                data = data.Where(u => u.Account == account);
            }
            //求总数
            count = data.Count();
            //分页
            var listPage = data.OrderByDescending(u => u.CreateTime)
                .Skip(limit * (page - 1))
                .Take(limit)
                .ToList();
            return listPage;


            #endregion



        }
        #region 添加用户的方法
        /// <summary>
        /// 添加用户的方法
        /// </summary>
        /// 
        public bool CreateUserInfo(UserInfo entity, out string msg)
        {
            //先判断这个用户非空信息
            if (string.IsNullOrWhiteSpace(entity.UserName))
            {
                msg = "用户姓名不能为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(entity.PassWord))
            {
                msg = "用户密码不能为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(entity.Account))
            {
                msg = "用户账号不能为空！";
                return false;
            }
            //判断账号是否重复

            UserInfo user = _UserInfoDAL.GetEntity().FirstOrDefault(u => u.Account == entity.Account);
            if (user != null)
            {
                msg = "账号已存在！";
                return false;
            }
            DepartmentInfo department = _repositorySys.DepartmentInfo.FirstOrDefault(d => d.Id == entity.DepartmentId);
            if (department == null)
            {
                msg = "不存在该部门！";
                return false;
            }
            //赋值Id
            entity.Id = Guid.NewGuid().ToString();
            entity.CreateTime = DateTime.Now;
            //更新到数据库
            bool isSuccess = _UserInfoDAL.CreateEntity(entity);
            /*if (isSuccess)
            {
                msg = $"添加{entity.UserName}成功！";
            }
            else
            {
                msg = $"添加失败！";
            }*/
            msg = isSuccess ? $"添加{entity.UserName}成功！" : "添加失败！";
            return isSuccess;
        }


        #endregion

        #region 用户软删除方法
        public bool DeleteUserInfo(string Id)
        {
            //根据Id查用户是否存在
            UserInfo userInfo = _UserInfoDAL.GetEntityById(Id);
            if (userInfo == null)
            {
                return false;
            }
            //修改状态
            userInfo.IsDelete = true;
            userInfo.DeleteTime = DateTime.Now;
            return _UserInfoDAL.UpdateEntity(userInfo);
        }
        #endregion

        #region 批量软删除方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids">软删除Id集合</param>
        /// <returns></returns>
        public bool DeleteUserInfo(List<string> ids)
        {
            // var userList = _UserInfoDAL.GetUserInfos().Where(u => ids.Contains(u.Id)).ToList();
            foreach (var item in ids)
            {
                //UserInfo userInfo = userList.FirstOrDefault(u=>u.Id==item);
                UserInfo userInfo = _UserInfoDAL.GetEntityById(item);
                if (userInfo == null)
                {
                    continue;
                }
                userInfo.IsDelete = true;
                userInfo.DeleteTime = DateTime.Now;
                _UserInfoDAL.UpdateEntity(userInfo);
            }
            return true;

        }
        #endregion

        #region 更新方法
        public bool UpdateUserInfo(UserInfo userInfo, out string msg)
        {
            #region 判断


            //判断

            UserInfo user = _UserInfoDAL.GetEntityById(userInfo.Id);
            if (user == null)
            {
                msg = "用户id无效";
                return false;
            }

            if (user.Account != userInfo.Account)
            {
                UserInfo users = _UserInfoDAL.GetEntity().FirstOrDefault(u => u.Account == userInfo.Account);
                if (users != null)
                {
                    msg = "账号已被使用！";
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(userInfo.Id))
            {
                msg = "Id不能为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(userInfo.UserName))
            {
                msg = "UserName不能为空！";
                return false;
            }
            /* if (string.IsNullOrWhiteSpace(userInfo.PassWord))
             {
                 msg = "PassWord不能为空！";
                 return false;
             }*/
            if (string.IsNullOrWhiteSpace(userInfo.Account))
            {
                msg = "Account不能为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(userInfo.DepartmentId))
            {
                msg = "DepartmentId不能为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(userInfo.Email))
            {
                msg = "Email不能为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(userInfo.PhoneNum))
            {
                msg = "PhoneNum不能为空！";
                return false;
            }
            #endregion


            user.Account = userInfo.Account;
            user.UserName = userInfo.UserName;
            //判断密码是否用原值
            user.PassWord = string.IsNullOrWhiteSpace(userInfo.PassWord) ? user.PassWord : MD5Helper.GetMD5(userInfo.PassWord);
            user.Email = userInfo.Email;
            user.DepartmentId = userInfo.DepartmentId;
            user.PhoneNum = userInfo.PhoneNum;
            user.Sex = userInfo.Sex;

            bool isSuccess = _UserInfoDAL.UpdateEntity(user);
            msg = isSuccess ? "修改成功" : "修改失败";

            return isSuccess;

        }
        #endregion

        #region 部门下拉框信息
        public object GetDepartmentSelect()
        {
            var department = _repositorySys.DepartmentInfo.Where(d => !d.IsDelete).Select(d => new
            {
                value = d.Id,
                title = d.DepartmentName
            }).ToList();

            return new
            {
                department
            };
        }
        #endregion
    }
}

