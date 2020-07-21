using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.IO;

namespace EFCoreDemo
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
            services.AddControllers();
            services.AddDbContext<MyDBContext>(context =>
            {
                context.UseSqlServer(Configuration["ConnectionStrings:SqlServer"]);
            });

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
                    //< GenerateDocumentationFile > true </ GenerateDocumentationFile >
                //</ PropertyGroup >
                 x.IncludeXmlComments(xmlPath, true);
            });
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
