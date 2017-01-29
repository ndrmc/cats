IF  NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[Adjustment]') 
         AND name = 'BusinessProcessID'
)
 

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
ALTER TABLE dbo.Adjustment ADD
	BusinessProcessID int NULL
GO
ALTER TABLE dbo.Adjustment ADD 
BusinessProcessID int NOT NULL CONSTRAINT DF_Adjustment_BusinessProcessID DEFAULT 0

GO
ALTER TABLE dbo.Adjustment SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
