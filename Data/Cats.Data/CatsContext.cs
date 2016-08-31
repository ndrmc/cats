using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Cats.Models;
using Cats.Models.Mapping;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Cats.Data
{
    public partial class CatsContext : DbContext
    {
        static CatsContext()
        {
            Database.SetInitializer<CatsContext>(null);
        }

        public CatsContext() : base("Name=CatsContext") { }

        // TODO: Add properties to access set of Poco classes
        public DbSet<DashboardWidget> DashboardWidgets { get; set; }
        public DbSet<UserDashboardPreference> UserDashboardPreferences { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<RegionalPSNPPledge> RegionalPSNPPledges { get; set; }
        public DbSet<RegionalRequest> RegionalRequests { get; set; }
        public DbSet<RegionalRequestDetail> RegionalRequestDetails { get; set; }
        public DbSet<ReliefRequisition> ReliefRequisitions { get; set; }
        public DbSet<ReliefRequisitionDetail> ReliefRequisitionDetails { get; set; }
        public DbSet<AdminUnit> AdminUnits { get; set; }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<CommodityType> CommodityTypes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<FDP> Fdps { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<AdminUnitType> AdminUnitTypes { get; set; }
        public DbSet<Hub> Hubs { get; set; }
        public DbSet<HubOwner> HubOwners { get; set; }
        public DbSet<DispatchAllocation> DispatchAllocations { get; set; }
        public DbSet<DispatchAllocationDetail> DispatchDetail { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<BidDetail> BidDetails { get; set; }
        public DbSet<Status> Statuses { get; set; }

        public DbSet<TransportBidPlan> TransportBidPlans { get; set; }
        public DbSet<TransportBidPlanDetail> TransportBidPlanDetails { get; set; }

        public DbSet<ProjectCodeAllocation> ProjectCodeAllocation { get; set; }

        public DbSet<TransportRequisition> TransportRequisition { get; set; }
        public DbSet<HubAllocation> HubAllocation { get; set; }
        public DbSet<ProjectCode> ProjectCode { get; set; }
        public DbSet<ShippingInstruction> ShippingInstruction { get; set; }


        public DbSet<BidWinner> BidWinners { get; set; }


        public DbSet<TransportOrder> TransportOrders { get; set; }
        public DbSet<TransportOrderDetail> TransportOrderDetails { get; set; }
        public DbSet<vwTransportOrder> vwTransportOrders { get; set; }

        public DbSet<TransportRequisitionDetail> TransportRequisitionDetails { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ReceiptAllocation> ReceiptAllocation { get; set; }


        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowStatus> WorkflowStatuses { get; set; }
        public DbSet<TransportBidQuotation> TransportBidQuotations { get; set; }
        public DbSet<TransportBidQuotationHeader> TransportBidQuotationHeaders { get; set; }

        public DbSet<ApplicationSetting> ApplicationSetting { get; set; }
        public DbSet<Ration> Rations { get; set; }

        public DbSet<NeedAssessmentHeader> NeedAssessmentHeader { get; set; }
        public DbSet<NeedAssessmentDetail> NeedAssessmentDetail { get; set; }

        public DbSet<HRD> HRDs { get; set; }
        public DbSet<HRDDetail> HRDDetails { get; set; }
        public DbSet<RationDetail> RationDetails { get; set; }
        public DbSet<RegionalPSNPPlan> RegionalPSNPPlans { get; set; }
        public DbSet<RegionalPSNPPlanDetail> RegionalPSNPPlanDetails { get; set; }
        public DbSet<HRDCommodityDetail> HrdCommodityDetails { get; set; }
        //public DbSet<LocalizedText> LocalizedTexts { get; set; }

        //public DbSet<Product> Products { get; set; }
        public DbSet<RequestDetailCommodity> RequestDetailCommodities { get; set; }

        public DbSet<GiftCertificate> GiftCertificates { get; set; }
        public DbSet<GiftCertificateDetail> GiftCertificateDetails { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Season> Seasons { get; set; }

        public DbSet<ProcessTemplate> ProcessTemplates { get; set; }
        public DbSet<StateTemplate> StateTemplates { get; set; }
        public DbSet<FlowTemplate> FlowTemplates { get; set; }

        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<ContributionDetail> ContributionDetails { get; set; }
        public DbSet<Donor> Donors { get; set; }

        public DbSet<vwPSNPAnnualPlan> vwPSNPAnnualPlans { get; set; }
        public DbSet<BusinessProcess> BusinessProcesss { get; set; }
        public DbSet<BusinessProcessState> BusinessProcessStates { get; set; }

        public DbSet<TypeOfNeedAssessment> TypeOfNeedAssessment { get; set; }


        public DbSet<NeedAssessmentSummary> NeedAssessmentSummary { get; set; }


        public DbSet<LetterTemplate> LetterTemplate { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<InKindContributionDetail> InKindContributionDetails { get; set; }
        public DbSet<UserHub> UserHub { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Audit> Audits { get; set; }

        public DbSet<TransReqWithoutTransporter> TransReqWithoutTransporters { get; set; }
        public DbSet<AllocationByRegion> AllocationByRegion { get; set; }
        public DbSet<Plan> HrdPlans { get; set; }
        public DbSet<WoredaHub> WoredaHubs { get; set; }
        public DbSet<PromisedContribution> PromisedContribution { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<TransporterAgreementVersion> TransporterAgreementVersions { get; set; }
        public DbSet<SIPCAllocation> SIPCAllocation { get; set; }
        public DbSet<HrdDonorCoverage> HrdDonorCoverages { get; set; }
        public DbSet<HrdDonorCoverageDetail> HrdDonorCoverageDetails { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryDetail> DeliveryDetails { get; set; }
        public DbSet<PaymentRequest> PaymentRequests { get; set; }

        public DbSet<IDPSReasonType> IDPSReasonTypes { get; set; }
        public DbSet<DistibtionStatus> DistibtionStatus { get; set; }
        public DbSet<WoredaHubLink> WoredaHubLinks { get; set; }
        public DbSet<ActionTypes> ActionTypeses { get; set; }
        public DbSet<WoredaStockDistribution> WoredaStockDistributions { get; set; }
        public DbSet<WoredaStockDistributionDetail> WoredaStockDistributionDetails { get; set; }
        public DbSet<DistributionByAgeDetail> DistributionByAgeDetails { get; set; }

        public DbSet<ReceiptPlan> ReceiptPlans { get; set; }
        public DbSet<ReceiptPlanDetail> ReceiptPlanDetails { get; set; }

        public DbSet<SupportType> SupportTypes { get; set; }

        public DbSet<Dispatch> Dispatches { get; set; }
        public DbSet<DispatchDetail> DispatchDetails { get; set; }
        public DbSet<OtherDispatchAllocation> OtherDispatchAllocations { get; set; }
        public DbSet<DeliveryReconcile> DeliveryReconciles { get; set; }
        public DbSet<LocalPurchase> LocalPurchases { get; set; }
        public DbSet<LocalPurchaseDetail> LocalPurchaseDetails { get; set; }

        public DbSet<DonationPlanHeader> DonationPlanHeaders { get; set; }
        public DbSet<DonationPlanDetail> DonationPlanDetails { get; set; }
        public DbSet<LoanReciptPlan> LoanReciptPlans { get; set; }
        public DbSet<LoanReciptPlanDetail> LoanReciptPlanDetails { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<TransporterCheque> TransporterCheques { get; set; }
        public DbSet<LossReason> LossReasons { get; set; }

        public DbSet<TransporterPaymentRequest> TransporterPaymentRequests { get; set; }
        public DbSet<VWRegionalRequest> VwRegionalRequests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DispatchMap());
            modelBuilder.Configurations.Add(new DispatchDetailMap());
            modelBuilder.Configurations.Add(new OtherDispatchAllocationMap());


            modelBuilder.Configurations.Add(new DeliveryReconcileMap());
            modelBuilder.Configurations.Add(new DistibtionStatusMap());
            modelBuilder.Configurations.Add(new PaymentRequestMap());
            modelBuilder.Configurations.Add(new SIPCAllocationMap());
            modelBuilder.Configurations.Add(new PromisedContributionMap());
            modelBuilder.Configurations.Add(new DashboardWidgetMap());
            modelBuilder.Configurations.Add(new UserDashboardPreferenceMap());
            modelBuilder.Configurations.Add(new BusinessProcessStateMap());
            //TODO: Add mapping information for each Poco model.
            modelBuilder.Configurations.Add(new RegionalPSNPPledgeMap());
            modelBuilder.Configurations.Add(new LogMap());
            modelBuilder.Configurations.Add(new BusinessProcessMap());
            modelBuilder.Configurations.Add(new ProcessTemplateMap());
            modelBuilder.Configurations.Add(new StateTemplateMap());
            modelBuilder.Configurations.Add(new FlowTemplateMap());
            modelBuilder.Configurations.Add(new WoredaHubLinkMap());
            modelBuilder.Configurations.Add(new RegionalRequestMap());
            modelBuilder.Configurations.Add(new RegionalRequestDetailMap());
            modelBuilder.Configurations.Add(new ReliefRequisitionMap());
            modelBuilder.Configurations.Add(new ReliefRequisitionDetailMap());
            modelBuilder.Configurations.Add(new AdminUnitMap());
            modelBuilder.Configurations.Add(new CommodityMap());
            modelBuilder.Configurations.Add(new CommodityTypeMap());
            modelBuilder.Configurations.Add(new FDPMap());
            modelBuilder.Configurations.Add(new ProgramMap());
            modelBuilder.Configurations.Add(new AdminUnitTypeMap());
            modelBuilder.Configurations.Add(new BidDetailMap());
            modelBuilder.Configurations.Add(new BidMap());
            modelBuilder.Configurations.Add(new StatusMap());
            modelBuilder.Configurations.Add(new TransporterAgreementVersionMap());
            modelBuilder.Configurations.Add(new TransporterMap());
            modelBuilder.Configurations.Add(new TransportBidPlanMap());
            modelBuilder.Configurations.Add(new TransportBidPlanDetailMap());

            modelBuilder.Configurations.Add(new ProjectCodeAllocationMap());

            modelBuilder.Configurations.Add(new HubMap());
            modelBuilder.Configurations.Add(new HubOwnerMap());
            modelBuilder.Configurations.Add(new HubAllocationMap());
            modelBuilder.Configurations.Add(new ProjectCodeMap());
            modelBuilder.Configurations.Add(new ShippingInstructionMap());

            modelBuilder.Configurations.Add(new BidWinnerMap());

            modelBuilder.Configurations.Add(new TransportOrderMap());
            modelBuilder.Configurations.Add(new TransportOrderDetailMap());
            modelBuilder.Configurations.Add(new vwTransportOrderMap());
            modelBuilder.Configurations.Add(new TransportRequisitionMap());
            modelBuilder.Configurations.Add(new TransportRequisitionDetailMap());
            modelBuilder.Configurations.Add(new TransactionMap());
            modelBuilder.Configurations.Add(new ReceiptAllocationMap());
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new WorkflowMap());
            modelBuilder.Configurations.Add(new WorkflowStatusMap());
            modelBuilder.Configurations.Add(new TransportBidQuotationMap());
            // modelBuilder.Configurations.Add(new TransportBidQuotationHeaderMap());
            modelBuilder.Configurations.Add(new ApplicationSettingMap());
            modelBuilder.Configurations.Add(new RationMap());

            modelBuilder.Configurations.Add(new NeedAssessmentHeaderMap());
            modelBuilder.Configurations.Add(new NeedAssessmentDetailMap());
            modelBuilder.Configurations.Add(new NeedAssessmentMap());

            modelBuilder.Configurations.Add(new HRDMap());
            modelBuilder.Configurations.Add(new HRDDetailMap());
            modelBuilder.Configurations.Add(new RationDetailMap());

            modelBuilder.Configurations.Add(new RegionalPSNPPlanMap());
            modelBuilder.Configurations.Add(new RegionalPSNPPlanDetailMap());
            modelBuilder.Configurations.Add(new WoredaHubMap());

            modelBuilder.Configurations.Add(new RequestDetailCommodityMap());

            modelBuilder.Configurations.Add(new GiftCertificateMap());
            modelBuilder.Configurations.Add(new GiftCertificateDetailMap());

            modelBuilder.Configurations.Add(new UnitMap());

            modelBuilder.Configurations.Add(new SeasonMap());

            modelBuilder.Configurations.Add(new ContributionMap());
            modelBuilder.Configurations.Add(new ContributionDetailMap());
            modelBuilder.Configurations.Add(new DonorMap());

            modelBuilder.Configurations.Add(new TypeOfNeedAssessmentMap());
            modelBuilder.Configurations.Add(new vwPSNPAnnualPlanMap());


            modelBuilder.Configurations.Add(new NeedAssessmentSummaryMap());

            modelBuilder.Configurations.Add(new LetterTemplateMap());
            modelBuilder.Configurations.Add(new CurrencyMap());

            modelBuilder.Configurations.Add(new InKindContributionDetailMap());
            modelBuilder.Configurations.Add(new CommodityGradeMap());
            modelBuilder.Configurations.Add(new CommoditySourceMap());

            modelBuilder.Configurations.Add(new AuditMap());

            modelBuilder.Configurations.Add(new TransReqWithoutTransporterMap());
            modelBuilder.Configurations.Add(new AllocationByRegionMap());
            modelBuilder.Configurations.Add(new PlanMap());
            modelBuilder.Configurations.Add(new NotificationMap());
            modelBuilder.Configurations.Add(new HRDDonorCoverageMap());
            modelBuilder.Configurations.Add(new HrdDonorCoverageDetailMap());
            modelBuilder.Configurations.Add(new DeliveryMap());
            modelBuilder.Configurations.Add(new DeliveryDetailMap());
            modelBuilder.Configurations.Add(new IDPSReasonTypeMap());
            modelBuilder.Configurations.Add(new TransportBidQuotationHeaderMap());
            modelBuilder.Configurations.Add(new ActionTypesMap());

            modelBuilder.Configurations.Add(new TemplateTypeMap());
            modelBuilder.Configurations.Add(new TemplateMap());
            modelBuilder.Configurations.Add(new TemplateFieldMap());

            modelBuilder.Configurations.Add(new WoredaStockDistributionMap());
            modelBuilder.Configurations.Add(new WoredaStockDistributionDetailMap());

            modelBuilder.Configurations.Add(new DistributionByAgeDetailMap());

            modelBuilder.Configurations.Add(new ReceiptPlanMap());
            modelBuilder.Configurations.Add(new ReceiptPlanDetailMap());


            modelBuilder.Configurations.Add(new DonationPlanHeaderMap());
            modelBuilder.Configurations.Add(new DonationPlanDetailMap());

            modelBuilder.Configurations.Add(new LocalPurchaseMap());
            modelBuilder.Configurations.Add(new LocalPurchaseDetailMap());
            modelBuilder.Configurations.Add(new LoanReciptPlanMap());
            modelBuilder.Configurations.Add(new LoanReciptPlanDetailMap());
            modelBuilder.Configurations.Add(new TransferMap());
            modelBuilder.Configurations.Add(new TransporterChequeMap());
            modelBuilder.Configurations.Add(new LossReasonMap());

            modelBuilder.Configurations.Add(new TransporterPaymentRequestMap());
            modelBuilder.Configurations.Add(new VWRegionalRequestMap());
            modelBuilder.Entity<SIPCAllocation>().Property(x => x.AllocatedAmount).HasPrecision(18, 10);
        }

    }



}
