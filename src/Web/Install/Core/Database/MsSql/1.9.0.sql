/*
ALTER TABLE dbo.cms_FeatureCategories
   ADD [Order] int NULL
GO

UPDATE dbo.cms_FeatureCategories
SET [Order] = 100

ALTER TABLE dbo.cms_FeatureCategories
   ALTER COLUMN [Order] int NOT NULL
GO


ALTER TABLE dbo.cms_FeatureCategories
   ADD [ImageSrc] nvarchar(255) NULL
GO

UPDATE cms_FeatureCategories SET [ImageSrc] = N'/Resources/img/64x64/kword.png'   WHERE FeatureCategoryId = 0
UPDATE cms_FeatureCategories SET [ImageSrc] = N'/Resources/img/64x64/lists2.png'   WHERE FeatureCategoryId = 1
UPDATE cms_FeatureCategories SET [ImageSrc] = N'/Resources/img/64x64/settings.png'   WHERE FeatureCategoryId = 2
UPDATE cms_FeatureCategories SET [ImageSrc] = N'/Resources/img/64x64/kchart.png'   WHERE FeatureCategoryId = 3
GO

ALTER TABLE dbo.cms_FeatureCategories
   ALTER COLUMN [ImageSrc] nvarchar(255) NOT NULL
GO

*/


/* THEMES
============================================ */
ALTER TABLE [dbo].[cms_Themes]
	ADD [Description] [nvarchar](512) NULL
GO


/* FEATURES
============================================ */
CREATE TABLE dbo.cms_FeatureCategories (
   [FeatureCategoryId] [int] NOT NULL,
   [Name] [nvarchar](50) NOT NULL ,
   [Order] int NOT NULL,
   [ImageSrc] nvarchar(255) NOT NULL,
   CONSTRAINT [PK_cms_FeatureCategories] PRIMARY KEY CLUSTERED 
   (
      [FeatureCategoryId] ASC
   )
)
GO

INSERT INTO [dbo].[cms_FeatureCategories] ([FeatureCategoryId],[Name], [Order], [ImageSrc]) VALUES(0,'ContentManagement', 100, N'/Resources/img/64x64/kword.png'   )
INSERT INTO [dbo].[cms_FeatureCategories] ([FeatureCategoryId],[Name], [Order], [ImageSrc]) VALUES(1,'Settings', 200         , N'/Resources/img/64x64/lists2.png'  )
INSERT INTO [dbo].[cms_FeatureCategories] ([FeatureCategoryId],[Name], [Order], [ImageSrc]) VALUES(2,'Statistics', 300       , N'/Resources/img/64x64/settings.png')
INSERT INTO [dbo].[cms_FeatureCategories] ([FeatureCategoryId],[Name], [Order], [ImageSrc]) VALUES(3,'Maintenance', 400      , N'/Resources/img/64x64/kchart.png'  )
GO


CREATE TABLE [dbo].[cms_Features](
	[FeaturesId] [int] NOT NULL,
	[FeatureCategoryId] [int] NOT NULL,
	--[Category] [nvarchar](255) NULL,
	[Order] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	--[Description] [nvarchar](255) NOT NULL,
	[ImageSrc] [nvarchar](255) NOT NULL,
	[LittleImageSrc] [nvarchar](255) NULL,
	[ImageAlt] [nvarchar](255) NULL,
	[Assembly] [nvarchar](255) NULL,
	[Controller] [nvarchar](255) NULL,
	[Action] [nvarchar](255) NULL,
	[Parameters] [nvarchar](255) NULL,
	[NewAction] [nvarchar](255) NULL,
   CONSTRAINT [PK_cms_Features] PRIMARY KEY CLUSTERED 
   (
      [FeaturesId] ASC
   )
)
GO
ALTER TABLE [dbo].[cms_Features]  WITH CHECK ADD  CONSTRAINT [FK_cms_Features_cms_FeatureCategories] FOREIGN KEY([FeatureCategoryId])
REFERENCES [dbo].[cms_FeatureCategories] ([FeatureCategoryId])
GO

ALTER TABLE [dbo].[cms_Features] CHECK CONSTRAINT [FK_cms_Features_cms_FeatureCategories]
GO


INSERT [dbo].[cms_Features] ([FeaturesId], [FeatureCategoryId], [Order], [Name], [ImageSrc], [LittleImageSrc], [ImageAlt], [Assembly], [Controller], [Action], [Parameters]) VALUES (0,  0,  1, N'Comments', N'/Resources/img/32x32/comments.png', N'/Resources/img/16x16/comments.png',        'comments', NULL, N'AdminComment', N'Index', NULL)
INSERT [dbo].[cms_Features] ([FeaturesId], [FeatureCategoryId], [Order], [Name], [ImageSrc], [LittleImageSrc], [ImageAlt], [Assembly], [Controller], [Action], [Parameters]) VALUES (1,  0,  0, N'Posts', N'/Resources/img/32x32/word.png', N'/Resources/img/16x16/word.png',                   'posts', NULL, N'AdminPost', N'Index', NULL)
INSERT [dbo].[cms_Features] ([FeaturesId], [FeatureCategoryId], [Order], [Name], [ImageSrc], [LittleImageSrc], [ImageAlt], [Assembly], [Controller], [Action], [Parameters]) VALUES (2,  0,  2, N'Pages', N'/Resources/img/32x32/pages.png', N'/Resources/img/16x16/pages.png',                 'pages', NULL, N'AdminPage', N'Index', NULL)
INSERT [dbo].[cms_Features] ([FeaturesId], [FeatureCategoryId], [Order], [Name], [ImageSrc], [LittleImageSrc], [ImageAlt], [Assembly], [Controller], [Action], [Parameters]) VALUES (3,  0, 20, N'Media Manager', N'/Resources/img/32x32/archive-32.png', N'/Resources/img/16x16/archive.png',  'media', NULL, N'MediaManager', N'Index', NULL)
INSERT [dbo].[cms_Features] ([FeaturesId], [FeatureCategoryId], [Order], [Name], [ImageSrc], [LittleImageSrc], [ImageAlt], [Assembly], [Controller], [Action], [Parameters]) VALUES (4,  0, 11, N'Categories', N'/Resources/img/32x32/category.png', N'/Resources/img/16x16/category.png',      'categories', NULL, N'AdminCategory', N'Index', NULL)
INSERT [dbo].[cms_Features] ([FeaturesId], [FeatureCategoryId], [Order], [Name], [ImageSrc], [LittleImageSrc], [ImageAlt], [Assembly], [Controller], [Action], [Parameters]) VALUES (5,  0, 10, N'Tags', N'/Resources/img/32x32/tag.png', N'/Resources/img/16x16/tags.png',                      'tags', NULL, N'AdminTag', N'Index', NULL)
INSERT [dbo].[cms_Features] ([FeaturesId], [FeatureCategoryId], [Order], [Name], [ImageSrc], [LittleImageSrc], [ImageAlt], [Assembly], [Controller], [Action], [Parameters]) VALUES (10, 1, 12, N'Themes', N'/Resources/img/32x32/themes.png', N'/Resources/img/16x16/themes.png',              'themes', NULL, N'Themes', N'Index', NULL)
INSERT [dbo].[cms_Features] ([FeaturesId], [FeatureCategoryId], [Order], [Name], [ImageSrc], [LittleImageSrc], [ImageAlt], [Assembly], [Controller], [Action], [Parameters]) VALUES (11, 1, 10, N'Configuration', N'/Resources/img/32x32/settings.png', N'/Resources/img/16x16/settings.png',   'settings', NULL, N'Site', N'Settings', NULL)
INSERT [dbo].[cms_Features] ([FeaturesId], [FeatureCategoryId], [Order], [Name], [ImageSrc], [LittleImageSrc], [ImageAlt], [Assembly], [Controller], [Action], [Parameters]) VALUES (12, 1, 20, N'Users', N'/Resources/img/32x32/users.png', N'/Resources/img/16x16/users.png',                 'users', NULL, N'Users', N'Index', NULL)
INSERT [dbo].[cms_Features] ([FeaturesId], [FeatureCategoryId], [Order], [Name], [ImageSrc], [LittleImageSrc], [ImageAlt], [Assembly], [Controller], [Action], [Parameters]) VALUES (13, 1, 21, N'User Groups', N'/Resources/img/32x32/group.png', N'/Resources/img/16x16/group.png',           'groups', NULL, N'Roles', N'Index', NULL)
INSERT [dbo].[cms_Features] ([FeaturesId], [FeatureCategoryId], [Order], [Name], [ImageSrc], [LittleImageSrc], [ImageAlt], [Assembly], [Controller], [Action], [Parameters]) VALUES (14, 1, 11, N'SEO', N'/Resources/img/32x32/seo.jpg', N'/Resources/img/16x16/seo.jpg',                       'seo', NULL, N'Seo', N'Index', NULL)
INSERT [dbo].[cms_Features] ([FeaturesId], [FeatureCategoryId], [Order], [Name], [ImageSrc], [LittleImageSrc], [ImageAlt], [Assembly], [Controller], [Action], [Parameters]) VALUES (15, 1, 13, N'Widgets', N'/Resources/img/32x32/widgets.png', N'/Resources/img/16x16/widgets.png',           'widgets', NULL, N'Widgets', N'Index', NULL)
INSERT [dbo].[cms_Features] ([FeaturesId], [FeatureCategoryId], [Order], [Name], [ImageSrc], [LittleImageSrc], [ImageAlt], [Assembly], [Controller], [Action], [Parameters]) VALUES (16, 1, 22, N'Messages', N'/Resources/img/32x32/mailbox.png', N'/Resources/img/16x16/mailbox.png',          'messages', NULL, N'Messages', N'Index', NULL)
INSERT [dbo].[cms_Features] ([FeaturesId], [FeatureCategoryId], [Order], [Name], [ImageSrc], [LittleImageSrc], [ImageAlt], [Assembly], [Controller], [Action], [Parameters]) VALUES (20, 2,  2, N'GoogleAnalytics', N'/Resources/img/32x32/charts.png', N'/Resources/img/16x16/charts.png',     'ga', NULL, N'GoogleData', N'Index', NULL)
INSERT [dbo].[cms_Features] ([FeaturesId], [FeatureCategoryId], [Order], [Name], [ImageSrc], [LittleImageSrc], [ImageAlt], [Assembly], [Controller], [Action], [Parameters]) VALUES (30, 3, 20, N'Rebuild Index', N'/Resources/img/32x32/searchdb.png', N'/Resources/img/16x16/searchdb.png',   'indexes', NULL, N'SearchIndex', N'Index', NULL)



CREATE TABLE [dbo].[cms_SiteFeatures](
	[SiteFeatureId] [int] NOT NULL,
	[SiteId] [int] NOT NULL,
	[FeatureId] [int] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
   CONSTRAINT [PK_cms_SiteFeatures_1] PRIMARY KEY CLUSTERED 
   (
	   [SiteFeatureId] ASC
   )
) 
GO
ALTER TABLE [dbo].[cms_SiteFeatures]  WITH CHECK ADD  CONSTRAINT [FK_cms_SiteFeatures_cms_Features] FOREIGN KEY([FeatureId])
REFERENCES [dbo].[cms_Features] ([FeatureId])
GO
ALTER TABLE [dbo].[cms_SiteFeatures] CHECK CONSTRAINT [FK_cms_SiteFeatures_cms_Features]
GO
ALTER TABLE [dbo].[cms_SiteFeatures]  WITH CHECK ADD  CONSTRAINT [FK_cms_SiteFeatures_cms_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[cms_Sites] ([SiteId])
GO
ALTER TABLE [dbo].[cms_SiteFeatures] CHECK CONSTRAINT [FK_cms_SiteFeatures_cms_Sites]
GO


   





/* UPGRADE VERSIONS 
============================================ */
-- update core assemblies version
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 9, [patch] = 0
GO
