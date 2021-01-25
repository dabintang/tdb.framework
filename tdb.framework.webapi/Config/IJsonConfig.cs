using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.Config
{
    /// <summary>
    /// appsettings.json配置
    /// </summary>
    public interface IJsonConfig
    {
        /// <summary>
        /// 获取appsettings.json配置信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetConfig<T>() where T : class, new();
    }
}
