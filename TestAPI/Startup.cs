using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdb.framework.webapi.standard.Auth;
using tdb.framework.webapi.standard.Cache;
using tdb.framework.webapi.standard.Config;
using tdb.framework.webapi.standard.Exceptions;
using tdb.framework.webapi.standard.Log;
using tdb.framework.webapi.standard.Validation;
using tdb.framework.webapi.Swagger;
using tdb.framework.webapi.APIVersion;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using tdb.framework.webapi.Auth;

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
            //services.AddTdbConsulConfig(appConfig.Consul.IP, appConfig.Consul.Port, appConfig.Consul.ServiceCode + "_");
            //var consulConfig = DistributedConfigurator.Ins.GetConfig<Controllers.ConsulConfig>();

            //����
            //services.AddTdbRedisCache(consulConfig.Redis.ConnectString.ToArray());
            services.AddTdbMemoryCache();

            //��־
            services.AddTdbNLogger();
            //services.AddTdbMySqlNLogger(consulConfig.MySqlLogConnStr, appConfig.Consul.ServiceCode, appConfig.ApiUrl);

            services.AddControllers(option => {
                //API��־

                //�쳣����
                option.AddTdbGlobalException();
            });

            //��������֤����Ȩ����
            services.AddTdbAuthJwtBearer("tangdabin20210419");
 
            //���api�汾���Ƽ��������
            services.AddTdbApiVersionExplorer();

            //swagger
            services.AddTdbSwaggerGenApiVer(o =>
            {
                o.ServiceCode = "TdbTest";
                o.ServiceName = "���Է���";
                o.LstXmlCommentsFileName.Add("TestAPI.xml");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //swagger
            app.UseTdbSwaggerAndUIApiVer(provider);

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
