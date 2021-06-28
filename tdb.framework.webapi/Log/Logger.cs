using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.Log
{
    /// <summary>
    /// 日志
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// 日志
        /// </summary>
        private static ILog _log = null;

        /// <summary>
        /// 日志实例
        /// </summary>
        public static ILog Ins
        {
            get
            {
                if (_log == null)
                {
                    throw new Exception("请先在[Startup.cs]的[ConfigureServices]方法内调用方法[services.AddTdbNLogger]");
                }

                return _log;
            }
        }

        /// <summary>
        /// 初始化日志
        /// </summary>
        internal static void InitNLog()
        {
            _log = new TdbNLog();
        }

        /// <summary>
        /// 初始化日志（Mysql）
        /// </summary>
        /// <param name="connectionString">MySQL数据库连接字符串</param>
        /// <param name="serviceCode">服务编码</param>
        /// <param name="serviceAddress">服务地址</param>
        internal static void InitMySqlNLog(string connectionString, string serviceCode = "", string serviceAddress = "")
        {
            _log = new TdbNLog(connectionString, serviceCode, serviceAddress);
        }

        /// <summary>
        /// 初始化日志
        /// </summary>
        /// <param name="log">日志服务</param>
        internal static void InitLog(ILog log)
        {
            _log = log;
        }
    }
}
