using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.Config
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
    }
}
