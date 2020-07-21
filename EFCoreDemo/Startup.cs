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

            //ע��swagger�ĵ�����
            services.AddSwaggerGen(x =>
            {
                //����ĵ���Ϣ
                x.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "CoreWebApi+Swagger+JWT",
                    Version = "v2",
                    Description = "NetCore WebApi",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "��ƽ��",
                        Email = "1203074898@qq.com"
                    }
                });

                // ʹ�÷����ȡxml�ļ�����������ļ���·��
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // ����xmlע��. �÷����ڶ����������ÿ�������ע�ͣ�Ĭ��Ϊfalse.
                //ǧ��Ҫ���ˣ�
                //< !--�������ʱswagger xmlȱʧ����-- >
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

            //����swagger�м��
            app.UseSwagger();

            //����SwaggerUI
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v2/swagger.json", "CoreWebApi");
                x.RoutePrefix = string.Empty;//Swagger�Ĵ���·�ɣ�Ĭ��Ϊswagger������Ϊ��ʱ����ֱ��ͨ��http://localhost:5000���ʵ�
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
