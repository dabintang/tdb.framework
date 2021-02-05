using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.Log
{
    /// <summary>
    /// 日志级别
    /// </summary>
    public enum EnumLogLevel
    {
        /// <summary>
        /// 痕迹
        /// </summary>
        Trace = 0,

        /// <summary>
        /// 调试
        /// </summary>
        Debug = 1,

        /// <summary>
        /// 信息
        /// </summary>
        Info = 2,

        /// <summary>
        /// 警告
        /// </summary>
        Warn = 3,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 4,

        /// <summary>
        /// 致命
        /// </summary>
        Fatal = 5,

        /// <summary>
        /// 关闭
        /// </summary>
        Off = 6
    }
}