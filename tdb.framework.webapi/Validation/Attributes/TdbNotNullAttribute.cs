using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tdb.framework.webapi.Validation.Attributes
{
    /// <summary>
    /// 不能为null
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TdbNotNullAttribute : ValidationAttribute
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            return (value != null);
        }

        /// <summary>
        /// 格式化消息字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            var errInfo = new ErrorInfo();
            errInfo.AttrType = this.GetType();
            errInfo.Msg = $"{ParamName}不能为null";

            return JsonConvert.SerializeObject(errInfo);
        }
    }
}
