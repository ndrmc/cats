BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DonationPlanHeader
	DROP CONSTRAINT FK_DonationPlanHeader_CommodityType
GO
ALTER TABLE dbo.CommodityType SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DonationPlanHeader
	DROP CONSTRAINT FK_DonationPlanHeader_ShippingInstruction
GO
ALTER TABLE dbo.ShippingInstruction SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DonationPlanHeader
	DROP CONSTRAINT FK_DonationPlanHeader_Program
GO
ALTER TABLE dbo.Program SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DonationPlanHeader
	DROP CONSTRAINT FK_DonationPlanHeader_Donor
GO
ALTER TABLE dbo.Donor SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DonationPlanHeader
	DROP CONSTRAINT FK_DonationPlanHeader_Commodity
GO
ALTER TABLE dbo.Commodity SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DonationPlanHeader
	DROP CONSTRAINT FK_DonationPlanHeader_UserProfile
GO
ALTER TABLE dbo.UserProfile SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DonationPlanHeader
	DROP CONSTRAINT DF_DonationPlanHeader_BusinessProcessID
GO
CREATE TABLE dbo.Tmp_DonationPlanHeader
	(
	DonationHeaderPlanID int NOT NULL IDENTITY (1, 1) NOT FOR REPLICATION,
	ShippingInstructionId int NOT NULL,
	GiftCertificateID int NULL,
	DonatedAmount decimal(18, 4) NULL,
	CommodityID int NOT NULL,
	CommodityTypeID int NOT NULL,
	DonorID int NOT NULL,
	ProgramID int NOT NULL,
	ETA datetime NOT NULL,
	Vessel nvarchar(50) NULL,
	ReferenceNo nvarchar(50) NULL,
	ModeOfTransport int NULL,
	TransactionGroupID uniqueidentifier NULL,
	IsCommited bit NULL,
	EnteredBy int NULL,
	AllocationDate datetime NULL,
	Remark nvarchar(500) NULL,
	Status int NULL,
	BusinessProcessID int NOT NULL,
	PartitionId int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_DonationPlanHeader SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_DonationPlanHeader ADD CONSTRAINT
	DF_DonationPlanHeader_BusinessProcessID DEFAULT ((0)) FOR BusinessProcessID
GO
SET IDENTITY_INSERT dbo.Tmp_DonationPlanHeader ON
GO
IF EXISTS(SELECT * FROM dbo.DonationPlanHeader)
	 EXEC('INSERT INTO dbo.Tmp_DonationPlanHeader (DonationHeaderPlanID, ShippingInstructionId, GiftCertificateID, DonatedAmount, CommodityID, CommodityTypeID, DonorID, ProgramID, ETA, Vessel, ReferenceNo, ModeOfTransport, TransactionGroupID, IsCommited, EnteredBy, AllocationDate, Remark, Status, BusinessProcessID, PartitionId)
		SELECT DonationHeaderPlanID, ShippingInstructionId, GiftCertificateID, DonatedAmount, CommodityID, CommodityTypeID, DonorID, ProgramID, ETA, Vessel, ReferenceNo, ModeOfTransport, TransactionGroupID, IsCommited, EnteredBy, AllocationDate, Remark, Status, BusinessProcessID, PartitionId FROM dbo.DonationPlanHeader WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_DonationPlanHeader OFF
GO
DROP TABLE dbo.DonationPlanHeader
GO
EXECUTE sp_rename N'dbo.Tmp_DonationPlanHeader', N'DonationPlanHeader', 'OBJECT' 
GO
ALTER TABLE dbo.DonationPlanHeader ADD CONSTRAINT
	PK_DonationPlanHeader PRIMARY KEY CLUSTERED 
	(
	DonationHeaderPlanID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.DonationPlanHeader ADD CONSTRAINT
	IX_DonationPlanHeader UNIQUE NONCLUSTERED 
	(
	ShippingInstructionId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.DonationPlanHeader ADD CONSTRAINT
	FK_DonationPlanHeader_UserProfile FOREIGN KEY
	(
	EnteredBy
	) REFERENCES dbo.UserProfile
	(
	UserProfileID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.DonationPlanHeader ADD CONSTRAINT
	FK_DonationPlanHeader_Commodity FOREIGN KEY
	(
	CommodityID
	) REFERENCES dbo.Commodity
	(
	CommodityID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.DonationPlanHeader ADD CONSTRAINT
	FK_DonationPlanHeader_Donor FOREIGN KEY
	(
	DonorID
	) REFERENCES dbo.Donor
	(
	DonorID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.DonationPlanHeader ADD CONSTRAINT
	FK_DonationPlanHeader_Program FOREIGN KEY
	(
	ProgramID
	) REFERENCES dbo.Program
	(
	ProgramID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.DonationPlanHeader ADD CONSTRAINT
	FK_DonationPlanHeader_ShippingInstruction FOREIGN KEY
	(
	ShippingInstructionId
	) REFERENCES dbo.ShippingInstruction
	(
	ShippingInstructionID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.DonationPlanHeader ADD CONSTRAINT
	FK_DonationPlanHeader_CommodityType FOREIGN KEY
	(
	CommodityTypeID
	) REFERENCES dbo.CommodityType
	(
	CommodityTypeID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT