SELECT        TOP (100) PERCENT Hub.Name AS hub, FDP_1.Name AS fdp, AdminUnit.Name AS woreda, AdminUnit_2.Name AS zone, AdminUnit_1.Name AS region, Commodity.Name AS commodity, 
                         DispatchAllocation.Amount AS Allocated, SUM(DispatchDetail.RequestedQuantityInMT) AS Dispatched, Procurement.Transporter.Name AS Transporter, Dispatch.Round, DispatchAllocation.Month, 
                         Program.Name AS program, Dispatch.DispatchDate, Commodity_1.Name AS [Commodity parent], Dispatch.RequisitionNo, ShippingInstruction.Value AS SI
FROM            Hub INNER JOIN
                         Dispatch ON Hub.HubID = Dispatch.HubID INNER JOIN
                         DispatchDetail ON Dispatch.DispatchID = DispatchDetail.DispatchID INNER JOIN
                         Procurement.Transporter ON Dispatch.TransporterID = Procurement.Transporter.TransporterID INNER JOIN
                         Program INNER JOIN
                         DispatchAllocation ON Program.ProgramID = DispatchAllocation.ProgramID INNER JOIN
                         AdminUnit AS AdminUnit_1 INNER JOIN
                         AdminUnit INNER JOIN
                         FDP AS FDP_1 ON AdminUnit.AdminUnitID = FDP_1.AdminUnitID INNER JOIN
                         AdminUnit AS AdminUnit_2 ON AdminUnit.ParentID = AdminUnit_2.AdminUnitID AND AdminUnit.ParentID = AdminUnit_2.AdminUnitID ON AdminUnit_1.AdminUnitID = AdminUnit_2.ParentID AND 
                         AdminUnit_1.AdminUnitID = AdminUnit_2.ParentID ON DispatchAllocation.FDPID = FDP_1.FDPID INNER JOIN
                         Commodity ON DispatchAllocation.CommodityID = Commodity.CommodityID ON Dispatch.DispatchAllocationID = DispatchAllocation.DispatchAllocationID INNER JOIN
                         ShippingInstruction ON DispatchAllocation.ShippingInstructionID = ShippingInstruction.ShippingInstructionID LEFT OUTER JOIN
                         Commodity AS Commodity_1 ON Commodity.ParentID = Commodity_1.CommodityID AND Commodity.ParentID = Commodity_1.CommodityID
GROUP BY Hub.Name, FDP_1.Name, AdminUnit.Name, AdminUnit_2.Name, AdminUnit_1.Name, Commodity.Name, DispatchAllocation.Amount, Procurement.Transporter.Name, Dispatch.Round, DispatchAllocation.Month, 
                         Program.Name, Dispatch.DispatchDate, Commodity_1.Name, Dispatch.RequisitionNo, ShippingInstruction.Value
HAVING        (Dispatch.DispatchDate BETWEEN @StartDate AND @EndDate OR
                         Dispatch.DispatchDate IS NULL) AND (Program.Name = @Program) AND (Dispatch.Round IN (@Round)) AND (AdminUnit_1.Name IN (@Region)) AND (AdminUnit_2.Name IN (@Zone)) AND 
                         (AdminUnit.Name IN (@Woreda)) AND (FDP_1.Name IN (@FDP)) AND (Procurement.Transporter.Name IN (@Transporter)) AND (Hub.Name IN (@hubName))
ORDER BY fdp, Allocated