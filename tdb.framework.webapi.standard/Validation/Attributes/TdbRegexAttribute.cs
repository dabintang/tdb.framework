using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace tdb.framework.webapi.standard
{
    /// <summary>
    /// 正则表达式验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TdbRegexAttribute : ValidationAttribute
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 正则表达式
        /// </summary>
        public string RegexText { get; set; }

        /// <summary>
        /// 错误提示
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            var reg = new Regex(this.RegexText);
            return reg.IsMatch(Convert.ToString(value) ?? "");
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
            errInfo.Msg = this.ErrMsg;

            return JsonConvert.SerializeObject(errInfo);
        }
    }
}
