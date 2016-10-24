USE [CatsDRMFSS]
GO

/****** Object:  View [dbo].[VWReceiptAllocation]    Script Date: 10/23/2016 10:36:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VWReceiptAllocation]
AS
SELECT dbo.ReceiptAllocation.ReceiptAllocationID, dbo.ReceiptAllocation.CommoditySourceID, dbo.ReceiptAllocation.SINumber, dbo.ReceiptAllocation.ProjectNumber, 
                  dbo.ReceiptAllocation.QuantityInMT AS AllocatedQuantityInMT, dbo.[Transaction].QuantityInMT AS ReceivedQuantityInMT, dbo.[Transaction].LedgerID, dbo.Receive.GRN, dbo.ReceiptAllocation.HubID, 
                  dbo.ReceiptAllocation.IsClosed, dbo.ReceiptAllocation.IsFalseGRN, dbo.ReceiptAllocation.ETA, dbo.ReceiptAllocation.IsCommited, dbo.ReceiptAllocation.GiftCertificateDetailID, dbo.ReceiptAllocation.CommodityID, 
                  dbo.ReceiptAllocation.UnitID, dbo.ReceiptAllocation.QuantityInUnit AS AllocatedQuantityInUnit, dbo.ReceiptAllocation.DonorID, dbo.ReceiptAllocation.ProgramID, dbo.ReceiptAllocation.PurchaseOrder, 
                  dbo.ReceiptAllocation.SupplierName, dbo.ReceiptAllocation.SourceHubID, dbo.ReceiptAllocation.OtherDocumentationRef, dbo.ReceiptAllocation.Remark, 
                  dbo.[Transaction].QuantityInUnit AS ReceivedQuantityInUnit
FROM     dbo.ReceiptAllocation LEFT OUTER JOIN
                  dbo.Receive ON dbo.ReceiptAllocation.ReceiptAllocationID = dbo.Receive.ReceiptAllocationID LEFT OUTER JOIN
                  dbo.ReceiveDetail ON dbo.Receive.ReceiveID = dbo.ReceiveDetail.ReceiveID LEFT OUTER JOIN
                  dbo.TransactionGroup ON dbo.ReceiveDetail.TransactionGroupID = dbo.TransactionGroup.TransactionGroupID LEFT OUTER JOIN
                  dbo.[Transaction] ON dbo.TransactionGroup.TransactionGroupID = dbo.[Transaction].TransactionGroupID
WHERE  (dbo.ReceiptAllocation.SINumber IS NOT NULL) OR
                  (dbo.ReceiptAllocation.ProjectNumber IS NOT NULL)

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[12] 4[9] 2[42] 3) )"
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
         Top = -360
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ReceiptAllocation"
            Begin Extent = 
               Top = 0
               Left = 301
               Bottom = 365
               Right = 558
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Receive"
            Begin Extent = 
               Top = 0
               Left = 632
               Bottom = 308
               Right = 912
            End
            DisplayFlags = 280
            TopColumn = 19
         End
         Begin Table = "ReceiveDetail"
            Begin Extent = 
               Top = 0
               Left = 1018
               Bottom = 308
               Right = 1245
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "TransactionGroup"
            Begin Extent = 
               Top = 4
               Left = 1275
               Bottom = 128
               Right = 1502
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transaction"
            Begin Extent = 
               Top = 367
               Left = 48
               Bottom = 530
               Right = 286
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
         Width = 2760
         Width = 2820
         Width = 2976
         Width = 1560
         Width = 2448
         Width = 2388
         Width = 2028
         Width = 2424
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
    ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VWReceiptAllocation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'     Width = 1200
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
         Width = 1200
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 4392
         Alias = 2340
         Table = 2172
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1356
         SortOrder = 1416
         GroupBy = 2196
         Filter = 2508
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VWReceiptAllocation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VWReceiptAllocation'
GO


