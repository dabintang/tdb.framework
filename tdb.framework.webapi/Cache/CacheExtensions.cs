using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.Cache
{
    /// <summary>
    /// 缓存
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// 添加缓存服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionStrings">连接字符串集合</param>
        public static void AddTdbRedisCache(this IServiceCollection services, string[] connectionStrings)
        {
            Cacher.InitRedisCache(connectionStrings);
        }

        /// <summary>
        /// 添加缓存服务（指定服务）
        /// </summary>
        /// <param name="services"></param>
        /// <param name="getCache">获取缓存服务</param>
        public static void AddTdbCache(this IServiceCollection services, Func<ICache> getCache)
        {
            Cacher.InitCache(getCache());
        }
    }
}
