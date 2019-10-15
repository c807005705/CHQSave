using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RZ_AutoGemaControl.services
{
    /// <summary>
    /// 自动注入实现类
    /// </summary>
    class AutoServiceImpl:IService
    {
        /// <summary>
        /// 构建
        /// </summary>
        private static ContainerBuilder Builder { get; set; } = new ContainerBuilder();
        /// <summary>
        /// 容器
        /// </summary>
        private static IContainer Container { get; set; }

        public void InjectionSingleInstance<TService>(object serverImpl)
        {
            Builder.RegisterInstance(serverImpl).
                 As<TService>().SingleInstance().PropertiesAutowired();
        }

        public void InjectionSingleInstance<TService, ServiceImpl>()
        {
            Builder.RegisterType<ServiceImpl>().
                 As<TService>().SingleInstance().PropertiesAutowired();
        }

        public void InjectionSingleInstance<TService>(Func<TService> func)
        {
            Builder.RegisterInstance(func() as object).
                As<TService>().SingleInstance().PropertiesAutowired();
        }

        public void InjectionType<TService, ServiceImpl>()
        {
            Builder.RegisterType<ServiceImpl>().As<TService>().PropertiesAutowired();
        }

        public void InjectionType<TService>(Func<TService> func)
        {
            if (func != null)
            {
                object data = func();
                Builder.RegisterInstance(data).As<TService>().
                    PropertiesAutowired();
            }
        }

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public void start()
        {
            Container = Builder.Build();
        }
    }
}
