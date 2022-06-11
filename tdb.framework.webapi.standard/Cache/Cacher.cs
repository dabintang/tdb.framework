using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.standard
{
    /// <summary>
    /// 缓存
    /// </summary>
    public class Cacher
    {
        /// <summary>
        /// 缓存
        /// </summary>
        private static ICache _cache = null;

        /// <summary>
        /// 缓存实例
        /// </summary>
        public static ICache Ins
        {
            get
            {
                if (_cache == null)
                {
                    throw new Exception("请先在[Startup.cs]的[ConfigureServices]方法内调用方法[services.AddTdbCache]");
                }

                return _cache;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="connectionStrings">连接字符串集合</param>
        internal static void InitRedisCache(string[] connectionStrings)
        {
            _cache = new TdbRedisCache(connectionStrings);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="cache">缓存服务</param>
        internal static void InitCache(ICache cache)
        {
            _cache = cache;
        }
    }
}
