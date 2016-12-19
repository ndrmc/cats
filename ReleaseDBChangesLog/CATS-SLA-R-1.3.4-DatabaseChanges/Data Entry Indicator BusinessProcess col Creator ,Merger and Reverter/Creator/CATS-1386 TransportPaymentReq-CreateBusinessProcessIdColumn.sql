
 
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
ALTER TABLE dbo.TransporterPaymentRequest ADD
	BusinessProcessID int NOT NULL CONSTRAINT DF_TransporterPaymentRequest_BusinessProcessID DEFAULT 0
GO
ALTER TABLE dbo.TransporterPaymentRequest SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
