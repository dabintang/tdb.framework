using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdb.framework.webapi.Common;
using tdb.framework.webapi.Log;

namespace tdb.framework.webapi.APILog
{
    /// <summary>
    /// 调接口日志过滤器特性
    /// </summary>
    public class APILogActionFilterAttribute : ActionFilterAttribute
    {
        ///// <summary>
        ///// 获取特性
        ///// </summary>
        ///// <param name="actionDes"></param>
        ///// <returns></returns>
        //public static T GetAttribute<T>(ActionDescriptor actionDes) where T : class
        //{
        //    if (actionDes != null)
        //    {
        //        if (actionDes..ControllerTypeInfo.GetCustomAttributes(inherit: true)
        //                                                .Any(attr => attr.GetType().Equals(typeof(AllowAnonymousAttribute)))
        //        || actionDes.MethodInfo.GetCustomAttributes(inherit: true)
        //                                                .Any(attr => attr.GetType().Equals(typeof(AllowAnonymousAttribute))))
        //        {
        //            isAnonymous = true;
        //        }
        //    }
        //}


        /// <summary>
        /// 进入接口
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format("进入接口 接口={0} 标识={1}", context.ActionDescriptor.DisplayName, context.HttpContext.GetHashCode()));
                sb.AppendLine("入参：");
                foreach (var key in context.ActionArguments.Keys)
                {
                    var strVal = JsonConvert.SerializeObject(context.ActionArguments[key]);
                    sb.AppendLine(string.Format("参数名={0} 参数值={1}", key, strVal));
                }

                //取特性
                var attr = HttpContextHelper.GetAttribute<APILogAttribute>(context);
                Logger.Ins.Log(this.GetLogLevel(attr), sb.ToString().Trim());
            }
            catch
            {
            }

            base.OnActionExecuting(context);
        }

        /// <summary>
        /// 离开接口
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);

            try
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format("离开接口 接口={0} 标识={1}", context.ActionDescriptor.DisplayName, context.HttpContext.GetHashCode()));

                if (context.Exception == null)
                {
                    sb.Append("出参：");

                    if (context.Result is ObjectResult)
                    {
                        sb.Append(JsonConvert.SerializeObject(((ObjectResult)context.Result).Value));
                    }
                    else if (context.Result is JsonResult)
                    {
                        sb.Append(JsonConvert.SerializeObject(((JsonResult)context.Result).Value));
                    }
                    else if (context.Result is FileResult)
                    {
                        sb.Append("文件流");
                    }
                    else
                    {
                        sb.Append($"未处理类型({context.Result.GetType()})");
                    }

                    //取特性
                    var attr = HttpContextHelper.GetAttribute<APILogAttribute>(context);
                    Logger.Ins.Log(this.GetLogLevel(attr), sb.ToString());
                }
                else
                {
                    sb.Append("抛异常：" + context.Exception.Message);
                    Logger.Ins.Error(context.Exception, sb.ToString());
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 从特性获取日志级别
        /// </summary>
        /// <param name="attr">特性</param>
        /// <returns></returns>
        private EnumLogLevel GetLogLevel(APILogAttribute attr)
        {
            if (attr == null)
            {
                return EnumLogLevel.Trace;
            }

            return attr.Level;
        }
    }
}
