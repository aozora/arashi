/* CONTROL PANEL ITEMS
============================================ */

ALTER TABLE [dbo].[cms_ControlPanelItems] 
   ADD IsExpanded bit NOT NULL DEFAULT(1)
GO

UPDATE [dbo].[cms_ControlPanelItems] 
SET IsExpanded = 0
WHERE [Category] IN (N'Settings', N'Maintenance')
GO

INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) 
   VALUES (14, N'Statistics', 3, N'GoogleAnalytics', N'View the Google statistics without leaving this site', N'/Resources/img/32x32/charts.png', N'/Resources/img/16x16/charts.png', NULL, N'GoogleData', N'Index', NULL)
GO

UPDATE [dbo].[cms_ControlPanelItems] 
SET [NewAction] = 'Upload'
WHERE [Text] = 'MediaManager'
GO





/* UPGRADE VERSIONS 
============================================ */
-- update core assemblies version
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 7
GO
