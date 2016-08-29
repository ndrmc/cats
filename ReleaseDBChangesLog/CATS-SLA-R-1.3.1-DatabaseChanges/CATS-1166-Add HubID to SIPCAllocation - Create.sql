USE [CatsDRMFSS]
GO

ALTER TABLE [CatsDRMFSS].[dbo].[SIPCAllocation]
ADD [HubID] [int] NULL
GO
UPDATE [CatsDRMFSS].[dbo].[SIPCAllocation]
SET [CatsDRMFSS].[dbo].[SIPCAllocation].[HubID] = [CatsDRMFSS].[dbo].[Transaction].[HubID]
FROM [CatsDRMFSS].[dbo].[Transaction]
WHERE [CatsDRMFSS].[dbo].[Transaction].ShippingInstructionID = [CatsDRMFSS].[dbo].[SIPCAllocation].[Code] AND 
[CatsDRMFSS].[dbo].[Transaction].LedgerID IN (SELECT LedgerID FROM Ledger WHERE Name = 'Goods On Hand')
GO
