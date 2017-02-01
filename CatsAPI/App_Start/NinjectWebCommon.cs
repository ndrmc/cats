using Cats.Rest.App_Start;
using Cats.Services.EarlyWarning;
using System;
using System.Web;
using Cats.Services.Administration;
using Cats.Services.Hub;
using Cats.Services.Logistics;
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
using Cats.Services.PSNP;
using HubOwnerService = Cats.Services.Hub.HubOwnerService;
using IHubOwnerService = Cats.Services.Hub.IHubOwnerService;
using IStoreService = Cats.Services.Hub.IStoreService;
using IUnitService = Cats.Services.EarlyWarning.IUnitService;
using IUserProfileService = Cats.Services.Administration.IUserProfileService;

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
            kernel.Bind<Cats.Services.Hub.IProjectCodeService>().To<Cats.Services.Hub.ProjectCodeService>();
            kernel.Bind<Cats.Services.Hub.IShippingInstructionService>().To<Cats.Services.Hub.ShippingInstructionService>();
            kernel.Bind<IStoreService>().To<Cats.Services.Hub.StoreService>();
            kernel.Bind<IProgramService>().To<ProgramService>();
            kernel.Bind<IUnitService>().To<Cats.Services.EarlyWarning.UnitService>();
            kernel.Bind<IStatusService>().To<StatusService>();
            kernel.Bind<IRationDetailService>().To<RationDetailService>();
            kernel.Bind<IUserProfileService>().To<Cats.Services.Administration.UserProfileService>();
            kernel.Bind<IReliefRequisitionService>().To<ReliefRequisitionService>();
            kernel.Bind<ITransactionService>().To<TransactionService>();
            kernel.Bind<ITransactionGroupService>().To<TransactionGroupService>();
            kernel.Bind<IAccountService>().To<AccountService>();
            kernel.Bind<Cats.Services.EarlyWarning.ICommodityTypeService>().To<Cats.Services.EarlyWarning.CommodityTypeService>();
            kernel.Bind<Services.Administration.IContactService>().To<Services.Administration.ContactService>();
            kernel.Bind<Cats.Services.Hub.ICommodityGradeService>().To<Cats.Services.Hub.CommodityGradeService>();

            kernel.Bind<IBidService>().To<BidService>();
            kernel.Bind<IBidDetailService>().To<BidDetailService>();
            kernel.Bind<IBidWinnerService>().To<BidWinnerService>();
            kernel.Bind<Services.Hub.ICommoditySourceService>().To<Services.Hub.CommoditySourceService>();
            kernel.Bind<ICurrencyService>().To<CurrencyService>();

            kernel.Bind<Services.EarlyWarning.IDonorService>().To<Services.EarlyWarning.DonorService>();

            kernel.Bind<IHRDService>().To<HRDService>();
            kernel.Bind<IHRDDetailService>().To<HRDDetailService>();
            kernel.Bind<Services.EarlyWarning.IGiftCertificateService>().To<Services.EarlyWarning.GiftCertificateService>();
            kernel.Bind<Services.EarlyWarning.IGiftCertificateDetailService>().To<Services.EarlyWarning.GiftCertificateDetailService>();
            kernel.Bind<IHubOwnerService>().To<HubOwnerService>();
            kernel.Bind<ILedgerService>().To<LedgerService>();

            kernel.Bind<ILedgerTypeService>().To<LedgerTypeService>();
            kernel.Bind<ILossReasonService>().To<LossReasonService>();
            kernel.Bind<IPlanService>().To<PlanService>();
            kernel.Bind<IRationService>().To<RationService>();
            kernel.Bind<IRationDetailService>().To<RationDetailService>();
            kernel.Bind<IRegionalRequestService>().To<RegionalRequestService>();
            kernel.Bind<ISeasonService>().To<SeasonService>();
            kernel.Bind<ITransportBidPlanService>().To<TransportBidPlanService>();
            kernel.Bind<ITransportBidPlanDetailService>().To<TransportBidPlanDetailService>();
            kernel.Bind<ITransportBidQuotationService>().To<TransportBidQuotationService>();
            kernel.Bind<IRegionalPSNPPlanDetailService>().To<RegionalPSNPPlanDetailService>();
            kernel.Bind<ISupportTypeService>().To<SupportTypeService>();
            kernel.Bind<IIDPSReasonTypeServices>().To<IDPSReasonTypeServices>();
            kernel.Bind<Cats.Services.Transaction.ITranscationTypeService>().To<Cats.Services.Transaction.TranscationTypeService>();
        }
    }
}
