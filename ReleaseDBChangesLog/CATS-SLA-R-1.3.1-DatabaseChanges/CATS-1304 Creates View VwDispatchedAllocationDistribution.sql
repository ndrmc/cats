USE [CatsDrmfss]
GO

/****** Object:  View [dbo].[VWDispatchAllocationDistribution]    Script Date: 10/20/2016 12:37:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VWDispatchAllocationDistribution]
AS
SELECT        Procurement.Transporter.Name AS Transporter, dbo.Commodity.Name AS Commodity, dbo.Dispatch.DispatchDate, SUM(dbo.DispatchAllocation.Amount) AS AllocatedAmount, 
                         SUM(dbo.DispatchDetail.RequestedQuantityInMT) AS DispatchedAmount, SUM(dbo.DispatchAllocation.Amount) - SUM(dbo.DispatchDetail.RequestedQuantityInMT) AS Diff1
FROM            dbo.TransporterPaymentRequest INNER JOIN
                         dbo.DispatchDetail INNER JOIN
                         Procurement.TransportOrder INNER JOIN
                         Procurement.Transporter INNER JOIN
                         dbo.DispatchAllocation INNER JOIN
                         dbo.Dispatch ON dbo.DispatchAllocation.DispatchAllocationID = dbo.Dispatch.DispatchAllocationID ON Procurement.Transporter.TransporterID = dbo.Dispatch.TransporterID INNER JOIN
                         Procurement.TransportOrderDetail ON dbo.Dispatch.FDPID = Procurement.TransportOrderDetail.FdpID ON Procurement.TransportOrder.TransportOrderID = Procurement.TransportOrderDetail.TransportOrderID ON 
                         dbo.DispatchDetail.DispatchID = dbo.Dispatch.DispatchID ON dbo.TransporterPaymentRequest.TransportOrderID = Procurement.TransportOrder.TransportOrderID INNER JOIN
                         dbo.Commodity ON Procurement.TransportOrderDetail.CommodityID = dbo.Commodity.CommodityID
GROUP BY Procurement.Transporter.Name, dbo.Commodity.Name, dbo.Dispatch.DispatchDate

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[16] 4[46] 2[21] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1[50] 2[25] 3) )"
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
         Configuration = "(H (1[75] 4) )"
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
         Begin Table = "TransporterPaymentRequest"
            Begin Extent = 
               Top = 176
               Left = 792
               Bottom = 306
               Right = 1042
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DispatchDetail"
            Begin Extent = 
               Top = 0
               Left = 0
               Bottom = 130
               Right = 222
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "TransportOrder (Procurement)"
            Begin Extent = 
               Top = 157
               Left = 523
               Bottom = 287
               Right = 763
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transporter (Procurement)"
            Begin Extent = 
               Top = 136
               Left = 0
               Bottom = 342
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DispatchAllocation"
            Begin Extent = 
               Top = 0
               Left = 617
               Bottom = 147
               Right = 851
            End
            DisplayFlags = 280
            TopColumn = 16
         End
         Begin Table = "Dispatch"
            Begin Extent = 
               Top = 0
               Left = 331
               Bottom = 125
               Right = 564
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TransportOrderDetail (Procurement)"
            Begin Extent = 
               Top = 220
               Left = 224
    ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VWDispatchAllocationDistribution'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'           Bottom = 339
               Right = 458
            End
            DisplayFlags = 280
            TopColumn = 9
         End
         Begin Table = "Commodity"
            Begin Extent = 
               Top = 318
               Left = 513
               Bottom = 448
               Right = 703
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
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VWDispatchAllocationDistribution'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VWDispatchAllocationDistribution'
GO


