using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
  public  interface IUserInfoBLL
    {
        /// <summary>
        /// 用户登录业务逻辑
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="msg"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool Login(string account, string password, out string msg, out string userName, out string userId);

        /// <summary>
        /// 查询用户列表的方法
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="limit">每页几条数据</param>
        /// <param name="account">用户账号</param>
        /// <param name="userName">用户姓名</param>
        /// <param name="count">数据总量</param>
        /// <returns></returns>
        List<GetUserInfoDTO> GetUserInfos(int page,int limit,string account,string userName,out int count);
        bool CreateUserInfo(UserInfo entity, out string msg);
        bool DeleteUserInfo(string Id);
        /// <summary>
        /// 批量用户软删除
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        bool DeleteUserInfo(List<string> ids);

        bool UpdateUserInfo(UserInfo userInfo, out string msg);
        /// <summary>
        /// 下拉框的部门信息
        /// </summary>
        /// <returns></returns>
        object GetDepartmentSelect();
        /// <summary>
        /// 获取修改个人资料的信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        object GetUserFile(string Id);
        GetUserInfoDTO GetUserFiles(string Id);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <param name="againPwd"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool UpdatePWD(string id,string oldPwd,string newPwd,string againPwd,out string msg);

        /// <summary>
        /// 查询用户列表的方法
        /// </summary>
        /// <returns></returns>
        List<GetUserInfoDTO> GetUserInfo();
    }
}
