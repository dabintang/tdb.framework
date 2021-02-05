using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.Config
{
    /// <summary>
    /// 分布式配置服务
    /// </summary>
    public class DistributedConfigurator
    {
        /// <summary>
        /// 分布式配置服务
        /// </summary>
        private static IDistributedConfig _config = null;

        /// <summary>
        /// 分布式配置服务
        /// </summary>
        public static IDistributedConfig Ins
        {
            get
            {
                if (_config == null)
                {
                    throw new Exception("请先在[Startup.cs]的[ConfigureServices]方法内调用方法[services.AddTdbDistributedConfig]");
                }

                return _config;
            }
        }

        /// <summary>
        /// 初始化consul分布式配置服务
        /// </summary>
        /// <param name="consulIP">consul服务IP</param>
        /// <param name="consulPort">consul服务端口</param>
        /// <param name="prefixKey">key前缀，一般用来区分不同服务</param>
        internal static void InitConsulConfig(string consulIP, int consulPort = 8500, string prefixKey = "TDB")
        {
            _config = new TdbConsulConfig(consulIP, consulPort, prefixKey);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config">指定服务</param>
        internal static void InitDistributedConfig(IDistributedConfig config)
        {
            _config = config;
        }
    }
}
