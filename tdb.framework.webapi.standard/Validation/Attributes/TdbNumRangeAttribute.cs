using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tdb.framework.webapi.standard
{
    /// <summary>
    /// 数值范围验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TdbNumRangeAttribute : ValidationAttribute
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public double MinValue { get; set; } = double.MinValue;

        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxValue { get; set; } = double.MaxValue;

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            var dVal = Convert.ToDouble(value);

            if (dVal < this.MinValue || dVal > this.MaxValue)
            {
                return false;
            }

            return true;
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

            if (this.MinValue != double.MinValue && this.MaxValue != double.MaxValue)
            {
                errInfo.Msg = $"{ParamName}的值应该在{this.MinValue}-{this.MaxValue}范围内";
            }
            else if (this.MinValue != double.MinValue)
            {
                errInfo.Msg = $"{ParamName}的值应该大于{this.MinValue}";
            }
            else
            {
                errInfo.Msg = $"{ParamName}的值应该小于{this.MaxValue}";
            }

            return JsonConvert.SerializeObject(errInfo);
        }
    }
}
