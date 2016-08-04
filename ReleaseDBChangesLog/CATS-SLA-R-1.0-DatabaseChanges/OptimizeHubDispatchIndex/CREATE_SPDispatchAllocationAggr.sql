USE [CatsDRMFSS]
GO

/****** Object:  StoredProcedure [dbo].[SPDispatchAllocationAggr]    Script Date: 8/3/2016 12:25:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Nathnael Getahun
-- Create date: July 31, 2016
-- Description:	Stored procedure that selects DispatchAllocations with their respective aggregated dispatched quantities
-- =============================================
CREATE PROCEDURE [dbo].[SPDispatchAllocationAggr] 
	-- Add the parameters for the stored procedure here
	(
	  @hubID int,
	  @isClosed bit,
	  @commodityTypeID int,
	  @woredaID int,
	  @zoneID int,
	  @regionID int
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF(@woredaID <> -1) 
		SELECT * FROM  dbo.VWDispatchAllocation
			WHERE HubID = @hubID AND IsClosed = @isClosed AND CommodityTypeID = @commodityTypeID AND WoredaID = @woredaID
	ELSE IF (@zoneID <> -1) 
		SELECT * FROM  dbo.VWDispatchAllocation
			WHERE HubID = @hubID AND IsClosed = @isClosed AND CommodityTypeID = @commodityTypeID AND ZoneID = @zoneID
	ELSE IF (@regionID <> -1) 
		SELECT * FROM  dbo.VWDispatchAllocation
			WHERE HubID = @hubID AND IsClosed = @isClosed AND CommodityTypeID = @commodityTypeID AND RegionName = @regionID
	ELSE
		SELECT * FROM  dbo.VWDispatchAllocation
			WHERE HubID = @hubID AND IsClosed = @isClosed AND CommodityTypeID = @commodityTypeID
END

GO


