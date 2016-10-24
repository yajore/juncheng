﻿using System;
using System.Web;

namespace Lawyers.Utilities
{
    public class CookiesHelper
    {
        /// <summary> 
        /// 获得Cookie的值 
        /// </summary> 
        /// <param name="cookieName"></param> 
        /// <returns></returns> 
        public static string GetCookieValue(string cookieName)
        {
            return GetCookieValue(cookieName, null);
        }

        /// <summary> 
        /// 获得Cookie的值 
        /// </summary> 
        /// <param name="cookieName"></param> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static string GetCookieValue(string cookieName, string key)
        {
            HttpRequest request = HttpContext.Current.Request;
            return GetCookieValue(request.Cookies[cookieName], key);
        }

        /// <summary> 
        /// 获得Cookie的子键值 
        /// </summary> 
        /// <param name="cookie"></param> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static string GetCookieValue(HttpCookie cookie, string key)
        {
            if (cookie != null)
            {
                if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                    return cookie.Values[key];

                return cookie.Value;
            }
            return "";
        }

        /// <summary> 
        /// 获得Cookie 
        /// </summary> 
        /// <param name="cookieName"></param> 
        /// <returns></returns> 
        public static HttpCookie GetCookie(string cookieName)
        {
            HttpRequest request = HttpContext.Current.Request;
            return request.Cookies[cookieName];
        }


        #region 删除Cookie

        /// <summary> 
        /// 删除Cookie 
        /// </summary> 
        /// <param name="cookieName"></param> 
        public static void RemoveCookie(string cookieName)
        {
            RemoveCookie(cookieName, null);
        }

        /// <summary> 
        /// 删除Cookie的子键 
        /// </summary> 
        /// <param name="cookieName"></param> 
        /// <param name="key"></param> 
        public static void RemoveCookie(string cookieName, string key)
        {
            HttpResponse response = HttpContext.Current.Response;
            {
                HttpCookie cookie = response.Cookies[cookieName];
                cookie.Expires = DateTime.Now.AddDays(-1);
                if (cookie != null)
                {
                    if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                        cookie.Values.Remove(key);
                    else
                    {
                        response.Cookies.Remove(cookieName);
                        response.SetCookie(cookie);
                    }

                }
            }
        }

        #endregion

        #region 设置/修改Cookie

        /// <summary> 
        /// 设置Cookie子键的值 
        /// </summary> 
        /// <param name="cookieName"></param> 
        /// <param name="key"></param> 
        /// <param name="value"></param> 
        public static void SetCookie(string cookieName, string key, string value)
        {
            SetCookie(cookieName, key, value, null);
        }

        /// <summary> 
        /// 设置Cookie值 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="value"></param> 
        public static void SetCookie(string key, string value)
        {
            SetCookie(key, null, value, null);
        }

        /// <summary> 
        /// 设置Cookie值和过期时间 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="value"></param> 
        /// <param name="expires"></param> 
        public static void SetCookie(string key, string value, DateTime expires)
        {
            SetCookie(key, null, value, expires);
        }

        /// <summary> 
        /// 设置Cookie过期时间 
        /// </summary> 
        /// <param name="cookieName"></param> 
        /// <param name="expires"></param> 
        public static void SetCookie(string cookieName, DateTime expires)
        {
            SetCookie(cookieName, null, null, expires);
        }

        /// <summary> 
        /// 设置Cookie 
        /// </summary> 
        /// <param name="cookieName"></param> 
        /// <param name="key"></param> 
        /// <param name="value"></param> 
        /// <param name="expires"></param> 
        public static void SetCookie(string cookieName, string key, string value, DateTime? expires)
        {
            HttpResponse response = HttpContext.Current.Response;
            {
                HttpCookie cookie = response.Cookies[cookieName];
                if (cookie != null)
                {
                    if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                        cookie.Values.Set(key, value);
                    else
                        if (!string.IsNullOrEmpty(value))
                        cookie.Value = value;
                    if (expires != null)
                        cookie.Expires = expires.Value;
                    response.SetCookie(cookie);
                }
            }
        }

        #endregion

        #region 添加Cookie

        /// <summary> 
        /// 添加Cookie 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="value"></param> 
        public static void AddCookie(string key, string value)
        {
            AddCookie(new HttpCookie(key, value));
        }

        /// <summary> 
        /// 添加Cookie 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="value"></param> 
        /// <param name="expires"></param> 
        public static void AddCookie(string key, string value, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(key, value);
            cookie.Expires = expires;
            AddCookie(cookie);
        }

        /// <summary> 
        /// 添加为Cookie.Values集合 
        /// </summary> 
        /// <param name="cookieName"></param> 
        /// <param name="key"></param> 
        /// <param name="value"></param> 
        public static void AddCookie(string cookieName, string key, string value)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Values.Add(key, value);
            AddCookie(cookie);
        }

        /// <summary> 
        /// 添加为Cookie集合 
        /// </summary> 
        /// <param name="cookieName">Cookie名称</param> 
        /// <param name="expires">过期时间</param> 
        public static void AddCookie(string cookieName, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = expires;
            AddCookie(cookie);
        }

        /// <summary> 
        /// 添加为Cookie.Values集合 
        /// </summary> 
        /// <param name="cookieName"></param> 
        /// <param name="key"></param> 
        /// <param name="value"></param> 
        /// <param name="expires"></param> 
        public static void AddCookie(string cookieName, string key, string value, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = expires;
            cookie.Values.Add(key, value);
            AddCookie(cookie);
        }

        /// <summary> 
        /// 添加Cookie 
        /// </summary> 
        /// <param name="cookie"></param> 
        public static void AddCookie(HttpCookie cookie)
        {
            HttpResponse response = HttpContext.Current.Response;
            //指定客户端脚本是否可以访问[默认为false] 
            cookie.HttpOnly = true;
            //指定统一的Path，比便能通存通取 
            cookie.Path = "/";
            //设置跨域,这样在其它二级域名下就都可以访问到了 
            //cookie.Domain = "Valonjiaju.cn";
            //启用.com域名
            ////cookie.Domain = WebConfigurationManager.AppSettings["AppDomain"] ?? "Valonjiaju.cn";
            HttpRequest request = HttpContext.Current.Request;
            if (request.Url.AbsoluteUri.Contains("localhost"))
                cookie.Domain = null;//本地调试加domain会取不到cookie
            response.AppendCookie(cookie);
        }

        /// <summary>
        /// www访问下添加cookie
        /// </summary>
        /// <param name="cookieName">cookieName</param>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="cookieTime">value</param>
        public static void AddCookiewww(string cookieName, string key, string value, DateTime cookieTime)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = cookieTime;
            cookie.Values.Add(key, value);
            HttpResponse response = HttpContext.Current.Response;
            //指定客户端脚本是否可以访问[默认为false] 
            cookie.HttpOnly = true;
            //指定统一的Path，比便能通存通取 
            cookie.Path = "/";
            //设置跨域,WWW 的情况可以看到cookie
            //cookie.Domain = "www.Valonjiaju.cn";
            HttpRequest request = HttpContext.Current.Request;
            if (request.Url.AbsoluteUri.Contains("localhost"))
                cookie.Domain = null;//本地调试加domain会取不到cookie
            response.AppendCookie(cookie);
        }
        #endregion
    }
}
