USE [CatsDRMFSS]
GO

/****** Object:  StoredProcedure [dbo].[SPRegionalNotReconcileDispatchAmount]    Script Date: 9/7/2016 2:51:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Nathnael Getahun	
-- Create date: September 07, 2016
-- Description:	A stored procedure that aggregates the amount of dispatch in a region that was not reconciled yet
-- =============================================
CREATE PROCEDURE [dbo].[SPRegionalNotReconcileDispatchAmount] 
	-- Add the parameters for the stored procedure here
	(
	  @regionID int
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT SUM(dbo.DispatchDetail.RequestedQuantityInMT) AS RegionalNonReconciledDispatchAmount
	FROM     dbo.Dispatch INNER JOIN
						dbo.DispatchDetail ON dbo.Dispatch.DispatchID = dbo.DispatchDetail.DispatchID INNER JOIN
						dbo.FDP ON dbo.Dispatch.FDPID = dbo.FDP.FDPID INNER JOIN
						dbo.VWAdminUnitHierarchy ON dbo.FDP.AdminUnitID = dbo.VWAdminUnitHierarchy.WoredaID
	WHERE  (dbo.VWAdminUnitHierarchy.RegionID = @regionID) AND (dbo.Dispatch.DispatchID NOT IN
							(SELECT DispatchID FROM dbo.DeliveryReconcile))
END


GO


