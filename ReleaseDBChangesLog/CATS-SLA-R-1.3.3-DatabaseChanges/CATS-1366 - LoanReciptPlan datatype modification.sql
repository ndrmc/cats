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
ALTER TABLE dbo.LoanReciptPlan
	DROP CONSTRAINT DF__LoanRecip__IsFal__42DEEF3D
GO
ALTER TABLE dbo.LoanReciptPlan
	DROP CONSTRAINT DF__LoanRecip__Busin__7F110B0D
GO
CREATE TABLE dbo.Tmp_LoanReciptPlan
	(
	LoanReciptPlanID int NOT NULL IDENTITY (1, 1) NOT FOR REPLICATION,
	ShippingInstructionID int NOT NULL,
	LoanSource nvarchar(50) NULL,
	ProgramID int NOT NULL,
	ProjectCode nvarchar(50) NOT NULL,
	CreatedDate datetime2(7) NOT NULL,
	ReferenceNumber nvarchar(50) NOT NULL,
	CommoditySourceID int NOT NULL,
	CommodityID int NOT NULL,
	Quantity decimal(18, 4) NOT NULL,
	StatusID int NOT NULL,
	Remark nvarchar(150) NULL,
	PartitionId int NULL,
	IsFalseGRN bit NOT NULL,
	BusinessProcessID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_LoanReciptPlan SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_LoanReciptPlan ADD CONSTRAINT
	DF__LoanRecip__IsFal__42DEEF3D DEFAULT ((0)) FOR IsFalseGRN
GO
ALTER TABLE dbo.Tmp_LoanReciptPlan ADD CONSTRAINT
	DF__LoanRecip__Busin__7F110B0D DEFAULT ((0)) FOR BusinessProcessID
GO
SET IDENTITY_INSERT dbo.Tmp_LoanReciptPlan ON
GO
IF EXISTS(SELECT * FROM dbo.LoanReciptPlan)
	 EXEC('INSERT INTO dbo.Tmp_LoanReciptPlan (LoanReciptPlanID, ShippingInstructionID, LoanSource, ProgramID, ProjectCode, CreatedDate, ReferenceNumber, CommoditySourceID, CommodityID, Quantity, StatusID, Remark, PartitionId, IsFalseGRN, BusinessProcessID)
		SELECT LoanReciptPlanID, ShippingInstructionID, LoanSource, ProgramID, ProjectCode, CreatedDate, ReferenceNumber, CommoditySourceID, CommodityID, Quantity, StatusID, Remark, PartitionId, IsFalseGRN, BusinessProcessID FROM dbo.LoanReciptPlan WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_LoanReciptPlan OFF
GO
ALTER TABLE dbo.LoanReciptPlanDetail
	DROP CONSTRAINT FK_LoanReciptPlanDetail_LoanReciptPlan
GO
DROP TABLE dbo.LoanReciptPlan
GO
EXECUTE sp_rename N'dbo.Tmp_LoanReciptPlan', N'LoanReciptPlan', 'OBJECT' 
GO
ALTER TABLE dbo.LoanReciptPlan ADD CONSTRAINT
	PK_LoanReciptPlan PRIMARY KEY CLUSTERED 
	(
	LoanReciptPlanID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER [REPPartition_LoanReciptPlan] ON dbo.LoanReciptPlan
        AFTER INSERT, UPDATE AS          
        BEGIN
           SET NOCOUNT ON
       UPDATE [dbo].[LoanReciptPlan]
       SET [PartitionId] = (SELECT [PartitionSettingId] FROM 
[dbo].[PartitionSetting] where [InstanceName] = @@SERVERNAME)
       WHERE [LoanReciptPlanID] IN 
            (SELECT [LoanReciptPlanID] from inserted)
        END
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.LoanReciptPlanDetail ADD CONSTRAINT
	FK_LoanReciptPlanDetail_LoanReciptPlan FOREIGN KEY
	(
	LoanReciptPlanID
	) REFERENCES dbo.LoanReciptPlan
	(
	LoanReciptPlanID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.LoanReciptPlanDetail SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
