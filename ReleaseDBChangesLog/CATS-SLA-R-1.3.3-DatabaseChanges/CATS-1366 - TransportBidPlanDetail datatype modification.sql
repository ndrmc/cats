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
CREATE TABLE Procurement.Tmp_TransportBidPlanDetail
	(
	TransportBidPlanDetailID int NOT NULL IDENTITY (1, 1) NOT FOR REPLICATION,
	DestinationID int NOT NULL,
	SourceID int NOT NULL,
	ProgramID int NOT NULL,
	BidPlanID int NOT NULL,
	Quantity decimal(18, 4) NULL,
	PartitionId int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE Procurement.Tmp_TransportBidPlanDetail SET (LOCK_ESCALATION = TABLE)
GO
DECLARE @v sql_variant 
SET @v = cast(N'Details the plan information by storing what is planned to be transported and in what quantity to where from which hub. ' as varchar(120))
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Procurement', N'TABLE', N'Tmp_TransportBidPlanDetail', NULL, NULL
GO
SET IDENTITY_INSERT Procurement.Tmp_TransportBidPlanDetail ON
GO
IF EXISTS(SELECT * FROM Procurement.TransportBidPlanDetail)
	 EXEC('INSERT INTO Procurement.Tmp_TransportBidPlanDetail (TransportBidPlanDetailID, DestinationID, SourceID, ProgramID, BidPlanID, Quantity, PartitionId)
		SELECT TransportBidPlanDetailID, DestinationID, SourceID, ProgramID, BidPlanID, Quantity, PartitionId FROM Procurement.TransportBidPlanDetail WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT Procurement.Tmp_TransportBidPlanDetail OFF
GO
DROP TABLE Procurement.TransportBidPlanDetail
GO
EXECUTE sp_rename N'Procurement.Tmp_TransportBidPlanDetail', N'TransportBidPlanDetail', 'OBJECT' 
GO
ALTER TABLE Procurement.TransportBidPlanDetail ADD CONSTRAINT
	PK_TransportBidPlanDetail PRIMARY KEY CLUSTERED 
	(
	TransportBidPlanDetailID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER [REPPartition_TransportBidPlanDetail] ON Procurement.TransportBidPlanDetail
        AFTER INSERT, UPDATE AS          
        BEGIN
           SET NOCOUNT ON
       UPDATE [Procurement].[TransportBidPlanDetail]
       SET [PartitionId] = (SELECT [PartitionSettingId] FROM 
[dbo].[PartitionSetting] where [InstanceName] = @@SERVERNAME)
       WHERE [TransportBidPlanDetailID] IN 
            (SELECT [TransportBidPlanDetailID] from inserted)
        END
GO
COMMIT