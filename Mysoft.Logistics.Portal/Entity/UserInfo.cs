using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mysoft.Logistics.Portal
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string Cx { get; set; }
        public DateTime AddTime { get; set; }
        public string Photo { get; set; }
    }
}