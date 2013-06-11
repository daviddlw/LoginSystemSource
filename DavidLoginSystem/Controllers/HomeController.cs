using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DavidLoginSystem.Common;
using DavidLoginSystem.Models;
using System.Web.Security;
using David.Commons.Helper;

namespace DavidLoginSystem.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (LoginHelper.IsLogin())
            {
                User loginUser = HttpContext.Session[SESSION_LOGIN_KEY] as User;
                return Redirect("/User/Index");
            }
            else
                return RedirectToAction("Login");
        }

        /// <summary>
        /// 相关帮助
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// 登录页
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View("Login");
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.Session.RemoveAll();
            HttpContext.Cache.Remove(CACHE_KEY);
            return RedirectToAction("Login");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public JsonResult LoginIn()
        {
            bool flag = false, isCorrectCode = false;
            string message = string.Empty;
            //对于用户权限的信息一次性load到内存中然后进行判断；
            IList<User> userLs = HttpContext.Cache["CacheUsers"] as List<User>;
            using (db = new LoginSystemEntities())
            {
                string userName = RequestHelper.Form("username").ToString();
                string password = RequestHelper.Form("password").ToString();
                string checkCode = RequestHelper.Form("code").ToString();

                //User loginUser = db.Users.FirstOrDefault(n => n.Status > 0 && n.UserName.Trim() == userName.Trim() && n.Password.Trim().ToLower() == password.Trim().ToLower());

                User loginUser = userLs.FirstOrDefault(n => n.Status > 0 && n.UserName.Trim() == userName.Trim() && n.Password.Trim().ToLower() == password.Trim().ToLower());

                if (string.IsNullOrEmpty(checkCode))
                    isCorrectCode = false;

                string currentCode = HttpContext.Session[SESSION_CODE] == null ? string.Empty : HttpContext.Session[SESSION_CODE].ToString();
                isCorrectCode = string.Compare(currentCode, checkCode, StringComparison.CurrentCultureIgnoreCase) == 0;

                if (isCorrectCode == false)
                    return Json(new { flag = 0, msg = "验证码不正确" }, JsonRequestBehavior.AllowGet);

                if (loginUser != null && isCorrectCode)
                {
                    FormsAuthentication.SetAuthCookie(userName, false);
                    HttpContext.Session[SESSION_LOGIN_KEY] = loginUser;
                    //HttpContext.Session.Timeout = 1;
                    if (HttpContext.Cache[CACHE_KEY] == null)
                        HttpContext.Cache[CACHE_KEY] = loginUser;
                    else
                    {
                        User cacheUser = HttpContext.Cache[CACHE_KEY] as User;
                        if (cacheUser == null)
                            HttpContext.Cache[CACHE_KEY] = loginUser;
                        else
                        {
                            if (cacheUser.CompareTo(loginUser) != 0)
                            {
                                HttpContext.Cache[CACHE_KEY] = loginUser;
                            }
                        }
                    }

                    flag = true;
                }
                else
                {
                    message = "用户名或密码不正确，请重新输入";
                    HttpContext.Session.Remove(SESSION_LOGIN_KEY);
                }
            }

            return Json(new { flag = flag == true ? 1 : 0, msg = message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 生成新的验证码
        /// </summary>
        /// <returns></returns>
        public ImageResult NewCheckCode()
        {
            CheckCode checkCode = CheckCodeUtil.GenerateNewCode();
            HttpContext.Session[SESSION_CODE] = checkCode.Code;
            return new ImageResult() { imageBuffer = checkCode.Buffer };
        }
    }
}
