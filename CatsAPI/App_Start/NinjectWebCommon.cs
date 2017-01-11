using Cats.Rest.App_Start;
using Cats.Services.EarlyWarning;
using System;
using System.Web;
using Cats.Services.Hub;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;

using AdminUnitService = Cats.Services.EarlyWarning.AdminUnitService;
using CommodityService = Cats.Services.EarlyWarning.CommodityService;
using CommonService = Cats.Services.Common.CommonService;
using FDPService = Cats.Services.EarlyWarning.FDPService;
using HubService = Cats.Services.EarlyWarning.HubService;
using IAdminUnitService = Cats.Services.EarlyWarning.IAdminUnitService;
using ICommodityService = Cats.Services.EarlyWarning.ICommodityService;
using ICommonService = Cats.Services.Common.ICommonService;
using IFDPService = Cats.Services.EarlyWarning.IFDPService;
using IHubService = Cats.Services.EarlyWarning.IHubService;
using IProgramService = Cats.Services.EarlyWarning.IProgramService;
using IProjectCodeService = Cats.Services.EarlyWarning.IProjectCodeService;
using IShippingInstructionService = Cats.Services.EarlyWarning.IShippingInstructionService;
using ProgramService = Cats.Services.EarlyWarning.ProgramService;
using ProjectCodeService = Cats.Services.EarlyWarning.ProjectCodeService;
using ShippingInstructionService = Cats.Services.EarlyWarning.ShippingInstructionService;

using Cats.Services.Procurement;

using IStoreService = Cats.Services.Hub.IStoreService;
using IUnitService = Cats.Services.EarlyWarning.IUnitService;

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

            kernel.Bind<Data.Hub.UnitWork.IUnitOfWork>().To<Cats.Data.Hub.UnitOfWork>();
            kernel.Bind<ICommodityService>().To<CommodityService>();
            kernel.Bind<ICommonService>().To<CommonService>();
            kernel.Bind<IAdminUnitService>().To<AdminUnitService>();
            kernel.Bind<IAdminUnitTypeService>().To<AdminUnitTypeService>();
            kernel.Bind<IFDPService>().To<FDPService>();
            kernel.Bind<IHubService>().To<HubService>();
            kernel.Bind<IProjectCodeService>().To<ProjectCodeService>();
            kernel.Bind<IShippingInstructionService>().To<ShippingInstructionService>();
            kernel.Bind<IStoreService>().To<Cats.Services.Hub.StoreService>();
            kernel.Bind<IProgramService>().To<ProgramService>();
            kernel.Bind<IUnitService>().To<Cats.Services.EarlyWarning.UnitService>();
            

           
            kernel.Bind<Cats.Services.EarlyWarning.ICommodityTypeService>().To<Cats.Services.EarlyWarning.CommodityTypeService>();
            kernel.Bind<Services.Administration.IContactService>().To<Services.Administration.ContactService>();
            kernel.Bind<Cats.Services.Hub.ICommodityGradeService>().To<Cats.Services.Hub.CommodityGradeService>();

            kernel.Bind<IBidService>().To<BidService>();
            kernel.Bind<IBidDetailService>().To<BidDetailService>();
            kernel.Bind<IBidWinnerService>().To<BidWinnerService>();
            kernel.Bind<Services.Hub.ICommoditySourceService>().To<Services.Hub.CommoditySourceService>();
            kernel.Bind<ICurrencyService>().To<CurrencyService>();

            kernel.Bind<IHRDService>().To<HRDService>();
            kernel.Bind<IHRDDetailService>().To<HRDDetailService>();
            kernel.Bind<Services.EarlyWarning.IGiftCertificateService>().To<Services.EarlyWarning.GiftCertificateService>();
            kernel.Bind<Services.EarlyWarning.IGiftCertificateDetailService>().To<Services.EarlyWarning.GiftCertificateDetailService>();
            kernel.Bind<IHubOwnerService>().To<HubOwnerService>();
            kernel.Bind<ILedgerService>().To<LedgerService>();
        }        
    }
}
