﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdb.common;

namespace tdb.framework.webapi.standard
{
    /// <summary>
    /// 调接口日志过滤器特性
    /// </summary>
    public class APILogActionFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 进入接口
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                //取特性
                var attr = HttpContextHelper.GetAttribute<APILogAttribute>(context);
                //日志级别
                var logLevel = this.GetLogLevel(attr);

                if (Logger.Ins.IsEnabled(logLevel))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(string.Format("进入接口 SID={0} 接口={1} 标识={2}", this.GetUID(context.HttpContext), context.ActionDescriptor.DisplayName, context.HttpContext.GetHashCode()));
                    sb.AppendLine("入参：");
                    foreach (var key in context.ActionArguments.Keys)
                    {
                        var strVal = JsonConvert.SerializeObject(context.ActionArguments[key]);
                        sb.AppendLine(string.Format("参数名={0} 参数值={1}", key, strVal));
                    }

                    Logger.Ins.Log(logLevel, sb.ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                Logger.Ins.Error(ex, "进入接口写日志异常");
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
                //取特性
                var attr = HttpContextHelper.GetAttribute<APILogAttribute>(context);
                //日志级别
                var logLevel = this.GetLogLevel(attr);

                if (Logger.Ins.IsEnabled(logLevel))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(string.Format("离开接口 SID={0} 接口={1} 标识={2}", this.GetUID(context.HttpContext), context.ActionDescriptor.DisplayName, context.HttpContext.GetHashCode()));

                    if (context.Exception == null)
                    {
                        sb.Append("出参：");

                        if (context.Result is ObjectResult)
                        {
                            sb.Append(JsonConvert.SerializeObject(((ObjectResult)context.Result).Value));
                        }
                        else if (context.Result is FileResult)
                        {
                            sb.Append("文件流");
                        }
                        else
                        {
                            if (CommHelper.IsExistProperty(context.Result, "Value"))
                            {
                                var value = CommHelper.ReflectGet(context.Result, "Value");
                                sb.Append(JsonConvert.SerializeObject(value));
                            }
                            else
                            {
                                sb.Append(JsonConvert.SerializeObject(context.Result));
                            }
                        }

                        Logger.Ins.Log(logLevel, sb.ToString());
                    }
                    else
                    {
                        sb.Append("抛异常：" + context.Exception.Message);
                        Logger.Ins.Log(logLevel, context.Exception, sb.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Ins.Error(ex, "离开接口写日志异常");
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

        /// <summary>
        /// 获取登录人编号
        /// </summary>
        /// <param name="context">http上下文</param>
        /// <returns></returns>
        private string GetUID(HttpContext context)
        {
            //无认证用户
            if (context == null || context.User == null)
            {
                return "";
            }

            return context.User.FindFirst(TdbClaimTypes.UID)?.Value ?? "";
        }
    }
}
