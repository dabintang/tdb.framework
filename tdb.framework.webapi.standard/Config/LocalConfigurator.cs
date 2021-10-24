using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.standard.Config
{
    /// <summary>
    /// 本地配置服务
    /// </summary>
    public class LocalConfigurator
    {
        /// <summary>
        /// 本地配置服务
        /// </summary>
        private static ILocalConfig _config = null;

        /// <summary>
        /// 本地配置服务
        /// </summary>
        public static ILocalConfig Ins
        {
            get
            {
                if (_config == null)
                {
                    throw new Exception("请先在[Startup.cs]的[ConfigureServices]方法内调用方法[services.AddTdbLocalConfig]");
                }

                return _config;
            }
        }

        /// <summary>
        /// 初始化appsettings.json配置
        /// </summary>
        internal static void InitJsonConfig()
        {
            _config = new TdbJsonConfig();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config">指定服务</param>
        internal static void InitLocalConfig(ILocalConfig config)
        {
            _config = config;
        }
    }
}
