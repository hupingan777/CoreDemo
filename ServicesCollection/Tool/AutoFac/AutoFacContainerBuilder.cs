
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace ServicesCollection.Tool.AutoFac
{
    public static class AutoFacContainerBuilder
    {
        public static void AddAutoFacContainer(this IServiceCollection services)
        {

            //var builder = new ContainerBuilder();//实例化 AutoFac  容器   

            //builder.RegisterAssemblyTypes(typeof(Program).Assembly).
            //        Where(x => x.Name.EndsWith("ServicesCollection", StringComparison.OrdinalIgnoreCase)).AsImplementedInterfaces();
            //builder.RegisterDynamicProxy();

            //var assemblys = Assembly.Load("ServicesCollection");//Service是继承接口的实现方法类库名称
            //var baseType = typeof(IDependency);//IDependency 是一个接口（所有要实现依赖注入的借口都要继承该接口）
            //builder.RegisterAssemblyTypes(assemblys)
            // .Where(m => baseType.IsAssignableFrom(m) && m != baseType)
            // .AsImplementedInterfaces().InstancePerLifetimeScope();

            //builder.Populate(services);
            //builder.Build();
        }
    }
}
