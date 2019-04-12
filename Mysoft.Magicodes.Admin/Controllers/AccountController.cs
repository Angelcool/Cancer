using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Mysoft.Magicodes.Admin.Controllers
{
    /// <summary>
    /// 账号相关控制项
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// 登陆/注册
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="remeberMe"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LogIn(string userName,string passWord,string remeberMe)
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LogOut()
        {
            return View();
        }
    }
}