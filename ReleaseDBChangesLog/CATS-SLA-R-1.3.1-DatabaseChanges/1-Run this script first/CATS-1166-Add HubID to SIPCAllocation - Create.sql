

ALTER TABLE [dbo].[SIPCAllocation]
ADD [HubID] [int] NULL
GO
UPDATE [dbo].[SIPCAllocation]
SET [dbo].[SIPCAllocation].[HubID] = [dbo].[Transaction].[HubID]
FROM [dbo].[Transaction]
WHERE [dbo].[Transaction].ShippingInstructionID = [dbo].[SIPCAllocation].[Code] AND 
[dbo].[Transaction].LedgerID IN (SELECT LedgerID FROM Ledger WHERE Name = 'Goods On Hand')
GO
