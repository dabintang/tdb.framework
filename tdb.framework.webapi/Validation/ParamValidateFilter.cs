using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tdb.framework.webapi.DTO;
using tdb.framework.webapi.Validation.Attributes;

namespace tdb.framework.webapi.Validation
{
    /// <summary>
    /// 参数验证过滤器
    /// </summary>
    public class ParamValidateFilter : IActionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid && context.ModelState.ErrorCount > 0)
            {
                //获取第一个错误信息
                var errModelState = context.ModelState.Where(m => m.Value.Errors.Count > 0).FirstOrDefault();
                var strErr = errModelState.Value.Errors.First().ErrorMessage;

                try
                {
                    var msgInfo = JsonConvert.DeserializeObject<ErrorInfo>(strErr);
                    context.Result = new ObjectResult(new BaseItemRes<object>(false, "ParamFail", msgInfo.Msg, null));
                }
                catch
                {
                    var field = errModelState.Key;
                    var message = $"入参类型错误（字段：{field}）";
                    context.Result = new Microsoft.AspNetCore.Mvc.ContentResult
                    {
                        Content = message,
                        StatusCode = 400,
                        ContentType = "text/html;charset=utf-8"
                    };
                }
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
