using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.appsettings;

namespace tdb.framework.webapi.standard.Config
{
    /// <summary>
    /// appsettings.json配置
    /// </summary>
    public class TdbJsonConfig : ILocalConfig
    {
        /// <summary>
        /// 配置重新加载事件
        /// </summary>
        public event AppsettingsConfigHelper._ConfigReload ConfigReload;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TdbJsonConfig()
        {
            AppsettingsConfigHelper.ConfigReload += OnConfigReload;
        }

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

        /// <summary>
        /// 配置重新加载
        /// </summary>
        /// <param name="config"></param>
        private void OnConfigReload(IConfigurationRoot config)
        {
            if (this.ConfigReload != null)
            {
                this.ConfigReload(config);
            }
        }
    }
}
