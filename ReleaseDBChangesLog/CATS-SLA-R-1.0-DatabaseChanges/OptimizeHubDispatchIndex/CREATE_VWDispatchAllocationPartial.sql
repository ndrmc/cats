USE [CatsDRMFSS]
GO

/****** Object:  View [dbo].[VWDispatchAllocationPartial]    Script Date: 8/18/2016 4:16:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VWDispatchAllocationPartial]
AS
SELECT dbo.DispatchAllocation.ShippingInstructionID, dbo.DispatchAllocation.ProjectCodeID, dbo.DispatchAllocation.HubID, dbo.DispatchAllocation.IsClosed, dbo.DispatchAllocation.Amount AS AmountInUnit, 
                  dbo.DispatchAllocation.DispatchAllocationID, dbo.DispatchAllocation.RequisitionNo, dbo.DispatchAllocation.BidRefNo, dbo.DispatchAllocation.Round, SUM(dbo.[Transaction].QuantityInMT) AS QuantityInMT, 
                  SUM(dbo.[Transaction].QuantityInUnit) AS QuantityInUnit, dbo.DispatchAllocation.CommodityID, dbo.DispatchAllocation.FDPID, dbo.DispatchAllocation.StoreID, dbo.DispatchAllocation.Year, 
                  dbo.DispatchAllocation.Month, dbo.DispatchAllocation.DonorID, dbo.DispatchAllocation.ProgramID, dbo.DispatchAllocation.RequisitionId, dbo.DispatchAllocation.ContractStartDate, 
                  dbo.DispatchAllocation.ContractEndDate, dbo.DispatchAllocation.Beneficiery, dbo.DispatchAllocation.Unit, dbo.DispatchAllocation.TransporterID, dbo.DispatchAllocation.TransportOrderID
FROM     dbo.DispatchAllocation LEFT OUTER JOIN
                  dbo.Dispatch ON dbo.Dispatch.DispatchAllocationID = dbo.DispatchAllocation.DispatchAllocationID LEFT OUTER JOIN
                  dbo.DispatchDetail ON dbo.DispatchDetail.DispatchID = dbo.Dispatch.DispatchID LEFT OUTER JOIN
                  dbo.TransactionGroup ON dbo.DispatchDetail.TransactionGroupID = dbo.TransactionGroup.TransactionGroupID LEFT OUTER JOIN
                  dbo.[Transaction] ON dbo.TransactionGroup.TransactionGroupID = dbo.[Transaction].TransactionGroupID
WHERE  (dbo.[Transaction].QuantityInUnit >= 0) AND (dbo.[Transaction].QuantityInMT >= 0) AND (dbo.[Transaction].LedgerID IN
                      (SELECT LedgerID
                       FROM      dbo.Ledger
                       WHERE   (Name LIKE 'Commited To FDP')))
GROUP BY dbo.DispatchAllocation.ShippingInstructionID, dbo.DispatchAllocation.ProjectCodeID, dbo.DispatchAllocation.HubID, dbo.DispatchAllocation.IsClosed, dbo.DispatchAllocation.Amount, 
                  dbo.DispatchAllocation.DispatchAllocationID, dbo.DispatchAllocation.RequisitionNo, dbo.DispatchAllocation.BidRefNo, dbo.DispatchAllocation.Round, dbo.DispatchAllocation.CommodityID, 
                  dbo.DispatchAllocation.FDPID, dbo.DispatchAllocation.StoreID, dbo.DispatchAllocation.Year, dbo.DispatchAllocation.Month, dbo.DispatchAllocation.DonorID, dbo.DispatchAllocation.ProgramID, 
                  dbo.DispatchAllocation.RequisitionId, dbo.DispatchAllocation.ContractStartDate, dbo.DispatchAllocation.ContractEndDate, dbo.DispatchAllocation.Beneficiery, dbo.DispatchAllocation.Unit, 
                  dbo.DispatchAllocation.TransporterID, dbo.DispatchAllocation.TransportOrderID

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[11] 4[21] 2[30] 3) )"
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
         Begin Table = "DispatchAllocation"
            Begin Extent = 
               Top = 0
               Left = 242
               Bottom = 308
               Right = 480
            End
            DisplayFlags = 280
            TopColumn = 14
         End
         Begin Table = "Dispatch"
            Begin Extent = 
               Top = 7
               Left = 512
               Bottom = 302
               Right = 787
            End
            DisplayFlags = 280
            TopColumn = 7
         End
         Begin Table = "DispatchDetail"
            Begin Extent = 
               Top = 8
               Left = 829
               Bottom = 315
               Right = 1089
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "TransactionGroup"
            Begin Extent = 
               Top = 0
               Left = 1152
               Bottom = 207
               Right = 1379
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transaction"
            Begin Extent = 
               Top = 308
               Left = 48
               Bottom = 471
               Right = 302
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
      Begin ColumnWidths = 26
         Width = 284
         Width = 2016
         Width = 1848
         Width = 1212
         Width = 1212
         Width = 1560
         Width = 1980
         Width = 1428
         Width = 1200
         Width = 1200
         Width = 1560
         Width = 1644
         Width = 1200
    ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VWDispatchAllocationPartial'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'     Width = 1200
         Width = 1668
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
         Width = 1200
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 2172
         Alias = 1860
         Table = 2124
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VWDispatchAllocationPartial'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VWDispatchAllocationPartial'
GO


