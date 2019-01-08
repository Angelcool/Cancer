using System;
using System.Data;
using System.Data.SqlClient;

namespace Mysoft.Magicodes.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class DbConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public static string DefaultSqlConnectionString = @"server=127.0.0.1;database=ormdemo;uid=root;pwd=Open0001;SslMode=none;";

        /// <summary>
        /// 创建数据里连接
        /// </summary>
        /// <param name="sqlConnectionString"></param>
        /// <returns></returns>
        public static IDbConnection CreateDbConnection(string sqlConnectionString = "")
        {
            if (string.IsNullOrEmpty(sqlConnectionString))
            {
                sqlConnectionString = DefaultSqlConnectionString;
            }
            IDbConnection conn = new SqlConnection(sqlConnectionString);
            conn.Open();
            return conn;
        }
    }
}
