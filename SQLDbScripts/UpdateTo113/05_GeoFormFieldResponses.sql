USE [GRASP]
GO

/****** Object:  View [dbo].[GeoFormFieldResponses]    Script Date: 11/19/2014 10:10:22 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[GeoFormFieldResponses]'))
DROP VIEW [dbo].[GeoFormFieldResponses]
GO

USE [GRASP]
GO

/****** Object:  View [dbo].[GeoFormFieldResponses]    Script Date: 11/19/2014 10:10:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GeoFormFieldResponses]
AS
SELECT     dbo.FormFieldResponses.*, dbo.ResponseMapping.FormName, dbo.ResponseMapping.FormID, dbo.ResponseMapping.ResponseID, 
                      dbo.ResponseMapping.FRCreateDate AS Expr1, dbo.ResponseMapping.FRCoordGeometry, dbo.FormFieldResponses.value AS Expr2, 
                      dbo.FormFieldResponses.RVRepeatCount AS Expr3, dbo.FormFieldResponses.formFieldId AS Expr4, dbo.FormFieldResponses.label AS Expr5, 
                      dbo.FormFieldResponses.name AS Expr6, dbo.FormFieldResponses.type AS Expr7, dbo.FormFieldResponses.positionIndex AS Expr8, 
                      dbo.FormFieldResponses.senderMsisdn AS Expr9, dbo.FormFieldResponses.nvalue AS Expr10, dbo.FormFieldResponses.dvalue AS Expr11, 
                      dbo.FormFieldResponses.ResponseStatusID AS Expr12
FROM         dbo.ResponseMapping INNER JOIN
                      dbo.FormFieldResponses ON dbo.ResponseMapping.ResponseID = dbo.FormFieldResponses.FormResponseID

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[56] 4[7] 2[13] 3) )"
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
         Begin Table = "ResponseMapping"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 184
               Right = 207
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FormFieldResponses"
            Begin Extent = 
               Top = 9
               Left = 363
               Bottom = 250
               Right = 532
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
      Begin ColumnWidths = 10
         Width = 284
         Width = 1500
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
      Begin ColumnWidths = 11
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'GeoFormFieldResponses'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'GeoFormFieldResponses'
GO
