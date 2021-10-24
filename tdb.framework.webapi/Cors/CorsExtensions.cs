using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdb.framework.webapi.Cors
{
    /// <summary>
    /// 跨域设置
    /// </summary>
    public static class CorsExtensions
    {
        /// <summary>
        /// 允许所有来源跨域
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddTdbAllAllowCors(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllAllowCorsPolicy", builder =>
                {
                    builder.WithOrigins("urls")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(_ => true) // =AllowAnyOrigin()
                        .AllowCredentials();
                })
            );

            return services;
        }

        /// <summary>
        /// 允许所有来源跨域
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseTdbAllAllowCors(this IApplicationBuilder app)
        {
            // 跨域设置
            return app.UseCors("AllAllowCorsPolicy");
        }
    }
}
