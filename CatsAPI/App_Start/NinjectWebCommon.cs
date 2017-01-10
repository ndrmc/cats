using Cats.Rest.App_Start;
using Cats.Services.Common;
using Cats.Services.EarlyWarning;
using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Cats.Services.Administration;
using Cats.Services.Hub;
using Cats.Services.Procurement;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace Cats.Rest.App_Start
{
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
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new Ninject.WebApi.DependencyResolver.NinjectDependencyResolver(kernel);
                RegisterServices(kernel);
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
            
            kernel.Bind<Cats.Data.UnitWork.IUnitOfWork>().To<Cats.Data.UnitWork.UnitOfWork>();
            kernel.Bind<Cats.Services.EarlyWarning.ICommodityService>().To<Cats.Services.EarlyWarning.CommodityService>();
            kernel.Bind<Services.Common.ICommonService>().To<Services.Common.CommonService>();
            kernel.Bind<Cats.Services.EarlyWarning.IAdminUnitService>().To<Cats.Services.EarlyWarning.AdminUnitService>();
            kernel.Bind<IAdminUnitTypeService>().To<AdminUnitTypeService>();
            kernel.Bind<Services.Hub.IHubService>().To<Services.Hub.HubService>();
            kernel.Bind <Services.EarlyWarning.IShippingInstructionService>().To<Services.EarlyWarning.ShippingInstructionService>();

            kernel.Bind<Cats.Services.EarlyWarning.IFDPService>().To<Cats.Services.EarlyWarning.FDPService>();
            kernel.Bind<Cats.Services.EarlyWarning.ICommodityTypeService>().To<Cats.Services.EarlyWarning.CommodityTypeService>();
            kernel.Bind<Services.Administration.IContactService>().To<Services.Administration.ContactService>();
            kernel.Bind<Cats.Services.Hub.ICommodityGradeService>().To<Cats.Services.Hub.CommodityGradeService>();
            kernel.Bind<IBidService>().To<BidService>();
            kernel.Bind<IBidDetailService>().To<BidDetailService>();
            kernel.Bind<IBidWinnerService>().To<BidWinnerService>();

        }        
    }
}
