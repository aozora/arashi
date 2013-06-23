/* CONVERT old varchar types to nvarchar
============================================ */
DROP INDEX IX_cms_Users_EmailSiteId ON [cms_Users]
GO
ALTER TABLE [cms_Users] ALTER COLUMN [Email] [nvarchar](100) NOT NULL
ALTER TABLE [cms_Users] ALTER COLUMN [WebSite] [nvarchar](100) NULL
CREATE UNIQUE NONCLUSTERED INDEX [IX_cms_Users_EmailSiteId] ON [dbo].[cms_Users] 
(
	[Email] ASC,
	[SiteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

DROP INDEX IX_cms_SiteHosts ON [cms_SiteHosts]
GO
DROP INDEX [IX_cms_SiteHosts_HostName] ON [cms_SiteHosts]
GO
ALTER TABLE [cms_SiteHosts] ALTER COLUMN [HostName] [nvarchar](100) NOT NULL
CREATE NONCLUSTERED INDEX [IX_cms_SiteHosts] ON [dbo].[cms_SiteHosts] 
(
	[HostName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_cms_SiteHosts_HostName] ON [dbo].[cms_SiteHosts] 
(
	[HostName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [cms_Roles] ALTER COLUMN [Name] [nvarchar](255) NOT NULL
GO

DROP INDEX IX_cms_Users ON [cms_Users]
GO
DROP INDEX [IX_cms_Users_EmailSiteId] ON [cms_Users]
GO
ALTER TABLE [cms_Users] ALTER COLUMN [Email] [nvarchar](100) NOT NULL
ALTER TABLE [cms_Users] ALTER COLUMN [DisplayName] [nvarchar](100) NULL
ALTER TABLE [cms_Users] ALTER COLUMN [Description] [nvarchar](max) NULL
ALTER TABLE [cms_Users] ALTER COLUMN [WebSite] [nvarchar](100) NULL
CREATE UNIQUE NONCLUSTERED INDEX [IX_cms_Users] ON [dbo].[cms_Users] 
(
	[SiteId] ASC,
	[DisplayName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_cms_Users_EmailSiteId] ON [dbo].[cms_Users] 
(
	[Email] ASC,
	[SiteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

DROP INDEX [IX_cms_Templates] ON [cms_Templates]
GO
ALTER TABLE [cms_Templates] ALTER COLUMN [Name] [nvarchar](255) NOT NULL
ALTER TABLE [cms_Templates] ALTER COLUMN [BasePath] [nvarchar](255) NOT NULL
ALTER TABLE [cms_Templates] ALTER COLUMN [ThumbnailSrc] [nvarchar](255) NULL
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_cms_Templates] ON [dbo].[cms_Templates] 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [cms_ControlPanelItems] ALTER COLUMN [Category] [nvarchar](255) NULL
ALTER TABLE [cms_ControlPanelItems] ALTER COLUMN [Text] [nvarchar](50) NOT NULL
ALTER TABLE [cms_ControlPanelItems] ALTER COLUMN [Description] [nvarchar](255) NOT NULL
ALTER TABLE [cms_ControlPanelItems] ALTER COLUMN [ImageSrc] [nvarchar](255) NOT NULL
ALTER TABLE [cms_ControlPanelItems] ALTER COLUMN [LittleImageSrc] [nvarchar](255) NULL
ALTER TABLE [cms_ControlPanelItems] ALTER COLUMN [ImageAlt] [nvarchar](255) NULL
ALTER TABLE [cms_ControlPanelItems] ALTER COLUMN [Controller] [nvarchar](255) NULL
ALTER TABLE [cms_ControlPanelItems] ALTER COLUMN [Action] [nvarchar](255) NULL
ALTER TABLE [cms_ControlPanelItems] ALTER COLUMN [Parameters] [nvarchar](255) NULL
GO
ALTER TABLE [cms_Versions] ALTER COLUMN [assembly] [nvarchar](255) NOT NULL
GO
ALTER TABLE [cms_TrackingInfos] ALTER COLUMN [HostReferrer] [nvarchar](255) NULL
ALTER TABLE [cms_TrackingInfos] ALTER COLUMN [UrlReferrer] [nvarchar](2068) NULL
ALTER TABLE [cms_TrackingInfos] ALTER COLUMN [TrackedUrl] [nvarchar](2068) NOT NULL
ALTER TABLE [cms_TrackingInfos] ALTER COLUMN [HttpMethod] [nvarchar](10) NOT NULL
ALTER TABLE [cms_TrackingInfos] ALTER COLUMN [AnonymousUserId] [nchar](36) NULL
ALTER TABLE [cms_TrackingInfos] ALTER COLUMN [UserIP] [nvarchar](40) NULL
ALTER TABLE [cms_TrackingInfos] ALTER COLUMN [UserLanguages] [nvarchar](255) NULL
ALTER TABLE [cms_TrackingInfos] ALTER COLUMN [BrowserType] [nvarchar](255) NULL
ALTER TABLE [cms_TrackingInfos] ALTER COLUMN [BrowserName] [nvarchar](255) NULL
ALTER TABLE [cms_TrackingInfos] ALTER COLUMN [BrowserVersion] [nvarchar](20) NULL
ALTER TABLE [cms_TrackingInfos] ALTER COLUMN [BrowserMajor] [nvarchar](5) NULL
ALTER TABLE [cms_TrackingInfos] ALTER COLUMN [BrowserMinor] [nvarchar](15) NULL
ALTER TABLE [cms_TrackingInfos] ALTER COLUMN [Platform] [nvarchar](255) NULL
GO

DROP INDEX [IX_cms_WidgetTypes] ON [cms_WidgetTypes]
GO
ALTER TABLE [cms_WidgetTypes] ALTER COLUMN [Name] [nvarchar](100) NOT NULL
ALTER TABLE [cms_WidgetTypes] ALTER COLUMN [AssemblyName] [nvarchar](100) NOT NULL
ALTER TABLE [cms_WidgetTypes] ALTER COLUMN [ClassName] [nvarchar](255) NOT NULL
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_cms_WidgetTypes] ON [dbo].[cms_WidgetTypes] 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


ALTER TABLE [cms_Versions] ALTER COLUMN [assembly] [nvarchar](255) NOT NULL
GO



/* cms_Users ExternalId Field
============================================ */
ALTER TABLE [dbo].[cms_Users]
   ADD ExternalId          nvarchar(255) NULL
GO
ALTER TABLE [dbo].[cms_Users]
   ADD ExternalProviderUri nvarchar(255) NULL
GO

/* cms_SystemConfiguration support for Facebook App
===================================================== */
ALTER TABLE [dbo].[cms_SystemConfiguration]
   ADD FacebookAppId          nvarchar(50) NULL,
       FacebookApiKey         nvarchar(50) NULL,
       FacebookApiSecret      nvarchar(50) NULL,
       FacebookCookieSupport  bit NULL
GO


/* UPGRADE VERSIONS 
============================================ */

INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (4, N'Arashi.Core.NHibernate.Wcf',  1, 1, 3)

-- update core assemblies version
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 3 WHERE [assembly] = N'Arashi.Core'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 3 WHERE [assembly] = N'Arashi.Core.Domain'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 3 WHERE [assembly] = N'Arashi.Core.NHibernate'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 3 WHERE [assembly] = N'Arashi.Core.NHibernate.Wcf'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 3 WHERE [assembly] = N'Arashi.Services'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 3 WHERE [assembly] = N'Arashi.Web'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 3 WHERE [assembly] = N'Arashi.Web.Mvc'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 3 WHERE [assembly] = N'Arashi.Web.Plugins'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 3 WHERE [assembly] = N'Arashi.Web.Widgets'
GO


SET ANSI_PADDING OFF
GO
