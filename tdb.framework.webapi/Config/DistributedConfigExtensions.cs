using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.Config
{
    /// <summary>
    /// 分布式配置服务
    /// </summary>
    public static class DistributedConfigExtensions
    {
        /// <summary>
        /// 添加consul分布式配置服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="consulIP">consul服务IP</param>
        /// <param name="consulPort">consul服务端口</param>
        /// <param name="prefixKey">key前缀，一般用来区分不同服务</param>
        public static void AddTdbConsulConfig(this IServiceCollection services, string consulIP, int consulPort = 8500, string prefixKey = "TDB")
        {
            DistributedConfigurator.InitConsulConfig(consulIP, consulPort, prefixKey);
        }

        /// <summary>
        /// 添加指定分布式配置服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="getService">获取服务</param>
        public static void AddTdbDistributedConfig(this IServiceCollection services, Func<IDistributedConfig> getService)
        {
            DistributedConfigurator.InitDistributedConfig(getService());
        }
    }
}
