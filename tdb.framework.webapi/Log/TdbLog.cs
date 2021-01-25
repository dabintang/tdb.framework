using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.nlog.mysql;

namespace tdb.framework.webapi.Log
{
    /// <summary>
    /// 日志
    /// </summary>
    public class TdbLog : ILog
    {
        /// <summary>
        /// 日志
        /// </summary>
        private NLogger _log;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TdbLog()
        {
            this._log = new NLogger();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">MySQL数据库连接字符串</param>
        /// <param name="servicesCode">服务编码</param>
        public TdbLog(string connectionString, string servicesCode = "TDB")
        {
            this._log = new NLogger(connectionString, servicesCode);
        }

        #region 实现接口

        /// <summary>
        /// 痕迹日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        public void Trace(string msg)
        {
            this._log.Trace(msg);
        }

        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        public void Debug(string msg)
        {
            this._log.Debug(msg);
        }

        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        public void Info(string msg)
        {
            this._log.Info(msg);
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        public void Warn(string msg)
        {
            this._log.Warn(msg);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        public void Error(string msg)
        {
            this._log.Error(msg);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="msg">日志内容</param>
        public void Error(Exception ex, string msg)
        {
            this._log.Error(ex, msg);
        }

        /// <summary>
        /// 致命日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        public void Fatal(string msg)
        {
            this._log.Fatal(msg);
        }

        /// <summary>
        /// 致命日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="msg">日志内容</param>
        public void Fatal(Exception ex, string msg)
        {
            this._log.Fatal(ex, msg);
        }

        #endregion
    }
}
