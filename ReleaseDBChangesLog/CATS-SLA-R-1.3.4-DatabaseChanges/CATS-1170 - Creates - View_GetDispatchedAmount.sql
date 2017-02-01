USE [Cats-v-1-3-v6]
GO

/****** Object:  View [dbo].[view_GetDispatchedAmount]    Script Date: 1/30/2017 10:41:42 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[view_GetDispatchedAmount]
AS
SELECT        SUM(dbo.[Transaction].QuantityInUnit) AS DispatchedAmount, dbo.DispatchDetail.DispatchID, dbo.TransactionGroup.TransactionGroupID
FROM            dbo.[Transaction] INNER JOIN
                         dbo.TransactionGroup ON dbo.[Transaction].TransactionGroupID = dbo.TransactionGroup.TransactionGroupID INNER JOIN
                         dbo.DispatchDetail ON dbo.TransactionGroup.TransactionGroupID = dbo.DispatchDetail.TransactionGroupID
WHERE        (dbo.[Transaction].LedgerID = 2) AND (dbo.[Transaction].TransactionGroupID IN
                             (SELECT        TransactionGroupID
                               FROM            dbo.DispatchDetail AS DispatchDetail_1))
GROUP BY dbo.DispatchDetail.DispatchID, dbo.TransactionGroup.TransactionGroupID

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
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
         Begin Table = "Transaction"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TransactionGroup"
            Begin Extent = 
               Top = 6
               Left = 280
               Bottom = 102
               Right = 475
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DispatchDetail"
            Begin Extent = 
               Top = 6
               Left = 513
               Bottom = 136
               Right = 735
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_GetDispatchedAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_GetDispatchedAmount'
GO


