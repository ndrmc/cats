USE [CatsDRMFSS]
GO

/****** Object:  View [dbo].[VWDispatchAllocation]    Script Date: 8/26/2016 6:29:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[VWDispatchAllocation]
AS
SELECT dbo.VWDispatchAllocationPartial.ShippingInstructionID, dbo.VWDispatchAllocationPartial.ProjectCodeID, dbo.VWDispatchAllocationPartial.HubID, dbo.VWDispatchAllocationPartial.IsClosed, 
                  dbo.Commodity.CommodityTypeID, dbo.VWDispatchAllocationPartial.AmountInUnit AS Amount, dbo.VWDispatchAllocationPartial.DispatchAllocationID, dbo.VWDispatchAllocationPartial.RequisitionNo, 
                  dbo.VWDispatchAllocationPartial.BidRefNo, dbo.VWDispatchAllocationPartial.Round, SUM(dbo.VWDispatchAllocationPartial.QuantityInMT) AS DispatchedAmount, SUM(dbo.VWDispatchAllocationPartial.QuantityInUnit) 
                  AS DispatchedAmountInUnit, dbo.VWDispatchAllocationPartial.CommodityID, dbo.VWDispatchAllocationPartial.FDPID, dbo.FDP.Name AS FDPName, dbo.VWAdminUnitHierarchy.WoredaID, 
                  dbo.VWAdminUnitHierarchy.WoredaName, dbo.VWAdminUnitHierarchy.ZoneID, dbo.VWAdminUnitHierarchy.ZoneName, dbo.VWAdminUnitHierarchy.RegionID, dbo.VWAdminUnitHierarchy.RegionName, 
                  dbo.Commodity.Name AS CommodityName, dbo.VWDispatchAllocationPartial.TransporterID, Procurement.Transporter.Name AS TransporterName, dbo.VWDispatchAllocationPartial.StoreID, 
                  dbo.VWDispatchAllocationPartial.Year, dbo.VWDispatchAllocationPartial.DonorID, dbo.VWDispatchAllocationPartial.Month, dbo.VWDispatchAllocationPartial.ProgramID, 
                  dbo.VWDispatchAllocationPartial.ContractStartDate, dbo.VWDispatchAllocationPartial.RequisitionId, dbo.VWDispatchAllocationPartial.ContractEndDate, dbo.VWDispatchAllocationPartial.Beneficiery, 
                  dbo.VWDispatchAllocationPartial.Unit, dbo.VWDispatchAllocationPartial.TransportOrderID
FROM     dbo.VWDispatchAllocationPartial LEFT OUTER JOIN
                  Procurement.Transporter ON dbo.VWDispatchAllocationPartial.TransporterID = Procurement.Transporter.TransporterID LEFT OUTER JOIN
                  dbo.Commodity ON dbo.VWDispatchAllocationPartial.CommodityID = dbo.Commodity.CommodityID LEFT OUTER JOIN
                  dbo.FDP ON dbo.FDP.FDPID = dbo.VWDispatchAllocationPartial.FDPID LEFT OUTER JOIN
                  dbo.VWAdminUnitHierarchy ON dbo.VWAdminUnitHierarchy.WoredaID = dbo.FDP.AdminUnitID
WHERE  (dbo.VWDispatchAllocationPartial.LedgerID IN
                      (SELECT LedgerID
                       FROM      dbo.Ledger
                       WHERE   (Name LIKE 'Commited To FDP'))) AND (dbo.VWDispatchAllocationPartial.QuantityInMT > 0) AND (dbo.VWDispatchAllocationPartial.QuantityInUnit > 0) OR
                  (dbo.VWDispatchAllocationPartial.LedgerID IS NULL)
GROUP BY dbo.VWDispatchAllocationPartial.ShippingInstructionID, dbo.VWDispatchAllocationPartial.ProjectCodeID, dbo.VWDispatchAllocationPartial.HubID, dbo.VWDispatchAllocationPartial.IsClosed, 
                  dbo.Commodity.CommodityTypeID, dbo.VWDispatchAllocationPartial.AmountInUnit, dbo.VWDispatchAllocationPartial.DispatchAllocationID, dbo.VWDispatchAllocationPartial.RequisitionNo, 
                  dbo.VWDispatchAllocationPartial.BidRefNo, dbo.VWDispatchAllocationPartial.Round, dbo.VWDispatchAllocationPartial.CommodityID, dbo.VWDispatchAllocationPartial.FDPID, dbo.FDP.Name, 
                  dbo.VWAdminUnitHierarchy.WoredaID, dbo.VWAdminUnitHierarchy.WoredaName, dbo.VWAdminUnitHierarchy.ZoneID, dbo.VWAdminUnitHierarchy.ZoneName, dbo.VWAdminUnitHierarchy.RegionID, 
                  dbo.VWAdminUnitHierarchy.RegionName, dbo.Commodity.Name, dbo.VWDispatchAllocationPartial.TransporterID, Procurement.Transporter.Name, dbo.VWDispatchAllocationPartial.StoreID, 
                  dbo.VWDispatchAllocationPartial.Year, dbo.VWDispatchAllocationPartial.DonorID, dbo.VWDispatchAllocationPartial.Month, dbo.VWDispatchAllocationPartial.ProgramID, 
                  dbo.VWDispatchAllocationPartial.ContractStartDate, dbo.VWDispatchAllocationPartial.RequisitionId, dbo.VWDispatchAllocationPartial.ContractEndDate, dbo.VWDispatchAllocationPartial.Beneficiery, 
                  dbo.VWDispatchAllocationPartial.Unit, dbo.VWDispatchAllocationPartial.TransportOrderID, dbo.VWDispatchAllocationPartial.RequisitionId
HAVING (dbo.VWDispatchAllocationPartial.ShippingInstructionID IS NOT NULL) OR
                  (dbo.VWDispatchAllocationPartial.ProjectCodeID IS NOT NULL)

GO


