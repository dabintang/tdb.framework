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
                    throw new Exception("请先在[Startup.cs]的[ConfigureServices]方法内调用方法[services.AddLog]");
                }

                return _log;
            }
        }

        /// <summary>
        /// 初始化日志
        /// </summary>
        internal static void InitLog()
        {
            _log = new TdbLog();
        }

        /// <summary>
        /// 初始化日志（Mysql）
        /// </summary>
        /// <param name="connectionString">MySQL数据库连接字符串</param>
        /// <param name="servicesCode">服务编码</param>
        internal static void InitMySqlLog(string connectionString, string servicesCode = "TDB")
        {
            _log = new TdbLog(connectionString, servicesCode);
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
