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
        public UserInfoBLL(
            IUserInfoDAL userInfoDAL,
            IDpartmentInfo dpartmentInfo
            )
        {
            // UserInfoDAL = new UserInfoDAL();
            _UserInfoDAL = userInfoDAL;
            _dpartmentInfoDAL = dpartmentInfo;
        }
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
        public List<GetUserInfoDTO> GetUserInfos(int page, int limit, string account, string userName, out int count)
        {
            //用户表
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
            foreach (var item in listPage)
            {
                var det = _dpartmentInfoDAL.GetDpartmentInfo().SingleOrDefault(d => d.Id == item.Id);
                GetUserInfoDTO data = new GetUserInfoDTO()
                {
                    Id = item.Id,
                    Account = item.Account,
                    Email = item.Email,
                    CreateTime = item.CreateTime,
                    DepartmentName = det==null?"":det.DepartmentName,
                    PhoneNum = item.PhoneNum,
                    Sex = item.Sex==0?"女":"男",
                    UserName = item.UserName
                };
                list.Add(data);//;把单个对象添加到返回集合中
            }
            //部门表
            var departmentList = _dpartmentInfoDAL.GetDpartmentInfo().ToList();
            return list;
        }
    }
}

