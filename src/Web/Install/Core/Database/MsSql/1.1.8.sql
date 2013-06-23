/* CONTROL PANEL ITEMS
============================================ */

ALTER TABLE [dbo].[cms_ControlPanelItems] 
   ADD HomePageEditAction nvarchar(255) NULL 
GO

INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) 
   VALUES (16, N'ContentManagement', 10, N'Tags', N'View and manage the tags associated to your posts', N'/Resources/img/32x32/tag.png', N'/Resources/img/16x16/tag.png', NULL, N'AdminTag', N'Index', NULL)
GO
INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) 
   VALUES (15, N'ContentManagement', 11, N'Categories', N'View and manage the categories associated to your posts', N'/Resources/img/32x32/category.png', N'/Resources/img/16x16/category.png', NULL, N'AdminCategory', N'Index', NULL)
GO


/* THEMES
============================================ */

-- create new table themes
CREATE TABLE [dbo].[cms_Themes](
	[ThemeId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[BasePath] [nvarchar](255) NOT NULL,
	[ThumbnailSrc] [nvarchar](255) NULL,
   [CustomOptionsAssembly] [nvarchar](255) NULL,
   [CustomOptionsController] [nvarchar](255) NULL,
   [CustomOptionsAction] [nvarchar](255) NULL,
 CONSTRAINT [PK_cms_themes] PRIMARY KEY CLUSTERED 
(
	[ThemeId] ASC
)
) 
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_cms_Themes] ON [dbo].[cms_Themes] 
(
	[Name] ASC
) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


INSERT INTO [Arashi].[dbo].[cms_Themes] ([ThemeId], [Name], [BasePath], [ThumbnailSrc])
     SELECT [TemplateId], [Name], [BasePath], [ThumbnailSrc] FROM [cms_Templates]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Templates_cms_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Templates]'))
   ALTER TABLE [dbo].[cms_Templates] DROP CONSTRAINT [FK_cms_Templates_cms_Sites]
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Sites_cms_Templates]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
   ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [FK_cms_Sites_cms_Templates]
GO

ALTER TABLE dbo.cms_Sites
   ADD [ThemeId] [int] NULL
GO

ALTER TABLE [dbo].[cms_Sites]  WITH CHECK 
   ADD CONSTRAINT [FK_cms_Sites_cms_Themes] FOREIGN KEY([ThemeId])
   REFERENCES [dbo].[cms_Themes] ([ThemeId])
GO

UPDATE dbo.cms_Sites
SET [ThemeId] = TemplateId
GO

ALTER TABLE dbo.cms_Sites
   DROP COLUMN TemplateId
GO

ALTER TABLE dbo.cms_SiteHosts
   DROP COLUMN TemplateId
GO

ALTER TABLE dbo.cms_SiteHosts
   ADD [ThemeId] [int] NULL
GO

-- drop old table
DROP TABLE [cms_Templates]
GO


-- change themes basepath
UPDATE [cms_Themes]
SET BasePath = REPLACE(BasePath, '/Templates', '/themes')


-- remove old themes
DELETE FROM [cms_Themes]
WHERE Name NOT IN ('default', 'boldy')

UPDATE [cms_Themes]
SET [CustomOptionsAssembly] = 'dbo.cms_Themes'
   ,[CustomOptionsController] = 'Boldy'
   ,[CustomOptionsAction] = 'Index'
WHERE Name = 'boldy'


/* SITE OPTIONS 
============================================ */
-- create new table themes
CREATE TABLE [dbo].[cms_SiteOptions](
	[SiteId] [int] NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Value] [nvarchar](2000) NULL,
 CONSTRAINT [PK_cms_SiteOptions_1] PRIMARY KEY CLUSTERED 
(
	[SiteId] ASC,
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[cms_SiteOptions]  WITH CHECK ADD  CONSTRAINT [FK_cms_SiteOptions_cms_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[cms_Sites] ([SiteId])
GO

ALTER TABLE [dbo].[cms_SiteOptions] CHECK CONSTRAINT [FK_cms_SiteOptions_cms_Sites]
GO



/* CONTENT ITEMS
============================================ */
ALTER TABLE dbo.cms_ContentItems
   ADD MetaDescription nvarchar(160) NULL
GO

ALTER TABLE dbo.cms_ContentItems
   ADD MetaKeywords nvarchar(1000) NULL
GO

ALTER TABLE dbo.cms_ContentItems
   ADD EnableMeta bit DEFAULT(0) NOT NULL
GO



/* USERS
============================================ */
ALTER TABLE dbo.cms_Users
   ADD AdminEditor int DEFAULT(0) NOT NULL
GO




/* UPGRADE VERSIONS 
============================================ */
-- update core assemblies version
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 8
GO
