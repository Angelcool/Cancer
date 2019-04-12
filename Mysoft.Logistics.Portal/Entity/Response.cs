using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mysoft.Logistics.Portal
{
    /// <summary>
    /// Response
    /// </summary>
    public class Response
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        private string _message;
        [JsonProperty(PropertyName = "message")]
        public string Message
        {
            get { return _message; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    Success = false;
                _message = value;
            }
        }

        public Response()
        {
            Success = true;
        }
    }

    /// <summary>
    /// Response<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RessponseSingle<T> : Response
    {
        /// <summary>
        /// Data
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// Response<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T> : Response
    {
        [JsonProperty(PropertyName = "total")]
        public int Total
        {
            get
            {
                if (Rows <= 0) return 1;
                int totalPage = (int)Math.Ceiling(Records/(Rows * 1.0));
                return totalPage <= 0 ? 1 : totalPage;
            }
        }
        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; }
        [JsonProperty(PropertyName = "records")]
        public int Records { get; set; }
        [JsonProperty(PropertyName = "costtime")]
        public float CostTime { get; set; }
        [JsonProperty(PropertyName = "rows")]
        public T Data { get; set; }
        [JsonIgnore]
        public int Rows { get; set; }
    }

    /// <summary>
    /// ResponseSet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseSet<T> : Response
    {
        /// <summary>
        /// 数据总条数
        /// </summary>
        private int _totalRecords;
        public int TotalRecords
        {
            get { return _totalRecords; }
            set
            {
                if (value <= 0) _totalRecords = Datas.Count;
                else _totalRecords = value;
            }
        }

        /// <summary>
        /// 要返回的数据
        /// </summary>
        public IList<T> Datas { get; set; }

        /// <summary>
        /// 查询分页的大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
