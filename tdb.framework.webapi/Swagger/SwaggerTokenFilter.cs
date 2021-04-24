//using Microsoft.AspNetCore.Authorization;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.SwaggerGen;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;

//namespace tdb.framework.webapi.Swagger
//{
//    /// <summary>
//    /// swagger 生成token输入框
//    /// </summary>
//    public class SwaggerTokenFilter : IOperationFilter
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="operation"></param>
//        /// <param name="context"></param>
//        public void Apply(OpenApiOperation operation, OperationFilterContext context)
//        {
//            //获取方法信息
//            MethodInfo methodInfo = null;
//            if (context.ApiDescription.TryGetMethodInfo(out methodInfo) == false)
//            {
//                return;
//            }

//            //判断方法是否允许匿名
//            var allowAnonymous = methodInfo.GetCustomAttributes().Where(m => m is IAllowAnonymous).Count() > 0 ||
//                                 methodInfo.ReflectedType.GetCustomAttributes().Where(m => m is IAllowAnonymous).Count() > 0;

//            //不允许匿名，添加token参数
//            if (allowAnonymous == false)
//            {
//                if (operation.Parameters == null)
//                {
//                    operation.Parameters = new List<OpenApiParameter>();
//                }

//                operation.Parameters.Add(new OpenApiParameter { Name = "Authorization", In = ParameterLocation.Header, Description = "token", Required = false });
//            }
//        }
//    }
//}
