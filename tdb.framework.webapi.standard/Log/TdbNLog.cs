using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.nlog.mysql;

namespace tdb.framework.webapi.standard
{
    /// <summary>
    /// 日志
    /// </summary>
    public class TdbNLog : ILog
    {
        /// <summary>
        /// 日志
        /// </summary>
        private NLogger _log;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TdbNLog()
        {
            this._log = new NLogger();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">MySQL数据库连接字符串</param>
        /// <param name="serviceCode">服务编码</param>
        /// <param name="serviceAddress">服务地址</param>
        public TdbNLog(string connectionString, string serviceCode = "", string serviceAddress = "")
        {
            this._log = new NLogger(connectionString, serviceCode, serviceAddress);
        }

        #region 实现接口

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="message">日志内容</param>
        public void Log(EnumLogLevel level, string message)
        {
            //日志级别转换
            LogLevel logLevel = this.CvtLogLevel(level);

            this._log.Log(logLevel, message);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="exception">异常</param>
        /// <param name="message">日志内容</param>
        public void Log(EnumLogLevel level, Exception exception, string message)
        {
            //日志级别转换
            LogLevel logLevel = this.CvtLogLevel(level);

            this._log.Log(logLevel, exception, message);
        }

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

        /// <summary>
        /// 是否启用指定级别的日志
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool IsEnabled(EnumLogLevel level)
        {
            //日志级别转换
            LogLevel logLevel = this.CvtLogLevel(level);

            return this._log.IsEnabled(logLevel);
        }

        /// <summary>
        /// 是否启用Fatal级别日志
        /// </summary>
        public bool IsFatalEnabled { get { return this._log.IsFatalEnabled; } }

        /// <summary>
        /// 是否启用Error级别日志
        /// </summary>
        public bool IsErrorEnabled { get { return this._log.IsErrorEnabled; } }

        /// <summary>
        /// 是否启用Warn级别日志
        /// </summary>
        public bool IsWarnEnabled { get { return this._log.IsWarnEnabled; } }

        /// <summary>
        /// 是否启用Info级别日志
        /// </summary>
        public bool IsInfoEnabled { get { return this._log.IsInfoEnabled; } }

        /// <summary>
        /// 是否启用Debug级别日志
        /// </summary>
        public bool IsDebugEnabled { get { return this._log.IsDebugEnabled; } }

        /// <summary>
        /// 是否启用Trace级别日志
        /// </summary>
        public bool IsTraceEnabled { get { return this._log.IsTraceEnabled; } }

        #endregion

        #region 私有方法

        /// <summary>
        /// 日志级别转换
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private LogLevel CvtLogLevel(EnumLogLevel level)
        {
            switch (level)
            {
                case EnumLogLevel.Trace:
                    return LogLevel.Trace;
                case EnumLogLevel.Debug:
                    return LogLevel.Debug;
                case EnumLogLevel.Info:
                    return LogLevel.Info;
                case EnumLogLevel.Warn:
                    return LogLevel.Warn;
                case EnumLogLevel.Error:
                    return LogLevel.Error;
                case EnumLogLevel.Fatal:
                    return LogLevel.Fatal;
                default:
                    return LogLevel.Off;
            }
        }

        #endregion
    }
}
