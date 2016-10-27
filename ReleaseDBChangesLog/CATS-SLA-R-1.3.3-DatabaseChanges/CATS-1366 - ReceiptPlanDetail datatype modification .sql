/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
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
ALTER TABLE dbo.ReceiptPlanDetail
	DROP CONSTRAINT FK_ReceiptPlanDetail_ReceiptPlan
GO
ALTER TABLE dbo.ReceiptPlan SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ReceiptPlanDetail
	DROP CONSTRAINT FK_ReceiptPlanDetail_Hub
GO
ALTER TABLE dbo.Hub SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ReceiptPlanDetail
	(
	ReceiptDetailId int NOT NULL IDENTITY (1, 1) NOT FOR REPLICATION,
	ReceiptHeaderId int NOT NULL,
	HubId int NOT NULL,
	Allocated decimal(18, 4) NOT NULL,
	Received decimal(18, 4) NULL,
	Balance decimal(18, 4) NULL,
	PartitionId int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ReceiptPlanDetail SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ReceiptPlanDetail ON
GO
IF EXISTS(SELECT * FROM dbo.ReceiptPlanDetail)
	 EXEC('INSERT INTO dbo.Tmp_ReceiptPlanDetail (ReceiptDetailId, ReceiptHeaderId, HubId, Allocated, Received, Balance, PartitionId)
		SELECT ReceiptDetailId, ReceiptHeaderId, HubId, Allocated, Received, Balance, PartitionId FROM dbo.ReceiptPlanDetail WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ReceiptPlanDetail OFF
GO
DROP TABLE dbo.ReceiptPlanDetail
GO
EXECUTE sp_rename N'dbo.Tmp_ReceiptPlanDetail', N'ReceiptPlanDetail', 'OBJECT' 
GO
ALTER TABLE dbo.ReceiptPlanDetail ADD CONSTRAINT
	PK_ReceiptPlanDetail PRIMARY KEY CLUSTERED 
	(
	ReceiptDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ReceiptPlanDetail ADD CONSTRAINT
	FK_ReceiptPlanDetail_Hub FOREIGN KEY
	(
	HubId
	) REFERENCES dbo.Hub
	(
	HubID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ReceiptPlanDetail ADD CONSTRAINT
	FK_ReceiptPlanDetail_ReceiptPlan FOREIGN KEY
	(
	ReceiptHeaderId
	) REFERENCES dbo.ReceiptPlan
	(
	ReceiptHeaderId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
