USE [CatsDRMFSS]
GO
/****** Object:  StoredProcedure [dbo].[SPReceiveAllocationAggr]    Script Date: 7/31/2016 4:59:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nathnael Getahun
-- Create date: July 31, 2016
-- Description:	Stored procedure that selects ReceiveAllocations with their respective aggregated received quantities
-- =============================================
CREATE PROCEDURE [dbo].[SPReceiveAllocationAggr] 
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
		SELECT ReceiptAllocationID, CommodityName, SINumber, ProjectNumber, QuantityInMT, HubID, IsClosed, IsFalseGRN, CommodityTypeID, CommoditySourceID, SUM(ReceivedQuantityInMT) 
					  AS ReceivedQuantity, HubID, IsClosed, IsFalseGRN, ETA, IsCommited, GiftCertificateDetailID, CommodityID, UnitID, QuantityInUnit, DonorID, ProgramID, PurchaseOrder, SupplierName, 
					  SourceHubID, OtherDocumentationRef, Remark
 
		FROM  dbo.VWReceiptAllocation
		WHERE HubID = @hubID AND IsClosed = @isClosed AND IsFalseGRN = @isFalseGRN AND CommodityTypeID = @commodityTypeID AND CommoditySourceID = @commoditySourceID AND (GRN LIKE '%%'		
		OR GRN IS NULL) 
		GROUP BY ReceiptAllocationID, HubID, IsClosed, IsFalseGRN, SINumber, ProjectNumber, QuantityInMT, CommodityName, CommodityTypeID, CommoditySourceID, ETA, IsCommited, GiftCertificateDetailID, 
				CommodityID, UnitID, QuantityInUnit, DonorID, ProgramID, PurchaseOrder, SupplierName, SourceHubID, OtherDocumentationRef, Remark
	ELSE
		SELECT ReceiptAllocationID, CommodityName, SINumber, ProjectNumber, QuantityInMT, HubID, IsClosed, IsFalseGRN, CommodityTypeID, CommoditySourceID, SUM(ReceivedQuantityInMT) 
					  AS ReceivedQuantity, HubID, IsClosed, IsFalseGRN, ETA, IsCommited, GiftCertificateDetailID, CommodityID, UnitID, QuantityInUnit, DonorID, ProgramID, PurchaseOrder, SupplierName, 
					  SourceHubID, OtherDocumentationRef, Remark
 
		FROM  dbo.VWReceiptAllocation
		WHERE HubID = @hubID AND IsClosed = @isClosed AND IsFalseGRN = @isFalseGRN AND CommodityTypeID = @commodityTypeID AND CommoditySourceID = @commoditySourceID AND (GRN LIKE '%'+@grn+'%') 
		GROUP BY ReceiptAllocationID, HubID, IsClosed, IsFalseGRN, SINumber, ProjectNumber, QuantityInMT, CommodityName, CommodityTypeID, CommoditySourceID, ETA, IsCommited, GiftCertificateDetailID, 
				CommodityID, UnitID, QuantityInUnit, DonorID, ProgramID, PurchaseOrder, SupplierName, SourceHubID, OtherDocumentationRef, Remark
	
END
