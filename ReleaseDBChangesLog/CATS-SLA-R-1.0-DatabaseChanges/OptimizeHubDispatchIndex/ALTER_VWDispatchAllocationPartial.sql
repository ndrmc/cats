USE [CatsDRMFSS]
GO

/****** Object:  View [dbo].[VWDispatchAllocationPartial]    Script Date: 8/26/2016 6:18:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[VWDispatchAllocationPartial]
AS
SELECT dbo.DispatchAllocation.ShippingInstructionID, dbo.DispatchAllocation.ProjectCodeID, dbo.DispatchAllocation.HubID, dbo.DispatchAllocation.IsClosed, dbo.DispatchAllocation.Amount AS AmountInUnit, 
                  dbo.DispatchAllocation.DispatchAllocationID, dbo.DispatchAllocation.RequisitionNo, dbo.DispatchAllocation.BidRefNo, dbo.DispatchAllocation.Round, dbo.[Transaction].QuantityInMT AS QuantityInMT, 
                  dbo.[Transaction].QuantityInUnit AS QuantityInUnit, dbo.DispatchAllocation.CommodityID, dbo.DispatchAllocation.FDPID, dbo.DispatchAllocation.StoreID, dbo.DispatchAllocation.Year, dbo.DispatchAllocation.Month, 
                  dbo.DispatchAllocation.DonorID, dbo.DispatchAllocation.ProgramID, dbo.DispatchAllocation.RequisitionId, dbo.DispatchAllocation.ContractStartDate, dbo.DispatchAllocation.ContractEndDate, 
                  dbo.DispatchAllocation.Beneficiery, dbo.DispatchAllocation.Unit, dbo.DispatchAllocation.TransporterID, dbo.DispatchAllocation.TransportOrderID, dbo.[Transaction].LedgerID
FROM     dbo.DispatchAllocation LEFT OUTER JOIN
                  dbo.Dispatch ON dbo.Dispatch.DispatchAllocationID = dbo.DispatchAllocation.DispatchAllocationID LEFT OUTER JOIN
                  dbo.DispatchDetail ON dbo.DispatchDetail.DispatchID = dbo.Dispatch.DispatchID LEFT OUTER JOIN
                  dbo.TransactionGroup ON dbo.DispatchDetail.TransactionGroupID = dbo.TransactionGroup.TransactionGroupID LEFT OUTER JOIN
                  dbo.[Transaction] ON dbo.TransactionGroup.TransactionGroupID = dbo.[Transaction].TransactionGroupID

GO


