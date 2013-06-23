/* CONTROL PANEL ITEMS
============================================ */
ALTER TABLE [dbo].[cms_Sites] ADD GoogleDataUserName  nvarchar(255) NULL
ALTER TABLE [dbo].[cms_Sites] ADD GoogleDataPassword  nvarchar(255) NULL
ALTER TABLE [dbo].[cms_Sites] ADD GoogleDataProfileID nvarchar(255) NULL
GO




/* UPGRADE VERSIONS 
============================================ */
-- update core assemblies version
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 6
GO
