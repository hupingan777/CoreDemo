using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.Extensions.Autofac;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServicesCollection.Tool.AutoFac;

namespace ServicesCollection
{
    public class Startup
    {
        public Startup(IConfiguration configuration) //IWebHostEnvironment env
        {
            //// In ASP.NET Core 3.0 `env` will be an IWebHostEnvironment, not IHostingEnvironment.
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //    .AddEnvironmentVariables();
            //this.Configuration = builder.Build();

            Configuration = configuration;
        }

        //public IConfigurationRoot Configuration { get; private set; }
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ConfigureServices is where you register dependencies.
        /// This gets called by the runtime before the ConfigureContainer method, below.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //注册swagger文档服务
            services.AddSwaggerGen(x =>
            {
                //添加文档信息
                x.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "CoreWebApi+Swagger+JWT",
                    Version = "v2",
                    Description = "NetCore WebApi",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "胡平安",
                        Email = "1203074898@qq.com"
                    }
                });

                // 使用反射获取xml文件。并构造出文件的路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // 启用xml注释. 该方法第二个参数启用控制器的注释，默认为false.
                //千万不要忘了：
                //< !--解决发布时swagger xml缺失问题-- >
                //< PropertyGroup >
                //<GenerateDocumentationFile>true</GenerateDocumentationFile>
                //</ PropertyGroup >
                x.IncludeXmlComments(xmlPath, true);
            });

            services.AddHttpClient();

        }

        /// <summary>
        /// ConfigureContainer is where you can register things directly with Autofac. 
        /// This runs after ConfigureServices so the things here will override registrations made in ConfigureServices.
        /// Don't build the container; that gets done for you by the factory.
        /// 配置参考地址：https://autofaccn.readthedocs.io/zh/latest/integration/aspnetcore.html
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //builder.RegisterAssemblyTypes(typeof(Program).Assembly).
            //        Where(x => x.Name.EndsWith("ServicesCollection", StringComparison.OrdinalIgnoreCase)).AsImplementedInterfaces();
            //builder.RegisterDynamicProxy();

            var assemblys = Assembly.Load("ServicesCollection");//Service是继承接口的实现方法类库名称
            var baseType = typeof(IDependency);//IDependency 是一个接口（所有要实现依赖注入的借口都要继承该接口）
            builder.RegisterAssemblyTypes(assemblys)
             .Where(m => baseType.IsAssignableFrom(m) && m != baseType)
             .AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //启用swagger中间件
            app.UseSwagger();

            //配置SwaggerUI
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v2/swagger.json", "CoreWebApi");
                x.RoutePrefix = string.Empty;//Swagger的代理路由，默认为swagger，设置为空时可以直接通过http://localhost:5000访问到
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
