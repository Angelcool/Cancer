using System;
using System.Data;
using System.Data.SqlClient;

namespace Mysoft.Magicodes.Repositories
{
    public enum DbEnum
    {
        SqlServer,
        MySql,
        Oracle,
        PostgreSql,
        SqlLite
    }

    /// <summary>
    /// 数据库连接配置
    /// </summary>
    public class DbConfig
    {
        
        /// <summary>
        /// 
        /// </summary>
        public static string DefaultSqlConnectionString = @"PORT=5432;DATABASE=magicodes;HOST=47.106.120.125;PASSWORD=Xiaohu520;USER ID=postgres";

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
