﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.framework.webapi.standard
{
    /// <summary>
    /// swagger
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// 添加Swagger服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">配置方法</param>
        /// <returns></returns>
        public static IServiceCollection AddTdbSwaggerGen(this IServiceCollection services, Action<SwaggerGenOptions> setupAction = null)
        {
            if (setupAction == null)
            {
                return services.AddSwaggerGen();
            }
            else
            {
                return services.AddSwaggerGen(setupAction);
            }
        }

        /// <summary>
        /// 添加swagger中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="setupAction">配置方法</param>
        /// <returns></returns>
        public static IApplicationBuilder UseTdbSwagger(this IApplicationBuilder app, Action<SwaggerOptions> setupAction = null)
        {
            if (setupAction == null)
            {
                return app.UseSwagger();
            }
            else
            {
                return app.UseSwagger(setupAction);
            }
        }

        /// <summary>
        /// 添加swagger ui中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="setupAction">配置方法</param>
        /// <returns></returns>
        public static IApplicationBuilder UseTdbSwaggerUI(this IApplicationBuilder app, Action<SwaggerUIOptions> setupAction = null)
        {
            if (setupAction == null)
            {
                return app.UseSwaggerUI();
            }
            else
            {
                return app.UseSwaggerUI(setupAction);
            }
        }
    }
}
