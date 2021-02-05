using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.framework.webapi.Log;

namespace tdb.framework.webapi.APILog
{
    /// <summary>
    /// 调用API日志
    /// </summary>
    public class APILogAttribute : Attribute
    {
        /// <summary>
        /// 日志级别
        /// </summary>
        public EnumLogLevel Level { get; set; }
    }
}
