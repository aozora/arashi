/****** Object:  Table [dbo].[cms_WidgetSettings]    Script Date: 01/11/2010 11:36:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* CONTENTITEM IsLogicallyDeleted Field
============================================ */
ALTER TABLE [dbo].[cms_ContentItems]
   ADD IsLogicallyDeleted bit NOT NULL DEFAULT(0)
GO

/* CONTENTITEM CUSTOM FIELDS
============================================ */
CREATE TABLE [dbo].[cms_ContentItemCustomFields](
	[ContentItemId] [int] NOT NULL,
	[Key] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_cms_ContentItemCustomFields] PRIMARY KEY CLUSTERED 
(
	[ContentItemId] ASC,
	[Key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/* ALTER cms_Pages
============================================ */
ALTER TABLE cms_Pages
   ADD CustomTemplateFile nvarchar(50) NULL
GO


/* USER AdminTheme
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
	AND t.name = 'cms_Users'
	AND o.name LIKE 'DF_cms_Users_AdminTheme%'

	-- delete if found
	IF NOT @name IS NULL BEGIN
		SELECT @sql = 'ALTER TABLE [cms_Users] DROP CONSTRAINT [' + @name + ']'
		EXECUTE sp_executesql @sql
	END


ALTER TABLE dbo.cms_Users
   ALTER COLUMN AdminTheme nvarchar(50) NULL
GO

UPDATE dbo.cms_Users
SET AdminTheme = 'arashi'
GO


/* UPGRADE VERSIONS 
============================================ */

-- update core assemblies version
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 1 WHERE [assembly] = N'Arashi.Core'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 1 WHERE [assembly] = N'Arashi.Core.Domain'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 1 WHERE [assembly] = N'Arashi.Core.NHibernate'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 1 WHERE [assembly] = N'Arashi.Services'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 1 WHERE [assembly] = N'Arashi.Web'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 1 WHERE [assembly] = N'Arashi.Web.Mvc'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 1 WHERE [assembly] = N'Arashi.Web.Plugins'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 1 WHERE [assembly] = N'Arashi.Web.Widgets'
GO


SET ANSI_PADDING OFF
GO
