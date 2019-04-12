using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Mysoft.Logistics.Portal.Responsitory
{
    /// <summary>
    /// SqlHelper
    /// </summary>
    public static class SqlHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string DefaultConnectionString = ConfigurationManager.AppSettings["DefaultConnectionString"];

        #region 属性

        /// <summary>
        /// GetSqlConnection
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static SqlConnection GetSqlConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(DefaultConnectionString);
            if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();

            return sqlConnection;
        }

        #endregion 

        #region ExecuteNonQuery

        /// <summary>
        /// 数据更新ExecuteNonQuery() + int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Response<int> ExecuteNonQuery(string sql, object parameters)
        {
            int result = -1;
            try
            {
                using (SqlConnection sqlCon = GetSqlConnection())
                {
                    result = sqlCon.Execute(sql, parameters);
                }

                return new Response<int> { Data = result };
            }
            catch (SqlException ex)
            {
                return new Response<int> { Message = ex.Message };
            }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 数据查询，返回一个object类型的值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Response<T> ExecuteScalar<T>(string sql, object parameters)
        {
            T result;
            try
            {
                using (SqlConnection sqlCon = GetSqlConnection())
                {
                    result = sqlCon.ExecuteScalar<T>(sql, parameters);
                }
                return new Response<T> { Data = result };
            }
            catch (SqlException ex)
            {
                return new Response<T> { Message = ex.Message };
            }
        }

        #endregion

        #region ExecuteDataTable

        /// <summary>
        /// 用于小数据集，读取内容放在内存中
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Response<T> FirstOrDefault<T>(string sql, object parameters)
        {
            try
            {
                T result;
                using (SqlConnection sqlCon = GetSqlConnection())
                {
                    result = sqlCon.QueryFirst<T>(sql, parameters);
                }

                return new Response<T> { Data = result };
            }
            catch (SqlException ex)
            {
                return new Response<T> { Message = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static ResponseSet<T> QueryForList<T>(string sql, object parameters)
        {
            try
            {
                IList<T> result;
                using (SqlConnection sqlCon = GetSqlConnection())
                {
                    result = sqlCon.Query<T>(sql, parameters).ToList();
                }

                return new ResponseSet<T> { Datas = result };
            }
            catch (SqlException ex)
            {
                return new ResponseSet<T> { Message = ex.Message };
            }
        }

        #endregion
    }
}