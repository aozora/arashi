/****** Object:  Table [dbo].[cms_WidgetSettings]    Script Date: 01/11/2010 11:36:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* CLEANUP Unused columns 
============================================ */

	DECLARE @name NVARCHAR(32), 
			@sql NVARCHAR(1000)

	-- FIND CONSTRAINT NAME
	SELECT @name = o.name 
	FROM sysobjects AS o
	LEFT JOIN sysobjects AS t ON o.parent_obj = t.id
	WHERE ISNULL(OBJECTPROPERTY(o.id, 'IsMSShipped'), 1) = 0
	AND o.name NOT LIKE '%dtproper%'
	AND o.name NOT LIKE 'dt[_]%'
	AND t.name = 'cms_Sites'
	AND o.name LIKE 'DF__cms_Sites__IsCul%'

	-- delete if found
	IF NOT @name IS NULL BEGIN
		SELECT @sql = 'ALTER TABLE [cms_Sites] DROP CONSTRAINT [' + @name + ']'
		EXECUTE sp_executesql @sql
	END

	ALTER TABLE cms_Sites
		DROP COLUMN IsCultureSupportEnabled
GO


/* Adding new columns 
============================================ */
ALTER TABLE [dbo].[cms_Users]
	ADD AdminCulture nvarchar(50) NULL
GO	
	-- set default value for existing users
	UPDATE [dbo].[cms_Users]
	SET AdminCulture = 'en-US'

	ALTER TABLE [dbo].[cms_Users]
		ALTER COLUMN AdminCulture nvarchar(50) NOT NULL
GO



/* UPGRADE VERSIONS 
============================================ */

-- remove old assembly
DELETE FROM [cms_Versions]
GO

-- add assemblies version
INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (1, N'Arashi.Core', 1, 1, 0)
INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (2, N'Arashi.Core.Domain', 1, 1, 0)
INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (3, N'Arashi.Core.NHibernate', 1, 1, 0)
INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (10, N'Arashi.Services', 1, 1, 0)
INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (20, N'Arashi.Web', 1, 1, 0)
INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (21, N'Arashi.Web.Mvc', 1, 1, 0)
INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (30, N'Arashi.Web.Plugins', 1, 1, 0)
INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (31, N'Arashi.Web.Widgets', 1, 1, 0)
GO



/* ADD NEW CONTROL PANEL ITEM
============================================ */
INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) 
	VALUES (12, N'Content Management', 20, N'Media Manager', N'Manage your media files', N'/Resources/img/32x32/archive-32.png', N'/Resources/img/16x16/archive.png', NULL, N'MediaManager', N'Index', NULL)
GO

SET ANSI_PADDING OFF
GO
