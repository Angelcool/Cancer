using System.Web.Mvc;
using System.Web.Security;

namespace Mysoft.Logistics.Portal
{
    /// <summary>
    /// PortalAuthorizeAttribute
    /// </summary>
    public class PortalAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// OnAuthorization
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false)) return;
            if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                //判断是否为ajax请求
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.HttpContext.Response.Write("{\"success\":false,\"message\":\"登陆失效，请重新登陆\",\"type\":\"error\",\"url\"=\"" + FormsAuthentication.LoginUrl + "\"}");
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect(FormsAuthentication.LoginUrl, true);
                }
            }
            else
            {
                //如果SESSION失效，则重新读取用户信息保存至SESSION
                if (WebHelper.GetUserFromSession<UserInfo>() == null)
                {
                    //var result = CommonHandler.RecoveryUserSession(WebHelper.GetLogOnUserId());
                    //if (!result.Success) filterContext.HttpContext.Response.Redirect(FormsAuthentication.LoginUrl, true);
                    //WebHelper.SetUserToSession<UserInfo>(result.Data);
                }
                else
                {

                }
            }
        }
    }

    /// <summary>
    /// 自定义错误处理页面
    /// </summary>
    public class PortalHandleErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 重写错误处理方法
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            //1.获取异常对象
            var ex = filterContext.Exception;

            //2.重定向到友好页面
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.HttpContext.Response.Write("{\"success\":false,\"message\":\"" + ex.Message + "\",\"type\":\"error\"}");
                filterContext.HttpContext.Response.End();
            }
            else
            {
                filterContext.Controller.ViewData["ErrorMessage"] = ex.Message;
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Error.cshtml",
                    ViewData = filterContext.Controller.ViewData,
                };
            }
            //3.标记异常处理完毕
            filterContext.ExceptionHandled = true;
        }
    }
}