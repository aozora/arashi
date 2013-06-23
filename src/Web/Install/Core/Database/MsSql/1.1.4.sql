/* CONTROL PANEL ITEMS
============================================ */
UPDATE [dbo].[cms_ControlPanelItems] SET [Category] = N'ContentManagement',  [Text] = N'Comments'      WHERE [ControlPanelItemId] = 0
UPDATE [dbo].[cms_ControlPanelItems] SET [Category] = N'Settings'          , [Text] = N'Themes'        WHERE [ControlPanelItemId] = 1
UPDATE [dbo].[cms_ControlPanelItems] SET [Category] = N'Settings'          , [Text] = N'Configuration' WHERE [ControlPanelItemId] = 2
UPDATE [dbo].[cms_ControlPanelItems] SET [Category] = N'Settings'          , [Text] = N'Users'         WHERE [ControlPanelItemId] = 4
UPDATE [dbo].[cms_ControlPanelItems] SET [Category] = N'Settings'          , [Text] = N'UserGroups'    WHERE [ControlPanelItemId] = 5
UPDATE [dbo].[cms_ControlPanelItems] SET [Category] = N'ContentManagement',  [Text] = N'Posts'         WHERE [ControlPanelItemId] = 6
UPDATE [dbo].[cms_ControlPanelItems] SET [Category] = N'Settings'          , [Text] = N'SEO'           WHERE [ControlPanelItemId] = 7
UPDATE [dbo].[cms_ControlPanelItems] SET [Category] = N'ContentManagement',  [Text] = N'Pages'         WHERE [ControlPanelItemId] = 8
UPDATE [dbo].[cms_ControlPanelItems] SET [Category] = N'Settings'          , [Text] = N'Widgets'       WHERE [ControlPanelItemId] = 9
UPDATE [dbo].[cms_ControlPanelItems] SET [Category] = N'Maintenance'       , [Text] = N'RebuildIndex'  WHERE [ControlPanelItemId] = 10
UPDATE [dbo].[cms_ControlPanelItems] SET [Category] = N'ContentManagement',  [Text] = N'MediaManager'  WHERE [ControlPanelItemId] = 12
UPDATE [dbo].[cms_ControlPanelItems] SET [Category] = N'Settings'          , [Text] = N'Messages'      WHERE [ControlPanelItemId] = 13


/* ROLES
============================================ */
UPDATE [dbo].[cms_Rights] SET [Name] = N'AccessAdmin'            , [RightGroup] = NULL                 WHERE [RightId] = 0
UPDATE [dbo].[cms_Rights] SET [Name] = N'SiteCreate'             , [RightGroup] = N'Site'              WHERE [RightId] = 1
UPDATE [dbo].[cms_Rights] SET [Name] = N'SiteDelete'             , [RightGroup] = N'Site'              WHERE [RightId] = 2
UPDATE [dbo].[cms_Rights] SET [Name] = N'DashboardAccess'        , [RightGroup] = NULL                 WHERE [RightId] = 3
UPDATE [dbo].[cms_Rights] SET [Name] = N'SystemConfigurationView', [RightGroup] = N'System'            WHERE [RightId] = 4
UPDATE [dbo].[cms_Rights] SET [Name] = N'SystemConfigurationEdit', [RightGroup] = N'System'            WHERE [RightId] = 5
UPDATE [dbo].[cms_Rights] SET [Name] = N'PostsView'              , [RightGroup] = N'Posts'             WHERE [RightId] = 10
UPDATE [dbo].[cms_Rights] SET [Name] = N'PostsEdit'              , [RightGroup] = N'Posts'             WHERE [RightId] = 11
UPDATE [dbo].[cms_Rights] SET [Name] = N'PostsDelete'            , [RightGroup] = N'Posts'             WHERE [RightId] = 12
UPDATE [dbo].[cms_Rights] SET [Name] = N'CommentsView'           , [RightGroup] = N'Comments'          WHERE [RightId] = 20
UPDATE [dbo].[cms_Rights] SET [Name] = N'CommentsEdit'           , [RightGroup] = N'Comments'          WHERE [RightId] = 21
UPDATE [dbo].[cms_Rights] SET [Name] = N'CommentsDelete'         , [RightGroup] = N'Comments'          WHERE [RightId] = 22
UPDATE [dbo].[cms_Rights] SET [Name] = N'PagesView'              , [RightGroup] = N'Pages'             WHERE [RightId] = 30
UPDATE [dbo].[cms_Rights] SET [Name] = N'PagesEdit'              , [RightGroup] = N'Pages'             WHERE [RightId] = 31
UPDATE [dbo].[cms_Rights] SET [Name] = N'PagesDelete'            , [RightGroup] = N'Pages'             WHERE [RightId] = 32
UPDATE [dbo].[cms_Rights] SET [Name] = N'SiteSettingsView'       , [RightGroup] = N'SiteSettings'      WHERE [RightId] = 50
UPDATE [dbo].[cms_Rights] SET [Name] = N'SiteSettingsEdit'       , [RightGroup] = N'SiteSettings'      WHERE [RightId] = 51
UPDATE [dbo].[cms_Rights] SET [Name] = N'TemplatesView'          , [RightGroup] = N'Templates'         WHERE [RightId] = 60
UPDATE [dbo].[cms_Rights] SET [Name] = N'TemplatesChange'        , [RightGroup] = N'Templates'         WHERE [RightId] = 61
UPDATE [dbo].[cms_Rights] SET [Name] = N'UsersView'              , [RightGroup] = N'Users'             WHERE [RightId] = 70
UPDATE [dbo].[cms_Rights] SET [Name] = N'UsersEdit'              , [RightGroup] = N'Users'             WHERE [RightId] = 71
UPDATE [dbo].[cms_Rights] SET [Name] = N'UsersDelete'            , [RightGroup] = N'Users'             WHERE [RightId] = 72
UPDATE [dbo].[cms_Rights] SET [Name] = N'RolesView'              , [RightGroup] = N'Roles'             WHERE [RightId] = 80
UPDATE [dbo].[cms_Rights] SET [Name] = N'RolesEdit'              , [RightGroup] = N'Roles'             WHERE [RightId] = 81
UPDATE [dbo].[cms_Rights] SET [Name] = N'RolesDelete'            , [RightGroup] = N'Roles'             WHERE [RightId] = 82
UPDATE [dbo].[cms_Rights] SET [Name] = N'FilesView'              , [RightGroup] = N'Files'             WHERE [RightId] = 90
UPDATE [dbo].[cms_Rights] SET [Name] = N'FilesEdit'              , [RightGroup] = N'Files'             WHERE [RightId] = 91
UPDATE [dbo].[cms_Rights] SET [Name] = N'FilesUpload'            , [RightGroup] = N'Files'             WHERE [RightId] = 92
UPDATE [dbo].[cms_Rights] SET [Name] = N'FilesDelete'            , [RightGroup] = N'Files'             WHERE [RightId] = 93
UPDATE [dbo].[cms_Rights] SET [Name] = N'MessagesView'           , [RightGroup] = N'Messages'          WHERE [RightId] = 94
UPDATE [dbo].[cms_Rights] SET [Name] = N'MessagesEdit'           , [RightGroup] = N'Messages'          WHERE [RightId] = 95
UPDATE [dbo].[cms_Rights] SET [Name] = N'MessagesDelete'         , [RightGroup] = N'Messages'          WHERE [RightId] = 96


/* UPGRADE VERSIONS 
============================================ */
-- update core assemblies version
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 4
GO
