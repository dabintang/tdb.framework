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
            //������֤
            services.AddTdbParamValidate();

            //����
            services.AddTdbJsonConfig();
            var appConfig = LocalConfigurator.Ins.GetConfig<Controllers.AppsettingsConfig>();
            services.AddTdbConsulConfig(appConfig.Consul.IP, appConfig.Consul.Port, appConfig.Consul.ServiceCode + "_");
            var consulConfig = DistributedConfigurator.Ins.GetConfig<Controllers.ConsulConfig>();

            //����
            //services.AddTdbRedisCache(consulConfig.Redis.ConnectString.ToArray());
            services.AddTdbMemoryCache();

            //��־
            //services.AddTdbNLogger();
            services.AddTdbMySqlNLogger(consulConfig.MySqlLogConnStr, appConfig.Consul.ServiceCode, appConfig.ApiUrl);

            services.AddControllers(option => {
                //API��־

                //�쳣����
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
                    //����Audience
                    ValidateAudience = false,
                    //����Issuer
                    ValidateIssuer = false,
                    //����ķ�����ʱ��ƫ����
                    ClockSkew = TimeSpan.Zero,

                    /***********************************TokenValidationParameters�Ĳ���Ĭ��ֵ***********************************/
                    // RequireSignedTokens = true,
                    // SaveSigninToken = false,
                    // ValidateActor = false,
                    // ������������������Ϊfalse�����Բ���֤Issuer��Audience�����ǲ�������������
                    // ValidateAudience = true,
                    // ValidateIssuer = true, 
                    // ValidateIssuerSigningKey = false,
                    // �Ƿ�Ҫ��Token��Claims�б������Expires
                    // RequireExpirationTime = true,
                    // ����ķ�����ʱ��ƫ����
                    // ClockSkew = TimeSpan.FromSeconds(300),
                    // �Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
                    // ValidateLifetime = true
                };
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        Logger.Ins.Fatal(context.Exception, "��֤��Ȩ�쳣");
                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        context.Response.Clear();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 403;
                        context.Response.WriteAsync("Ȩ�޲���");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                     {
                         context.HandleResponse();
                         context.Response.Clear();
                         context.Response.ContentType = "application/json";
                         context.Response.StatusCode = 401;
                         context.Response.WriteAsync("��֤δͨ��");
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
                    Title = "Web API �ĵ�",
                    Description = "���� Web API �ĵ�"
                });

                //�ӿ�ע��
                var xmlAPI = Path.Combine(AppContext.BaseDirectory, "TestAPI.xml");
                c.IncludeXmlComments(xmlAPI, true);

                //����ע��

                //����token�����
                //c.OperationFilter<SwaggerTokenFilter>();

                //���Authorization
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
        /// autofac����ע�����
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // ����ע��
            builder.Register(c => new Controllers.CallLogger(Console.Out));

            //���ýӿڴ�������
            //builder.RegisterType<Controllers.Circle>().AsImplementedInterfaces().EnableInterfaceInterceptors().InstancePerLifetimeScope();
            builder.RegisterType<Controllers.Circle>().Named<Controllers.IShape>(typeof(Controllers.Circle).Name).AsImplementedInterfaces().EnableInterfaceInterceptors().InstancePerLifetimeScope();
            builder.RegisterType<Controllers.Circle0>().Named<Controllers.IShape>(typeof(Controllers.Circle0).Name).AsImplementedInterfaces().EnableInterfaceInterceptors().InstancePerLifetimeScope();
        }
    }
}
