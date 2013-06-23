/* CONTROL PANEL ITEMS
============================================ */
ALTER TABLE [dbo].[cms_ControlPanelItems] 
   ADD NewAction nvarchar(255) NULL
GO


UPDATE [cms_ControlPanelItems]
SET NewAction = 'NewPost'
WHERE [ControlPanelItemId] = 6 -- Posts
GO

UPDATE [cms_ControlPanelItems]
SET NewAction = 'NewPage'
WHERE [ControlPanelItemId] = 8 -- Pages
GO

UPDATE [cms_ControlPanelItems]
SET NewAction = 'NewUser'
WHERE [ControlPanelItemId] = 4 -- Users
GO

UPDATE [cms_ControlPanelItems]
SET NewAction = 'NewRole'
WHERE [ControlPanelItemId] = 5 -- UserGroups
GO


/* UPGRADE VERSIONS 
============================================ */
-- update core assemblies version
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 5
GO
