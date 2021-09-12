using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdb.framework.webapi.Auth;
using tdb.framework.webapi.Cache;
using tdb.framework.webapi.Config;
using tdb.framework.webapi.Exceptions;
using tdb.framework.webapi.Log;
using tdb.framework.webapi.Swagger;
using tdb.framework.webapi.Validation;

namespace TestAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //参数验证
            services.AddTdbParamValidate();

            //配置
            services.AddTdbJsonConfig();
            var appConfig = LocalConfigurator.Ins.GetConfig<Controllers.AppsettingsConfig>();
            services.AddTdbConsulConfig(appConfig.Consul.IP, appConfig.Consul.Port, appConfig.Consul.ServiceCode + "_");
            var consulConfig = DistributedConfigurator.Ins.GetConfig<Controllers.ConsulConfig>();

            //缓存
            //services.AddTdbRedisCache(consulConfig.Redis.ConnectString.ToArray());
            services.AddTdbMemoryCache();

            //日志
            //services.AddTdbNLogger();
            services.AddTdbMySqlNLogger(consulConfig.MySqlLogConnStr, appConfig.Consul.ServiceCode, appConfig.ApiUrl);

            services.AddControllers(option => {
                //API日志

                //异常处理
                option.AddTdbGlobalException();
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = TdbClaimTypes.Name,
                    RoleClaimType = TdbClaimTypes.Role,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("tangdabin20210419")),
                    //不验Audience
                    ValidateAudience = false,
                    //不验Issuer
                    ValidateIssuer = false,
                    //允许的服务器时间偏移量
                    ClockSkew = TimeSpan.Zero,

                    /***********************************TokenValidationParameters的参数默认值***********************************/
                    // RequireSignedTokens = true,
                    // SaveSigninToken = false,
                    // ValidateActor = false,
                    // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                    // ValidateAudience = true,
                    // ValidateIssuer = true, 
                    // ValidateIssuerSigningKey = false,
                    // 是否要求Token的Claims中必须包含Expires
                    // RequireExpirationTime = true,
                    // 允许的服务器时间偏移量
                    // ClockSkew = TimeSpan.FromSeconds(300),
                    // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                    // ValidateLifetime = true
                };
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        Logger.Ins.Fatal(context.Exception, "认证授权异常");
                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        context.Response.Clear();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 403;
                        context.Response.WriteAsync("权限不足");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                     {
                         context.HandleResponse();
                         context.Response.Clear();
                         context.Response.ContentType = "application/json";
                         context.Response.StatusCode = 401;
                         context.Response.WriteAsync("认证未通过");
                         return Task.CompletedTask;
                     }
                };
            });

            //swagger
            services.AddTdbSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Web API 文档",
                    Description = "测试 Web API 文档"
                });

                //接口注释
                var xmlAPI = Path.Combine(AppContext.BaseDirectory, "TestAPI.xml");
                c.IncludeXmlComments(xmlAPI, true);

                //参数注释

                //生成token输入框
                //c.OperationFilter<SwaggerTokenFilter>();

                //添加Authorization
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //swagger
            app.UseTdbSwagger();
            app.UseTdbSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                c.DocExpansion(DocExpansion.None);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// autofac容器注册服务
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // 类型注入
            builder.Register(c => new Controllers.CallLogger(Console.Out));

            //启用接口代理拦截
            //builder.RegisterType<Controllers.Circle>().AsImplementedInterfaces().EnableInterfaceInterceptors().InstancePerLifetimeScope();
            builder.RegisterType<Controllers.Circle>().Named<Controllers.IShape>(typeof(Controllers.Circle).Name).AsImplementedInterfaces().EnableInterfaceInterceptors().InstancePerLifetimeScope();
            builder.RegisterType<Controllers.Circle0>().Named<Controllers.IShape>(typeof(Controllers.Circle0).Name).AsImplementedInterfaces().EnableInterfaceInterceptors().InstancePerLifetimeScope();
        }
    }
}
