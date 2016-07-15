namespace Cats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DashboardWidget",
                c => new
                    {
                        DashboardWidgetID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        Icon = c.String(maxLength: 255),
                        Source = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.DashboardWidgetID);
            
            CreateTable(
                "dbo.UserDashboardPreference",
                c => new
                    {
                        UserDashboardPreferenceID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        DashboardWidgetID = c.Int(nullable: false),
                        OrderNo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserDashboardPreferenceID)
                .ForeignKey("dbo.DashboardWidget", t => t.DashboardWidgetID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.DashboardWidgetID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FullName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Disabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Thread = c.String(nullable: false, maxLength: 255),
                        Level = c.String(nullable: false, maxLength: 50),
                        Logger = c.String(nullable: false, maxLength: 255),
                        Message = c.String(nullable: false, maxLength: 4000),
                        Exception = c.String(maxLength: 2000),
                        LogUser = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RegionalPSNPPledges",
                c => new
                    {
                        RegionalPSNPPledgeID = c.Int(nullable: false, identity: true),
                        RegionalPSNPPlanID = c.Int(nullable: false),
                        DonorID = c.Int(nullable: false),
                        CommodityID = c.Int(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitID = c.Int(nullable: false),
                        PledgeDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RegionalPSNPPledgeID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .ForeignKey("dbo.Donor", t => t.DonorID)
                .ForeignKey("dbo.RegionalPSNPPlan", t => t.RegionalPSNPPlanID)
                .ForeignKey("dbo.Unit", t => t.UnitID)
                .Index(t => t.CommodityID)
                .Index(t => t.DonorID)
                .Index(t => t.RegionalPSNPPlanID)
                .Index(t => t.UnitID);
            
            CreateTable(
                "dbo.Commodity",
                c => new
                    {
                        CommodityID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        LongName = c.String(maxLength: 500),
                        NameAM = c.String(maxLength: 50),
                        CommodityCode = c.String(maxLength: 5),
                        CommodityTypeID = c.Int(nullable: false),
                        ParentID = c.Int(),
                    })
                .PrimaryKey(t => t.CommodityID)
                .ForeignKey("dbo.Commodity", t => t.ParentID)
                .ForeignKey("dbo.CommodityType", t => t.CommodityTypeID)
                .Index(t => t.ParentID)
                .Index(t => t.CommodityTypeID);
            
            CreateTable(
                "dbo.DispatchAllocation",
                c => new
                    {
                        DispatchAllocationID = c.Guid(nullable: false),
                        PartitionId = c.Int(),
                        HubID = c.Int(),
                        StoreID = c.Int(),
                        Year = c.Int(),
                        Month = c.Int(),
                        Round = c.Int(),
                        DonorID = c.Int(),
                        ProgramID = c.Int(),
                        CommodityID = c.Int(nullable: false),
                        RequisitionNo = c.String(),
                        RequisitionId = c.Int(),
                        BidRefNo = c.String(),
                        ContractStartDate = c.DateTime(),
                        ContractEndDate = c.DateTime(),
                        Beneficiery = c.Int(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Unit = c.Int(nullable: false),
                        TransporterID = c.Int(),
                        FDPID = c.Int(nullable: false),
                        ShippingInstructionID = c.Int(),
                        ProjectCodeID = c.Int(),
                        TransportOrderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DispatchAllocationID)
                .ForeignKey("dbo.Program", t => t.ProgramID)
                .ForeignKey("dbo.Hub", t => t.HubID)
                .ForeignKey("dbo.FDP", t => t.FDPID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .Index(t => t.ProgramID)
                .Index(t => t.HubID)
                .Index(t => t.FDPID)
                .Index(t => t.CommodityID);
            
            CreateTable(
                "dbo.Dispatch",
                c => new
                    {
                        DispatchID = c.Guid(nullable: false),
                        PartitionId = c.Int(),
                        HubID = c.Int(nullable: false),
                        GIN = c.String(nullable: false, maxLength: 7),
                        FDPID = c.Int(),
                        WeighBridgeTicketNumber = c.String(maxLength: 50),
                        RequisitionNo = c.String(nullable: false, maxLength: 50),
                        BidNumber = c.String(nullable: false, maxLength: 50),
                        TransporterID = c.Int(nullable: false),
                        DriverName = c.String(nullable: false, maxLength: 50),
                        PlateNo_Prime = c.String(nullable: false, maxLength: 50),
                        PlateNo_Trailer = c.String(maxLength: 50),
                        PeriodYear = c.Int(nullable: false),
                        PeriodMonth = c.Int(nullable: false),
                        Round = c.Int(nullable: false),
                        UserProfileID = c.Int(nullable: false),
                        DispatchDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Remark = c.String(maxLength: 4000),
                        DispatchedByStoreMan = c.String(nullable: false, maxLength: 50),
                        DispatchAllocationID = c.Guid(),
                        OtherDispatchAllocationID = c.Guid(),
                    })
                .PrimaryKey(t => t.DispatchID)
                .ForeignKey("dbo.DispatchAllocation", t => t.DispatchAllocationID)
                .ForeignKey("dbo.FDP", t => t.FDPID)
                .ForeignKey("dbo.Hub", t => t.HubID)
                .ForeignKey("dbo.OtherDispatchAllocation", t => t.OtherDispatchAllocationID)
                .ForeignKey("Procurement.Transporter", t => t.TransporterID)
                .Index(t => t.DispatchAllocationID)
                .Index(t => t.FDPID)
                .Index(t => t.HubID)
                .Index(t => t.OtherDispatchAllocationID)
                .Index(t => t.TransporterID);
            
            CreateTable(
                "dbo.FDP",
                c => new
                    {
                        FDPID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        NameAM = c.String(maxLength: 50),
                        AdminUnitID = c.Int(nullable: false),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        HubID = c.Int(),
                    })
                .PrimaryKey(t => t.FDPID)
                .ForeignKey("dbo.AdminUnit", t => t.AdminUnitID)
                .Index(t => t.AdminUnitID);
            
            CreateTable(
                "dbo.AdminUnit",
                c => new
                    {
                        AdminUnitID = c.Int(nullable: false, identity: true),
                        code = c.Int(),
                        Name = c.String(maxLength: 50),
                        NameAM = c.String(maxLength: 50),
                        AdminUnitTypeID = c.Int(),
                        ParentID = c.Int(),
                    })
                .PrimaryKey(t => t.AdminUnitID)
                .ForeignKey("dbo.AdminUnit", t => t.ParentID)
                .ForeignKey("dbo.AdminUnitType", t => t.AdminUnitTypeID)
                .Index(t => t.ParentID)
                .Index(t => t.AdminUnitTypeID);
            
            CreateTable(
                "dbo.AdminUnitType",
                c => new
                    {
                        AdminUnitTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        NameAM = c.String(maxLength: 50),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AdminUnitTypeID);
            
            CreateTable(
                "dbo.Donor",
                c => new
                    {
                        DonorID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        DonorCode = c.String(maxLength: 3),
                        IsResponsibleDonor = c.Boolean(nullable: false),
                        IsSourceDonor = c.Boolean(nullable: false),
                        LongName = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.DonorID);
            
            CreateTable(
                "dbo.GiftCertificate",
                c => new
                    {
                        GiftCertificateID = c.Int(nullable: false, identity: true),
                        GiftDate = c.DateTime(nullable: false),
                        DonorID = c.Int(nullable: false),
                        ShippingInstructionID = c.Int(nullable: false),
                        ReferenceNo = c.String(nullable: false, maxLength: 50),
                        Vessel = c.String(maxLength: 50),
                        ETA = c.DateTime(nullable: false),
                        IsPrinted = c.Boolean(nullable: false),
                        ProgramID = c.Int(nullable: false),
                        DModeOfTransport = c.Int(nullable: false),
                        PortName = c.String(maxLength: 50),
                        StatusID = c.Int(nullable: false),
                        PartitionId = c.Int(),
                        DeclarationNumber = c.String(),
                        TransactionGroupID = c.Guid(),
                    })
                .PrimaryKey(t => t.GiftCertificateID)
                .ForeignKey("dbo.Detail", t => t.DModeOfTransport)
                .ForeignKey("dbo.Donor", t => t.DonorID)
                .ForeignKey("dbo.Program", t => t.ProgramID)
                .ForeignKey("dbo.ShippingInstruction", t => t.ShippingInstructionID)
                .Index(t => t.DModeOfTransport)
                .Index(t => t.DonorID)
                .Index(t => t.ProgramID)
                .Index(t => t.ShippingInstructionID);
            
            CreateTable(
                "dbo.Detail",
                c => new
                    {
                        DetailID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        MasterID = c.Int(nullable: false),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DetailID)
                .ForeignKey("dbo.Master", t => t.MasterID)
                .Index(t => t.MasterID);
            
            CreateTable(
                "dbo.Master",
                c => new
                    {
                        MasterID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MasterID);
            
            CreateTable(
                "dbo.GiftCertificateDetail",
                c => new
                    {
                        GiftCertificateDetailID = c.Int(nullable: false, identity: true),
                        PartitionId = c.Int(),
                        TransactionGroupID = c.Int(nullable: false),
                        GiftCertificateID = c.Int(nullable: false),
                        CommodityID = c.Int(nullable: false),
                        WeightInMT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillOfLoading = c.String(maxLength: 50),
                        AccountNumber = c.Int(nullable: false),
                        EstimatedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EstimatedTax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        YearPurchased = c.String(),
                        DFundSourceID = c.Int(nullable: false),
                        DCurrencyID = c.Int(nullable: false),
                        DBudgetTypeID = c.Int(nullable: false),
                        ExpiryDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.GiftCertificateDetailID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID, cascadeDelete: false)
                .ForeignKey("dbo.Detail", t => t.DFundSourceID, cascadeDelete: false)
                .ForeignKey("dbo.Detail", t => t.DCurrencyID, cascadeDelete: false)
                .ForeignKey("dbo.Detail", t => t.DBudgetTypeID, cascadeDelete: false)
                .ForeignKey("dbo.GiftCertificate", t => t.GiftCertificateID, cascadeDelete: false)
                .Index(t => t.CommodityID)
                .Index(t => t.DFundSourceID)
                .Index(t => t.DCurrencyID)
                .Index(t => t.DBudgetTypeID)
                .Index(t => t.GiftCertificateID);
            
            CreateTable(
                "dbo.ReceiptAllocation",
                c => new
                    {
                        ReceiptAllocationID = c.Guid(nullable: false),
                        PartitionId = c.Int(),
                        IsCommited = c.Boolean(nullable: false),
                        ETA = c.DateTime(nullable: false),
                        ProjectNumber = c.String(nullable: false, maxLength: 50),
                        GiftCertificateDetailID = c.Int(),
                        CommodityID = c.Int(nullable: false),
                        SINumber = c.String(maxLength: 50),
                        UnitID = c.Int(),
                        QuantityInUnit = c.Decimal(precision: 18, scale: 2),
                        QuantityInMT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HubID = c.Int(nullable: false),
                        DonorID = c.Int(),
                        ProgramID = c.Int(nullable: false),
                        CommoditySourceID = c.Int(nullable: false),
                        IsClosed = c.Boolean(nullable: false),
                        PurchaseOrder = c.String(maxLength: 50),
                        SupplierName = c.String(maxLength: 50),
                        SourceHubID = c.Int(),
                        OtherDocumentationRef = c.String(maxLength: 50),
                        IsFalseGRN = c.Boolean(nullable: false),
                        ReceiptPlanID = c.Int(),
                        Remark = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ReceiptAllocationID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .ForeignKey("dbo.Donor", t => t.DonorID)
                .ForeignKey("dbo.Hub", t => t.HubID)
                .ForeignKey("dbo.Hub", t => t.SourceHubID)
                .ForeignKey("dbo.Program", t => t.ProgramID)
                .ForeignKey("dbo.Unit", t => t.UnitID)
                .ForeignKey("dbo.CommoditySource", t => t.CommoditySourceID)
                .ForeignKey("dbo.GiftCertificateDetail", t => t.GiftCertificateDetailID)
                .Index(t => t.CommodityID)
                .Index(t => t.DonorID)
                .Index(t => t.HubID)
                .Index(t => t.SourceHubID)
                .Index(t => t.ProgramID)
                .Index(t => t.UnitID)
                .Index(t => t.CommoditySourceID)
                .Index(t => t.GiftCertificateDetailID);
            
            CreateTable(
                "dbo.Hub",
                c => new
                    {
                        HubID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        HubOwnerID = c.Int(nullable: false),
                        HubParentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HubID)
                .ForeignKey("dbo.HubOwner", t => t.HubOwnerID)
                .Index(t => t.HubOwnerID);
            
            CreateTable(
                "dbo.HubOwner",
                c => new
                    {
                        HubOwnerID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        LongName = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.HubOwnerID);
            
            CreateTable(
                "dbo.Transaction",
                c => new
                    {
                        TransactionID = c.Guid(nullable: false),
                        PartitionId = c.Int(),
                        TransactionGroupID = c.Guid(nullable: false),
                        LedgerID = c.Int(nullable: false),
                        HubOwnerID = c.Int(),
                        AccountID = c.Int(),
                        HubID = c.Int(),
                        StoreID = c.Int(),
                        Stack = c.Int(),
                        ProjectCodeID = c.Int(),
                        ShippingInstructionID = c.Int(),
                        ProgramID = c.Int(nullable: false),
                        ParentCommodityID = c.Int(),
                        CommodityID = c.Int(),
                        CommodityGradeID = c.Int(),
                        QuantityInMT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityInUnit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitID = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        RegionID = c.Int(),
                        Month = c.Int(),
                        Round = c.Int(),
                        DonorID = c.Int(),
                        CommoditySourceID = c.Int(),
                        GiftTypeID = c.Int(),
                        FDPID = c.Int(),
                        TransactionType = c.Int(),
                        PlanId = c.Int(),
                        TransporterId = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionID)
                .ForeignKey("dbo.Program", t => t.ProgramID)
                .ForeignKey("dbo.ProjectCode", t => t.ProjectCodeID)
                .ForeignKey("dbo.ShippingInstruction", t => t.ShippingInstructionID)
                .ForeignKey("dbo.TransactionGroup", t => t.TransactionGroupID)
                .ForeignKey("dbo.Unit", t => t.UnitID)
                .ForeignKey("dbo.HubOwner", t => t.HubOwnerID)
                .ForeignKey("dbo.Store", t => t.StoreID)
                .ForeignKey("dbo.Hub", t => t.HubID)
                .ForeignKey("dbo.CommodityGrade", t => t.CommodityGradeID)
                .Index(t => t.ProgramID)
                .Index(t => t.ProjectCodeID)
                .Index(t => t.ShippingInstructionID)
                .Index(t => t.TransactionGroupID)
                .Index(t => t.UnitID)
                .Index(t => t.HubOwnerID)
                .Index(t => t.StoreID)
                .Index(t => t.HubID)
                .Index(t => t.CommodityGradeID);
            
            CreateTable(
                "dbo.ProjectCode",
                c => new
                    {
                        ProjectCodeID = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ProjectCodeID);
            
            CreateTable(
                "dbo.ShippingInstruction",
                c => new
                    {
                        ShippingInstructionID = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ShippingInstructionID);
            
            CreateTable(
                "dbo.OtherDispatchAllocation",
                c => new
                    {
                        OtherDispatchAllocationID = c.Guid(nullable: false),
                        PartitionId = c.Int(),
                        AgreementDate = c.DateTime(nullable: false),
                        CommodityID = c.Int(nullable: false),
                        EstimatedDispatchDate = c.DateTime(nullable: false),
                        HubID = c.Int(nullable: false),
                        ToHubID = c.Int(nullable: false),
                        ProgramID = c.Int(nullable: false),
                        UnitID = c.Int(nullable: false),
                        QuantityInUnit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityInMT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReasonID = c.Int(nullable: false),
                        ReferenceNumber = c.String(nullable: false, maxLength: 50),
                        Remark = c.String(maxLength: 4000),
                        ShippingInstructionID = c.Int(nullable: false),
                        ProjectCodeID = c.Int(nullable: false),
                        TransporterID = c.Int(),
                        IsClosed = c.Boolean(nullable: false),
                        Hub1_HubID = c.Int(),
                    })
                .PrimaryKey(t => t.OtherDispatchAllocationID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .ForeignKey("dbo.Hub", t => t.HubID)
                .ForeignKey("dbo.Hub", t => t.Hub1_HubID)
                .ForeignKey("dbo.Program", t => t.ProgramID)
                .ForeignKey("dbo.ProjectCode", t => t.ProjectCodeID)
                .ForeignKey("dbo.ShippingInstruction", t => t.ShippingInstructionID)
                .ForeignKey("Procurement.Transporter", t => t.TransporterID)
                .Index(t => t.CommodityID)
                .Index(t => t.HubID)
                .Index(t => t.Hub1_HubID)
                .Index(t => t.ProgramID)
                .Index(t => t.ProjectCodeID)
                .Index(t => t.ShippingInstructionID)
                .Index(t => t.TransporterID);
            
            CreateTable(
                "dbo.Program",
                c => new
                    {
                        ProgramID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 50),
                        LongName = c.String(maxLength: 500),
                        ShortCode = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ProgramID);
            
            CreateTable(
                "dbo.TransReqWithoutTransporter",
                c => new
                    {
                        TransReqWithoutTransporterID = c.Int(nullable: false, identity: true),
                        RequisitionDetailID = c.Int(nullable: false),
                        TransportRequisitionID = c.Int(nullable: false),
                        IsAssigned = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TransReqWithoutTransporterID)
                .ForeignKey("Logistics.TransportRequisitionDetail", t => t.TransportRequisitionID)
                .ForeignKey("EarlyWarning.ReliefRequisitionDetail", t => t.RequisitionDetailID)
                .Index(t => t.TransportRequisitionID)
                .Index(t => t.RequisitionDetailID);
            
            CreateTable(
                "dbo.SIPCAllocation",
                c => new
                    {
                        SIPCAllocationID = c.Int(nullable: false, identity: true),
                        FDPID = c.Int(nullable: false),
                        RequisitionDetailID = c.Int(nullable: false),
                        Code = c.Int(nullable: false),
                        AllocatedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AllocationType = c.String(),
                        PartitionId = c.Int(),
                        TransactionGroupID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.SIPCAllocationID)
                .ForeignKey("EarlyWarning.ReliefRequisitionDetail", t => t.RequisitionDetailID)
                .ForeignKey("dbo.TransactionGroup", t => t.TransactionGroupID)
                .Index(t => t.RequisitionDetailID)
                .Index(t => t.TransactionGroupID);
            
            CreateTable(
                "dbo.TransactionGroup",
                c => new
                    {
                        TransactionGroupID = c.Guid(nullable: false),
                        PartitionID = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionGroupID);
            
            CreateTable(
                "dbo.InternalMovement",
                c => new
                    {
                        InternalMovementID = c.Guid(nullable: false),
                        PartitionId = c.Int(),
                        HubID = c.Int(nullable: false),
                        TransactionGroupID = c.Guid(),
                        TransferDate = c.DateTime(nullable: false),
                        ReferenceNumber = c.String(),
                        DReason = c.Int(nullable: false),
                        Notes = c.String(),
                        ApprovedBy = c.String(),
                        Detail_DetailID = c.Int(),
                    })
                .PrimaryKey(t => t.InternalMovementID)
                .ForeignKey("dbo.Detail", t => t.Detail_DetailID)
                .ForeignKey("dbo.TransactionGroup", t => t.TransactionGroupID)
                .Index(t => t.Detail_DetailID)
                .Index(t => t.TransactionGroupID);
            
            CreateTable(
                "dbo.DeliveryReconcile",
                c => new
                    {
                        DeliveryReconcileID = c.Int(nullable: false, identity: true),
                        GRN = c.String(nullable: false, maxLength: 50),
                        FDPID = c.Int(nullable: false),
                        DispatchID = c.Guid(nullable: false),
                        WayBillNo = c.String(maxLength: 50),
                        RequsitionNo = c.String(maxLength: 50),
                        HubID = c.Int(nullable: false),
                        GIN = c.String(nullable: false, maxLength: 50),
                        ReceivedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceivedDate = c.DateTime(nullable: false),
                        LossAmount = c.Decimal(precision: 18, scale: 2),
                        LossReason = c.Int(),
                        TransactionGroupID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.DeliveryReconcileID)
                .ForeignKey("dbo.Dispatch", t => t.DispatchID)
                .ForeignKey("dbo.FDP", t => t.FDPID)
                .ForeignKey("dbo.Hub", t => t.HubID)
                .ForeignKey("dbo.TransactionGroup", t => t.TransactionGroupID)
                .Index(t => t.DispatchID)
                .Index(t => t.FDPID)
                .Index(t => t.HubID)
                .Index(t => t.TransactionGroupID);
            
            CreateTable(
                "dbo.RegionalPSNPPlan",
                c => new
                    {
                        RegionalPSNPPlanID = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        RationID = c.Int(nullable: false),
                        StatusID = c.Int(nullable: false),
                        PlanId = c.Int(nullable: false),
                        TransactionGroupID = c.Guid(nullable: false),
                        PartitionId = c.Int(),
                        User = c.Int(),
                    })
                .PrimaryKey(t => t.RegionalPSNPPlanID)
                .ForeignKey("dbo.Plan", t => t.PlanId)
                .ForeignKey("dbo.Ration", t => t.RationID)
                .ForeignKey("dbo.BusinessProcess", t => t.StatusID)
                .ForeignKey("dbo.TransactionGroup", t => t.TransactionGroupID)
                .Index(t => t.PlanId)
                .Index(t => t.RationID)
                .Index(t => t.StatusID)
                .Index(t => t.TransactionGroupID);
            
            CreateTable(
                "dbo.Ration",
                c => new
                    {
                        RationID = c.Int(nullable: false, identity: true),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        IsDefaultRation = c.Boolean(),
                        RefrenceNumber = c.String(),
                    })
                .PrimaryKey(t => t.RationID);
            
            CreateTable(
                "dbo.RationDetail",
                c => new
                    {
                        RationDetailID = c.Int(nullable: false, identity: true),
                        RationID = c.Int(nullable: false),
                        CommodityID = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 6),
                        UnitID = c.Int(),
                    })
                .PrimaryKey(t => t.RationDetailID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .ForeignKey("dbo.Ration", t => t.RationID)
                .ForeignKey("dbo.Unit", t => t.UnitID)
                .Index(t => t.CommodityID)
                .Index(t => t.RationID)
                .Index(t => t.UnitID);
            
            CreateTable(
                "dbo.Unit",
                c => new
                    {
                        UnitID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.UnitID);
            
            CreateTable(
                "dbo.DeliveryDetail",
                c => new
                    {
                        DeliveryDetailID = c.Guid(nullable: false),
                        CommodityID = c.Int(nullable: false),
                        UnitID = c.Int(nullable: false),
                        SentQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceivedQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.DeliveryDetailID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .ForeignKey("dbo.Delivery", t => t.DeliveryID)
                .ForeignKey("dbo.Unit", t => t.UnitID)
                .Index(t => t.CommodityID)
                .Index(t => t.DeliveryID)
                .Index(t => t.UnitID);
            
            CreateTable(
                "dbo.Delivery",
                c => new
                    {
                        DeliveryID = c.Guid(nullable: false),
                        ReceivingNumber = c.String(),
                        DonorID = c.Int(),
                        TransporterID = c.Int(nullable: false),
                        PlateNoPrimary = c.String(nullable: false, maxLength: 20),
                        PlateNoTrailler = c.String(maxLength: 20),
                        DriverName = c.String(nullable: false, maxLength: 100),
                        FDPID = c.Int(nullable: false),
                        DispatchID = c.Guid(),
                        WayBillNo = c.String(maxLength: 50),
                        RequisitionNo = c.String(maxLength: 50),
                        HubID = c.Int(),
                        InvoiceNo = c.String(),
                        DeliveryBy = c.String(maxLength: 100),
                        DeliveryDate = c.DateTime(),
                        ReceivedBy = c.String(maxLength: 100),
                        ReceivedDate = c.DateTime(),
                        DocumentReceivedDate = c.DateTime(),
                        DocumentReceivedBy = c.Int(),
                        Status = c.Int(),
                        ActionType = c.Int(),
                        ActionTypeRemark = c.String(),
                        TransactionGroupID = c.Guid(),
                    })
                .PrimaryKey(t => t.DeliveryID)
                .ForeignKey("dbo.Donor", t => t.DonorID)
                .ForeignKey("dbo.FDP", t => t.FDPID)
                .ForeignKey("dbo.Hub", t => t.HubID)
                .Index(t => t.DonorID)
                .Index(t => t.FDPID)
                .Index(t => t.HubID);
            
            CreateTable(
                "dbo.TransporterPaymentRequest",
                c => new
                    {
                        TransporterPaymentRequestID = c.Int(nullable: false, identity: true),
                        ReferenceNo = c.String(nullable: false, maxLength: 50),
                        TransportOrderID = c.Int(nullable: false),
                        DeliveryID = c.Guid(nullable: false),
                        ShortageBirr = c.Decimal(precision: 18, scale: 2),
                        LabourCostRate = c.Decimal(precision: 18, scale: 2),
                        LabourCost = c.Decimal(precision: 18, scale: 2),
                        RejectedAmount = c.Decimal(precision: 18, scale: 2),
                        RejectionReason = c.String(),
                        RequestedDate = c.DateTime(nullable: false),
                        BusinessProcessID = c.Int(nullable: false),
                        ShortageQty = c.Int(),
                        LossReason = c.String(),
                        GIN = c.String(),
                        PartitionId = c.Int(),
                    })
                .PrimaryKey(t => t.TransporterPaymentRequestID)
                .ForeignKey("dbo.BusinessProcess", t => t.BusinessProcessID)
                .ForeignKey("dbo.Delivery", t => t.DeliveryID)
                .ForeignKey("Procurement.TransportOrder", t => t.TransportOrderID)
                .Index(t => t.BusinessProcessID)
                .Index(t => t.DeliveryID)
                .Index(t => t.TransportOrderID);
            
            CreateTable(
                "dbo.BusinessProcess",
                c => new
                    {
                        BusinessProcessID = c.Int(nullable: false, identity: true),
                        DocumentID = c.Int(nullable: false),
                        DocumentType = c.String(),
                        ProcessTypeID = c.Int(nullable: false),
                        CurrentStateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BusinessProcessID)
                .ForeignKey("dbo.ProcessTemplate", t => t.ProcessTypeID)
                .ForeignKey("dbo.BusinessProcessState", t => t.CurrentStateID)
                .Index(t => t.ProcessTypeID)
                .Index(t => t.CurrentStateID);
            
            CreateTable(
                "dbo.ProcessTemplate",
                c => new
                    {
                        ProcessTemplateID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        GraphicsData = c.String(),
                    })
                .PrimaryKey(t => t.ProcessTemplateID);
            
            CreateTable(
                "dbo.StateTemplate",
                c => new
                    {
                        StateTemplateID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AllowedAccessLevel = c.Int(nullable: false),
                        StateType = c.Int(nullable: false),
                        StateNo = c.Int(nullable: false),
                        ParentProcessTemplateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StateTemplateID)
                .ForeignKey("dbo.ProcessTemplate", t => t.ParentProcessTemplateID)
                .Index(t => t.ParentProcessTemplateID);
            
            CreateTable(
                "dbo.FlowTemplate",
                c => new
                    {
                        FlowTemplateID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentProcessTemplateID = c.Int(nullable: false),
                        InitialStateID = c.Int(nullable: false),
                        FinalStateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FlowTemplateID)
                .ForeignKey("dbo.ProcessTemplate", t => t.ParentProcessTemplateID)
                .ForeignKey("dbo.StateTemplate", t => t.InitialStateID)
                .ForeignKey("dbo.StateTemplate", t => t.FinalStateID)
                .Index(t => t.ParentProcessTemplateID)
                .Index(t => t.InitialStateID)
                .Index(t => t.FinalStateID);
            
            CreateTable(
                "dbo.BusinessProcessState",
                c => new
                    {
                        BusinessProcessStateID = c.Int(nullable: false, identity: true),
                        StateID = c.Int(nullable: false),
                        PerformedBy = c.String(),
                        DatePerformed = c.DateTime(nullable: false),
                        Comment = c.String(),
                        ParentBusinessProcessID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BusinessProcessStateID)
                .ForeignKey("dbo.StateTemplate", t => t.StateID)
                .ForeignKey("dbo.BusinessProcess", t => t.ParentBusinessProcessID)
                .Index(t => t.StateID)
                .Index(t => t.ParentBusinessProcessID);
            
            CreateTable(
                "dbo.PaymentRequest",
                c => new
                    {
                        PaymentRequestID = c.Int(nullable: false, identity: true),
                        TransportOrderID = c.Int(nullable: false),
                        RequestedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransportedQuantityInQTL = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReferenceNo = c.String(),
                        RequestedDate = c.DateTime(nullable: false),
                        BusinessProcessID = c.Int(nullable: false),
                        LabourCostRate = c.Decimal(precision: 18, scale: 2),
                        LabourCost = c.Decimal(precision: 18, scale: 2),
                        RejectedAmount = c.Decimal(precision: 18, scale: 2),
                        RejectionReason = c.String(),
                        PartitionId = c.Int(),
                    })
                .PrimaryKey(t => t.PaymentRequestID)
                .ForeignKey("Procurement.TransportOrder", t => t.TransportOrderID)
                .ForeignKey("dbo.BusinessProcess", t => t.BusinessProcessID)
                .Index(t => t.TransportOrderID)
                .Index(t => t.BusinessProcessID);
            
            CreateTable(
                "dbo.TransporterCheque",
                c => new
                    {
                        TransporterChequeId = c.Int(nullable: false, identity: true),
                        PartitionId = c.Int(),
                        BusinessProcessID = c.Int(nullable: false),
                        CheckNo = c.String(nullable: false, maxLength: 50),
                        PaymentVoucherNo = c.String(nullable: false, maxLength: 50),
                        BankName = c.String(nullable: false, maxLength: 50),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreparedBy = c.Int(nullable: false),
                        AppovedBy = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        AppovedDate = c.DateTime(nullable: false),
                        PaymentDate = c.DateTime(),
                        IssueDate = c.DateTime(nullable: false),
                        PaidBy = c.Int(),
                    })
                .PrimaryKey(t => t.TransporterChequeId)
                .ForeignKey("dbo.UserProfile", t => t.PreparedBy)
                .ForeignKey("dbo.UserProfile", t => t.AppovedBy)
                .ForeignKey("dbo.BusinessProcess", t => t.BusinessProcessID)
                .Index(t => t.PreparedBy)
                .Index(t => t.AppovedBy)
                .Index(t => t.BusinessProcessID);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserProfileID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        GrandFatherName = c.String(),
                        ActiveInd = c.Boolean(nullable: false),
                        LoggedInInd = c.Boolean(nullable: false),
                        LogginDate = c.DateTime(),
                        LogOutDate = c.DateTime(),
                        FailedAttempts = c.Int(nullable: false),
                        LockedInInd = c.Boolean(nullable: false),
                        LanguageCode = c.String(),
                        DatePreference = c.String(),
                        PreferedWeightMeasurment = c.String(),
                        Keyboard = c.String(),
                        MobileNumber = c.String(),
                        Email = c.String(),
                        DefaultTheme = c.String(),
                        PartitionId = c.Int(),
                        DefaultHub = c.Int(),
                        RegionID = c.Int(),
                        RegionalUser = c.Boolean(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                        TariffEntry = c.Boolean(),
                    })
                .PrimaryKey(t => t.UserProfileID);
            
            CreateTable(
                "dbo.DonationPlanHeader",
                c => new
                    {
                        DonationHeaderPlanID = c.Int(nullable: false, identity: true),
                        ShippingInstructionId = c.Int(nullable: false),
                        GiftCertificateID = c.Int(),
                        CommodityID = c.Int(nullable: false),
                        CommodityTypeID = c.Int(),
                        DonatedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DonorID = c.Int(nullable: false),
                        ProgramID = c.Int(nullable: false),
                        ETA = c.DateTime(nullable: false),
                        Vessel = c.String(maxLength: 50),
                        ReferenceNo = c.String(maxLength: 50),
                        ModeOfTransport = c.Int(),
                        TransactionGroupID = c.Guid(),
                        IsCommited = c.Boolean(),
                        EnteredBy = c.Int(),
                        AllocationDate = c.DateTime(),
                        Remark = c.String(maxLength: 500),
                        Status = c.Int(),
                    })
                .PrimaryKey(t => t.DonationHeaderPlanID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .ForeignKey("dbo.CommodityType", t => t.CommodityTypeID)
                .ForeignKey("dbo.ShippingInstruction", t => t.ShippingInstructionId)
                .ForeignKey("dbo.Donor", t => t.DonorID)
                .ForeignKey("dbo.Program", t => t.ProgramID)
                .ForeignKey("dbo.UserProfile", t => t.EnteredBy)
                .Index(t => t.CommodityID)
                .Index(t => t.CommodityTypeID)
                .Index(t => t.ShippingInstructionId)
                .Index(t => t.DonorID)
                .Index(t => t.ProgramID)
                .Index(t => t.EnteredBy);
            
            CreateTable(
                "dbo.CommodityType",
                c => new
                    {
                        CommodityTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.CommodityTypeID);
            
            CreateTable(
                "dbo.DonationPlanDetail",
                c => new
                    {
                        DonationDetailPlanID = c.Int(nullable: false, identity: true),
                        DonationHeaderPlanID = c.Int(nullable: false),
                        HubID = c.Int(nullable: false),
                        AllocatedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceivedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.DonationDetailPlanID)
                .ForeignKey("dbo.DonationPlanHeader", t => t.DonationHeaderPlanID)
                .ForeignKey("dbo.Hub", t => t.HubID)
                .Index(t => t.DonationHeaderPlanID)
                .Index(t => t.HubID);
            
            CreateTable(
                "dbo.WoredaStcokDistribution",
                c => new
                    {
                        WoredaStockDistributionID = c.Int(nullable: false, identity: true),
                        WoredaID = c.Int(nullable: false),
                        ProgramID = c.Int(nullable: false),
                        PlanID = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        DirectSupport = c.Int(),
                        PublicSupport = c.Int(),
                        ActualBeneficairies = c.Int(nullable: false),
                        DistributionDate = c.DateTime(nullable: false),
                        DistributedBy = c.Int(),
                        Remark = c.String(maxLength: 200),
                        MaleLessThan5Years = c.Int(nullable: false),
                        FemaleLessThan5Years = c.Int(nullable: false),
                        MaleBetween5And18Years = c.Int(nullable: false),
                        FemaleBetween5And18Years = c.Int(nullable: false),
                        MaleAbove18Years = c.Int(nullable: false),
                        FemaleAbove18Years = c.Int(nullable: false),
                        TransactionGroupID = c.Guid(),
                    })
                .PrimaryKey(t => t.WoredaStockDistributionID)
                .ForeignKey("dbo.UserProfile", t => t.DistributedBy)
                .ForeignKey("dbo.AdminUnit", t => t.WoredaID)
                .Index(t => t.DistributedBy)
                .Index(t => t.WoredaID);
            
            CreateTable(
                "dbo.WoredaStockDistributionDetail",
                c => new
                    {
                        WoredaStockDistributionDetailID = c.Int(nullable: false, identity: true),
                        WoredaStockDistributionID = c.Int(nullable: false),
                        FDPID = c.Int(nullable: false),
                        CommodityID = c.Int(nullable: false),
                        DistributedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EndingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalIn = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalOut = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DistributionStartDate = c.DateTime(),
                        DistributionEndDate = c.DateTime(),
                        LossAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LossReason = c.Int(),
                        RequisitionId = c.Int(),
                    })
                .PrimaryKey(t => t.WoredaStockDistributionDetailID)
                .ForeignKey("dbo.FDP", t => t.FDPID)
                .ForeignKey("dbo.WoredaStcokDistribution", t => t.WoredaStockDistributionID)
                .Index(t => t.FDPID)
                .Index(t => t.WoredaStockDistributionID);
            
            CreateTable(
                "dbo.HRD",
                c => new
                    {
                        HRDID = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        SeasonID = c.Int(),
                        DateCreated = c.DateTime(nullable: false),
                        PublishedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        RationID = c.Int(nullable: false),
                        Status = c.Int(),
                        PlanID = c.Int(nullable: false),
                        PartitionId = c.Int(),
                        TransactionGroupID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.HRDID)
                .ForeignKey("dbo.TransactionGroup", t => t.TransactionGroupID)
                .ForeignKey("dbo.Ration", t => t.RationID)
                .ForeignKey("dbo.UserProfile", t => t.CreatedBy)
                .ForeignKey("dbo.Season", t => t.SeasonID)
                .ForeignKey("dbo.Plan", t => t.PlanID)
                .Index(t => t.TransactionGroupID)
                .Index(t => t.RationID)
                .Index(t => t.CreatedBy)
                .Index(t => t.SeasonID)
                .Index(t => t.PlanID);
            
            CreateTable(
                "dbo.HRDDetail",
                c => new
                    {
                        HRDDetailID = c.Int(nullable: false, identity: true),
                        HRDID = c.Int(nullable: false),
                        AdminUnitID = c.Int(nullable: false),
                        Beneficiaries = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        StartingMonth = c.Int(nullable: false),
                        PartitionId = c.Int(),
                    })
                .PrimaryKey(t => t.HRDDetailID)
                .ForeignKey("dbo.HRD", t => t.HRDID)
                .ForeignKey("dbo.AdminUnit", t => t.AdminUnitID)
                .Index(t => t.HRDID)
                .Index(t => t.AdminUnitID);
            
            CreateTable(
                "dbo.Season",
                c => new
                    {
                        SeasonID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.SeasonID);
            
            CreateTable(
                "dbo.Plan",
                c => new
                    {
                        PlanID = c.Int(nullable: false, identity: true),
                        PlanName = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        ProgramID = c.Int(nullable: false),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        PartitionId = c.Int(),
                    })
                .PrimaryKey(t => t.PlanID)
                .ForeignKey("dbo.Program", t => t.ProgramID)
                .Index(t => t.ProgramID);
            
            CreateTable(
                "dbo.Contribution",
                c => new
                    {
                        ContributionID = c.Int(nullable: false, identity: true),
                        DonorID = c.Int(nullable: false),
                        HRDID = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        PartitionId = c.Int(),
                        ContributionType = c.String(),
                    })
                .PrimaryKey(t => t.ContributionID)
                .ForeignKey("dbo.Donor", t => t.DonorID)
                .ForeignKey("dbo.HRD", t => t.HRDID)
                .Index(t => t.DonorID)
                .Index(t => t.HRDID);
            
            CreateTable(
                "dbo.ContributionDetail",
                c => new
                    {
                        ContributionDetailID = c.Int(nullable: false, identity: true),
                        ContributionID = c.Int(nullable: false),
                        PledgeReferenceNo = c.String(maxLength: 50),
                        PledgeDate = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ContributionDetailID)
                .ForeignKey("dbo.Currency", t => t.CurrencyID)
                .ForeignKey("dbo.Contribution", t => t.ContributionID)
                .Index(t => t.CurrencyID)
                .Index(t => t.ContributionID);
            
            CreateTable(
                "dbo.Currency",
                c => new
                    {
                        CurrencyID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CurrencyID);
            
            CreateTable(
                "dbo.InKindContributionDetail",
                c => new
                    {
                        InKindContributionDetailID = c.Int(nullable: false, identity: true),
                        ContributionID = c.Int(nullable: false),
                        ReferenceNumber = c.String(),
                        ContributionDate = c.DateTime(nullable: false),
                        CommodityID = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.InKindContributionDetailID)
                .ForeignKey("dbo.Contribution", t => t.ContributionID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .Index(t => t.ContributionID)
                .Index(t => t.CommodityID);
            
            CreateTable(
                "dbo.LoanReciptPlanDetail",
                c => new
                    {
                        LoanReciptPlanDetailID = c.Int(nullable: false, identity: true),
                        LoanReciptPlanID = c.Int(nullable: false),
                        HubID = c.Int(nullable: false),
                        RecievedQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ApprovedBy = c.Int(nullable: false),
                        RecievedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LoanReciptPlanDetailID)
                .ForeignKey("dbo.LoanReciptPlan", t => t.LoanReciptPlanID)
                .ForeignKey("dbo.Hub", t => t.HubID)
                .ForeignKey("dbo.UserProfile", t => t.ApprovedBy)
                .Index(t => t.LoanReciptPlanID)
                .Index(t => t.HubID)
                .Index(t => t.ApprovedBy);
            
            CreateTable(
                "dbo.LoanReciptPlan",
                c => new
                    {
                        LoanReciptPlanID = c.Int(nullable: false, identity: true),
                        ShippingInstructionID = c.Int(nullable: false),
                        LoanSource = c.String(),
                        ProgramID = c.Int(nullable: false),
                        ProjectCode = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ReferenceNumber = c.String(),
                        CommoditySourceID = c.Int(nullable: false),
                        CommodityID = c.Int(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StatusID = c.Int(nullable: false),
                        IsFalseGRN = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LoanReciptPlanID)
                .ForeignKey("dbo.Program", t => t.ProgramID)
                .ForeignKey("dbo.CommoditySource", t => t.CommoditySourceID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .ForeignKey("dbo.ShippingInstruction", t => t.ShippingInstructionID)
                .Index(t => t.ProgramID)
                .Index(t => t.CommoditySourceID)
                .Index(t => t.CommodityID)
                .Index(t => t.ShippingInstructionID);
            
            CreateTable(
                "dbo.CommoditySource",
                c => new
                    {
                        CommoditySourceID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.CommoditySourceID);
            
            CreateTable(
                "dbo.Transfer",
                c => new
                    {
                        TransferID = c.Int(nullable: false, identity: true),
                        ShippingInstructionID = c.Int(nullable: false),
                        SourceHubID = c.Int(nullable: false),
                        ProgramID = c.Int(nullable: false),
                        CommoditySourceID = c.Int(nullable: false),
                        CommodityID = c.Int(nullable: false),
                        DestinationHubID = c.Int(nullable: false),
                        ProjectCode = c.String(nullable: false, maxLength: 50),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        ReferenceNumber = c.String(nullable: false, maxLength: 50),
                        StatusID = c.Int(nullable: false),
                        Remark = c.String(maxLength: 150),
                        SourceSwap = c.Int(nullable: false),
                        DestinationSwap = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TransferID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .ForeignKey("dbo.CommoditySource", t => t.CommoditySourceID)
                .ForeignKey("dbo.Hub", t => t.SourceHubID)
                .ForeignKey("dbo.Hub", t => t.DestinationHubID)
                .ForeignKey("dbo.Hub", t => t.SourceSwap)
                .ForeignKey("dbo.Hub", t => t.DestinationSwap)
                .ForeignKey("dbo.Program", t => t.ProgramID)
                .ForeignKey("dbo.ShippingInstruction", t => t.ShippingInstructionID)
                .Index(t => t.CommodityID)
                .Index(t => t.CommoditySourceID)
                .Index(t => t.SourceHubID)
                .Index(t => t.DestinationHubID)
                .Index(t => t.SourceSwap)
                .Index(t => t.DestinationSwap)
                .Index(t => t.ProgramID)
                .Index(t => t.ShippingInstructionID);
            
            CreateTable(
                "dbo.TransporterChequeDetail",
                c => new
                    {
                        TransporterChequeDetailID = c.Int(nullable: false, identity: true),
                        TransporterChequeID = c.Int(nullable: false),
                        TransporterPaymentRequestID = c.Int(nullable: false),
                        PartitionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TransporterChequeDetailID)
                .ForeignKey("dbo.TransporterCheque", t => t.TransporterChequeID)
                .ForeignKey("dbo.TransporterPaymentRequest", t => t.TransporterPaymentRequestID)
                .Index(t => t.TransporterChequeID)
                .Index(t => t.TransporterPaymentRequestID);
            
            CreateTable(
                "dbo.DispatchDetail",
                c => new
                    {
                        DispatchDetailID = c.Guid(nullable: false),
                        PartitionId = c.Int(),
                        TransactionGroupID = c.Guid(),
                        DispatchID = c.Guid(),
                        CommodityID = c.Int(nullable: false),
                        RequestedQunatityInUnit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitID = c.Int(nullable: false),
                        RequestedQuantityInMT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityPerUnit = c.Decimal(precision: 18, scale: 2),
                        Description = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.DispatchDetailID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .ForeignKey("dbo.Dispatch", t => t.DispatchID)
                .ForeignKey("dbo.Unit", t => t.UnitID)
                .ForeignKey("dbo.TransactionGroup", t => t.TransactionGroupID)
                .Index(t => t.CommodityID)
                .Index(t => t.DispatchID)
                .Index(t => t.UnitID)
                .Index(t => t.TransactionGroupID);
            
            CreateTable(
                "dbo.RegionalPSNPPlanDetail",
                c => new
                    {
                        RegionalPSNPPlanDetailID = c.Int(nullable: false, identity: true),
                        RegionalPSNPPlanID = c.Int(nullable: false),
                        PlanedWoredaID = c.Int(nullable: false),
                        BeneficiaryCount = c.Int(nullable: false),
                        FoodRatio = c.Int(nullable: false),
                        CashRatio = c.Int(nullable: false),
                        Item3Ratio = c.Int(nullable: false),
                        Item4Ratio = c.Int(nullable: false),
                        StartingMonth = c.Int(nullable: false),
                        Contingency = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RegionalPSNPPlanDetailID)
                .ForeignKey("dbo.RegionalPSNPPlan", t => t.RegionalPSNPPlanID)
                .ForeignKey("dbo.AdminUnit", t => t.PlanedWoredaID)
                .Index(t => t.RegionalPSNPPlanID)
                .Index(t => t.PlanedWoredaID);
            
            CreateTable(
                "dbo.LocalPurchase",
                c => new
                    {
                        LocalPurchaseID = c.Int(nullable: false, identity: true),
                        GiftCertificateID = c.Int(nullable: false),
                        ShippingInstructionID = c.Int(nullable: false),
                        ProjectCode = c.String(),
                        CommodityID = c.Int(nullable: false),
                        DonorID = c.Int(nullable: false),
                        ProgramID = c.Int(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        PurchaseOrder = c.String(),
                        SupplierName = c.String(),
                        ReferenceNumber = c.String(),
                        StatusID = c.Int(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.LocalPurchaseID)
                .ForeignKey("dbo.GiftCertificate", t => t.GiftCertificateID)
                .ForeignKey("dbo.ShippingInstruction", t => t.ShippingInstructionID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .ForeignKey("dbo.Donor", t => t.DonorID)
                .ForeignKey("dbo.Program", t => t.ProgramID)
                .Index(t => t.GiftCertificateID)
                .Index(t => t.ShippingInstructionID)
                .Index(t => t.CommodityID)
                .Index(t => t.DonorID)
                .Index(t => t.ProgramID);
            
            CreateTable(
                "dbo.LocalPurchaseDetail",
                c => new
                    {
                        LocalPurchaseDetailID = c.Int(nullable: false, identity: true),
                        LocalPurchaseID = c.Int(nullable: false),
                        HubID = c.Int(nullable: false),
                        AllocatedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RecievedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.LocalPurchaseDetailID)
                .ForeignKey("dbo.LocalPurchase", t => t.LocalPurchaseID)
                .ForeignKey("dbo.Hub", t => t.HubID)
                .Index(t => t.LocalPurchaseID)
                .Index(t => t.HubID);
            
            CreateTable(
                "dbo.Store",
                c => new
                    {
                        StoreID = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Name = c.String(),
                        HubID = c.Int(nullable: false),
                        IsTemporary = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        StackCount = c.Int(nullable: false),
                        StoreManName = c.String(),
                    })
                .PrimaryKey(t => t.StoreID)
                .ForeignKey("dbo.Hub", t => t.HubID)
                .Index(t => t.HubID);
            
            CreateTable(
                "dbo.PromisedContribution",
                c => new
                    {
                        PromisedContributionId = c.Int(nullable: false, identity: true),
                        PromisedQuantity = c.Single(),
                        DeliveredQuantity = c.Single(),
                        ExpectedTimeOfArrival = c.DateTime(nullable: false),
                        DonorId = c.Int(nullable: false),
                        CommodityId = c.Int(nullable: false),
                        HubId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PromisedContributionId)
                .ForeignKey("dbo.Commodity", t => t.CommodityId)
                .ForeignKey("dbo.Donor", t => t.DonorId)
                .ForeignKey("dbo.Hub", t => t.HubId)
                .Index(t => t.CommodityId)
                .Index(t => t.DonorId)
                .Index(t => t.HubId);
            
            CreateTable(
                "dbo.WoredaHubLink",
                c => new
                    {
                        WoredaHubLinkID = c.Int(nullable: false, identity: true),
                        WoredaHubID = c.Int(nullable: false),
                        WoredaID = c.Int(nullable: false),
                        HubID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WoredaHubLinkID)
                .ForeignKey("dbo.AdminUnit", t => t.WoredaID)
                .ForeignKey("dbo.Hub", t => t.HubID)
                .ForeignKey("dbo.WoredaHub", t => t.WoredaHubID)
                .Index(t => t.WoredaID)
                .Index(t => t.HubID)
                .Index(t => t.WoredaHubID);
            
            CreateTable(
                "dbo.WoredaHub",
                c => new
                    {
                        WoredaHubID = c.Int(nullable: false, identity: true),
                        HRDID = c.Int(nullable: false),
                        PlanID = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WoredaHubID);
            
            CreateTable(
                "dbo.ReceiptPlanDetail",
                c => new
                    {
                        ReceiptDetailId = c.Int(nullable: false, identity: true),
                        ReceiptHeaderId = c.Int(nullable: false),
                        HubId = c.Int(nullable: false),
                        Allocated = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Received = c.Decimal(precision: 18, scale: 2),
                        Balance = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ReceiptDetailId)
                .ForeignKey("dbo.Hub", t => t.HubId)
                .ForeignKey("dbo.ReceiptPlan", t => t.ReceiptHeaderId)
                .Index(t => t.HubId)
                .Index(t => t.ReceiptHeaderId);
            
            CreateTable(
                "dbo.ReceiptPlan",
                c => new
                    {
                        ReceiptHeaderId = c.Int(nullable: false, identity: true),
                        GiftCertificateDetailId = c.Int(nullable: false),
                        ReceiptDate = c.DateTime(),
                        EnteredBy = c.Int(),
                        IsClosed = c.Boolean(),
                        Remark = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ReceiptHeaderId)
                .ForeignKey("dbo.GiftCertificateDetail", t => t.GiftCertificateDetailId)
                .Index(t => t.GiftCertificateDetailId);
            
            CreateTable(
                "dbo.TransporterAgreementVersion",
                c => new
                    {
                        TransporterAgreementVersionID = c.Int(nullable: false, identity: true),
                        BidID = c.Int(nullable: false),
                        TransporterID = c.Int(nullable: false),
                        AgreementDocxFile = c.Binary(nullable: false),
                        IssueDate = c.DateTime(nullable: false),
                        Current = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TransporterAgreementVersionID)
                .ForeignKey("procurement.Bid", t => t.BidID)
                .ForeignKey("Procurement.Transporter", t => t.TransporterID)
                .Index(t => t.BidID)
                .Index(t => t.TransporterID);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ContactID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PhoneNo = c.String(),
                        FDPID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ContactID)
                .ForeignKey("dbo.FDP", t => t.FDPID)
                .Index(t => t.FDPID);
            
            CreateTable(
                "dbo.DistributionByAgeDetail",
                c => new
                    {
                        DistributionByAgeDetailID = c.Int(nullable: false, identity: true),
                        DistributionHeaderID = c.Int(nullable: false),
                        FDPID = c.Int(nullable: false),
                        MaleLessThan5Years = c.Int(nullable: false),
                        FemaleLessThan5Years = c.Int(nullable: false),
                        MaleBetween5And18Years = c.Int(nullable: false),
                        FemaleBetween5And18Years = c.Int(nullable: false),
                        MaleAbove18Years = c.Int(nullable: false),
                        FemaleAbove18Years = c.Int(nullable: false),
                        WoredaStockDistribution_WoredaStockDistributionID = c.Int(),
                    })
                .PrimaryKey(t => t.DistributionByAgeDetailID)
                .ForeignKey("dbo.WoredaStcokDistribution", t => t.WoredaStockDistribution_WoredaStockDistributionID)
                .ForeignKey("dbo.FDP", t => t.FDPID)
                .Index(t => t.WoredaStockDistribution_WoredaStockDistributionID)
                .Index(t => t.FDPID);
            
            CreateTable(
                "dbo.HRDCommodityDetail",
                c => new
                    {
                        HRDCommodityDetailID = c.Int(nullable: false, identity: true),
                        HRDDetailID = c.Int(nullable: false),
                        CommodityID = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.HRDCommodityDetailID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .Index(t => t.CommodityID);
            
            CreateTable(
                "dbo.DispatchAllocationDetail",
                c => new
                    {
                        DiapatcheAllocationDetailId = c.Int(nullable: false, identity: true),
                        ReliefRequisitionDetailId = c.Int(nullable: false),
                        ReliefRequistionId = c.Int(nullable: false),
                        Fdpid = c.Int(nullable: false),
                        Beneficiaries = c.Int(nullable: false),
                        ProjectID = c.String(),
                        SINumber = c.String(),
                        Zone = c.String(),
                        Wereda = c.String(),
                        Commodity = c.String(),
                        Quantity = c.Single(nullable: false),
                        Hub = c.String(),
                        ReliefRequistion_RequisitionID = c.Int(),
                    })
                .PrimaryKey(t => t.DiapatcheAllocationDetailId)
                .ForeignKey("EarlyWarning.ReliefRequisition", t => t.ReliefRequistion_RequisitionID)
                .ForeignKey("dbo.FDP", t => t.Fdpid)
                .Index(t => t.ReliefRequistion_RequisitionID)
                .Index(t => t.Fdpid);
            
            CreateTable(
                "dbo.vwTransportOrder",
                c => new
                    {
                        TransportOrderDetailID = c.Int(nullable: false),
                        TransportOrderID = c.Int(nullable: false),
                        TransportOrderNo = c.String(nullable: false, maxLength: 50),
                        OrderDate = c.DateTime(nullable: false),
                        RequestedDispatchDate = c.DateTime(nullable: false),
                        OrderExpiryDate = c.DateTime(nullable: false),
                        OrderStartDate = c.DateTime(nullable: false),
                        OrderEndDate = c.DateTime(nullable: false),
                        BidDocumentNo = c.String(nullable: false, maxLength: 50),
                        PerformanceBondReceiptNo = c.String(maxLength: 50),
                        TransporterID = c.Int(nullable: false),
                        ConsignerName = c.String(maxLength: 50),
                        TransporterSignedName = c.String(maxLength: 50),
                        ConsignerDate = c.DateTime(),
                        TransporterSignedDate = c.DateTime(),
                        ContractNumber = c.String(maxLength: 50),
                        FdpID = c.Int(nullable: false),
                        SourceWarehouseID = c.Int(nullable: false),
                        QuantityQtl = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DistanceFromOrigin = c.Decimal(precision: 18, scale: 2),
                        TariffPerQtl = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RequisitionID = c.Int(nullable: false),
                        CommodityID = c.Int(nullable: false),
                        ZoneID = c.Int(),
                        DonorID = c.Int(),
                        FDPName = c.String(nullable: false, maxLength: 50),
                        HubName = c.String(nullable: false, maxLength: 50),
                        RequisitionNo = c.String(maxLength: 255),
                        CommodityName = c.String(nullable: false, maxLength: 50),
                        DonorName = c.String(maxLength: 50),
                        WoredaName = c.String(maxLength: 50),
                        ZoneName = c.String(maxLength: 50),
                        TransporterName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => new { t.TransportOrderDetailID, t.TransportOrderID });
            
            CreateTable(
                "dbo.Workflow",
                c => new
                    {
                        WorkflowID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.WorkflowID);
            
            CreateTable(
                "dbo.WorkflowStatus",
                c => new
                    {
                        StatusID = c.Int(nullable: false),
                        WorkflowID = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.StatusID, t.WorkflowID })
                .ForeignKey("dbo.Workflow", t => t.WorkflowID)
                .Index(t => t.WorkflowID);
            
            CreateTable(
                "dbo.ApplicationSetting",
                c => new
                    {
                        SettingID = c.Int(nullable: false, identity: true),
                        SettingName = c.String(),
                        SettingValue = c.String(),
                    })
                .PrimaryKey(t => t.SettingID);
            
            CreateTable(
                "dbo.vwPSNPAnnualPlan",
                c => new
                    {
                        RegionalPSNPPlanID = c.Int(nullable: false),
                        WoredaID = c.Int(nullable: false),
                        ZoneID = c.Int(nullable: false),
                        FoodRatio = c.Int(),
                        CashRatio = c.Int(),
                        BeneficiaryCount = c.Int(),
                        Duration = c.Int(),
                        Year = c.Int(),
                        WoredaName = c.String(maxLength: 50),
                        ZoneName = c.String(maxLength: 50),
                        Region = c.String(),
                        StatusID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RegionalPSNPPlanID, t.WoredaID, t.ZoneID });
            
            CreateTable(
                "dbo.vw_NeedAssessment",
                c => new
                    {
                        NeedAID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Season = c.String(),
                        TypeOfNeedAssessment = c.String(),
                        NeedAApproved = c.Boolean(nullable: false),
                        Year = c.Int(nullable: false),
                        PSNPFromWoredasMale = c.Int(nullable: false),
                        PSNPFromWoredasFemale = c.Int(nullable: false),
                        NonPSNPFromWoredasMale = c.Int(nullable: false),
                        NonPSNPFromWoredasFemale = c.Int(nullable: false),
                        TotalBeneficiaries = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NeedAID);
            
            CreateTable(
                "dbo.LetterTemplate",
                c => new
                    {
                        LetterTemplateID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        FileName = c.String(maxLength: 50),
                        TemplateType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LetterTemplateID);
            
            CreateTable(
                "dbo.UserHub",
                c => new
                    {
                        UserHubID = c.Int(nullable: false, identity: true),
                        UserProfileID = c.Int(nullable: false),
                        HubID = c.Int(nullable: false),
                        IsDefault = c.String(),
                    })
                .PrimaryKey(t => t.UserHubID)
                .ForeignKey("dbo.Hub", t => t.HubID)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileID)
                .Index(t => t.HubID)
                .Index(t => t.UserProfileID);
            
            CreateTable(
                "dbo.Audit",
                c => new
                    {
                        AuditID = c.Guid(nullable: false),
                        HubID = c.Int(),
                        PartitionId = c.Int(),
                        LoginID = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        Action = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        TableName = c.String(nullable: false, maxLength: 100),
                        PrimaryKey = c.String(maxLength: 100),
                        ColumnName = c.String(maxLength: 3000),
                        NewValue = c.String(),
                        OldValue = c.String(),
                    })
                .PrimaryKey(t => t.AuditID);
            
            CreateTable(
                "dbo.AllocationByRegion",
                c => new
                    {
                        Hub = c.String(nullable: false, maxLength: 50),
                        Status = c.Int(),
                        RegionID = c.Int(),
                        Name = c.String(maxLength: 50),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        BenficiaryNo = c.Int(),
                    })
                .PrimaryKey(t => t.Hub);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 500),
                        Url = c.String(nullable: false, maxLength: 50),
                        RecordId = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        TypeOfNotification = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        Id = c.Int(nullable: false),
                        Role = c.Int(),
                        Application = c.String(),
                    })
                .PrimaryKey(t => t.NotificationId);
            
            CreateTable(
                "dbo.IDPSReasonType",
                c => new
                    {
                        IDPSId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.IDPSId);
            
            CreateTable(
                "dbo.DistibtionStatus",
                c => new
                    {
                        FDPID = c.Int(nullable: false, identity: true),
                        PlanName = c.String(maxLength: 50),
                        WoredaID = c.Int(nullable: false),
                        RegionName = c.String(),
                        WoredaName = c.String(maxLength: 50),
                        Status = c.Int(nullable: false),
                        RegionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FDPID);
            
            CreateTable(
                "dbo.Actions",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.ActionId);
            
            CreateTable(
                "dbo.SupportType",
                c => new
                    {
                        SupportTypeID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.SupportTypeID);
            
            CreateTable(
                "dbo.LossReason",
                c => new
                    {
                        LossReasonId = c.Int(nullable: false, identity: true),
                        LossReasonEg = c.String(),
                        LossReasonAm = c.String(),
                        LossReasonCodeEg = c.String(),
                        LossReasonCodeAm = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.LossReasonId);
            
            CreateTable(
                "dbo.RegionalRequestAllocation",
                c => new
                    {
                        RegionalRequestDetailID = c.Int(nullable: false),
                        RegionalRequestID = c.Int(nullable: false),
                        CommodityID = c.Int(nullable: false),
                        RequestDate = c.DateTime(nullable: false),
                        ProgramID = c.Int(nullable: false),
                        Program = c.String(),
                        RationID = c.Int(nullable: false),
                        RationName = c.String(),
                        Month = c.Int(nullable: false),
                        RegionName = c.String(),
                        RequestNumber = c.String(),
                        Year = c.Int(nullable: false),
                        FDPID = c.Int(nullable: false),
                        FDPName = c.String(),
                        Woreda = c.String(),
                        ZoneName = c.String(),
                        Beneficiaries = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Commodity = c.String(),
                        AllocatedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.RegionalRequestDetailID, t.RegionalRequestID, t.CommodityID });
            
            CreateTable(
                "dbo.CommodityGrade",
                c => new
                    {
                        CommodityGradeID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 50),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommodityGradeID);
            
            CreateTable(
                "dbo.TemplateType",
                c => new
                    {
                        TemplateTypeId = c.Int(nullable: false, identity: true),
                        TemplateObject = c.String(nullable: false, maxLength: 50),
                        Remark = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.TemplateTypeId);
            
            CreateTable(
                "dbo.Template",
                c => new
                    {
                        TemplateId = c.Int(nullable: false, identity: true),
                        TemplateType = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        FileName = c.String(),
                        Remark = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.TemplateId)
                .ForeignKey("dbo.TemplateType", t => t.TemplateType)
                .Index(t => t.TemplateType);
            
            CreateTable(
                "dbo.TemplateFields",
                c => new
                    {
                        TemplateFieldId = c.Int(nullable: false, identity: true),
                        TemplateId = c.Int(),
                        Field = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.TemplateFieldId);
            
            CreateTable(
                "procurement.Bid",
                c => new
                    {
                        BidID = c.Int(nullable: false, identity: true),
                        PartitionId = c.Int(),
                        RegionID = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        BidNumber = c.String(nullable: false),
                        OpeningDate = c.DateTime(nullable: false),
                        StatusID = c.Int(nullable: false),
                        TransportBidPlanID = c.Int(nullable: false),
                        BidBondAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        startTime = c.String(),
                        endTime = c.String(),
                        BidOpeningTime = c.String(),
                        UserProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BidID)
                .ForeignKey("dbo.AdminUnit", t => t.RegionID)
                .ForeignKey("Procurement.Status", t => t.StatusID)
                .ForeignKey("Procurement.TransportBidPlan", t => t.TransportBidPlanID)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId)
                .Index(t => t.RegionID)
                .Index(t => t.StatusID)
                .Index(t => t.TransportBidPlanID)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "Procurement.Status",
                c => new
                    {
                        StatusID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.StatusID);
            
            CreateTable(
                "Procurement.BidDetail",
                c => new
                    {
                        BidDetailID = c.Int(nullable: false, identity: true),
                        BidID = c.Int(nullable: false),
                        AmountForReliefProgram = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountForPSNPProgram = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BidDocumentPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CPO = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.BidDetailID)
                .ForeignKey("procurement.Bid", t => t.BidID)
                .Index(t => t.BidID);
            
            CreateTable(
                "Procurement.BidWinner",
                c => new
                    {
                        BidWinnerID = c.Int(nullable: false, identity: true),
                        BidID = c.Int(nullable: false),
                        SourceID = c.Int(nullable: false),
                        DestinationID = c.Int(nullable: false),
                        CommodityID = c.Int(),
                        TransporterID = c.Int(nullable: false),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Tariff = c.Decimal(precision: 18, scale: 2),
                        Position = c.Int(),
                        Status = c.Int(),
                        expiryDate = c.DateTime(),
                        BusinessProcessID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BidWinnerID)
                .ForeignKey("procurement.Bid", t => t.BidID)
                .ForeignKey("Procurement.Transporter", t => t.TransporterID)
                .ForeignKey("dbo.AdminUnit", t => t.DestinationID)
                .ForeignKey("dbo.Hub", t => t.SourceID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .ForeignKey("dbo.BusinessProcess", t => t.BusinessProcessID)
                .Index(t => t.BidID)
                .Index(t => t.TransporterID)
                .Index(t => t.DestinationID)
                .Index(t => t.SourceID)
                .Index(t => t.CommodityID)
                .Index(t => t.BusinessProcessID);
            
            CreateTable(
                "Procurement.Transporter",
                c => new
                    {
                        TransporterID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Region = c.Int(nullable: false),
                        SubCity = c.String(),
                        Zone = c.Int(nullable: false),
                        Woreda = c.Int(nullable: false),
                        Kebele = c.String(),
                        HouseNo = c.String(),
                        TelephoneNo = c.String(),
                        MobileNo = c.String(),
                        Email = c.String(),
                        Ownership = c.String(),
                        VehicleCount = c.Int(nullable: false),
                        LiftCapacityFrom = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LiftCapacityTo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LiftCapacityTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Capital = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EmployeeCountMale = c.Int(nullable: false),
                        EmployeeCountFemale = c.Int(nullable: false),
                        OwnerName = c.String(),
                        OwnerMobile = c.String(),
                        ManagerName = c.String(),
                        ManagerMobile = c.String(),
                        PartitionId = c.Int(),
                        OwnedByDRMFSS = c.Boolean(nullable: false),
                        ExperienceFrom = c.DateTime(nullable: false),
                        ExperienceTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TransporterID);
            
            CreateTable(
                "Procurement.TransportOrder",
                c => new
                    {
                        TransportOrderID = c.Int(nullable: false, identity: true),
                        TransportOrderNo = c.String(nullable: false, maxLength: 50),
                        ContractNumber = c.String(maxLength: 50),
                        OrderDate = c.DateTime(nullable: false),
                        RequestedDispatchDate = c.DateTime(nullable: false),
                        OrderExpiryDate = c.DateTime(nullable: false),
                        BidDocumentNo = c.String(nullable: false, maxLength: 50),
                        PerformanceBondReceiptNo = c.String(maxLength: 50),
                        PerformanceBondAmount = c.Decimal(precision: 18, scale: 2),
                        TransporterID = c.Int(nullable: false),
                        ConsignerName = c.String(maxLength: 50),
                        TransporterSignedName = c.String(maxLength: 50),
                        ConsignerDate = c.DateTime(nullable: false),
                        TransporterSignedDate = c.DateTime(nullable: false),
                        StatusID = c.Int(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        PartitionId = c.Int(),
                        TransportRequiqsitionId = c.Int(),
                    })
                .PrimaryKey(t => t.TransportOrderID)
                .ForeignKey("Procurement.Transporter", t => t.TransporterID)
                .Index(t => t.TransporterID);
            
            CreateTable(
                "Procurement.TransportOrderDetail",
                c => new
                    {
                        TransportOrderDetailID = c.Int(nullable: false, identity: true),
                        TransportOrderID = c.Int(nullable: false),
                        FdpID = c.Int(nullable: false),
                        SourceWarehouseID = c.Int(nullable: false),
                        QuantityQtl = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DistanceFromOrigin = c.Decimal(precision: 18, scale: 4),
                        TariffPerQtl = c.Decimal(nullable: false, precision: 18, scale: 4),
                        RequisitionID = c.Int(nullable: false),
                        CommodityID = c.Int(nullable: false),
                        ZoneID = c.Int(),
                        DonorID = c.Int(),
                        BidID = c.Int(),
                        IsChanged = c.Boolean(nullable: false),
                        WinnerAssignedByLogistics = c.Boolean(),
                        PartitionId = c.Int(),
                    })
                .PrimaryKey(t => t.TransportOrderDetailID)
                .ForeignKey("dbo.AdminUnit", t => t.ZoneID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .ForeignKey("dbo.Donor", t => t.DonorID)
                .ForeignKey("dbo.FDP", t => t.FdpID)
                .ForeignKey("dbo.Hub", t => t.SourceWarehouseID)
                .ForeignKey("EarlyWarning.ReliefRequisition", t => t.RequisitionID)
                .ForeignKey("Procurement.TransportOrder", t => t.TransportOrderID)
                .Index(t => t.ZoneID)
                .Index(t => t.CommodityID)
                .Index(t => t.DonorID)
                .Index(t => t.FdpID)
                .Index(t => t.SourceWarehouseID)
                .Index(t => t.RequisitionID)
                .Index(t => t.TransportOrderID);
            
            CreateTable(
                "Procurement.TransportBidPlanDetail",
                c => new
                    {
                        TransportBidPlanDetailID = c.Int(nullable: false, identity: true),
                        BidPlanID = c.Int(nullable: false),
                        DestinationID = c.Int(nullable: false),
                        SourceID = c.Int(nullable: false),
                        ProgramID = c.Int(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PartitionId = c.Int(),
                    })
                .PrimaryKey(t => t.TransportBidPlanDetailID)
                .ForeignKey("Procurement.TransportBidPlan", t => t.BidPlanID)
                .ForeignKey("dbo.AdminUnit", t => t.DestinationID)
                .ForeignKey("dbo.Hub", t => t.SourceID)
                .ForeignKey("dbo.Program", t => t.ProgramID)
                .Index(t => t.BidPlanID)
                .Index(t => t.DestinationID)
                .Index(t => t.SourceID)
                .Index(t => t.ProgramID);
            
            CreateTable(
                "Procurement.TransportBidPlan",
                c => new
                    {
                        TransportBidPlanID = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        YearHalf = c.Int(nullable: false),
                        ProgramID = c.Int(nullable: false),
                        PartitionId = c.Int(),
                    })
                .PrimaryKey(t => t.TransportBidPlanID)
                .ForeignKey("dbo.Program", t => t.ProgramID)
                .Index(t => t.ProgramID);
            
            CreateTable(
                "Procurement.TransportBidQuotation",
                c => new
                    {
                        TransportBidQuotationID = c.Int(nullable: false, identity: true),
                        TransportBidQuotationHeaderID = c.Int(nullable: false),
                        BidID = c.Int(nullable: false),
                        TransporterID = c.Int(nullable: false),
                        SourceID = c.Int(nullable: false),
                        DestinationID = c.Int(nullable: false),
                        Tariff = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsWinner = c.Boolean(nullable: false),
                        Position = c.Int(nullable: false),
                        Remark = c.String(maxLength: 50),
                        PartitionId = c.Int(),
                    })
                .PrimaryKey(t => t.TransportBidQuotationID)
                .ForeignKey("dbo.AdminUnit", t => t.DestinationID)
                .ForeignKey("dbo.Hub", t => t.SourceID)
                .ForeignKey("Procurement.TransportBidQuotationHeader", t => t.TransportBidQuotationHeaderID)
                .ForeignKey("Procurement.Transporter", t => t.TransporterID)
                .ForeignKey("procurement.Bid", t => t.BidID)
                .Index(t => t.DestinationID)
                .Index(t => t.SourceID)
                .Index(t => t.TransportBidQuotationHeaderID)
                .Index(t => t.TransporterID)
                .Index(t => t.BidID);
            
            CreateTable(
                "Procurement.TransportBidQuotationHeader",
                c => new
                    {
                        TransportBidQuotationHeaderID = c.Int(nullable: false, identity: true),
                        BidQuotationDate = c.DateTime(),
                        TransporterId = c.Int(),
                        BidId = c.Int(),
                        RegionID = c.Int(),
                        EnteredBy = c.String(maxLength: 50),
                        Status = c.Int(nullable: false),
                        PartitionId = c.Int(),
                    })
                .PrimaryKey(t => t.TransportBidQuotationHeaderID)
                .ForeignKey("dbo.AdminUnit", t => t.RegionID)
                .ForeignKey("procurement.Bid", t => t.BidId)
                .ForeignKey("Procurement.Transporter", t => t.TransporterId)
                .Index(t => t.RegionID)
                .Index(t => t.BidId)
                .Index(t => t.TransporterId);
            
            CreateTable(
                "Logistics.ProjectCodeAllocation",
                c => new
                    {
                        ProjectCodeAllocationID = c.Int(nullable: false, identity: true),
                        HubAllocationID = c.Int(nullable: false),
                        ProjectCodeID = c.Int(),
                        Amount_FromProject = c.Int(),
                        SINumberID = c.Int(),
                        Amount_FromSI = c.Int(),
                        AllocatedBy = c.Int(nullable: false),
                        AlloccationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectCodeAllocationID)
                .ForeignKey("dbo.ProjectCode", t => t.ProjectCodeID)
                .ForeignKey("Logistics.HubAllocation", t => t.HubAllocationID)
                .ForeignKey("dbo.ShippingInstruction", t => t.SINumberID)
                .Index(t => t.ProjectCodeID)
                .Index(t => t.HubAllocationID)
                .Index(t => t.SINumberID);
            
            CreateTable(
                "Logistics.HubAllocation",
                c => new
                    {
                        HubAllocationID = c.Int(nullable: false, identity: true),
                        ReferenceNo = c.String(),
                        RequisitionID = c.Int(nullable: false),
                        HubID = c.Int(nullable: false),
                        SatelliteWarehouseID = c.Int(),
                        AllocationDate = c.DateTime(nullable: false),
                        AllocatedBy = c.Int(nullable: false),
                        PartitionId = c.Int(),
                    })
                .PrimaryKey(t => t.HubAllocationID)
                .ForeignKey("dbo.Hub", t => t.HubID)
                .ForeignKey("EarlyWarning.ReliefRequisition", t => t.RequisitionID)
                .Index(t => t.HubID)
                .Index(t => t.RequisitionID);
            
            CreateTable(
                "Logistics.TransportRequisitionDetail",
                c => new
                    {
                        TransportRequisitionDetailID = c.Int(nullable: false, identity: true),
                        TransportRequisitionID = c.Int(nullable: false),
                        RequisitionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TransportRequisitionDetailID)
                .ForeignKey("EarlyWarning.ReliefRequisition", t => t.RequisitionID)
                .ForeignKey("Logistics.TransportRequisition", t => t.TransportRequisitionID)
                .Index(t => t.RequisitionID)
                .Index(t => t.TransportRequisitionID);
            
            CreateTable(
                "Logistics.TransportRequisition",
                c => new
                    {
                        TransportRequisitionID = c.Int(nullable: false, identity: true),
                        TransportRequisitionNo = c.String(nullable: false, maxLength: 50),
                        RegionID = c.Int(nullable: false),
                        ProgramID = c.Int(nullable: false),
                        RequestedBy = c.Int(nullable: false),
                        RequestedDate = c.DateTime(nullable: false),
                        CertifiedBy = c.Int(nullable: false),
                        CertifiedDate = c.DateTime(nullable: false),
                        Remark = c.String(),
                        Status = c.Int(nullable: false),
                        PartitionId = c.Int(),
                    })
                .PrimaryKey(t => t.TransportRequisitionID)
                .ForeignKey("dbo.AdminUnit", t => t.RegionID)
                .ForeignKey("dbo.Program", t => t.ProgramID)
                .Index(t => t.RegionID)
                .Index(t => t.ProgramID);
            
            CreateTable(
                "EarlyWarning.RegionalRequest",
                c => new
                    {
                        RegionalRequestID = c.Int(nullable: false, identity: true),
                        RegionID = c.Int(nullable: false),
                        ProgramID = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        RequestDate = c.DateTime(nullable: false),
                        Year = c.Int(nullable: false),
                        Season = c.Int(nullable: false),
                        RequestNumber = c.String(nullable: false),
                        Remark = c.String(maxLength: 400),
                        Status = c.Int(nullable: false),
                        RationID = c.Int(nullable: false),
                        DonorID = c.Int(),
                        Round = c.Int(),
                        PlanID = c.Int(nullable: false),
                        IDPSReasonType = c.Int(),
                        PartitionId = c.Int(),
                        RequestedBy = c.Int(),
                        ApprovedBy = c.Int(),
                        Contingency = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RegionalRequestID)
                .ForeignKey("dbo.Ration", t => t.RationID)
                .ForeignKey("dbo.UserProfile", t => t.RequestedBy)
                .ForeignKey("dbo.UserProfile", t => t.ApprovedBy)
                .ForeignKey("dbo.AdminUnit", t => t.RegionID)
                .ForeignKey("dbo.Program", t => t.ProgramID)
                .ForeignKey("dbo.Donor", t => t.DonorID)
                .ForeignKey("dbo.Plan", t => t.PlanID)
                .Index(t => t.RationID)
                .Index(t => t.RequestedBy)
                .Index(t => t.ApprovedBy)
                .Index(t => t.RegionID)
                .Index(t => t.ProgramID)
                .Index(t => t.DonorID)
                .Index(t => t.PlanID);
            
            CreateTable(
                "EarlyWarning.RegionalRequestDetail",
                c => new
                    {
                        RegionalRequestDetailID = c.Int(nullable: false, identity: true),
                        RegionalRequestID = c.Int(nullable: false),
                        FDPID = c.Int(nullable: false),
                        Beneficiaries = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RegionalRequestDetailID)
                .ForeignKey("EarlyWarning.RegionalRequest", t => t.RegionalRequestID)
                .ForeignKey("dbo.FDP", t => t.FDPID)
                .Index(t => t.RegionalRequestID)
                .Index(t => t.FDPID);
            
            CreateTable(
                "EarlyWarning.RequestDetailCommodity",
                c => new
                    {
                        RequestCommodityID = c.Int(nullable: false, identity: true),
                        RegionalRequestDetailID = c.Int(nullable: false),
                        CommodityID = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitID = c.Int(),
                    })
                .PrimaryKey(t => t.RequestCommodityID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .ForeignKey("EarlyWarning.RegionalRequestDetail", t => t.RegionalRequestDetailID)
                .Index(t => t.CommodityID)
                .Index(t => t.RegionalRequestDetailID);
            
            CreateTable(
                "EarlyWarning.ReliefRequisition",
                c => new
                    {
                        RequisitionID = c.Int(nullable: false, identity: true),
                        CommodityID = c.Int(),
                        RegionID = c.Int(),
                        ZoneID = c.Int(),
                        Round = c.Int(),
                        Month = c.Int(nullable: false),
                        RequisitionNo = c.String(maxLength: 255),
                        RequestedBy = c.Int(),
                        RequestedDate = c.DateTime(),
                        ApprovedBy = c.Int(),
                        ApprovedDate = c.DateTime(),
                        Status = c.Int(),
                        ProgramID = c.Int(nullable: false),
                        RationID = c.Int(),
                        RegionalRequestID = c.Int(),
                        PartitionId = c.Int(),
                    })
                .PrimaryKey(t => t.RequisitionID)
                .ForeignKey("dbo.AdminUnit", t => t.RegionID)
                .ForeignKey("dbo.AdminUnit", t => t.ZoneID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .ForeignKey("dbo.Program", t => t.ProgramID)
                .ForeignKey("EarlyWarning.RegionalRequest", t => t.RegionalRequestID)
                .Index(t => t.RegionID)
                .Index(t => t.ZoneID)
                .Index(t => t.CommodityID)
                .Index(t => t.ProgramID)
                .Index(t => t.RegionalRequestID);
            
            CreateTable(
                "EarlyWarning.ReliefRequisitionDetail",
                c => new
                    {
                        RequisitionDetailID = c.Int(nullable: false, identity: true),
                        RequisitionID = c.Int(nullable: false),
                        CommodityID = c.Int(nullable: false),
                        BenficiaryNo = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        FDPID = c.Int(nullable: false),
                        DonorID = c.Int(),
                        Contingency = c.Decimal(precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.RequisitionDetailID)
                .ForeignKey("dbo.Commodity", t => t.CommodityID)
                .ForeignKey("dbo.Donor", t => t.DonorID)
                .ForeignKey("dbo.FDP", t => t.FDPID)
                .ForeignKey("EarlyWarning.ReliefRequisition", t => t.RequisitionID)
                .Index(t => t.CommodityID)
                .Index(t => t.DonorID)
                .Index(t => t.FDPID)
                .Index(t => t.RequisitionID);
            
            CreateTable(
                "EarlyWarning.NeedAssessment",
                c => new
                    {
                        NeedAID = c.Int(nullable: false, identity: true),
                        Region = c.Int(nullable: false),
                        Season = c.Int(nullable: false),
                        Year = c.Int(),
                        NeedADate = c.DateTime(),
                        NeddACreatedBy = c.Int(),
                        NeedAApproved = c.Boolean(),
                        NeedAApprovedBy = c.Int(),
                        PlanID = c.Int(nullable: false),
                        TypeOfNeedAssessment = c.Int(),
                        Remark = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.NeedAID)
                .ForeignKey("dbo.AdminUnit", t => t.Region)
                .ForeignKey("dbo.UserProfile", t => t.NeddACreatedBy)
                .ForeignKey("dbo.UserProfile", t => t.NeedAApprovedBy)
                .ForeignKey("dbo.Season", t => t.Season)
                .ForeignKey("EarlyWarning.TypeOfNeedAssessment", t => t.TypeOfNeedAssessment)
                .ForeignKey("dbo.Plan", t => t.PlanID)
                .Index(t => t.Region)
                .Index(t => t.NeddACreatedBy)
                .Index(t => t.NeedAApprovedBy)
                .Index(t => t.Season)
                .Index(t => t.TypeOfNeedAssessment)
                .Index(t => t.PlanID);
            
            CreateTable(
                "EarlyWarning.TypeOfNeedAssessment",
                c => new
                    {
                        TypeOfNeedAssessmentID = c.Int(nullable: false, identity: true),
                        TypeOfNeedAssessment = c.String(nullable: false, maxLength: 50),
                        Remark = c.String(maxLength: 10, fixedLength: true),
                    })
                .PrimaryKey(t => t.TypeOfNeedAssessmentID);
            
            CreateTable(
                "EarlyWarning.NeedAssessmentHeader",
                c => new
                    {
                        NAHeaderId = c.Int(nullable: false, identity: true),
                        NeedAID = c.Int(),
                        Zone = c.Int(),
                        Remark = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.NAHeaderId)
                .ForeignKey("dbo.AdminUnit", t => t.Zone)
                .ForeignKey("EarlyWarning.NeedAssessment", t => t.NeedAID)
                .Index(t => t.Zone)
                .Index(t => t.NeedAID);
            
            CreateTable(
                "EarlyWarning.NeedAssessmentDetail",
                c => new
                    {
                        NAId = c.Int(nullable: false, identity: true),
                        NeedAId = c.Int(),
                        Woreda = c.Int(),
                        ProjectedMale = c.Int(nullable: false),
                        ProjectedFemale = c.Int(nullable: false),
                        RegularPSNP = c.Int(nullable: false),
                        PSNP = c.Int(nullable: false),
                        NonPSNP = c.Int(nullable: false),
                        Contingencybudget = c.Int(nullable: false),
                        TotalBeneficiaries = c.Int(nullable: false),
                        PSNPFromWoredasMale = c.Int(nullable: false),
                        PSNPFromWoredasFemale = c.Int(nullable: false),
                        PSNPFromWoredasDOA = c.Int(nullable: false),
                        NonPSNPFromWoredasMale = c.Int(nullable: false),
                        NonPSNPFromWoredasFemale = c.Int(nullable: false),
                        NonPSNPFromWoredasDOA = c.Int(nullable: false),
                        Remark = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.NAId)
                .ForeignKey("dbo.AdminUnit", t => t.Woreda)
                .ForeignKey("EarlyWarning.NeedAssessmentHeader", t => t.NeedAId)
                .Index(t => t.Woreda)
                .Index(t => t.NeedAId);
            
            CreateTable(
                "EarlyWarning.HRDDonorCoverage",
                c => new
                    {
                        HrdDonorCovarageID = c.Int(nullable: false, identity: true),
                        DonorID = c.Int(nullable: false),
                        HRDID = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Remark = c.String(),
                        PartitionId = c.Int(),
                    })
                .PrimaryKey(t => t.HrdDonorCovarageID)
                .ForeignKey("dbo.Donor", t => t.DonorID)
                .ForeignKey("dbo.HRD", t => t.HRDID)
                .Index(t => t.DonorID)
                .Index(t => t.HRDID);
            
            CreateTable(
                "EarlyWarning.HRDDonorCoverageDetail",
                c => new
                    {
                        HRDDonorCoverageDetailID = c.Int(nullable: false, identity: true),
                        HRDDonorCoverageID = c.Int(nullable: false),
                        WoredaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HRDDonorCoverageDetailID)
                .ForeignKey("dbo.AdminUnit", t => t.WoredaID)
                .ForeignKey("EarlyWarning.HRDDonorCoverage", t => t.HRDDonorCoverageID)
                .Index(t => t.WoredaID)
                .Index(t => t.HRDDonorCoverageID);
            
        }
        
        public override void Down()
        {
            DropIndex("EarlyWarning.HRDDonorCoverageDetail", new[] { "HRDDonorCoverageID" });
            DropIndex("EarlyWarning.HRDDonorCoverageDetail", new[] { "WoredaID" });
            DropIndex("EarlyWarning.HRDDonorCoverage", new[] { "HRDID" });
            DropIndex("EarlyWarning.HRDDonorCoverage", new[] { "DonorID" });
            DropIndex("EarlyWarning.NeedAssessmentDetail", new[] { "NeedAId" });
            DropIndex("EarlyWarning.NeedAssessmentDetail", new[] { "Woreda" });
            DropIndex("EarlyWarning.NeedAssessmentHeader", new[] { "NeedAID" });
            DropIndex("EarlyWarning.NeedAssessmentHeader", new[] { "Zone" });
            DropIndex("EarlyWarning.NeedAssessment", new[] { "PlanID" });
            DropIndex("EarlyWarning.NeedAssessment", new[] { "TypeOfNeedAssessment" });
            DropIndex("EarlyWarning.NeedAssessment", new[] { "Season" });
            DropIndex("EarlyWarning.NeedAssessment", new[] { "NeedAApprovedBy" });
            DropIndex("EarlyWarning.NeedAssessment", new[] { "NeddACreatedBy" });
            DropIndex("EarlyWarning.NeedAssessment", new[] { "Region" });
            DropIndex("EarlyWarning.ReliefRequisitionDetail", new[] { "RequisitionID" });
            DropIndex("EarlyWarning.ReliefRequisitionDetail", new[] { "FDPID" });
            DropIndex("EarlyWarning.ReliefRequisitionDetail", new[] { "DonorID" });
            DropIndex("EarlyWarning.ReliefRequisitionDetail", new[] { "CommodityID" });
            DropIndex("EarlyWarning.ReliefRequisition", new[] { "RegionalRequestID" });
            DropIndex("EarlyWarning.ReliefRequisition", new[] { "ProgramID" });
            DropIndex("EarlyWarning.ReliefRequisition", new[] { "CommodityID" });
            DropIndex("EarlyWarning.ReliefRequisition", new[] { "ZoneID" });
            DropIndex("EarlyWarning.ReliefRequisition", new[] { "RegionID" });
            DropIndex("EarlyWarning.RequestDetailCommodity", new[] { "RegionalRequestDetailID" });
            DropIndex("EarlyWarning.RequestDetailCommodity", new[] { "CommodityID" });
            DropIndex("EarlyWarning.RegionalRequestDetail", new[] { "FDPID" });
            DropIndex("EarlyWarning.RegionalRequestDetail", new[] { "RegionalRequestID" });
            DropIndex("EarlyWarning.RegionalRequest", new[] { "PlanID" });
            DropIndex("EarlyWarning.RegionalRequest", new[] { "DonorID" });
            DropIndex("EarlyWarning.RegionalRequest", new[] { "ProgramID" });
            DropIndex("EarlyWarning.RegionalRequest", new[] { "RegionID" });
            DropIndex("EarlyWarning.RegionalRequest", new[] { "ApprovedBy" });
            DropIndex("EarlyWarning.RegionalRequest", new[] { "RequestedBy" });
            DropIndex("EarlyWarning.RegionalRequest", new[] { "RationID" });
            DropIndex("Logistics.TransportRequisition", new[] { "ProgramID" });
            DropIndex("Logistics.TransportRequisition", new[] { "RegionID" });
            DropIndex("Logistics.TransportRequisitionDetail", new[] { "TransportRequisitionID" });
            DropIndex("Logistics.TransportRequisitionDetail", new[] { "RequisitionID" });
            DropIndex("Logistics.HubAllocation", new[] { "RequisitionID" });
            DropIndex("Logistics.HubAllocation", new[] { "HubID" });
            DropIndex("Logistics.ProjectCodeAllocation", new[] { "SINumberID" });
            DropIndex("Logistics.ProjectCodeAllocation", new[] { "HubAllocationID" });
            DropIndex("Logistics.ProjectCodeAllocation", new[] { "ProjectCodeID" });
            DropIndex("Procurement.TransportBidQuotationHeader", new[] { "TransporterId" });
            DropIndex("Procurement.TransportBidQuotationHeader", new[] { "BidId" });
            DropIndex("Procurement.TransportBidQuotationHeader", new[] { "RegionID" });
            DropIndex("Procurement.TransportBidQuotation", new[] { "BidID" });
            DropIndex("Procurement.TransportBidQuotation", new[] { "TransporterID" });
            DropIndex("Procurement.TransportBidQuotation", new[] { "TransportBidQuotationHeaderID" });
            DropIndex("Procurement.TransportBidQuotation", new[] { "SourceID" });
            DropIndex("Procurement.TransportBidQuotation", new[] { "DestinationID" });
            DropIndex("Procurement.TransportBidPlan", new[] { "ProgramID" });
            DropIndex("Procurement.TransportBidPlanDetail", new[] { "ProgramID" });
            DropIndex("Procurement.TransportBidPlanDetail", new[] { "SourceID" });
            DropIndex("Procurement.TransportBidPlanDetail", new[] { "DestinationID" });
            DropIndex("Procurement.TransportBidPlanDetail", new[] { "BidPlanID" });
            DropIndex("Procurement.TransportOrderDetail", new[] { "TransportOrderID" });
            DropIndex("Procurement.TransportOrderDetail", new[] { "RequisitionID" });
            DropIndex("Procurement.TransportOrderDetail", new[] { "SourceWarehouseID" });
            DropIndex("Procurement.TransportOrderDetail", new[] { "FdpID" });
            DropIndex("Procurement.TransportOrderDetail", new[] { "DonorID" });
            DropIndex("Procurement.TransportOrderDetail", new[] { "CommodityID" });
            DropIndex("Procurement.TransportOrderDetail", new[] { "ZoneID" });
            DropIndex("Procurement.TransportOrder", new[] { "TransporterID" });
            DropIndex("Procurement.BidWinner", new[] { "BusinessProcessID" });
            DropIndex("Procurement.BidWinner", new[] { "CommodityID" });
            DropIndex("Procurement.BidWinner", new[] { "SourceID" });
            DropIndex("Procurement.BidWinner", new[] { "DestinationID" });
            DropIndex("Procurement.BidWinner", new[] { "TransporterID" });
            DropIndex("Procurement.BidWinner", new[] { "BidID" });
            DropIndex("Procurement.BidDetail", new[] { "BidID" });
            DropIndex("procurement.Bid", new[] { "UserProfileId" });
            DropIndex("procurement.Bid", new[] { "TransportBidPlanID" });
            DropIndex("procurement.Bid", new[] { "StatusID" });
            DropIndex("procurement.Bid", new[] { "RegionID" });
            DropIndex("dbo.Template", new[] { "TemplateType" });
            DropIndex("dbo.UserHub", new[] { "UserProfileID" });
            DropIndex("dbo.UserHub", new[] { "HubID" });
            DropIndex("dbo.WorkflowStatus", new[] { "WorkflowID" });
            DropIndex("dbo.DispatchAllocationDetail", new[] { "Fdpid" });
            DropIndex("dbo.DispatchAllocationDetail", new[] { "ReliefRequistion_RequisitionID" });
            DropIndex("dbo.HRDCommodityDetail", new[] { "CommodityID" });
            DropIndex("dbo.DistributionByAgeDetail", new[] { "FDPID" });
            DropIndex("dbo.DistributionByAgeDetail", new[] { "WoredaStockDistribution_WoredaStockDistributionID" });
            DropIndex("dbo.Contact", new[] { "FDPID" });
            DropIndex("dbo.TransporterAgreementVersion", new[] { "TransporterID" });
            DropIndex("dbo.TransporterAgreementVersion", new[] { "BidID" });
            DropIndex("dbo.ReceiptPlan", new[] { "GiftCertificateDetailId" });
            DropIndex("dbo.ReceiptPlanDetail", new[] { "ReceiptHeaderId" });
            DropIndex("dbo.ReceiptPlanDetail", new[] { "HubId" });
            DropIndex("dbo.WoredaHubLink", new[] { "WoredaHubID" });
            DropIndex("dbo.WoredaHubLink", new[] { "HubID" });
            DropIndex("dbo.WoredaHubLink", new[] { "WoredaID" });
            DropIndex("dbo.PromisedContribution", new[] { "HubId" });
            DropIndex("dbo.PromisedContribution", new[] { "DonorId" });
            DropIndex("dbo.PromisedContribution", new[] { "CommodityId" });
            DropIndex("dbo.Store", new[] { "HubID" });
            DropIndex("dbo.LocalPurchaseDetail", new[] { "HubID" });
            DropIndex("dbo.LocalPurchaseDetail", new[] { "LocalPurchaseID" });
            DropIndex("dbo.LocalPurchase", new[] { "ProgramID" });
            DropIndex("dbo.LocalPurchase", new[] { "DonorID" });
            DropIndex("dbo.LocalPurchase", new[] { "CommodityID" });
            DropIndex("dbo.LocalPurchase", new[] { "ShippingInstructionID" });
            DropIndex("dbo.LocalPurchase", new[] { "GiftCertificateID" });
            DropIndex("dbo.RegionalPSNPPlanDetail", new[] { "PlanedWoredaID" });
            DropIndex("dbo.RegionalPSNPPlanDetail", new[] { "RegionalPSNPPlanID" });
            DropIndex("dbo.DispatchDetail", new[] { "TransactionGroupID" });
            DropIndex("dbo.DispatchDetail", new[] { "UnitID" });
            DropIndex("dbo.DispatchDetail", new[] { "DispatchID" });
            DropIndex("dbo.DispatchDetail", new[] { "CommodityID" });
            DropIndex("dbo.TransporterChequeDetail", new[] { "TransporterPaymentRequestID" });
            DropIndex("dbo.TransporterChequeDetail", new[] { "TransporterChequeID" });
            DropIndex("dbo.Transfer", new[] { "ShippingInstructionID" });
            DropIndex("dbo.Transfer", new[] { "ProgramID" });
            DropIndex("dbo.Transfer", new[] { "DestinationSwap" });
            DropIndex("dbo.Transfer", new[] { "SourceSwap" });
            DropIndex("dbo.Transfer", new[] { "DestinationHubID" });
            DropIndex("dbo.Transfer", new[] { "SourceHubID" });
            DropIndex("dbo.Transfer", new[] { "CommoditySourceID" });
            DropIndex("dbo.Transfer", new[] { "CommodityID" });
            DropIndex("dbo.LoanReciptPlan", new[] { "ShippingInstructionID" });
            DropIndex("dbo.LoanReciptPlan", new[] { "CommodityID" });
            DropIndex("dbo.LoanReciptPlan", new[] { "CommoditySourceID" });
            DropIndex("dbo.LoanReciptPlan", new[] { "ProgramID" });
            DropIndex("dbo.LoanReciptPlanDetail", new[] { "ApprovedBy" });
            DropIndex("dbo.LoanReciptPlanDetail", new[] { "HubID" });
            DropIndex("dbo.LoanReciptPlanDetail", new[] { "LoanReciptPlanID" });
            DropIndex("dbo.InKindContributionDetail", new[] { "CommodityID" });
            DropIndex("dbo.InKindContributionDetail", new[] { "ContributionID" });
            DropIndex("dbo.ContributionDetail", new[] { "ContributionID" });
            DropIndex("dbo.ContributionDetail", new[] { "CurrencyID" });
            DropIndex("dbo.Contribution", new[] { "HRDID" });
            DropIndex("dbo.Contribution", new[] { "DonorID" });
            DropIndex("dbo.Plan", new[] { "ProgramID" });
            DropIndex("dbo.HRDDetail", new[] { "AdminUnitID" });
            DropIndex("dbo.HRDDetail", new[] { "HRDID" });
            DropIndex("dbo.HRD", new[] { "PlanID" });
            DropIndex("dbo.HRD", new[] { "SeasonID" });
            DropIndex("dbo.HRD", new[] { "CreatedBy" });
            DropIndex("dbo.HRD", new[] { "RationID" });
            DropIndex("dbo.HRD", new[] { "TransactionGroupID" });
            DropIndex("dbo.WoredaStockDistributionDetail", new[] { "WoredaStockDistributionID" });
            DropIndex("dbo.WoredaStockDistributionDetail", new[] { "FDPID" });
            DropIndex("dbo.WoredaStcokDistribution", new[] { "WoredaID" });
            DropIndex("dbo.WoredaStcokDistribution", new[] { "DistributedBy" });
            DropIndex("dbo.DonationPlanDetail", new[] { "HubID" });
            DropIndex("dbo.DonationPlanDetail", new[] { "DonationHeaderPlanID" });
            DropIndex("dbo.DonationPlanHeader", new[] { "EnteredBy" });
            DropIndex("dbo.DonationPlanHeader", new[] { "ProgramID" });
            DropIndex("dbo.DonationPlanHeader", new[] { "DonorID" });
            DropIndex("dbo.DonationPlanHeader", new[] { "ShippingInstructionId" });
            DropIndex("dbo.DonationPlanHeader", new[] { "CommodityTypeID" });
            DropIndex("dbo.DonationPlanHeader", new[] { "CommodityID" });
            DropIndex("dbo.TransporterCheque", new[] { "BusinessProcessID" });
            DropIndex("dbo.TransporterCheque", new[] { "AppovedBy" });
            DropIndex("dbo.TransporterCheque", new[] { "PreparedBy" });
            DropIndex("dbo.PaymentRequest", new[] { "BusinessProcessID" });
            DropIndex("dbo.PaymentRequest", new[] { "TransportOrderID" });
            DropIndex("dbo.BusinessProcessState", new[] { "ParentBusinessProcessID" });
            DropIndex("dbo.BusinessProcessState", new[] { "StateID" });
            DropIndex("dbo.FlowTemplate", new[] { "FinalStateID" });
            DropIndex("dbo.FlowTemplate", new[] { "InitialStateID" });
            DropIndex("dbo.FlowTemplate", new[] { "ParentProcessTemplateID" });
            DropIndex("dbo.StateTemplate", new[] { "ParentProcessTemplateID" });
            DropIndex("dbo.BusinessProcess", new[] { "CurrentStateID" });
            DropIndex("dbo.BusinessProcess", new[] { "ProcessTypeID" });
            DropIndex("dbo.TransporterPaymentRequest", new[] { "TransportOrderID" });
            DropIndex("dbo.TransporterPaymentRequest", new[] { "DeliveryID" });
            DropIndex("dbo.TransporterPaymentRequest", new[] { "BusinessProcessID" });
            DropIndex("dbo.Delivery", new[] { "HubID" });
            DropIndex("dbo.Delivery", new[] { "FDPID" });
            DropIndex("dbo.Delivery", new[] { "DonorID" });
            DropIndex("dbo.DeliveryDetail", new[] { "UnitID" });
            DropIndex("dbo.DeliveryDetail", new[] { "DeliveryID" });
            DropIndex("dbo.DeliveryDetail", new[] { "CommodityID" });
            DropIndex("dbo.RationDetail", new[] { "UnitID" });
            DropIndex("dbo.RationDetail", new[] { "RationID" });
            DropIndex("dbo.RationDetail", new[] { "CommodityID" });
            DropIndex("dbo.RegionalPSNPPlan", new[] { "TransactionGroupID" });
            DropIndex("dbo.RegionalPSNPPlan", new[] { "StatusID" });
            DropIndex("dbo.RegionalPSNPPlan", new[] { "RationID" });
            DropIndex("dbo.RegionalPSNPPlan", new[] { "PlanId" });
            DropIndex("dbo.DeliveryReconcile", new[] { "TransactionGroupID" });
            DropIndex("dbo.DeliveryReconcile", new[] { "HubID" });
            DropIndex("dbo.DeliveryReconcile", new[] { "FDPID" });
            DropIndex("dbo.DeliveryReconcile", new[] { "DispatchID" });
            DropIndex("dbo.InternalMovement", new[] { "TransactionGroupID" });
            DropIndex("dbo.InternalMovement", new[] { "Detail_DetailID" });
            DropIndex("dbo.SIPCAllocation", new[] { "TransactionGroupID" });
            DropIndex("dbo.SIPCAllocation", new[] { "RequisitionDetailID" });
            DropIndex("dbo.TransReqWithoutTransporter", new[] { "RequisitionDetailID" });
            DropIndex("dbo.TransReqWithoutTransporter", new[] { "TransportRequisitionID" });
            DropIndex("dbo.OtherDispatchAllocation", new[] { "TransporterID" });
            DropIndex("dbo.OtherDispatchAllocation", new[] { "ShippingInstructionID" });
            DropIndex("dbo.OtherDispatchAllocation", new[] { "ProjectCodeID" });
            DropIndex("dbo.OtherDispatchAllocation", new[] { "ProgramID" });
            DropIndex("dbo.OtherDispatchAllocation", new[] { "Hub1_HubID" });
            DropIndex("dbo.OtherDispatchAllocation", new[] { "HubID" });
            DropIndex("dbo.OtherDispatchAllocation", new[] { "CommodityID" });
            DropIndex("dbo.Transaction", new[] { "CommodityGradeID" });
            DropIndex("dbo.Transaction", new[] { "HubID" });
            DropIndex("dbo.Transaction", new[] { "StoreID" });
            DropIndex("dbo.Transaction", new[] { "HubOwnerID" });
            DropIndex("dbo.Transaction", new[] { "UnitID" });
            DropIndex("dbo.Transaction", new[] { "TransactionGroupID" });
            DropIndex("dbo.Transaction", new[] { "ShippingInstructionID" });
            DropIndex("dbo.Transaction", new[] { "ProjectCodeID" });
            DropIndex("dbo.Transaction", new[] { "ProgramID" });
            DropIndex("dbo.Hub", new[] { "HubOwnerID" });
            DropIndex("dbo.ReceiptAllocation", new[] { "GiftCertificateDetailID" });
            DropIndex("dbo.ReceiptAllocation", new[] { "CommoditySourceID" });
            DropIndex("dbo.ReceiptAllocation", new[] { "UnitID" });
            DropIndex("dbo.ReceiptAllocation", new[] { "ProgramID" });
            DropIndex("dbo.ReceiptAllocation", new[] { "SourceHubID" });
            DropIndex("dbo.ReceiptAllocation", new[] { "HubID" });
            DropIndex("dbo.ReceiptAllocation", new[] { "DonorID" });
            DropIndex("dbo.ReceiptAllocation", new[] { "CommodityID" });
            DropIndex("dbo.GiftCertificateDetail", new[] { "GiftCertificateID" });
            DropIndex("dbo.GiftCertificateDetail", new[] { "DBudgetTypeID" });
            DropIndex("dbo.GiftCertificateDetail", new[] { "DCurrencyID" });
            DropIndex("dbo.GiftCertificateDetail", new[] { "DFundSourceID" });
            DropIndex("dbo.GiftCertificateDetail", new[] { "CommodityID" });
            DropIndex("dbo.Detail", new[] { "MasterID" });
            DropIndex("dbo.GiftCertificate", new[] { "ShippingInstructionID" });
            DropIndex("dbo.GiftCertificate", new[] { "ProgramID" });
            DropIndex("dbo.GiftCertificate", new[] { "DonorID" });
            DropIndex("dbo.GiftCertificate", new[] { "DModeOfTransport" });
            DropIndex("dbo.AdminUnit", new[] { "AdminUnitTypeID" });
            DropIndex("dbo.AdminUnit", new[] { "ParentID" });
            DropIndex("dbo.FDP", new[] { "AdminUnitID" });
            DropIndex("dbo.Dispatch", new[] { "TransporterID" });
            DropIndex("dbo.Dispatch", new[] { "OtherDispatchAllocationID" });
            DropIndex("dbo.Dispatch", new[] { "HubID" });
            DropIndex("dbo.Dispatch", new[] { "FDPID" });
            DropIndex("dbo.Dispatch", new[] { "DispatchAllocationID" });
            DropIndex("dbo.DispatchAllocation", new[] { "CommodityID" });
            DropIndex("dbo.DispatchAllocation", new[] { "FDPID" });
            DropIndex("dbo.DispatchAllocation", new[] { "HubID" });
            DropIndex("dbo.DispatchAllocation", new[] { "ProgramID" });
            DropIndex("dbo.Commodity", new[] { "CommodityTypeID" });
            DropIndex("dbo.Commodity", new[] { "ParentID" });
            DropIndex("dbo.RegionalPSNPPledges", new[] { "UnitID" });
            DropIndex("dbo.RegionalPSNPPledges", new[] { "RegionalPSNPPlanID" });
            DropIndex("dbo.RegionalPSNPPledges", new[] { "DonorID" });
            DropIndex("dbo.RegionalPSNPPledges", new[] { "CommodityID" });
            DropIndex("dbo.UserDashboardPreference", new[] { "UserID" });
            DropIndex("dbo.UserDashboardPreference", new[] { "DashboardWidgetID" });
            DropForeignKey("EarlyWarning.HRDDonorCoverageDetail", "HRDDonorCoverageID", "EarlyWarning.HRDDonorCoverage");
            DropForeignKey("EarlyWarning.HRDDonorCoverageDetail", "WoredaID", "dbo.AdminUnit");
            DropForeignKey("EarlyWarning.HRDDonorCoverage", "HRDID", "dbo.HRD");
            DropForeignKey("EarlyWarning.HRDDonorCoverage", "DonorID", "dbo.Donor");
            DropForeignKey("EarlyWarning.NeedAssessmentDetail", "NeedAId", "EarlyWarning.NeedAssessmentHeader");
            DropForeignKey("EarlyWarning.NeedAssessmentDetail", "Woreda", "dbo.AdminUnit");
            DropForeignKey("EarlyWarning.NeedAssessmentHeader", "NeedAID", "EarlyWarning.NeedAssessment");
            DropForeignKey("EarlyWarning.NeedAssessmentHeader", "Zone", "dbo.AdminUnit");
            DropForeignKey("EarlyWarning.NeedAssessment", "PlanID", "dbo.Plan");
            DropForeignKey("EarlyWarning.NeedAssessment", "TypeOfNeedAssessment", "EarlyWarning.TypeOfNeedAssessment");
            DropForeignKey("EarlyWarning.NeedAssessment", "Season", "dbo.Season");
            DropForeignKey("EarlyWarning.NeedAssessment", "NeedAApprovedBy", "dbo.UserProfile");
            DropForeignKey("EarlyWarning.NeedAssessment", "NeddACreatedBy", "dbo.UserProfile");
            DropForeignKey("EarlyWarning.NeedAssessment", "Region", "dbo.AdminUnit");
            DropForeignKey("EarlyWarning.ReliefRequisitionDetail", "RequisitionID", "EarlyWarning.ReliefRequisition");
            DropForeignKey("EarlyWarning.ReliefRequisitionDetail", "FDPID", "dbo.FDP");
            DropForeignKey("EarlyWarning.ReliefRequisitionDetail", "DonorID", "dbo.Donor");
            DropForeignKey("EarlyWarning.ReliefRequisitionDetail", "CommodityID", "dbo.Commodity");
            DropForeignKey("EarlyWarning.ReliefRequisition", "RegionalRequestID", "EarlyWarning.RegionalRequest");
            DropForeignKey("EarlyWarning.ReliefRequisition", "ProgramID", "dbo.Program");
            DropForeignKey("EarlyWarning.ReliefRequisition", "CommodityID", "dbo.Commodity");
            DropForeignKey("EarlyWarning.ReliefRequisition", "ZoneID", "dbo.AdminUnit");
            DropForeignKey("EarlyWarning.ReliefRequisition", "RegionID", "dbo.AdminUnit");
            DropForeignKey("EarlyWarning.RequestDetailCommodity", "RegionalRequestDetailID", "EarlyWarning.RegionalRequestDetail");
            DropForeignKey("EarlyWarning.RequestDetailCommodity", "CommodityID", "dbo.Commodity");
            DropForeignKey("EarlyWarning.RegionalRequestDetail", "FDPID", "dbo.FDP");
            DropForeignKey("EarlyWarning.RegionalRequestDetail", "RegionalRequestID", "EarlyWarning.RegionalRequest");
            DropForeignKey("EarlyWarning.RegionalRequest", "PlanID", "dbo.Plan");
            DropForeignKey("EarlyWarning.RegionalRequest", "DonorID", "dbo.Donor");
            DropForeignKey("EarlyWarning.RegionalRequest", "ProgramID", "dbo.Program");
            DropForeignKey("EarlyWarning.RegionalRequest", "RegionID", "dbo.AdminUnit");
            DropForeignKey("EarlyWarning.RegionalRequest", "ApprovedBy", "dbo.UserProfile");
            DropForeignKey("EarlyWarning.RegionalRequest", "RequestedBy", "dbo.UserProfile");
            DropForeignKey("EarlyWarning.RegionalRequest", "RationID", "dbo.Ration");
            DropForeignKey("Logistics.TransportRequisition", "ProgramID", "dbo.Program");
            DropForeignKey("Logistics.TransportRequisition", "RegionID", "dbo.AdminUnit");
            DropForeignKey("Logistics.TransportRequisitionDetail", "TransportRequisitionID", "Logistics.TransportRequisition");
            DropForeignKey("Logistics.TransportRequisitionDetail", "RequisitionID", "EarlyWarning.ReliefRequisition");
            DropForeignKey("Logistics.HubAllocation", "RequisitionID", "EarlyWarning.ReliefRequisition");
            DropForeignKey("Logistics.HubAllocation", "HubID", "dbo.Hub");
            DropForeignKey("Logistics.ProjectCodeAllocation", "SINumberID", "dbo.ShippingInstruction");
            DropForeignKey("Logistics.ProjectCodeAllocation", "HubAllocationID", "Logistics.HubAllocation");
            DropForeignKey("Logistics.ProjectCodeAllocation", "ProjectCodeID", "dbo.ProjectCode");
            DropForeignKey("Procurement.TransportBidQuotationHeader", "TransporterId", "Procurement.Transporter");
            DropForeignKey("Procurement.TransportBidQuotationHeader", "BidId", "procurement.Bid");
            DropForeignKey("Procurement.TransportBidQuotationHeader", "RegionID", "dbo.AdminUnit");
            DropForeignKey("Procurement.TransportBidQuotation", "BidID", "procurement.Bid");
            DropForeignKey("Procurement.TransportBidQuotation", "TransporterID", "Procurement.Transporter");
            DropForeignKey("Procurement.TransportBidQuotation", "TransportBidQuotationHeaderID", "Procurement.TransportBidQuotationHeader");
            DropForeignKey("Procurement.TransportBidQuotation", "SourceID", "dbo.Hub");
            DropForeignKey("Procurement.TransportBidQuotation", "DestinationID", "dbo.AdminUnit");
            DropForeignKey("Procurement.TransportBidPlan", "ProgramID", "dbo.Program");
            DropForeignKey("Procurement.TransportBidPlanDetail", "ProgramID", "dbo.Program");
            DropForeignKey("Procurement.TransportBidPlanDetail", "SourceID", "dbo.Hub");
            DropForeignKey("Procurement.TransportBidPlanDetail", "DestinationID", "dbo.AdminUnit");
            DropForeignKey("Procurement.TransportBidPlanDetail", "BidPlanID", "Procurement.TransportBidPlan");
            DropForeignKey("Procurement.TransportOrderDetail", "TransportOrderID", "Procurement.TransportOrder");
            DropForeignKey("Procurement.TransportOrderDetail", "RequisitionID", "EarlyWarning.ReliefRequisition");
            DropForeignKey("Procurement.TransportOrderDetail", "SourceWarehouseID", "dbo.Hub");
            DropForeignKey("Procurement.TransportOrderDetail", "FdpID", "dbo.FDP");
            DropForeignKey("Procurement.TransportOrderDetail", "DonorID", "dbo.Donor");
            DropForeignKey("Procurement.TransportOrderDetail", "CommodityID", "dbo.Commodity");
            DropForeignKey("Procurement.TransportOrderDetail", "ZoneID", "dbo.AdminUnit");
            DropForeignKey("Procurement.TransportOrder", "TransporterID", "Procurement.Transporter");
            DropForeignKey("Procurement.BidWinner", "BusinessProcessID", "dbo.BusinessProcess");
            DropForeignKey("Procurement.BidWinner", "CommodityID", "dbo.Commodity");
            DropForeignKey("Procurement.BidWinner", "SourceID", "dbo.Hub");
            DropForeignKey("Procurement.BidWinner", "DestinationID", "dbo.AdminUnit");
            DropForeignKey("Procurement.BidWinner", "TransporterID", "Procurement.Transporter");
            DropForeignKey("Procurement.BidWinner", "BidID", "procurement.Bid");
            DropForeignKey("Procurement.BidDetail", "BidID", "procurement.Bid");
            DropForeignKey("procurement.Bid", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("procurement.Bid", "TransportBidPlanID", "Procurement.TransportBidPlan");
            DropForeignKey("procurement.Bid", "StatusID", "Procurement.Status");
            DropForeignKey("procurement.Bid", "RegionID", "dbo.AdminUnit");
            DropForeignKey("dbo.Template", "TemplateType", "dbo.TemplateType");
            DropForeignKey("dbo.UserHub", "UserProfileID", "dbo.UserProfile");
            DropForeignKey("dbo.UserHub", "HubID", "dbo.Hub");
            DropForeignKey("dbo.WorkflowStatus", "WorkflowID", "dbo.Workflow");
            DropForeignKey("dbo.DispatchAllocationDetail", "Fdpid", "dbo.FDP");
            DropForeignKey("dbo.DispatchAllocationDetail", "ReliefRequistion_RequisitionID", "EarlyWarning.ReliefRequisition");
            DropForeignKey("dbo.HRDCommodityDetail", "CommodityID", "dbo.Commodity");
            DropForeignKey("dbo.DistributionByAgeDetail", "FDPID", "dbo.FDP");
            DropForeignKey("dbo.DistributionByAgeDetail", "WoredaStockDistribution_WoredaStockDistributionID", "dbo.WoredaStcokDistribution");
            DropForeignKey("dbo.Contact", "FDPID", "dbo.FDP");
            DropForeignKey("dbo.TransporterAgreementVersion", "TransporterID", "Procurement.Transporter");
            DropForeignKey("dbo.TransporterAgreementVersion", "BidID", "procurement.Bid");
            DropForeignKey("dbo.ReceiptPlan", "GiftCertificateDetailId", "dbo.GiftCertificateDetail");
            DropForeignKey("dbo.ReceiptPlanDetail", "ReceiptHeaderId", "dbo.ReceiptPlan");
            DropForeignKey("dbo.ReceiptPlanDetail", "HubId", "dbo.Hub");
            DropForeignKey("dbo.WoredaHubLink", "WoredaHubID", "dbo.WoredaHub");
            DropForeignKey("dbo.WoredaHubLink", "HubID", "dbo.Hub");
            DropForeignKey("dbo.WoredaHubLink", "WoredaID", "dbo.AdminUnit");
            DropForeignKey("dbo.PromisedContribution", "HubId", "dbo.Hub");
            DropForeignKey("dbo.PromisedContribution", "DonorId", "dbo.Donor");
            DropForeignKey("dbo.PromisedContribution", "CommodityId", "dbo.Commodity");
            DropForeignKey("dbo.Store", "HubID", "dbo.Hub");
            DropForeignKey("dbo.LocalPurchaseDetail", "HubID", "dbo.Hub");
            DropForeignKey("dbo.LocalPurchaseDetail", "LocalPurchaseID", "dbo.LocalPurchase");
            DropForeignKey("dbo.LocalPurchase", "ProgramID", "dbo.Program");
            DropForeignKey("dbo.LocalPurchase", "DonorID", "dbo.Donor");
            DropForeignKey("dbo.LocalPurchase", "CommodityID", "dbo.Commodity");
            DropForeignKey("dbo.LocalPurchase", "ShippingInstructionID", "dbo.ShippingInstruction");
            DropForeignKey("dbo.LocalPurchase", "GiftCertificateID", "dbo.GiftCertificate");
            DropForeignKey("dbo.RegionalPSNPPlanDetail", "PlanedWoredaID", "dbo.AdminUnit");
            DropForeignKey("dbo.RegionalPSNPPlanDetail", "RegionalPSNPPlanID", "dbo.RegionalPSNPPlan");
            DropForeignKey("dbo.DispatchDetail", "TransactionGroupID", "dbo.TransactionGroup");
            DropForeignKey("dbo.DispatchDetail", "UnitID", "dbo.Unit");
            DropForeignKey("dbo.DispatchDetail", "DispatchID", "dbo.Dispatch");
            DropForeignKey("dbo.DispatchDetail", "CommodityID", "dbo.Commodity");
            DropForeignKey("dbo.TransporterChequeDetail", "TransporterPaymentRequestID", "dbo.TransporterPaymentRequest");
            DropForeignKey("dbo.TransporterChequeDetail", "TransporterChequeID", "dbo.TransporterCheque");
            DropForeignKey("dbo.Transfer", "ShippingInstructionID", "dbo.ShippingInstruction");
            DropForeignKey("dbo.Transfer", "ProgramID", "dbo.Program");
            DropForeignKey("dbo.Transfer", "DestinationSwap", "dbo.Hub");
            DropForeignKey("dbo.Transfer", "SourceSwap", "dbo.Hub");
            DropForeignKey("dbo.Transfer", "DestinationHubID", "dbo.Hub");
            DropForeignKey("dbo.Transfer", "SourceHubID", "dbo.Hub");
            DropForeignKey("dbo.Transfer", "CommoditySourceID", "dbo.CommoditySource");
            DropForeignKey("dbo.Transfer", "CommodityID", "dbo.Commodity");
            DropForeignKey("dbo.LoanReciptPlan", "ShippingInstructionID", "dbo.ShippingInstruction");
            DropForeignKey("dbo.LoanReciptPlan", "CommodityID", "dbo.Commodity");
            DropForeignKey("dbo.LoanReciptPlan", "CommoditySourceID", "dbo.CommoditySource");
            DropForeignKey("dbo.LoanReciptPlan", "ProgramID", "dbo.Program");
            DropForeignKey("dbo.LoanReciptPlanDetail", "ApprovedBy", "dbo.UserProfile");
            DropForeignKey("dbo.LoanReciptPlanDetail", "HubID", "dbo.Hub");
            DropForeignKey("dbo.LoanReciptPlanDetail", "LoanReciptPlanID", "dbo.LoanReciptPlan");
            DropForeignKey("dbo.InKindContributionDetail", "CommodityID", "dbo.Commodity");
            DropForeignKey("dbo.InKindContributionDetail", "ContributionID", "dbo.Contribution");
            DropForeignKey("dbo.ContributionDetail", "ContributionID", "dbo.Contribution");
            DropForeignKey("dbo.ContributionDetail", "CurrencyID", "dbo.Currency");
            DropForeignKey("dbo.Contribution", "HRDID", "dbo.HRD");
            DropForeignKey("dbo.Contribution", "DonorID", "dbo.Donor");
            DropForeignKey("dbo.Plan", "ProgramID", "dbo.Program");
            DropForeignKey("dbo.HRDDetail", "AdminUnitID", "dbo.AdminUnit");
            DropForeignKey("dbo.HRDDetail", "HRDID", "dbo.HRD");
            DropForeignKey("dbo.HRD", "PlanID", "dbo.Plan");
            DropForeignKey("dbo.HRD", "SeasonID", "dbo.Season");
            DropForeignKey("dbo.HRD", "CreatedBy", "dbo.UserProfile");
            DropForeignKey("dbo.HRD", "RationID", "dbo.Ration");
            DropForeignKey("dbo.HRD", "TransactionGroupID", "dbo.TransactionGroup");
            DropForeignKey("dbo.WoredaStockDistributionDetail", "WoredaStockDistributionID", "dbo.WoredaStcokDistribution");
            DropForeignKey("dbo.WoredaStockDistributionDetail", "FDPID", "dbo.FDP");
            DropForeignKey("dbo.WoredaStcokDistribution", "WoredaID", "dbo.AdminUnit");
            DropForeignKey("dbo.WoredaStcokDistribution", "DistributedBy", "dbo.UserProfile");
            DropForeignKey("dbo.DonationPlanDetail", "HubID", "dbo.Hub");
            DropForeignKey("dbo.DonationPlanDetail", "DonationHeaderPlanID", "dbo.DonationPlanHeader");
            DropForeignKey("dbo.DonationPlanHeader", "EnteredBy", "dbo.UserProfile");
            DropForeignKey("dbo.DonationPlanHeader", "ProgramID", "dbo.Program");
            DropForeignKey("dbo.DonationPlanHeader", "DonorID", "dbo.Donor");
            DropForeignKey("dbo.DonationPlanHeader", "ShippingInstructionId", "dbo.ShippingInstruction");
            DropForeignKey("dbo.DonationPlanHeader", "CommodityTypeID", "dbo.CommodityType");
            DropForeignKey("dbo.DonationPlanHeader", "CommodityID", "dbo.Commodity");
            DropForeignKey("dbo.TransporterCheque", "BusinessProcessID", "dbo.BusinessProcess");
            DropForeignKey("dbo.TransporterCheque", "AppovedBy", "dbo.UserProfile");
            DropForeignKey("dbo.TransporterCheque", "PreparedBy", "dbo.UserProfile");
            DropForeignKey("dbo.PaymentRequest", "BusinessProcessID", "dbo.BusinessProcess");
            DropForeignKey("dbo.PaymentRequest", "TransportOrderID", "Procurement.TransportOrder");
            DropForeignKey("dbo.BusinessProcessState", "ParentBusinessProcessID", "dbo.BusinessProcess");
            DropForeignKey("dbo.BusinessProcessState", "StateID", "dbo.StateTemplate");
            DropForeignKey("dbo.FlowTemplate", "FinalStateID", "dbo.StateTemplate");
            DropForeignKey("dbo.FlowTemplate", "InitialStateID", "dbo.StateTemplate");
            DropForeignKey("dbo.FlowTemplate", "ParentProcessTemplateID", "dbo.ProcessTemplate");
            DropForeignKey("dbo.StateTemplate", "ParentProcessTemplateID", "dbo.ProcessTemplate");
            DropForeignKey("dbo.BusinessProcess", "CurrentStateID", "dbo.BusinessProcessState");
            DropForeignKey("dbo.BusinessProcess", "ProcessTypeID", "dbo.ProcessTemplate");
            DropForeignKey("dbo.TransporterPaymentRequest", "TransportOrderID", "Procurement.TransportOrder");
            DropForeignKey("dbo.TransporterPaymentRequest", "DeliveryID", "dbo.Delivery");
            DropForeignKey("dbo.TransporterPaymentRequest", "BusinessProcessID", "dbo.BusinessProcess");
            DropForeignKey("dbo.Delivery", "HubID", "dbo.Hub");
            DropForeignKey("dbo.Delivery", "FDPID", "dbo.FDP");
            DropForeignKey("dbo.Delivery", "DonorID", "dbo.Donor");
            DropForeignKey("dbo.DeliveryDetail", "UnitID", "dbo.Unit");
            DropForeignKey("dbo.DeliveryDetail", "DeliveryID", "dbo.Delivery");
            DropForeignKey("dbo.DeliveryDetail", "CommodityID", "dbo.Commodity");
            DropForeignKey("dbo.RationDetail", "UnitID", "dbo.Unit");
            DropForeignKey("dbo.RationDetail", "RationID", "dbo.Ration");
            DropForeignKey("dbo.RationDetail", "CommodityID", "dbo.Commodity");
            DropForeignKey("dbo.RegionalPSNPPlan", "TransactionGroupID", "dbo.TransactionGroup");
            DropForeignKey("dbo.RegionalPSNPPlan", "StatusID", "dbo.BusinessProcess");
            DropForeignKey("dbo.RegionalPSNPPlan", "RationID", "dbo.Ration");
            DropForeignKey("dbo.RegionalPSNPPlan", "PlanId", "dbo.Plan");
            DropForeignKey("dbo.DeliveryReconcile", "TransactionGroupID", "dbo.TransactionGroup");
            DropForeignKey("dbo.DeliveryReconcile", "HubID", "dbo.Hub");
            DropForeignKey("dbo.DeliveryReconcile", "FDPID", "dbo.FDP");
            DropForeignKey("dbo.DeliveryReconcile", "DispatchID", "dbo.Dispatch");
            DropForeignKey("dbo.InternalMovement", "TransactionGroupID", "dbo.TransactionGroup");
            DropForeignKey("dbo.InternalMovement", "Detail_DetailID", "dbo.Detail");
            DropForeignKey("dbo.SIPCAllocation", "TransactionGroupID", "dbo.TransactionGroup");
            DropForeignKey("dbo.SIPCAllocation", "RequisitionDetailID", "EarlyWarning.ReliefRequisitionDetail");
            DropForeignKey("dbo.TransReqWithoutTransporter", "RequisitionDetailID", "EarlyWarning.ReliefRequisitionDetail");
            DropForeignKey("dbo.TransReqWithoutTransporter", "TransportRequisitionID", "Logistics.TransportRequisitionDetail");
            DropForeignKey("dbo.OtherDispatchAllocation", "TransporterID", "Procurement.Transporter");
            DropForeignKey("dbo.OtherDispatchAllocation", "ShippingInstructionID", "dbo.ShippingInstruction");
            DropForeignKey("dbo.OtherDispatchAllocation", "ProjectCodeID", "dbo.ProjectCode");
            DropForeignKey("dbo.OtherDispatchAllocation", "ProgramID", "dbo.Program");
            DropForeignKey("dbo.OtherDispatchAllocation", "Hub1_HubID", "dbo.Hub");
            DropForeignKey("dbo.OtherDispatchAllocation", "HubID", "dbo.Hub");
            DropForeignKey("dbo.OtherDispatchAllocation", "CommodityID", "dbo.Commodity");
            DropForeignKey("dbo.Transaction", "CommodityGradeID", "dbo.CommodityGrade");
            DropForeignKey("dbo.Transaction", "HubID", "dbo.Hub");
            DropForeignKey("dbo.Transaction", "StoreID", "dbo.Store");
            DropForeignKey("dbo.Transaction", "HubOwnerID", "dbo.HubOwner");
            DropForeignKey("dbo.Transaction", "UnitID", "dbo.Unit");
            DropForeignKey("dbo.Transaction", "TransactionGroupID", "dbo.TransactionGroup");
            DropForeignKey("dbo.Transaction", "ShippingInstructionID", "dbo.ShippingInstruction");
            DropForeignKey("dbo.Transaction", "ProjectCodeID", "dbo.ProjectCode");
            DropForeignKey("dbo.Transaction", "ProgramID", "dbo.Program");
            DropForeignKey("dbo.Hub", "HubOwnerID", "dbo.HubOwner");
            DropForeignKey("dbo.ReceiptAllocation", "GiftCertificateDetailID", "dbo.GiftCertificateDetail");
            DropForeignKey("dbo.ReceiptAllocation", "CommoditySourceID", "dbo.CommoditySource");
            DropForeignKey("dbo.ReceiptAllocation", "UnitID", "dbo.Unit");
            DropForeignKey("dbo.ReceiptAllocation", "ProgramID", "dbo.Program");
            DropForeignKey("dbo.ReceiptAllocation", "SourceHubID", "dbo.Hub");
            DropForeignKey("dbo.ReceiptAllocation", "HubID", "dbo.Hub");
            DropForeignKey("dbo.ReceiptAllocation", "DonorID", "dbo.Donor");
            DropForeignKey("dbo.ReceiptAllocation", "CommodityID", "dbo.Commodity");
            DropForeignKey("dbo.GiftCertificateDetail", "GiftCertificateID", "dbo.GiftCertificate");
            DropForeignKey("dbo.GiftCertificateDetail", "DBudgetTypeID", "dbo.Detail");
            DropForeignKey("dbo.GiftCertificateDetail", "DCurrencyID", "dbo.Detail");
            DropForeignKey("dbo.GiftCertificateDetail", "DFundSourceID", "dbo.Detail");
            DropForeignKey("dbo.GiftCertificateDetail", "CommodityID", "dbo.Commodity");
            DropForeignKey("dbo.Detail", "MasterID", "dbo.Master");
            DropForeignKey("dbo.GiftCertificate", "ShippingInstructionID", "dbo.ShippingInstruction");
            DropForeignKey("dbo.GiftCertificate", "ProgramID", "dbo.Program");
            DropForeignKey("dbo.GiftCertificate", "DonorID", "dbo.Donor");
            DropForeignKey("dbo.GiftCertificate", "DModeOfTransport", "dbo.Detail");
            DropForeignKey("dbo.AdminUnit", "AdminUnitTypeID", "dbo.AdminUnitType");
            DropForeignKey("dbo.AdminUnit", "ParentID", "dbo.AdminUnit");
            DropForeignKey("dbo.FDP", "AdminUnitID", "dbo.AdminUnit");
            DropForeignKey("dbo.Dispatch", "TransporterID", "Procurement.Transporter");
            DropForeignKey("dbo.Dispatch", "OtherDispatchAllocationID", "dbo.OtherDispatchAllocation");
            DropForeignKey("dbo.Dispatch", "HubID", "dbo.Hub");
            DropForeignKey("dbo.Dispatch", "FDPID", "dbo.FDP");
            DropForeignKey("dbo.Dispatch", "DispatchAllocationID", "dbo.DispatchAllocation");
            DropForeignKey("dbo.DispatchAllocation", "CommodityID", "dbo.Commodity");
            DropForeignKey("dbo.DispatchAllocation", "FDPID", "dbo.FDP");
            DropForeignKey("dbo.DispatchAllocation", "HubID", "dbo.Hub");
            DropForeignKey("dbo.DispatchAllocation", "ProgramID", "dbo.Program");
            DropForeignKey("dbo.Commodity", "CommodityTypeID", "dbo.CommodityType");
            DropForeignKey("dbo.Commodity", "ParentID", "dbo.Commodity");
            DropForeignKey("dbo.RegionalPSNPPledges", "UnitID", "dbo.Unit");
            DropForeignKey("dbo.RegionalPSNPPledges", "RegionalPSNPPlanID", "dbo.RegionalPSNPPlan");
            DropForeignKey("dbo.RegionalPSNPPledges", "DonorID", "dbo.Donor");
            DropForeignKey("dbo.RegionalPSNPPledges", "CommodityID", "dbo.Commodity");
            DropForeignKey("dbo.UserDashboardPreference", "UserID", "dbo.User");
            DropForeignKey("dbo.UserDashboardPreference", "DashboardWidgetID", "dbo.DashboardWidget");
            DropTable("EarlyWarning.HRDDonorCoverageDetail");
            DropTable("EarlyWarning.HRDDonorCoverage");
            DropTable("EarlyWarning.NeedAssessmentDetail");
            DropTable("EarlyWarning.NeedAssessmentHeader");
            DropTable("EarlyWarning.TypeOfNeedAssessment");
            DropTable("EarlyWarning.NeedAssessment");
            DropTable("EarlyWarning.ReliefRequisitionDetail");
            DropTable("EarlyWarning.ReliefRequisition");
            DropTable("EarlyWarning.RequestDetailCommodity");
            DropTable("EarlyWarning.RegionalRequestDetail");
            DropTable("EarlyWarning.RegionalRequest");
            DropTable("Logistics.TransportRequisition");
            DropTable("Logistics.TransportRequisitionDetail");
            DropTable("Logistics.HubAllocation");
            DropTable("Logistics.ProjectCodeAllocation");
            DropTable("Procurement.TransportBidQuotationHeader");
            DropTable("Procurement.TransportBidQuotation");
            DropTable("Procurement.TransportBidPlan");
            DropTable("Procurement.TransportBidPlanDetail");
            DropTable("Procurement.TransportOrderDetail");
            DropTable("Procurement.TransportOrder");
            DropTable("Procurement.Transporter");
            DropTable("Procurement.BidWinner");
            DropTable("Procurement.BidDetail");
            DropTable("Procurement.Status");
            DropTable("procurement.Bid");
            DropTable("dbo.TemplateFields");
            DropTable("dbo.Template");
            DropTable("dbo.TemplateType");
            DropTable("dbo.CommodityGrade");
            DropTable("dbo.RegionalRequestAllocation");
            DropTable("dbo.LossReason");
            DropTable("dbo.SupportType");
            DropTable("dbo.Actions");
            DropTable("dbo.DistibtionStatus");
            DropTable("dbo.IDPSReasonType");
            DropTable("dbo.Notification");
            DropTable("dbo.AllocationByRegion");
            DropTable("dbo.Audit");
            DropTable("dbo.UserHub");
            DropTable("dbo.LetterTemplate");
            DropTable("dbo.vw_NeedAssessment");
            DropTable("dbo.vwPSNPAnnualPlan");
            DropTable("dbo.ApplicationSetting");
            DropTable("dbo.WorkflowStatus");
            DropTable("dbo.Workflow");
            DropTable("dbo.vwTransportOrder");
            DropTable("dbo.DispatchAllocationDetail");
            DropTable("dbo.HRDCommodityDetail");
            DropTable("dbo.DistributionByAgeDetail");
            DropTable("dbo.Contact");
            DropTable("dbo.TransporterAgreementVersion");
            DropTable("dbo.ReceiptPlan");
            DropTable("dbo.ReceiptPlanDetail");
            DropTable("dbo.WoredaHub");
            DropTable("dbo.WoredaHubLink");
            DropTable("dbo.PromisedContribution");
            DropTable("dbo.Store");
            DropTable("dbo.LocalPurchaseDetail");
            DropTable("dbo.LocalPurchase");
            DropTable("dbo.RegionalPSNPPlanDetail");
            DropTable("dbo.DispatchDetail");
            DropTable("dbo.TransporterChequeDetail");
            DropTable("dbo.Transfer");
            DropTable("dbo.CommoditySource");
            DropTable("dbo.LoanReciptPlan");
            DropTable("dbo.LoanReciptPlanDetail");
            DropTable("dbo.InKindContributionDetail");
            DropTable("dbo.Currency");
            DropTable("dbo.ContributionDetail");
            DropTable("dbo.Contribution");
            DropTable("dbo.Plan");
            DropTable("dbo.Season");
            DropTable("dbo.HRDDetail");
            DropTable("dbo.HRD");
            DropTable("dbo.WoredaStockDistributionDetail");
            DropTable("dbo.WoredaStcokDistribution");
            DropTable("dbo.DonationPlanDetail");
            DropTable("dbo.CommodityType");
            DropTable("dbo.DonationPlanHeader");
            DropTable("dbo.UserProfile");
            DropTable("dbo.TransporterCheque");
            DropTable("dbo.PaymentRequest");
            DropTable("dbo.BusinessProcessState");
            DropTable("dbo.FlowTemplate");
            DropTable("dbo.StateTemplate");
            DropTable("dbo.ProcessTemplate");
            DropTable("dbo.BusinessProcess");
            DropTable("dbo.TransporterPaymentRequest");
            DropTable("dbo.Delivery");
            DropTable("dbo.DeliveryDetail");
            DropTable("dbo.Unit");
            DropTable("dbo.RationDetail");
            DropTable("dbo.Ration");
            DropTable("dbo.RegionalPSNPPlan");
            DropTable("dbo.DeliveryReconcile");
            DropTable("dbo.InternalMovement");
            DropTable("dbo.TransactionGroup");
            DropTable("dbo.SIPCAllocation");
            DropTable("dbo.TransReqWithoutTransporter");
            DropTable("dbo.Program");
            DropTable("dbo.OtherDispatchAllocation");
            DropTable("dbo.ShippingInstruction");
            DropTable("dbo.ProjectCode");
            DropTable("dbo.Transaction");
            DropTable("dbo.HubOwner");
            DropTable("dbo.Hub");
            DropTable("dbo.ReceiptAllocation");
            DropTable("dbo.GiftCertificateDetail");
            DropTable("dbo.Master");
            DropTable("dbo.Detail");
            DropTable("dbo.GiftCertificate");
            DropTable("dbo.Donor");
            DropTable("dbo.AdminUnitType");
            DropTable("dbo.AdminUnit");
            DropTable("dbo.FDP");
            DropTable("dbo.Dispatch");
            DropTable("dbo.DispatchAllocation");
            DropTable("dbo.Commodity");
            DropTable("dbo.RegionalPSNPPledges");
            DropTable("dbo.Log");
            DropTable("dbo.User");
            DropTable("dbo.UserDashboardPreference");
            DropTable("dbo.DashboardWidget");
        }
    }
}
