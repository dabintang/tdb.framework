using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.standard
{
    /// <summary>
    /// 全局异常捕获过滤器
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// 进行异常转码返回、并记录日志
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            //异常是否已处理
            if (context.ExceptionHandled == false)
            {
                var tdbEx = context.Exception as TdbException;
                var statusCode = tdbEx == null ? 500 : 501;
                var content = tdbEx == null ? "服务器忙" : tdbEx.Message;

                //返回比较友好的错误信息
                context.Result = new Microsoft.AspNetCore.Mvc.ContentResult
                {
                    StatusCode = statusCode,
                    Content = content,
                    ContentType = "text/html;charset=utf-8"
                };

                //写日志
                try
                {
                    Logger.Ins.Error(context.Exception, context.Exception.Message);
                }
                catch
                {
                    Console.WriteLine($"[HttpGlobalExceptionFilter.OnException]未加载日志服务，ex：{context.Exception.Message}");
                }

                context.ExceptionHandled = true; //异常已处理了
            }
        }
    }
}
