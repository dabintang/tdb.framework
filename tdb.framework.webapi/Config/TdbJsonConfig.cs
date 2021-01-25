using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.appsettings;

namespace tdb.framework.webapi.Config
{
    /// <summary>
    /// appsettings.json配置
    /// </summary>
    public class TdbJsonConfig : IJsonConfig
    {
        #region 实现接口

        /// <summary>
        /// 获取appsettings.json配置信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetConfig<T>() where T : class, new()
        {
            return AppsettingsConfigHelper.GetConfig<T>();
        }

        #endregion
    }
}
