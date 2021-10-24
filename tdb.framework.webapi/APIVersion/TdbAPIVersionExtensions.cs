using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.APIVersion
{
    /// <summary>
    /// api版本控制
    /// </summary>
    public static class TdbAPIVersionExtensions
    {
        /// <summary>
        /// 添加api版本控制服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">设置配置方法</param>
        /// <returns></returns>
        public static IServiceCollection AddTdbApiVersioning(this IServiceCollection services, Action<ApiVersioningOptions> setupAction = null)
        {
            if (setupAction == null)
            {
                return services.AddApiVersioning();
            }
            else
            {
                return services.AddApiVersioning(setupAction);
            }
        }

        /// <summary>
        /// 添加api版本浏览服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">设置配置方法</param>
        /// <returns></returns>
        public static IServiceCollection AddTdbVersionedApiExplorer(this IServiceCollection services, Action<ApiExplorerOptions> setupAction)
        {
            if (setupAction == null)
            {
                return services.AddVersionedApiExplorer();
            }
            else
            {
                return services.AddVersionedApiExplorer(setupAction);
            }
        }

        /// <summary>
        /// 添加api版本控制及浏览服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddTdbApiVersionExplorer(this IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                //为true时，API会在响应的header中返回支持的版本信息
                o.ReportApiVersions = true;
                ////请求中未指定版本时默认为1.0
                //o.DefaultApiVersion = new ApiVersion(1, 0);
                ////版本号以什么形式，什么字段传递
                //o.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("api-version"));
                ////在不提供版本号时，默认为1.0  如果不添加此配置，不提供版本号时会报错"message": "An API version is required, but was not specified."
                //o.AssumeDefaultVersionWhenUnspecified = true;
                ////默认以当前最高版本进行访问
                //o.ApiVersionSelector = new CurrentImplementationApiVersionSelector(o);
            }).AddVersionedApiExplorer(o =>
            {
                //以通知swagger替换控制器路由中的版本并配置api版本
                o.SubstituteApiVersionInUrl = true;
                // 版本名的格式：v+版本号
                o.GroupNameFormat = "'v'VVV";
                ////未指定时采用默认版本
                //o.AssumeDefaultVersionWhenUnspecified = true;
            });

            return services;
        }
    }
}
