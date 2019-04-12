using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Security;

namespace Mysoft.Logistics.Portal
{
    /// <summary>
    /// WebHelper
    /// </summary>
    public static class WebHelper
    {
        #region 公开方法

        /// <summary>
        /// 接口同步状态集合
        /// </summary>
        public static Dictionary<string, int> InterfaceConfigsStatus = new Dictionary<string, int>();

        //public static ActionResult Json(this Controller, object data)
        //{
        //    return new JsonNetResult(data);
        //}


        /// <summary>
        /// 接口同步状态集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<string, int> AddOrRemoveConfig(string key, int value)
        {
            if (InterfaceConfigsStatus.ContainsKey(key))
            {
                InterfaceConfigsStatus.Remove(key);
            }
            else
            {
                InterfaceConfigsStatus.Add(key, value);
            }

            return InterfaceConfigsStatus;
        }

        /// <summary>
        /// 判断给定的IP地址是否为内网IP。
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <returns>是否为内网IP</returns>
        public static bool IsInnerIPAddress(this string ipAddress)
        {
            ipAddress = ipAddress == "::1" ? "127.0.0.1" : ipAddress;

            var ipNumber = ipAddress.AsNumber();

            // 私有IP：
            // A类:10.0.0.0-10.255.255.255
            // B类:172.16.0.0-172.31.255.255 
            // C类:192.168.0.0-192.168.255.255   
            // 当然，还有127这个网段是环回地址
            var aBegin = AsNumber("10.0.0.0");
            var aEnd = AsNumber("10.255.255.255");
            var bBegin = AsNumber("172.16.0.0");
            var bEnd = AsNumber("172.31.255.255");
            var cBegin = AsNumber("192.168.0.0");
            var cEnd = AsNumber("192.168.255.255");

            return InRange(ipNumber, aBegin, aEnd) ||
                InRange(ipNumber, bBegin, bEnd) ||
                InRange(ipNumber, cBegin, cEnd) ||
                ipAddress.Equals("127.0.0.1");
        }

        /// <summary>
        /// 获取客户端的IP地址。
        /// </summary>
        /// <returns>客户端IP地址</returns>
        public static string GetClientIPAddress()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork).ToString();
        }

        /// <summary>
        /// 获取登录用户Id。
        /// </summary>
        /// <returns>登录用户Id</returns>
        public static string GetLogOnUserId()
        {
            if (HttpContext.Current == null)
            {
                return null;
            }

            if (HttpContext.Current.User == null || HttpContext.Current.User.Identity == null)
            {
                return null;
            }

            var identity = HttpContext.Current.User.Identity;

            return identity.IsAuthenticated ? identity.Name.Split(',').FirstOrDefault() : null;
        }

        /// <summary>
        /// 获取登录账户。
        /// </summary>
        /// <returns>登录账户</returns>
        public static string GetLogOnAccount()
        {
            if (HttpContext.Current == null)
            {
                return null;
            }

            if (HttpContext.Current.User == null || HttpContext.Current.User.Identity == null)
            {
                return null;
            }

            var identity = HttpContext.Current.User.Identity;

            return identity.IsAuthenticated ? identity.Name.Split(',')[1] : null;
        }

        /// <summary>
        /// 获取登录名称。
        /// </summary>
        /// <returns>登录名称</returns>
        public static string GetLogOnName()
        {
            if (HttpContext.Current == null)
            {
                return null;
            }

            if (HttpContext.Current.User == null || HttpContext.Current.User.Identity == null)
            {
                return null;
            }

            var identity = HttpContext.Current.User.Identity;

            return identity.IsAuthenticated ? identity.Name.Split(',').LastOrDefault() : null;
        }

        /// <summary>
        /// 设置身份认证票证。
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="account">账号</param>
        /// <param name="name">姓名</param>
        /// <param name="createPersistentCookie">是否创建持久Cookie</param>
        public static void SetAuthCookie(string userId, string account, string name = null, bool createPersistentCookie = true)
        {
            if (HttpContext.Current == null)
            {
                return;
            }
            FormsAuthentication.SetAuthCookie($"{userId},{account},{name}", createPersistentCookie);
        }

        /// <summary>
        /// 将用户基本信息保存到Session中。
        /// </summary>
        /// <typeparam name="T">用户基本信息类型</typeparam>
        /// <param name="entity">用户基本信息</param>
        public static void SetUserToSession<T>(T entity)
        {
            if (HttpContext.Current == null || string.IsNullOrWhiteSpace(GetLogOnUserId()))
            {
                return;
            }

            HttpContext.Current.Session[GetLogOnUserId()] = entity;
        }

        /// <summary>
        /// 从Session中获取保存的用户基本信息。
        /// </summary>
        /// <typeparam name="T">用户基本信息类型</typeparam>
        /// <returns>用户基本信息</returns>
        public static T GetUserFromSession<T>()
        {
            if (HttpContext.Current == null || string.IsNullOrWhiteSpace(GetLogOnUserId()))
            {
                return default(T);
            }

            return (T)HttpContext.Current.Session[GetLogOnUserId()];
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 将IP地址转换为数字。
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <returns>数字</returns>
        private static long AsNumber(this string ipAddress)
        {
            var ips = ipAddress.Split('.');

            return int.Parse(ips[0]) * (256 ^ 3) + int.Parse(ips[1]) * (256 ^ 2) + int.Parse(ips[2]) * 256 + int.Parse(ips[3]);
        }

        /// <summary>
        /// 判断一个数字在某个范围内。
        /// </summary>
        /// <param name="value">需要判断的数字</param>
        /// <param name="begin">开始</param>
        /// <param name="end">结束</param>
        /// <returns>在范围内</returns>
        private static bool InRange(long value, long begin, long end)
        {
            return (value >= begin) && (value <= end);
        }

        #endregion
    }
}