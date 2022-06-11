using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tdb.framework.webapi.standard
{
    /// <summary>
    /// 枚举类型合法性验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TdbEnumDataTypeAttribute : ValidationAttribute
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 错误提示
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        public TdbEnumDataTypeAttribute(Type enumType)
        {
            this.EnumType = enumType;
        }

        /// <summary>
        /// 枚举类型
        /// </summary>
        public Type EnumType { get; }

        /// <summary>
        /// 验证合法性
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            return Enum.IsDefined(this.EnumType, value);
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
