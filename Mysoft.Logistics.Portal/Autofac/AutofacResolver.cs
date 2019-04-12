//using Autofac;
//using Autofac.Integration.Mvc;
//using Autofac.Integration.WebApi;
//using System.Reflection;

//namespace EPMSUpgrade
//{
//    /// <summary>
//    /// AutofacResolver
//    /// </summary>
//    public class AutofacResolver
//    {
//        #region 字段

//        private static ContainerBuilder _builder;
//        private static object _locker = new object();

//        #endregion

//        #region 属性

//        /// <summary>
//        /// Autofac容器。
//        /// </summary>
//        public static IContainer Container { get; private set; }

//        #endregion

//        #region 构造函数

//        /// <summary>
//        /// 初始化Autofac容器
//        /// </summary>
//        static AutofacResolver()
//        {
//            if (_builder == null)
//            {
//                lock (_locker)
//                {
//                    _builder = new ContainerBuilder();

//                    // 当前执行代码的程序集。
//                    var executingAssembly = Assembly.GetExecutingAssembly();

//                    //内置日志服务注册
//                    //_builder.Register(c => new ServiceLog()).As<IServiceLog>().InstancePerRequest();

//                    //注册DapperModule配置模块。
//                    _builder.RegisterModule<DapperModule>();

//                    //注册缓存。
//                    //_builder.RegisterType<DefaultCacheProvider>()
//                    //    .As<CacheProvider>()
//                    //    .InstancePerLifetimeScope();

//                    //注册Logger。
//                    //_builder.Register(c => new Logger { Cache = c.Resolve<CacheProvider>() })
//                    //    .As<ILogger>()
//                    //    .InstancePerLifetimeScope();

//                    //根据名称约定（服务层的接口和实现均以Service结尾），实现服务接口和服务实现的依赖
//                    //_builder.RegisterAssemblyTypes(typeof(ServiceSupport).Assembly)
//                    //  .Where(t => t.IsSubclassOf(typeof(ServiceSupport)))
//                    //  .PropertiesAutowired()
//                    //  .AsImplementedInterfaces()
//                    //  .InstancePerLifetimeScope();

//                    //WebApi注册。
//                    _builder.RegisterApiControllers(executingAssembly)
//                        .InstancePerRequest();

//                    //注册mvc容器的实现
//                    _builder.RegisterControllers(Assembly.GetExecutingAssembly())
//                      .InstancePerRequest();

//                    // 注册Model Binder。
//                    _builder.RegisterModelBinders(executingAssembly);
//                    _builder.RegisterModelBinderProvider();

//                    // 启用ActionFilter属性注入。
//                    _builder.RegisterFilterProvider();

//                    Container = _builder.Build();
//                }
//            }
//        }

//        #endregion
//    }
//}
