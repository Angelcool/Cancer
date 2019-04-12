using System.Web;
using System.Web.Mvc;

namespace Mysoft.Logistics.Portal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //自定义权限验证
            filters.Add(new PortalAuthorizeAttribute());
            //自定义错误处理
            filters.Add(new PortalHandleErrorAttribute());
        }
    }
}
