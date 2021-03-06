[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SotnWiki.MvcClient.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(SotnWiki.MvcClient.App_Start.NinjectWebCommon), "Stop")]

namespace SotnWiki.MvcClient.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject_Modules;
    using AutoMapper;
    using SotnWiki.MvcClient.App_Start.AutomapperProfiles;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                InitializeAutomapperConfig();
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(new ServicesNinjectModule());
            kernel.Load(new DataNinjectModule());
        }

        private static void InitializeAutomapperConfig()
        {
            Mapper.Initialize(NinjectWebCommon.AddProfilesToAutomapperConfig);
        }

        private static void AddProfilesToAutomapperConfig(IMapperConfigurationExpression config)
        {
            config.AddProfile(new PageViewsProfile());
            config.AddProfile(new EditViewsProfile());
            config.AddProfile(new RunViewsProfile());
        }
    }
}
