using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tdb.common;

namespace tdb.framework.webapi
{
    /// <summary>
    /// swagger扩展类
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// 添加Swagger服务（api版本管理）
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">配置方法</param>
        /// <returns></returns>
        public static IServiceCollection AddTdbSwaggerGenApiVer(this IServiceCollection services, Action<TdbSwaggerOptions> setupAction)
        {
            services.AddSwaggerGen();
            services.AddOptions<SwaggerGenOptions>().Configure<IApiVersionDescriptionProvider>((o, provider) =>
            {
                o.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                //配置信息
                var config = new TdbSwaggerOptions();
                setupAction(config);

                // 添加文档信息
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var apiInfo = new OpenApiInfo()
                    {
                        //标题
                        Title = $"{config.ServiceCode} V{description.ApiVersion}",
                        //当前版本
                        Version = description.ApiVersion.ToString(),
                        //文档说明
                        Description = $"{config.ServiceName} API接口文档（版本：{description.ApiVersion}）"
                    };
                    //联系方式
                    if (config.Contact != null)
                    {
                        apiInfo.Contact = config.Contact;
                    }

                    //当有弃用标记时的提示信息（注：只有当所有该版本的控制器都设置Deprecated=true时，IsDeprecated才会=true）
                    if (description.IsDeprecated)
                    {
                        apiInfo.Description += "（此版本已过期，不再维护更新）";
                    }

                    o.SwaggerDoc(description.GroupName, apiInfo);
                }

                // 加载XML注释
                if (config.LstXmlCommentsFileName != null)
                {
                    foreach(var xmlFileName in config.LstXmlCommentsFileName)
                    {
                        o.IncludeXmlComments(CommHelper.GetFullFileName(xmlFileName), true);
                    }
                }

                //添加Authorization
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });
                o.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }

        /// <summary>
        /// 添加swagger中间件（api版本管理）
        /// </summary>
        /// <param name="app"></param>
        /// <param name="provider">api版本描述提供者</param>
        /// <param name="swaggerRoutePrefix">swagger路由前缀</param>
        /// <returns></returns>
        public static IApplicationBuilder UseTdbSwaggerAndUIApiVer(this IApplicationBuilder app, IApiVersionDescriptionProvider provider, string swaggerRoutePrefix = "swagger")
        {
            //添加Swagger中间件，主要用于拦截swagger.json请求，从而可以获取返回所需的接口架构信息
            app.UseSwagger();
            //添加SwaggerUI中间件，主要用于拦截swagger/index.html页面请求，返回页面给前端
            app.UseSwaggerUI(options =>
            {
                //为每个版本创建一个JSON
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    //自由指定头部显示的下拉版本内容
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", "Version_" + description.ApiVersion);
                }

                //如果是为空，访问路径就为：根域名/index.html，注意localhost:8001/swagger是访问不到的
                //options.RoutePrefix = string.Empty;
                // 如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "tdbswagger"; 则访问路径为 根域名/tdbswagger/index.html
                options.RoutePrefix = swaggerRoutePrefix;
            });

            return app;
        }
    }

    /// <summary>
    /// swagger选项
    /// </summary>
    public class TdbSwaggerOptions
    {
        /// <summary>
        /// 服务编码
        /// </summary>
        public string ServiceCode { get; set; } = "";

        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName { get; set; } = "";

        /// <summary>
        /// xml注释文件名集合
        /// </summary>
        public List<string> LstXmlCommentsFileName { get; set; } = new List<string>();

        /// <summary>
        /// 联系方式
        /// </summary>
        public OpenApiContact Contact { get; set; }
    }
}
