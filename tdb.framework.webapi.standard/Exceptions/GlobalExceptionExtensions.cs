using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.standard
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    public static class GlobalExceptionExtensions
    {
        /// <summary>
        /// 添加全局异常处理服务
        /// </summary>
        /// <param name="options"></param>
        public static void AddTdbGlobalException(this MvcOptions options)
        {
            options.Filters.Add<GlobalExceptionFilter>();
        }
    }
}
