using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.Common
{
    /// <summary>
    /// 通用
    /// </summary>
    public class HttpContextHelper
    {
        /// <summary>
        /// 获取接口指定的特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(FilterContext context) where T : Attribute
        {
            if (context == null || context.ActionDescriptor == null)
            {
                return null;
            }

            T attr = null;

            //类和方法中的特性
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDescriptor != null)
            {
                //取方法上的特性
                attr = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;
                if (attr != null)
                {
                    return attr;
                }

                //取控制器上的特性
                attr = actionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;
                if (attr != null)
                {
                    return attr;
                }
            }

            //取过滤器管道中的特性
            attr = context.Filters.Where(m => m is T).FirstOrDefault() as T;
            return attr;
        }
    }
}
