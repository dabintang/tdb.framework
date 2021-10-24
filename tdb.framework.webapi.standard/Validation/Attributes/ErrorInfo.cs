using System;
using System.Collections.Generic;
using System.Text;

namespace tdb.framework.webapi.standard.Validation.Attributes
{
    /// <summary>
    /// 错误信息
    /// </summary>
    public class ErrorInfo
    {
        /// <summary>
        /// 特性类型
        /// </summary>
        public Type AttrType { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Msg { get; set; }
    }
}
