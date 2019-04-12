using System;
using System.Configuration;
using System.Data;

namespace Mysoft.Logistics.Portal
{
    /// <summary>
    /// 
    /// </summary>
    public class CommonHandler
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string DefaultConnectionString = ConfigurationManager.AppSettings["DefaultConnectionString"];

        public static DataTable GetDeployment(string[] uuid)
        {
            throw new NotImplementedException();
        }
    }
}