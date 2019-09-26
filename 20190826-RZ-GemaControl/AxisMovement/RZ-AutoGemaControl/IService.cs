using System;

namespace RZ_AutoGemaControl
{
    /// <summary>
    /// 服务
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// 单例注入
        /// </summary>
        /// <typeparam name="TService">单例的接口类型</typeparam>
        /// <param name="serverImpl">单例的现实类</param>
        void InjectionSingleInstance<TService>(object serverImpl);
        /// <summary>
        /// 单例注入
        /// </summary>
        /// <typeparam name="TService">单例的接口类型</typeparam>
        /// <typeparam name="ServiceImpl">单例的现实类</typeparam>
        void InjectionSingleInstance<TService, ServiceImpl>();
        /// <summary>
        /// 单例注入
        /// </summary>
        /// <typeparam name="TService">单例的接口类型</typeparam>
        /// <param name="func">返回的TService的实例</param>
        void InjectionSingleInstance<TService>(Func<TService> func);
        /// <summary>
        /// 注入类型
        /// </summary>
        /// <typeparam name="TService">注入的接口类型</typeparam>
        /// <typeparam name="ServiceImpl">接口的实现类</typeparam>
        void InjectionType<TService, ServiceImpl>();
        /// <summary>
        /// 注入类型
        /// </summary>
        /// <typeparam name="TService">注入的接口类型</typeparam>
        /// <param name="func">每次注入创建实例的时候需要的创建过程</param>
        void InjectionType<TService>(Func<TService> func);
        /// <summary>
        /// 获取容器中的对象实例
        /// </summary>
        /// <typeparam name="T">对象接口类型</typeparam>
        /// <returns>对象实例</returns>
        T Resolve<T>();
        /// <summary>
        /// 启动容器
        /// </summary>
        void start();



    }
}