using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace David.Commons.Helper
{
    /// <summary>
    /// Request帮助类。
    /// </summary>
    public static class RequestHelper
    {
        #region 方法区域

        /// <summary>
        /// 获取当前请求的QueryString。
        /// </summary>
        /// <returns>QueryString。</returns>
        public static string QueryString()
        {
            return HttpUtility.UrlDecode(Safe(HttpContext.Current.Request.QueryString.ToString()));
        }

        /// <summary>
        /// 查询当前请求的QueryString中的指定键的值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>值。</returns>
        public static string Query(string key)
        {
            string str = HttpContext.Current.Request.QueryString[key];
            if (string.IsNullOrEmpty(str) || str.Equals("null")) return string.Empty;
            return HttpUtility.UrlDecode(Safe(str.Trim()));
        }

        /// <summary>
        /// 获取上传的文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static HttpPostedFile FileQuery(string key)
        {
            HttpPostedFile file = HttpContext.Current.Request.Files[key];
            if (file == null || file.ContentLength == 0) return null;
            return file;
        }

        /// <summary>
        /// 取得QueryString中以逗号隔开的数组
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>值数组。</returns>
        public static string[] QueryArray(string key)
        {
            string str = Query(key);
            string[] values = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return values;
        }

        /// <summary>
        /// 获取当前页面上指定字段的值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>值。</returns>
        public static string Form(string key)
        {
            string str = HttpContext.Current.Request.Form[key];
            if (string.IsNullOrEmpty(str)) return string.Empty;
            return Safe(str.Trim());
        }

        /// <summary>
        /// 获取当前页面上指定字段的值，如果值为空，返回指定的值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>值。</returns>
        public static string Form(string key, string defaultValue)
        {
            string str = HttpContext.Current.Request.Form[key];
            if (string.IsNullOrEmpty(str)) return defaultValue;
            return Safe(str.Trim());
        }

        /// <summary>
        /// 获取当前页面上指定字段的值，并以数组形式返回。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>值数组。</returns>
        public static string[] FormArray(string key)
        {
            string str = Form(key);
            string[] values = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return values;
        }

        /// <summary>
        /// 对目标字符串进行安全处理。
        /// </summary>
        /// <param name="value">目标字符串。</param>
        /// <returns>安全处理后的字符串。</returns>
        public static string Safe(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Replace("'", string.Empty);
        }

        /// <summary>
        /// 去除目标字符串的HTML格式。
        /// </summary>
        /// <param name="value">目标字符串。</param>
        /// <returns>去除HTML格式后的字符串。</returns>
        public static string RemoveHtml(string value)
        {
            return Regex.Replace(value, @"</?[^<]+>", "");
        }

        /// <summary>
        /// 获取指定路径在服务端的物理路径。
        /// </summary>
        /// <param name="path">目标路径。</param>
        /// <returns>服务端的物理路径。</returns>
        public static string ServerPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }

        /// <summary>
        /// 获取指定键的参数值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>参数值。</returns>
        public static string Param(string key)
        {
            string str = HttpContext.Current.Request.Params[key];
            if (string.IsNullOrEmpty(str)) return string.Empty;
            return HttpUtility.UrlDecode(str.Trim());
        }

        /// <summary>
        /// 获取指定键的参数值，并以列表形式返回。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>参数列表。</returns>
        public static List<string> ParamList(string key)
        {
            List<string> list = new List<string>();

            string str = Param(key);
            string[] values = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            return values.ToList();
        }

        #endregion
    }
}