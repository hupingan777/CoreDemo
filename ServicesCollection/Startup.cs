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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

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
                //<GenerateDocumentationFile>true</GenerateDocumentationFile>
                //</ PropertyGroup >
                x.IncludeXmlComments(xmlPath, true);
            });

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Program).Assembly).
                    Where(x => x.Name.EndsWith("ServicesCollection", StringComparison.OrdinalIgnoreCase)).AsImplementedInterfaces();
            builder.RegisterDynamicProxy();

            var assemblys = Assembly.Load("ServicesCollection");//Service�Ǽ̳нӿڵ�ʵ�ַ����������
            var baseType = typeof(IDependency);//IDependency ��һ���ӿڣ�����Ҫʵ������ע��Ľ�ڶ�Ҫ�̳иýӿڣ�
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
