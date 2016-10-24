using System;

namespace Lawyers.Utilities
{
    /// <summary>
    /// WebCache 的摘要说明
    /// </summary>
    public class WebCache
    {
        private static readonly System.Web.Caching.Cache ObjCache = System.Web.HttpRuntime.Cache;

        #region Get 获取一个对象
        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <param name="key">对象Key</param>
        /// <returns></returns>
        public static object Get(string key)
        {
            object obj = null;
            if (ObjCache[key] != null) obj = ObjCache.Get(key);
            return obj;
        }
        #endregion

        #region Exist 对象是否存在
        /// <summary>
        /// Exist 使用Exist判断对象的存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exist(string key)
        {
            if (ObjCache[key] == null) return false;
            return true;
        }
        #endregion

        #region Set 更新一个对象
        /// <summary>
        /// 更新一个对象
        /// </summary>
        /// <param name="key">对象Key</param>
        /// <param name="min">要进行缓存的时间(Minutes)</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static void Set(string key, object obj, DateTime expiry)
        {
            if (ObjCache[key] != null) ObjCache.Remove(key);
            ObjCache.Insert(key, obj, null, expiry, TimeSpan.Zero);
        }
        #endregion

        #region Del 删除一个对象
        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <param name="key">对象Key</param>
        /// <returns></returns>
        public static void Del(string key)
        {
            if (ObjCache[key] != null) ObjCache.Remove(key);
        }
        #endregion

        #region 缓存中的总项数
        /// <summary>
        /// 缓存中的总项数
        /// </summary>
        public static int Count
        {
            get { return ObjCache.Count; }
        }
        #endregion

        #region 缓存中的总字节数
        /// <summary>
        /// 缓存中的总字节数
        /// </summary>
        public static long PrivateBytes
        {
            get { return ObjCache.EffectivePrivateBytesLimit; }
        }
        #endregion
    }
}