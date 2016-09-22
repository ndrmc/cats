USE [CatsDRMFSS]
GO

/****** Object:  View [dbo].[WoredaDistributionDashboardView]    Script Date: 9/22/2016 11:51:18 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[WoredaDistributionDashboardView]
AS
SELECT dbo.AdminUnit.Name AS WoredaName, dbo.WoredaStcokDistribution.WoredaID, AdminUnit_1.Name AS Zone, AdminUnit_2.Name AS RegionName, AdminUnit_2.AdminUnitID AS RegionID, 
                  COUNT(DISTINCT dbo.FDP.FDPID) AS FdpCount, dbo.[Plan].PlanName, dbo.[Plan].Status, dbo.WoredaStcokDistribution.DistributionDate, dbo.WoredaStcokDistribution.Month, 
                  dbo.WoredaStockDistributionDetail.WoredaStockDistributionID
FROM     dbo.WoredaStcokDistribution INNER JOIN
                  dbo.WoredaStockDistributionDetail ON dbo.WoredaStcokDistribution.WoredaStockDistributionID = dbo.WoredaStockDistributionDetail.WoredaStockDistributionID INNER JOIN
                  dbo.FDP ON dbo.WoredaStockDistributionDetail.FDPID = dbo.FDP.FDPID INNER JOIN
                  dbo.AdminUnit ON dbo.WoredaStcokDistribution.WoredaID = dbo.AdminUnit.AdminUnitID INNER JOIN
                  dbo.AdminUnit AS AdminUnit_1 ON dbo.AdminUnit.ParentID = AdminUnit_1.AdminUnitID INNER JOIN
                  dbo.AdminUnit AS AdminUnit_2 ON AdminUnit_1.ParentID = AdminUnit_2.AdminUnitID INNER JOIN
                  dbo.[Plan] ON dbo.WoredaStcokDistribution.PlanID = dbo.[Plan].PlanID
GROUP BY dbo.AdminUnit.Name, dbo.[Plan].PlanName, dbo.AdminUnit.Name, dbo.WoredaStcokDistribution.WoredaID, AdminUnit_1.Name, AdminUnit_2.Name, AdminUnit_2.AdminUnitID, dbo.[Plan].Status, 
                  dbo.WoredaStcokDistribution.DistributionDate, dbo.WoredaStcokDistribution.Month, dbo.WoredaStockDistributionDetail.WoredaStockDistributionID

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[25] 4[24] 2[10] 3) )"
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
         Begin Table = "WoredaStcokDistribution"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 195
               Right = 278
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "WoredaStockDistributionDetail"
            Begin Extent = 
               Top = 15
               Left = 314
               Bottom = 179
               Right = 594
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FDP"
            Begin Extent = 
               Top = 32
               Left = 710
               Bottom = 162
               Right = 880
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AdminUnit"
            Begin Extent = 
               Top = 154
               Left = 309
               Bottom = 284
               Right = 493
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AdminUnit_1"
            Begin Extent = 
               Top = 157
               Left = 611
               Bottom = 287
               Right = 795
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AdminUnit_2"
            Begin Extent = 
               Top = 178
               Left = 54
               Bottom = 308
               Right = 238
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Plan"
            Begin Extent = 
               Top = 308
               Left = 48
               Bottom = 471
               Right = 278
       ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WoredaDistributionDashboardView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'     End
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
      Begin ColumnWidths = 12
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1200
         Width = 1200
         Width = 1200
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 1512
         Table = 2328
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1356
         SortOrder = 1416
         GroupBy = 1350
         Filter = 1356
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WoredaDistributionDashboardView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WoredaDistributionDashboardView'
GO


