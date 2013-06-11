using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DavidLoginSystem.Models;
using DavidLoginSystem.Common;
using System.Web.Security;

namespace DavidLoginSystem.Controllers
{
    public class BaseController : Controller
    {
        #region 私有变量

        protected LoginSystemEntities db;
        protected const string SESSION_LOGIN_KEY = "LoginUser";
        protected const string SESSION_CODE = "VerifyCode";
        protected const string CACHE_KEY = "CacheKey";
        protected const string CACHE_USERS = "CacheUsers";

        #endregion

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (LoginHelper.IsLogin())
            {
                if (filterContext.HttpContext.Session[SESSION_LOGIN_KEY] == null)
                {
                    FormsAuthentication.SignOut();
                    HttpContext.Session.RemoveAll();
                    HttpContext.Cache.Remove(CACHE_KEY);
                    filterContext.HttpContext.Response.Write("<script language=\"javascript\" type=\"text/javascript\">window.location.href=\"/Home/Login\";</script>");
                }
            }
        }
    }

    /// <summary>
    /// 图形Result
    /// </summary>
    public class ImageResult : ActionResult
    {
        public byte[] imageBuffer;
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "image/jpeg";
            context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.HttpContext.Response.BufferOutput = false;
            context.HttpContext.Response.OutputStream.Write(imageBuffer, 0, imageBuffer.Length);
        }
    }
}
