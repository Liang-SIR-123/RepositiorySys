using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public class MD5Helper
    {
        
            /// <summary>
            /// 文本MD5加密
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static string GetMD5(string str)
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] strByte = Encoding.UTF8.GetBytes(str);
                    byte[] hashBytes = md5.ComputeHash(strByte);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
       
    }
}
