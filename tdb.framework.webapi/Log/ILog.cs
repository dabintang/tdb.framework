using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.Log
{
    /// <summary>
    /// 日志
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="message">日志内容</param>
        void Log(EnumLogLevel level, string message);

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="exception">异常</param>
        /// <param name="message">日志内容</param>
        void Log(EnumLogLevel level, Exception exception, string message);

        /// <summary>
        /// 痕迹日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        void Trace(string msg);

        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        void Debug(string msg);

        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        void Info(string msg);

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        void Warn(string msg);

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        void Error(string msg);

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="msg">日志内容</param>
        void Error(Exception ex, string msg);

        /// <summary>
        /// 致命日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        void Fatal(string msg);

        /// <summary>
        /// 致命日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="msg">日志内容</param>
        void Fatal(Exception ex, string msg);

        /// <summary>
        /// 是否启用指定级别的日志
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        bool IsEnabled(EnumLogLevel level);

        /// <summary>
        /// 是否启用Fatal级别日志
        /// </summary>
        bool IsFatalEnabled { get; }

        /// <summary>
        /// 是否启用Error级别日志
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// 是否启用Warn级别日志
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        /// 是否启用Info级别日志
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        /// 是否启用Debug级别日志
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// 是否启用Trace级别日志
        /// </summary>
        bool IsTraceEnabled { get; }
    }
}
