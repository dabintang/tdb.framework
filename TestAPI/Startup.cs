using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tdb.framework.webapi.Cache;
using tdb.framework.webapi.Config;
using tdb.framework.webapi.Exceptions;
using tdb.framework.webapi.Log;
using tdb.framework.webapi.Swagger;

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
            //����
            services.AddTdbJsonConfig();
            var appConfig = LocalConfigurator.Ins.GetConfig<Controllers.AppsettingsConfig>();
            services.AddTdbConsulConfig(appConfig.Consul.IP, appConfig.Consul.Port, appConfig.Consul.ServiceCode + "_");
            var consulConfig = DistributedConfigurator.Ins.GetConfig<Controllers.ConsulConfig>();

            //����
            services.AddTdbRedisCache(consulConfig.Redis.ConnectString.ToArray());

            //��־
            //services.AddTdbNLogger();
            services.AddTdbMySqlNLogger(consulConfig.MySqlLogConnStr, $"{appConfig.Consul.ServiceCode}_{appConfig.ApiUrl}");

            services.AddControllers(option => {
                //API��־

                //�쳣����
                option.AddTdbGlobalException();
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
                c.OperationFilter<SwaggerTokenFilter>();
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
    }
}
