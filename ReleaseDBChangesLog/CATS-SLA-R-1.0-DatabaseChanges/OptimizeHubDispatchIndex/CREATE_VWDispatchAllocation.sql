USE [CatsDRMFSS]
GO

/****** Object:  View [dbo].[VWDispatchAllocation]    Script Date: 8/26/2016 6:29:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VWDispatchAllocation]
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

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[9] 4[28] 2[34] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "VWDispatchAllocationPartial"
            Begin Extent = 
               Top = 0
               Left = 474
               Bottom = 282
               Right = 712
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transporter (Procurement)"
            Begin Extent = 
               Top = 7
               Left = 1405
               Bottom = 170
               Right = 1669
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Commodity"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 250
               Right = 268
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FDP"
            Begin Extent = 
               Top = 0
               Left = 861
               Bottom = 282
               Right = 1055
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "VWAdminUnitHierarchy"
            Begin Extent = 
               Top = 0
               Left = 1146
               Bottom = 280
               Right = 1357
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 38
         Width = 284
         Width = 1800
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 2160
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1776
         Wid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VWDispatchAllocation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'th = 2412
         Width = 1632
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1860
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1404
         Width = 1488
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 2592
         Alias = 2196
         Table = 2580
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1356
         SortOrder = 1416
         GroupBy = 1350
         Filter = 1356
         Or = 1260
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VWDispatchAllocation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VWDispatchAllocation'
GO


