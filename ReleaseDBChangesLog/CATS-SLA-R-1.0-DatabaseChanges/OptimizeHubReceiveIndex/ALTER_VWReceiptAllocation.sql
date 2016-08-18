USE [CatsDRMFSS]
GO

/****** Object:  View [dbo].[VWReceiptAllocation]    Script Date: 8/18/2016 5:48:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[VWReceiptAllocation]
AS
SELECT dbo.ReceiptAllocation.ReceiptAllocationID, dbo.Commodity.CommodityTypeID, dbo.ReceiptAllocation.CommoditySourceID, dbo.Commodity.Name AS CommodityName, dbo.ReceiptAllocation.SINumber, 
                  dbo.ReceiptAllocation.ProjectNumber, dbo.ReceiptAllocation.QuantityInMT, SUM(dbo.[Transaction].QuantityInMT) AS ReceivedQuantityInMT, dbo.Receive.GRN, dbo.ReceiptAllocation.HubID, 
                  dbo.ReceiptAllocation.IsClosed, dbo.ReceiptAllocation.IsFalseGRN, dbo.ReceiptAllocation.ETA, dbo.ReceiptAllocation.IsCommited, dbo.ReceiptAllocation.GiftCertificateDetailID, dbo.ReceiptAllocation.CommodityID, 
                  dbo.ReceiptAllocation.UnitID, dbo.ReceiptAllocation.QuantityInUnit, dbo.ReceiptAllocation.DonorID, dbo.ReceiptAllocation.ProgramID, dbo.ReceiptAllocation.PurchaseOrder, dbo.ReceiptAllocation.SupplierName, 
                  dbo.ReceiptAllocation.SourceHubID, dbo.ReceiptAllocation.OtherDocumentationRef, dbo.ReceiptAllocation.Remark
FROM     dbo.ReceiptAllocation LEFT OUTER JOIN
                  dbo.Receive ON dbo.ReceiptAllocation.ReceiptAllocationID = dbo.Receive.ReceiptAllocationID LEFT OUTER JOIN
                  dbo.ReceiveDetail ON dbo.Receive.ReceiveID = dbo.ReceiveDetail.ReceiveID LEFT OUTER JOIN
                  dbo.Commodity ON dbo.ReceiptAllocation.CommodityID = dbo.Commodity.CommodityID LEFT OUTER JOIN
                  dbo.TransactionGroup ON dbo.ReceiveDetail.TransactionGroupID = dbo.TransactionGroup.TransactionGroupID LEFT OUTER JOIN
                  dbo.[Transaction] ON dbo.TransactionGroup.TransactionGroupID = dbo.[Transaction].TransactionGroupID
WHERE  (dbo.[Transaction].QuantityInUnit >= 0) AND (dbo.[Transaction].QuantityInMT >= 0) AND (dbo.[Transaction].LedgerID IN
                      (SELECT LedgerID
                       FROM      dbo.Ledger
                       WHERE   (Name LIKE 'Statistics')))
GROUP BY dbo.ReceiptAllocation.ReceiptAllocationID, dbo.Commodity.Name, dbo.ReceiptAllocation.SINumber, dbo.ReceiptAllocation.ProjectNumber, dbo.ReceiptAllocation.QuantityInMT, dbo.ReceiptAllocation.HubID, 
                  dbo.ReceiptAllocation.IsClosed, dbo.ReceiptAllocation.IsFalseGRN, dbo.Receive.GRN, dbo.Commodity.CommodityTypeID, dbo.[Transaction].LedgerID, dbo.ReceiptAllocation.CommoditySourceID, 
                  dbo.ReceiptAllocation.ETA, dbo.ReceiptAllocation.GiftCertificateDetailID, dbo.ReceiptAllocation.CommodityID, dbo.ReceiptAllocation.UnitID, dbo.ReceiptAllocation.QuantityInUnit, dbo.ReceiptAllocation.DonorID, 
                  dbo.ReceiptAllocation.ProgramID, dbo.ReceiptAllocation.PurchaseOrder, dbo.ReceiptAllocation.SupplierName, dbo.ReceiptAllocation.SourceHubID, dbo.ReceiptAllocation.OtherDocumentationRef, 
                  dbo.ReceiptAllocation.Remark, dbo.ReceiptAllocation.IsCommited

GO


