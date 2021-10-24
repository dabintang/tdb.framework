using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tdb.framework.webapi.standard.Validation.Attributes
{
    /// <summary>
    /// 字符串长度验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TdbStringLengthAttribute : StringLengthAttribute
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maximumLength">最大长度</param>
        public TdbStringLengthAttribute(int maximumLength) : base(maximumLength)
        {
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

            if (this.MinimumLength > 0)
            {
                errInfo.Msg = $"{ParamName}的长度应该在{this.MinimumLength}-{this.MaximumLength}个字符范围内";
            }
            else
            {
                errInfo.Msg = $"{ParamName}的长度不能超过{this.MaximumLength}个字符";
            }

            return JsonConvert.SerializeObject(errInfo);
        }
    }
}
