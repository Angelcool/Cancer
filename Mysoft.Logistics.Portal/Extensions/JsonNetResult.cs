﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Mysoft.Logistics.Portal
{
    /// <summary>
    /// JsonNetResult
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        /// <summary>
        /// 设置
        /// </summary>
        public JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            //这句是解决问题的关键,也就是json.net官方给出的解决配置选项.                 
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public JsonNetResult()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public JsonNetResult(object data)
        {
            Data = data;
        }

        /// <summary>
        /// ExecuteResult
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            //if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            //{
            //    throw new InvalidOperationException("JSON GET is not allowed");
            //}
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;
            if (this.ContentEncoding != null)  response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null) return;
            var scriptSerializer = JsonSerializer.Create(this.Settings);
            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, this.Data);
                response.Write(sw.ToString());
            }
        }
    }
}