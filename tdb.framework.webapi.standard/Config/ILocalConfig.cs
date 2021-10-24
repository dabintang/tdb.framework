using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.appsettings;

namespace tdb.framework.webapi.standard.Config
{
    /// <summary>
    /// 本地配置服务
    /// </summary>
    public interface ILocalConfig
    {
        /// <summary>
        /// 获取本地配置信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetConfig<T>() where T : class, new();

        /// <summary>
        /// 配置重新加载事件
        /// </summary>
        event AppsettingsConfigHelper._ConfigReload ConfigReload;
    }
}
