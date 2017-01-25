
/****** Object:  StoredProcedure [dbo].[SPReceiveAllocationAggr]    Script Date: 1/24/2017 12:57:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Nathnael Getahun
-- Create date: July 31, 2016
-- Description:	Stored procedure that selects ReceiveAllocations with their respective aggregated received quantities
-- =============================================
ALTER PROCEDURE [dbo].[SPReceiveAllocationAggr] 
	-- Add the parameters for the stored procedure here
	(
	  @hubID int,
	  @isClosed bit,
	  @isFalseGRN bit,
	  @commodityTypeID int,
	  @commoditySourceID int,
	  @grn varchar(max)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF(@grn IS NULL OR @grn = '') 	
		SELECT ReceiptAllocationID, dbo.Commodity.Name AS CommodityName, SINumber, ProjectNumber, dbo.VWReceiptAllocation.AllocatedQuantityInMT AS QuantityInMT, HubID, IsClosed, IsFalseGRN, 
		              CommodityTypeID, CommoditySourceID,  SUM(dbo.VWReceiptAllocation.ReceivedQuantityInMT) AS ReceivedQuantity, HubID, IsClosed, IsFalseGRN, ETA, IsCommited, 
					  GiftCertificateDetailID, dbo.VWReceiptAllocation.CommodityID, 
					  UnitID, dbo.VWReceiptAllocation.AllocatedQuantityInUnit AS QuantityInUnit, DonorID, ProgramID, PurchaseOrder, SupplierName, SourceHubID, OtherDocumentationRef, Remark,
					  IsNull(dbo.VWReceiptAllocation.BusinessProcessID, 0) As BusinessProcessID
		FROM     dbo.VWReceiptAllocation LEFT OUTER JOIN
						  dbo.Commodity ON dbo.VWReceiptAllocation.CommodityID = dbo.Commodity.CommodityID
		WHERE  ((dbo.VWReceiptAllocation.LedgerID IN
							  (SELECT LedgerID FROM dbo.Ledger WHERE   (Name LIKE 'Statistics'))) OR (dbo.VWReceiptAllocation.LedgerID IS NULL))
                              AND HubID = @hubID AND IsClosed = @isClosed AND IsFalseGRN = @isFalseGRN 
							  AND CommodityTypeID = @commodityTypeID AND CommoditySourceID = @commoditySourceID
		GROUP BY dbo.VWReceiptAllocation.ReceiptAllocationID, dbo.Commodity.Name, dbo.VWReceiptAllocation.SINumber, dbo.VWReceiptAllocation.ProjectNumber, dbo.VWReceiptAllocation.HubID, 
						  dbo.VWReceiptAllocation.AllocatedQuantityInMT, dbo.VWReceiptAllocation.IsClosed, dbo.VWReceiptAllocation.IsFalseGRN, dbo.Commodity.CommodityTypeID, 
						  dbo.VWReceiptAllocation.CommoditySourceID, dbo.VWReceiptAllocation.ETA, dbo.VWReceiptAllocation.GiftCertificateDetailID, dbo.VWReceiptAllocation.CommodityID, dbo.VWReceiptAllocation.UnitID, 
						  dbo.VWReceiptAllocation.DonorID, dbo.VWReceiptAllocation.ProgramID, dbo.VWReceiptAllocation.PurchaseOrder, dbo.VWReceiptAllocation.SupplierName, dbo.VWReceiptAllocation.SourceHubID, 
						  dbo.VWReceiptAllocation.OtherDocumentationRef, dbo.VWReceiptAllocation.Remark, dbo.VWReceiptAllocation.IsCommited, dbo.VWReceiptAllocation.AllocatedQuantityInUnit,
						  dbo.VWReceiptAllocation.BusinessProcessID
	ELSE
		SELECT ReceiptAllocationID, dbo.Commodity.Name AS CommodityName, SINumber, ProjectNumber, dbo.VWReceiptAllocation.AllocatedQuantityInMT AS QuantityInMT, HubID, IsClosed, IsFalseGRN, 
		              CommodityTypeID, CommoditySourceID,  SUM(dbo.VWReceiptAllocation.ReceivedQuantityInMT) AS ReceivedQuantity, HubID, IsClosed, IsFalseGRN, ETA, IsCommited, 
					  GiftCertificateDetailID, dbo.VWReceiptAllocation.CommodityID, 
					  UnitID, dbo.VWReceiptAllocation.AllocatedQuantityInUnit AS QuantityInUnit, DonorID, ProgramID, PurchaseOrder, SupplierName, SourceHubID, OtherDocumentationRef, Remark,
					  IsNull(dbo.VWReceiptAllocation.BusinessProcessID, 0) As BusinessProcessID
		FROM     dbo.VWReceiptAllocation LEFT OUTER JOIN
						  dbo.Commodity ON dbo.VWReceiptAllocation.CommodityID = dbo.Commodity.CommodityID
		WHERE  ((dbo.VWReceiptAllocation.LedgerID IN
							  (SELECT LedgerID FROM dbo.Ledger WHERE   (Name LIKE 'Statistics'))) OR (dbo.VWReceiptAllocation.LedgerID IS NULL))
							  AND HubID = @hubID AND IsClosed = @isClosed AND IsFalseGRN = @isFalseGRN 
							  AND CommodityTypeID = @commodityTypeID AND CommoditySourceID = @commoditySourceID AND (GRN LIKE '%'+@grn+'%')
		GROUP BY dbo.VWReceiptAllocation.ReceiptAllocationID, dbo.Commodity.Name, dbo.VWReceiptAllocation.SINumber, dbo.VWReceiptAllocation.ProjectNumber, dbo.VWReceiptAllocation.HubID, 
						  dbo.VWReceiptAllocation.AllocatedQuantityInMT, dbo.VWReceiptAllocation.IsClosed, dbo.VWReceiptAllocation.IsFalseGRN, dbo.Commodity.CommodityTypeID, 
						  dbo.VWReceiptAllocation.CommoditySourceID, dbo.VWReceiptAllocation.ETA, dbo.VWReceiptAllocation.GiftCertificateDetailID, dbo.VWReceiptAllocation.CommodityID, dbo.VWReceiptAllocation.UnitID, 
						  dbo.VWReceiptAllocation.DonorID, dbo.VWReceiptAllocation.ProgramID, dbo.VWReceiptAllocation.PurchaseOrder, dbo.VWReceiptAllocation.SupplierName, dbo.VWReceiptAllocation.SourceHubID, 
						  dbo.VWReceiptAllocation.OtherDocumentationRef, dbo.VWReceiptAllocation.Remark, dbo.VWReceiptAllocation.IsCommited, dbo.VWReceiptAllocation.AllocatedQuantityInUnit,
						  dbo.VWReceiptAllocation.BusinessProcessID
END

