using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.standard
{
    /// <summary>
    /// 本地配置服务
    /// </summary>
    public static class LocalConfigExtensions
    {
        /// <summary>
        /// 添加appsettings.json配置服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddTdbJsonConfig(this IServiceCollection services)
        {
            LocalConfigurator.InitJsonConfig();
        }

        /// <summary>
        /// 添加指定服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="getService">获取服务</param>
        public static void AddTdbLocalConfig(this IServiceCollection services, Func<ILocalConfig> getService)
        {
            LocalConfigurator.InitLocalConfig(getService());
        }
    }
}
