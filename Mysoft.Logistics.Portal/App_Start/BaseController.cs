using System.Data;
using System.Text;
using System.Web.Mvc;

namespace Mysoft.Logistics.Portal
{
    /// <summary>
    /// BaseController
    /// </summary>
    public class BaseController : Controller
    {
        #region 部署点列表

        /// <summary>
        /// 部署点列表
        /// </summary>
        public static DataTable DeployMent(params string[] uuid)
        {
            return CommonHandler.GetDeployment(uuid);
        }

        #endregion

        #region 本地URL

        /// <summary>
        /// 本地URL
        /// </summary>
        public string RequestUrl
        {
            get
            {
                //return "http://10.30.10.197";
                return string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority);
            }
            set { }
        }

        #endregion

        #region 重写JSON方法

        /// <summary>
        /// 重写JSON方法
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        protected override JsonResult Json(object data, string contentType,
        Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        #endregion

        #region Alert

        /// <summary>
        /// Alert
        /// </summary>
        /// <param name="message"></param>
        /// <param name="mtype">1:success、2:error、3:warning</param>
        /// <returns></returns>
        public virtual JsonResult Alert(string message, int mtype = 1, string url = "")
        {
            string type = "success";
            switch (mtype)
            {
                case 1:
                    type = "success";
                    break;
                case 2:
                    type = "error";
                    break;
                case 3:
                    type = "warning";
                    break;
            }
            if (!string.IsNullOrEmpty(message)) type = "error";
            if (string.IsNullOrEmpty(url))
                return new JsonNetResult(new { success = string.IsNullOrEmpty(message), message, type });
            return new JsonNetResult(new { success = string.IsNullOrEmpty(message), message, type, url });
        }

        #endregion

        #region GetClientUrl

        /// <summary>
        /// GetClientUrl
        /// </summary>
        /// <returns></returns>
        public string GetClientUrl()
        {
            if (RequestUrl.StartsWith("http://127.0.0.1"))
            {
                RequestUrl = RequestUrl.Replace("http://127.0.0.1", WebHelper.GetClientIPAddress());
            }

            return RequestUrl + "/Api/EPMSUpgrade/UpdateUpgradeProgress";
        }

        /// <summary>
        /// 
        /// </summary>
        public string UpgradeServiceUrl
        {
            get
            {
                if (RequestUrl.StartsWith("http://127.0.0.1"))
                {
                    RequestUrl = RequestUrl.Replace("http://127.0.0.1", WebHelper.GetClientIPAddress());
                }

                return RequestUrl + "/Api/EPMSUpgrade/UpdateWinServiceStatus";
            }
        }

        #endregion

        /// <summary>
        /// 检查用户权限 这里用把所有的请求添加到数据库，暂时没有时间加
        /// </summary>
        /// <param name="filterContext"></param>
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    //判断用户是否具有请求地址的权限
        //    var result = CommonHandler.RecoveryUserSession(WebHelper.GetLogOnUserId());
        //    if (!result.Success) throw new Exception(result.Message);
        //    //如果地址在菜单权限中则说明由权限 此处应该用菜单集合的,不应该用菜单的集合字段字符串
        //    if (!result.Data.MenuItem.Contains(filterContext.HttpContext.Request.RawUrl))
        //    {
        //        if (filterContext.HttpContext.Request.IsAjaxRequest())
        //        {
        //            filterContext.HttpContext.Response.StatusCode = 401;
        //            filterContext.HttpContext.Response.Write("{\"success\":false,\"message\":\"您没有权限访问此页面\",\"type\":\"error\"}");
        //            filterContext.HttpContext.Response.End();
        //        }
        //        else
        //        {
        //            //直接跳转到登陆页面
        //            filterContext.HttpContext.Response.Redirect(FormsAuthentication.LoginUrl, true);
        //        }
        //    }
        //    else
        //    {
        //        base.OnActionExecuting(filterContext);
        //    }
        //}
    }
}