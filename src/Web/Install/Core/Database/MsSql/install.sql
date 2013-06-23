/****** Object:  Table [dbo].[cms_WidgetSettings]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_WidgetSettings](
	[WidgetSettingId] [int] NOT NULL,
	[WidgetId] [int] NOT NULL,
	[Key] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_cms_WidgetSettings] PRIMARY KEY CLUSTERED 
(
	[WidgetSettingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_cms_WidgetSettings] ON [dbo].[cms_WidgetSettings] 
(
	[WidgetId] ASC,
	[Key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Users]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cms_Users](
	[UserId] [int] NOT NULL,
	[SiteId] [int] NULL,
	[Password] [varchar](100) NOT NULL,
	[FirstName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[TimeZone] [int] NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[DisplayName] [varchar](100) NULL,
	[Description] [nvarchar](max) NULL,
	[WebSite] [varchar](100) NULL,
	[IsActive] [bit] NULL,
	[LastLogin] [datetime] NULL,
	[LastIp] [varchar](40) NULL,
	[PasswordQuestion] [varchar](255) NULL,
	[PasswordAnswer] [varchar](255) NULL,
	[FailedPasswordAttemptCount] [int] NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NULL,
	[FailedPasswordAnswerAttemptCount] [int] NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NULL,
	[IsLogicallyDeleted] [bit] NOT NULL,
	[AdminTheme] [nvarchar](50) NULL,
	[AdminCulture] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[Version] [int] NOT NULL,
 CONSTRAINT [PK_cms_users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
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
/****** Object:  Table [dbo].[cms_SiteHosts]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cms_SiteHosts](
	[SiteHostId] [int] NOT NULL,
	[SiteId] [int] NOT NULL,
	[HostName] [varchar](100) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[TemplateId] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_cms_sitehosts] PRIMARY KEY CLUSTERED 
(
	[SiteHostId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
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
/****** Object:  Table [dbo].[cms_SeoSettings]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_SeoSettings](
	[SeoSettingsId] [int] NOT NULL,
	[SiteId] [int] NOT NULL,
	[HomeTitle] [nvarchar](70) NULL,
	[HomeDescription] [nvarchar](160) NULL,
	[HomeKeywords] [nvarchar](255) NULL,
	[RewriteTitles] [bit] NULL,
	[PostTitleFormat] [nvarchar](50) NULL,
	[PageTitleFormat] [nvarchar](50) NULL,
	[CategoryTitleFormat] [nvarchar](50) NULL,
	[TagTitleFormat] [nvarchar](50) NULL,
	[SearchTitleFormat] [nvarchar](50) NULL,
	[ArchiveTitleFormat] [nvarchar](50) NULL,
	[Page404TitleFormat] [nvarchar](50) NULL,
	[DescriptionFormat] [nvarchar](50) NULL,
	[UseCategoriesForMeta] [bit] NULL,
	[GenerateKeywordsForPost] [bit] NULL,
	[UseNoIndexForCategories] [bit] NULL,
	[UseNoIndexForArchives] [bit] NULL,
	[UseNoIndexForTags] [bit] NULL,
	[GenerateDescriptions] [bit] NULL,
	[CapitalizeCategoryTitles] [bit] NULL,
 CONSTRAINT [PK_cms_SeoSettings] PRIMARY KEY CLUSTERED 
(
	[SeoSettingsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_UserRoles]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_UserRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_cms_UserRoles_1] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Roles]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cms_Roles](
	[RoleId] [int] NOT NULL,
	[SiteId] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
 CONSTRAINT [PK_cms_roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cms_Categories]    Script Date: 03/30/2010 13:10:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_Categories](
	[CategoryId] [int] NOT NULL,
	[SiteId] [int] NOT NULL,
	[ParentCategoryId] [int] NULL,
	[Path] [nvarchar](80) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[FriendlyName] [nvarchar](100) NULL,
	[Position] [int] NOT NULL,
 CONSTRAINT [PK_cms_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Comments]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_Comments](
	[CommentId] [int] NOT NULL,
	[ContentItemId] [int] NOT NULL,
	[UserId] [int] NULL,
	[Name] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NULL,
	[Url] [nvarchar](100) NULL,
	[CommentText] [nvarchar](2000) NOT NULL,
	[UserIp] [nvarchar](15) NULL,
	[UserAgent] [nvarchar](255) NULL,
	[CommentStatus] [int] NOT NULL,
	[CommentType] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [PK_cms_Comments] PRIMARY KEY CLUSTERED 
(
	[CommentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_CategoriesContentItems]    Script Date: 03/30/2010 13:10:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_CategoriesContentItems](
	[CategoryId] [int] NOT NULL,
	[ContentItemId] [int] NOT NULL,
 CONSTRAINT [PK_cms_CategoriesContentItems] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC,
	[ContentItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_ContentItems]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_ContentItems](
	[ContentItemId] [int] NOT NULL,
	[GlobalId] [nvarchar](255) NOT NULL,
	[WorkflowStatus] [int] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[FriendlyName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Version] [int] NOT NULL,
	[Culture] [nvarchar](5) NOT NULL,
	[SiteId] [int] NOT NULL,
	[AllowComments] [bit] NOT NULL,
	[AllowPings] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[PublishedDate] [datetime] NULL,
	[PublishedUntil] [datetime] NULL,
	[Author] [int] NOT NULL,
	[UpdatedBy] [int] NULL,
	[PublishedBy] [int] NULL,
 CONSTRAINT [PK_cms_ContentItem] PRIMARY KEY CLUSTERED 
(
	[ContentItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_cms_ContentItems] ON [dbo].[cms_ContentItems] 
(
	[FriendlyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_cms_ContentItems_1] ON [dbo].[cms_ContentItems] 
(
	[CreatedDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_QuotesOfTheDay]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_QuotesOfTheDay](
	[QuoteOfTheDayId] [int] NOT NULL,
	[SiteId] [int] NOT NULL,
	[Quote] [nvarchar](2000) NULL,
	[Author] [nvarchar](50) NULL,
 CONSTRAINT [PK_cms_QuotesOfTheDay] PRIMARY KEY CLUSTERED 
(
	[QuoteOfTheDayId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Posts]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_Posts](
	[ContentItemId] [int] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_cms_Posts] PRIMARY KEY CLUSTERED 
(
	[ContentItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Pages]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_Pages](
	[ContentItemId] [int] NOT NULL,
	[ParentPageId] [int] NULL,
	[Position] [int] NOT NULL,
   [CustomTemplateFile] nvarchar(50) NULL,
	[Content] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_cms_Pages_1] PRIMARY KEY CLUSTERED 
(
	[ContentItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Sites]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_Sites](
	[SiteId] [int] NOT NULL,
	[Name] [nvarchar](70) NOT NULL,
	[Description] [nvarchar](160) NULL,
	[TemplateId] [int] NULL,
	[SeoSettingsId] [int] NULL,
	[AllowRegistration] [bit] NOT NULL,
	[AllowPasswordRetrieval] [bit] NOT NULL,
	[EnableAnonymous] [bit] NOT NULL,
	[DefaultPage] [int] NULL,
	[DefaultCulture] [nvarchar](50) NOT NULL,
	[TimeZone] [int] NOT NULL,
	[DateFormat] [nvarchar](20) NULL,
	[TimeFormat] [nvarchar](20) NULL,
	[RoleId] [int] NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Status] [int] NOT NULL,
	[TrackingCode] [nvarchar](255) NULL,
	[MaxPostsPerPage] [int] NULL,
	[MaxSyndicationFeeds] [int] NULL,
	[FeedUseSummary] [bit] NOT NULL,
	[AllowPings] [bit] NOT NULL,
	[AllowComments] [bit] NOT NULL,
	[AllowCommentsOnlyForRegisteredUsers] [bit] NOT NULL,
	[MaxCommentsPerPage] [int] NULL,
	[SortCommentsFromOlderToNewest] [bit] NOT NULL,
	[SendEmailForNewComment] [bit] NOT NULL,
	[SendEmailForNewModeration] [bit] NOT NULL,
	[ShowAvatars] [bit] NOT NULL,
	[MaxLinksInComments] [int] NOT NULL,
	[ModerationKeys] [nvarchar](2000) NULL,
	[BlacklistKeys] [nvarchar](2000) NULL,
	[EnableCaptchaForComments] [bit] NOT NULL,
	[CaptchaPrivateKey] [nvarchar](50) NULL,
	[CaptchaPublicKey] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[Version] [int] NOT NULL,
 CONSTRAINT [PK_cms_sites] PRIMARY KEY CLUSTERED 
(
	[SiteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Templates]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cms_Templates](
	[TemplateId] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[SiteId] [int] NULL,
	[BasePath] [varchar](255) NOT NULL,
	[ThumbnailSrc] [varchar](255) NULL,
 CONSTRAINT [PK_cms_templates] PRIMARY KEY CLUSTERED 
(
	[TemplateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_cms_Templates] ON [dbo].[cms_Templates] 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_TagsContentItems]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_TagsContentItems](
	[TagId] [int] NOT NULL,
	[ContentItemId] [int] NOT NULL,
 CONSTRAINT [PK_cms_TagsContentItems_1] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC,
	[ContentItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Tags]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_Tags](
	[TagId] [int] NOT NULL,
	[SiteId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[FriendlyName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_cms_Tags] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_SystemConfiguration]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_SystemConfiguration](
	[SystemConfigurationId] [int] NOT NULL,
	[SmtpHost] [nvarchar](50) NULL,
	[SmtpHostPort] [int] NULL,
	[SmtpRequireSSL] [bit] NULL,
	[SmtpUserName] [nvarchar](255) NULL,
	[SmtpUserPassword] [nvarchar](255) NULL,
	[SmtpDomain] [nvarchar](255) NULL,
 CONSTRAINT [PK_cms_SystemConfiguration] PRIMARY KEY CLUSTERED 
(
	[SystemConfigurationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_ControlPanelItems]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cms_ControlPanelItems](
	[ControlPanelItemId] [int] NOT NULL,
	[Category] [varchar](255) NULL,
	[ViewOrder] [int] NOT NULL,
	[Text] [varchar](50) NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[ImageSrc] [varchar](255) NOT NULL,
	[LittleImageSrc] [varchar](255) NULL,
	[ImageAlt] [varchar](255) NULL,
	[Controller] [varchar](255) NULL,
	[Action] [varchar](255) NULL,
	[Parameters] [varchar](255) NULL,
 CONSTRAINT [PK_cms_ControlPanelItems] PRIMARY KEY CLUSTERED 
(
	[ControlPanelItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cms_ContentItemRoles]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_ContentItemRoles](
	[ContentItemRoleId] [int] NOT NULL,
	[ContentItemId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[ViewAllowed] [bit] NOT NULL,
	[EditAllowed] [bit] NOT NULL,
 CONSTRAINT [PK_cms_ContentItemRole] PRIMARY KEY CLUSTERED 
(
	[ContentItemRoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_cms_ContentItemRole_roleid_contentitemid] ON [dbo].[cms_ContentItemRoles] 
(
	[RoleId] ASC,
	[ContentItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_RoleRights]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_RoleRights](
	[RoleId] [int] NOT NULL,
	[RightId] [int] NOT NULL,
 CONSTRAINT [PK_cms_RoleRights] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[RightId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Rights]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_Rights](
	[RightId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[RightGroup] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_cms_Rights] PRIMARY KEY CLUSTERED 
(
	[RightId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UC_right_name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Versions]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cms_Versions](
	[versionid] [int] NOT NULL,
	[assembly] [varchar](255) NOT NULL,
	[major] [int] NOT NULL,
	[minor] [int] NOT NULL,
	[patch] [int] NOT NULL,
 CONSTRAINT [PK_cms_Versions] PRIMARY KEY CLUSTERED 
(
	[versionid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cms_TrackingInfos]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cms_TrackingInfos](
	[TrackingInfoId] [int] NOT NULL,
	[HostReferrer] [varchar](255) NULL,
	[UrlReferrer] [varchar](2068) NULL,
	[TrackedUrl] [varchar](2068) NOT NULL,
	[HttpMethod] [varchar](10) NOT NULL,
	[LoggedUserId] [int] NULL,
	[AnonymousUserId] [char](36) NULL,
	[UserIP] [varchar](40) NULL,
	[UserLanguages] [varchar](255) NULL,
	[BrowserType] [varchar](255) NULL,
	[BrowserName] [varchar](255) NULL,
	[BrowserVersion] [varchar](20) NULL,
	[BrowserMajor] [varchar](5) NULL,
	[BrowserMinor] [varchar](15) NULL,
	[Platform] [varchar](255) NULL,
	[TrackingDate] [datetime] NOT NULL,
 CONSTRAINT [PK_cms_TrackingInfos] PRIMARY KEY CLUSTERED 
(
	[TrackingInfoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ELMAH_Error]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ELMAH_Error](
	[ErrorId] [uniqueidentifier] NOT NULL,
	[Application] [nvarchar](60) NOT NULL,
	[Host] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](100) NOT NULL,
	[Source] [nvarchar](60) NOT NULL,
	[Message] [nvarchar](500) NOT NULL,
	[User] [nvarchar](50) NOT NULL,
	[StatusCode] [int] NOT NULL,
	[TimeUtc] [datetime] NOT NULL,
	[Sequence] [int] IDENTITY(1,1) NOT NULL,
	[AllXml] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ELMAH_Error] PRIMARY KEY NONCLUSTERED 
(
	[ErrorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ELMAH_Error_App_Time_Seq] ON [dbo].[ELMAH_Error] 
(
	[Application] ASC,
	[TimeUtc] DESC,
	[Sequence] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Widgets]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_Widgets](
	[WidgetId] [int] NOT NULL,
	[WidgetTypeId] [int] NOT NULL,
	[SiteId] [int] NOT NULL,
	[Title] [nvarchar](50) NULL,
	[PlaceHolder] [int] NOT NULL,
	[Position] [int] NOT NULL,
 CONSTRAINT [PK_cms_Widgets] PRIMARY KEY CLUSTERED 
(
	[WidgetId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_WidgetTypes]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cms_WidgetTypes](
	[WidgetTypeId] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[AssemblyName] [varchar](100) NOT NULL,
	[ClassName] [varchar](255) NOT NULL,
 CONSTRAINT [PK_cms_WidgetTypes] PRIMARY KEY CLUSTERED 
(
	[WidgetTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_cms_WidgetTypes] ON [dbo].[cms_WidgetTypes] 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


CREATE TABLE [dbo].[cms_Messages](
	[MessageId] [int] NOT NULL,
	[SiteId] [int] NOT NULL,
	[From] [nvarchar](255) NOT NULL,
	[To] [nvarchar](2000) NOT NULL,
	[Cc] [nvarchar](2000) NULL,
	[Bcc] [nvarchar](2000) NULL,
	[Subject] [nvarchar](255) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[Status] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[AttemptsCount] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_cms_Messages] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [IX_cms_Messages] ON [dbo].[cms_Messages] 
(
	[Status] ASC,
	[Type] ASC,
	[SiteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[cms_Messages]  WITH CHECK ADD  CONSTRAINT [FK_cms_Messages_cms_Sites] FOREIGN KEY([SiteId])
   REFERENCES [dbo].[cms_Sites] ([SiteId])
GO

ALTER TABLE [dbo].[cms_Messages] CHECK CONSTRAINT [FK_cms_Messages_cms_Sites]
GO

ALTER TABLE [dbo].[cms_Messages] ADD  CONSTRAINT [DF__cms_Messa__Attem__31632898]  DEFAULT ((0)) FOR [AttemptsCount]
GO




/****** Object:  Table [dbo].[hibernate_unique_key]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hibernate_unique_key](
	[next_hi] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_LogError]    Script Date: 03/30/2010 13:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ELMAH_LogError]
(
    @ErrorId UNIQUEIDENTIFIER,
    @Application NVARCHAR(60),
    @Host NVARCHAR(30),
    @Type NVARCHAR(100),
    @Source NVARCHAR(60),
    @Message NVARCHAR(500),
    @User NVARCHAR(50),
    @AllXml NVARCHAR(MAX),
    @StatusCode INT,
    @TimeUtc DATETIME
)
AS

    SET NOCOUNT ON

    INSERT
    INTO
        [ELMAH_Error]
        (
            [ErrorId],
            [Application],
            [Host],
            [Type],
            [Source],
            [Message],
            [User],
            [AllXml],
            [StatusCode],
            [TimeUtc]
        )
    VALUES
        (
            @ErrorId,
            @Application,
            @Host,
            @Type,
            @Source,
            @Message,
            @User,
            @AllXml,
            @StatusCode,
            @TimeUtc
        )
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorXml]    Script Date: 03/30/2010 13:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ELMAH_GetErrorXml]
(
    @Application NVARCHAR(60),
    @ErrorId UNIQUEIDENTIFIER
)
AS

    SET NOCOUNT ON

    SELECT 
        [AllXml]
    FROM 
        [ELMAH_Error]
    WHERE
        [ErrorId] = @ErrorId
    AND
        [Application] = @Application
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorsXml]    Script Date: 03/30/2010 13:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ELMAH_GetErrorsXml]
(
    @Application NVARCHAR(60),
    @PageIndex INT = 0,
    @PageSize INT = 15,
    @TotalCount INT OUTPUT
)
AS 

    SET NOCOUNT ON

    DECLARE @FirstTimeUTC DATETIME
    DECLARE @FirstSequence INT
    DECLARE @StartRow INT
    DECLARE @StartRowIndex INT

    SELECT 
        @TotalCount = COUNT(1) 
    FROM 
        [ELMAH_Error]
    WHERE 
        [Application] = @Application

    -- Get the ID of the first error for the requested page

    SET @StartRowIndex = @PageIndex * @PageSize + 1

    IF @StartRowIndex <= @TotalCount
    BEGIN

        SET ROWCOUNT @StartRowIndex

        SELECT  
            @FirstTimeUTC = [TimeUtc],
            @FirstSequence = [Sequence]
        FROM 
            [ELMAH_Error]
        WHERE   
            [Application] = @Application
        ORDER BY 
            [TimeUtc] DESC, 
            [Sequence] DESC

    END
    ELSE
    BEGIN

        SET @PageSize = 0

    END

    -- Now set the row count to the requested page size and get
    -- all records below it for the pertaining application.

    SET ROWCOUNT @PageSize

    SELECT 
        errorId     = [ErrorId], 
        application = [Application],
        host        = [Host], 
        type        = [Type],
        source      = [Source],
        message     = [Message],
        [user]      = [User],
        statusCode  = [StatusCode], 
        time        = CONVERT(VARCHAR(50), [TimeUtc], 126) + 'Z'
    FROM 
        [ELMAH_Error] error
    WHERE
        [Application] = @Application
    AND
        [TimeUtc] <= @FirstTimeUTC
    AND 
        [Sequence] <= @FirstSequence
    ORDER BY
        [TimeUtc] DESC, 
        [Sequence] DESC
    FOR
        XML AUTO
GO
/****** Object:  Table [dbo].[cms_WidgetDefaultSettings]    Script Date: 03/30/2010 13:10:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_WidgetDefaultSettings](
	[WidgetDefaultSettingId] [int] NOT NULL,
	[WidgetTypeId] [int] NOT NULL,
	[Key] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_cms_WidgetTypeDefaultSettings] PRIMARY KEY CLUSTERED 
(
	[WidgetDefaultSettingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_cms_WidgetTypeDefaultSettings] ON [dbo].[cms_WidgetDefaultSettings] 
(
	[WidgetTypeId] ASC,
	[Key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
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

/* CONTENTITEM IsLogicallyDeleted Field
============================================ */
ALTER TABLE [dbo].[cms_ContentItems]
   ADD IsLogicallyDeleted bit NOT NULL DEFAULT(0)
GO



/****** Object:  Default [DF_cms_Comments_CommentStatus]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Comments] ADD  CONSTRAINT [DF_cms_Comments_CommentStatus]  DEFAULT ((0)) FOR [CommentStatus]
GO
/****** Object:  Default [DF_cms_Comments_CommentType]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Comments] ADD  CONSTRAINT [DF_cms_Comments_CommentType]  DEFAULT ((0)) FOR [CommentType]
GO
/****** Object:  Default [DF_cms_ContentItems_AllowComments]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_ContentItems] ADD  CONSTRAINT [DF_cms_ContentItems_AllowComments]  DEFAULT ((1)) FOR [AllowComments]
GO
/****** Object:  Default [DF_cms_ContentItems_AllowPings]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_ContentItems] ADD  CONSTRAINT [DF_cms_ContentItems_AllowPings]  DEFAULT ((1)) FOR [AllowPings]
GO
/****** Object:  Default [DF__cms_Right__Creat__11AA861D]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Rights] ADD  CONSTRAINT [DF__cms_Right__Creat__11AA861D]  DEFAULT (getdate()) FOR [CreatedDate]
GO
/****** Object:  Default [DF_cms_SeoSettings_RewriteTitles]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_SeoSettings] ADD  CONSTRAINT [DF_cms_SeoSettings_RewriteTitles]  DEFAULT ((1)) FOR [RewriteTitles]
GO
/****** Object:  Default [DF_cms_SeoSettings_UseCategoriesForMeta]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_SeoSettings] ADD  CONSTRAINT [DF_cms_SeoSettings_UseCategoriesForMeta]  DEFAULT ((0)) FOR [UseCategoriesForMeta]
GO
/****** Object:  Default [DF_cms_SeoSettings_GenerateKeywordsForPost]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_SeoSettings] ADD  CONSTRAINT [DF_cms_SeoSettings_GenerateKeywordsForPost]  DEFAULT ((1)) FOR [GenerateKeywordsForPost]
GO
/****** Object:  Default [DF_cms_SeoSettings_UseNoIndexForCategories]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_SeoSettings] ADD  CONSTRAINT [DF_cms_SeoSettings_UseNoIndexForCategories]  DEFAULT ((1)) FOR [UseNoIndexForCategories]
GO
/****** Object:  Default [DF_cms_SeoSettings_UseNoIndexForArchives]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_SeoSettings] ADD  CONSTRAINT [DF_cms_SeoSettings_UseNoIndexForArchives]  DEFAULT ((1)) FOR [UseNoIndexForArchives]
GO
/****** Object:  Default [DF_cms_SeoSettings_UseNoIndexForTags]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_SeoSettings] ADD  CONSTRAINT [DF_cms_SeoSettings_UseNoIndexForTags]  DEFAULT ((0)) FOR [UseNoIndexForTags]
GO
/****** Object:  Default [DF_cms_SeoSettings_GenerateDescriptions]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_SeoSettings] ADD  CONSTRAINT [DF_cms_SeoSettings_GenerateDescriptions]  DEFAULT ((1)) FOR [GenerateDescriptions]
GO
/****** Object:  Default [DF_cms_SeoSettings_CapitalizeCategoryTitles]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_SeoSettings] ADD  CONSTRAINT [DF_cms_SeoSettings_CapitalizeCategoryTitles]  DEFAULT ((1)) FOR [CapitalizeCategoryTitles]
GO
/****** Object:  Default [DF__cms_SiteH__IsDef__1ADEEA9C]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_SiteHosts] ADD  CONSTRAINT [DF__cms_SiteH__IsDef__1ADEEA9C]  DEFAULT ((0)) FOR [IsDefault]
GO
/****** Object:  Default [DF__cms_sites__allow__1273C1CD]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites] ADD  CONSTRAINT [DF__cms_sites__allow__1273C1CD]  DEFAULT ((1)) FOR [AllowRegistration]
GO
/****** Object:  Default [DF__cms_sites__allow__1367E606]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites] ADD  CONSTRAINT [DF__cms_sites__allow__1367E606]  DEFAULT ((1)) FOR [AllowPasswordRetrieval]
GO
/****** Object:  Default [DF__cms_sites__enabl__145C0A3F]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites] ADD  CONSTRAINT [DF__cms_sites__enabl__145C0A3F]  DEFAULT ((0)) FOR [EnableAnonymous]
GO
/****** Object:  Default [DF_cms_Sites_TimeZone]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites] ADD  CONSTRAINT [DF_cms_Sites_TimeZone]  DEFAULT ((-60)) FOR [TimeZone]
GO
/****** Object:  Default [DF__cms_sites__statu__173876EA]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites] ADD  CONSTRAINT [DF__cms_sites__statu__173876EA]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF_cms_Sites_FeedUseSummary]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites] ADD  CONSTRAINT [DF_cms_Sites_FeedUseSummary]  DEFAULT ((0)) FOR [FeedUseSummary]
GO
/****** Object:  Default [DF_cms_Sites_AllowPings]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites] ADD  CONSTRAINT [DF_cms_Sites_AllowPings]  DEFAULT ((1)) FOR [AllowPings]
GO
/****** Object:  Default [DF_cms_Sites_AllowComments]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites] ADD  CONSTRAINT [DF_cms_Sites_AllowComments]  DEFAULT ((1)) FOR [AllowComments]
GO
/****** Object:  Default [DF_cms_Sites_AllowCommentsOnlyForRegisteredUsers]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites] ADD  CONSTRAINT [DF_cms_Sites_AllowCommentsOnlyForRegisteredUsers]  DEFAULT ((0)) FOR [AllowCommentsOnlyForRegisteredUsers]
GO
/****** Object:  Default [DF_cms_Sites_SortCommentsFromOlderToNewest]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites] ADD  CONSTRAINT [DF_cms_Sites_SortCommentsFromOlderToNewest]  DEFAULT ((1)) FOR [SortCommentsFromOlderToNewest]
GO
/****** Object:  Default [DF_cms_Sites_SendEmailForNewComment]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites] ADD  CONSTRAINT [DF_cms_Sites_SendEmailForNewComment]  DEFAULT ((1)) FOR [SendEmailForNewComment]
GO
/****** Object:  Default [DF_cms_Sites_SendEmailForNewModeration]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites] ADD  CONSTRAINT [DF_cms_Sites_SendEmailForNewModeration]  DEFAULT ((1)) FOR [SendEmailForNewModeration]
GO
/****** Object:  Default [DF_cms_Sites_ShowAvatars]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites] ADD  CONSTRAINT [DF_cms_Sites_ShowAvatars]  DEFAULT ((1)) FOR [ShowAvatars]
GO
/****** Object:  Default [DF_cms_Sites_MaxLinksInComments]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites] ADD  CONSTRAINT [DF_cms_Sites_MaxLinksInComments]  DEFAULT ((2)) FOR [MaxLinksInComments]
GO
/****** Object:  Default [DF_cms_Sites_EnableCaptchaForComments]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites] ADD  CONSTRAINT [DF_cms_Sites_EnableCaptchaForComments]  DEFAULT ((1)) FOR [EnableCaptchaForComments]
GO
/****** Object:  Default [DF_cms_Users_TimeZone]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Users] ADD  CONSTRAINT [DF_cms_Users_TimeZone]  DEFAULT ((0)) FOR [TimeZone]
GO
/****** Object:  Default [AddDateDflt]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Users] ADD  CONSTRAINT [AddDateDflt]  DEFAULT ((0)) FOR [IsLogicallyDeleted]
GO
/****** Object:  Default [DF_cms_Users_AdminTheme]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Users] ADD  CONSTRAINT [DF_cms_Users_AdminTheme]  DEFAULT ((0)) FOR [AdminTheme]
GO
/****** Object:  Default [DF_ELMAH_Error_ErrorId]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[ELMAH_Error] ADD  CONSTRAINT [DF_ELMAH_Error_ErrorId]  DEFAULT (newid()) FOR [ErrorId]
GO
/****** Object:  ForeignKey [FK_cms_Categories_cms_Categories]    Script Date: 03/30/2010 13:10:41 ******/
ALTER TABLE [dbo].[cms_Categories]  WITH CHECK ADD  CONSTRAINT [FK_cms_Categories_cms_Categories] FOREIGN KEY([ParentCategoryId])
REFERENCES [dbo].[cms_Categories] ([CategoryId])
GO
ALTER TABLE [dbo].[cms_Categories] CHECK CONSTRAINT [FK_cms_Categories_cms_Categories]
GO
/****** Object:  ForeignKey [FK_cms_Categories_cms_Sites]    Script Date: 03/30/2010 13:10:41 ******/
ALTER TABLE [dbo].[cms_Categories]  WITH CHECK ADD  CONSTRAINT [FK_cms_Categories_cms_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[cms_Sites] ([SiteId])
GO
ALTER TABLE [dbo].[cms_Categories] CHECK CONSTRAINT [FK_cms_Categories_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_CategoryContentItems_cms_Categories]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_CategoriesContentItems]  WITH CHECK ADD  CONSTRAINT [FK_cms_CategoryContentItems_cms_Categories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[cms_Categories] ([CategoryId])
GO
ALTER TABLE [dbo].[cms_CategoriesContentItems] CHECK CONSTRAINT [FK_cms_CategoryContentItems_cms_Categories]
GO
/****** Object:  ForeignKey [FK_cms_CategoryContentItems_cms_ContentItems]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_CategoriesContentItems]  WITH CHECK ADD  CONSTRAINT [FK_cms_CategoryContentItems_cms_ContentItems] FOREIGN KEY([ContentItemId])
REFERENCES [dbo].[cms_ContentItems] ([ContentItemId])
GO
ALTER TABLE [dbo].[cms_CategoriesContentItems] CHECK CONSTRAINT [FK_cms_CategoryContentItems_cms_ContentItems]
GO
/****** Object:  ForeignKey [FK_cms_Comments_cms_ContentItems_ContentItemId]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Comments]  WITH CHECK ADD  CONSTRAINT [FK_cms_Comments_cms_ContentItems_ContentItemId] FOREIGN KEY([ContentItemId])
REFERENCES [dbo].[cms_ContentItems] ([ContentItemId])
GO
ALTER TABLE [dbo].[cms_Comments] CHECK CONSTRAINT [FK_cms_Comments_cms_ContentItems_ContentItemId]
GO
/****** Object:  ForeignKey [FK_cms_Comments_cms_Users_UserId]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Comments]  WITH CHECK ADD  CONSTRAINT [FK_cms_Comments_cms_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[cms_Users] ([UserId])
GO
ALTER TABLE [dbo].[cms_Comments] CHECK CONSTRAINT [FK_cms_Comments_cms_Users_UserId]
GO
/****** Object:  ForeignKey [FK_cms_ContentItems_cms_Sites]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_ContentItems]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentItems_cms_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[cms_Sites] ([SiteId])
GO
ALTER TABLE [dbo].[cms_ContentItems] CHECK CONSTRAINT [FK_cms_ContentItems_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_Pages_cms_ContentItems]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Pages]  WITH CHECK ADD  CONSTRAINT [FK_cms_Pages_cms_ContentItems] FOREIGN KEY([ContentItemId])
REFERENCES [dbo].[cms_ContentItems] ([ContentItemId])
GO
ALTER TABLE [dbo].[cms_Pages] CHECK CONSTRAINT [FK_cms_Pages_cms_ContentItems]
GO
/****** Object:  ForeignKey [FK_cms_Posts_cms_ContentItems]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Posts]  WITH CHECK ADD  CONSTRAINT [FK_cms_Posts_cms_ContentItems] FOREIGN KEY([ContentItemId])
REFERENCES [dbo].[cms_ContentItems] ([ContentItemId])
GO
ALTER TABLE [dbo].[cms_Posts] CHECK CONSTRAINT [FK_cms_Posts_cms_ContentItems]
GO
/****** Object:  ForeignKey [FK_cms_QuotesOfTheDay_cms_Sites]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_QuotesOfTheDay]  WITH CHECK ADD  CONSTRAINT [FK_cms_QuotesOfTheDay_cms_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[cms_Sites] ([SiteId])
GO
ALTER TABLE [dbo].[cms_QuotesOfTheDay] CHECK CONSTRAINT [FK_cms_QuotesOfTheDay_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_Roles_cms_Sites]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Roles]  WITH CHECK ADD  CONSTRAINT [FK_cms_Roles_cms_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[cms_Sites] ([SiteId])
GO
ALTER TABLE [dbo].[cms_Roles] CHECK CONSTRAINT [FK_cms_Roles_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_SeoSettings_cms_Sites]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_SeoSettings]  WITH CHECK ADD  CONSTRAINT [FK_cms_SeoSettings_cms_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[cms_Sites] ([SiteId])
GO
ALTER TABLE [dbo].[cms_SeoSettings] CHECK CONSTRAINT [FK_cms_SeoSettings_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_SiteHosts_cms_Sites]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_SiteHosts]  WITH CHECK ADD  CONSTRAINT [FK_cms_SiteHosts_cms_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[cms_Sites] ([SiteId])
GO
ALTER TABLE [dbo].[cms_SiteHosts] CHECK CONSTRAINT [FK_cms_SiteHosts_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_Sites_cms_Templates]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Sites]  WITH CHECK ADD  CONSTRAINT [FK_cms_Sites_cms_Templates] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[cms_Templates] ([TemplateId])
GO
ALTER TABLE [dbo].[cms_Sites] CHECK CONSTRAINT [FK_cms_Sites_cms_Templates]
GO
/****** Object:  ForeignKey [FK_cms_Tags_cms_Sites]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Tags]  WITH CHECK ADD  CONSTRAINT [FK_cms_Tags_cms_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[cms_Sites] ([SiteId])
GO
ALTER TABLE [dbo].[cms_Tags] CHECK CONSTRAINT [FK_cms_Tags_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_TagsContentItems_cms_ContentItems]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_TagsContentItems]  WITH CHECK ADD  CONSTRAINT [FK_cms_TagsContentItems_cms_ContentItems] FOREIGN KEY([ContentItemId])
REFERENCES [dbo].[cms_ContentItems] ([ContentItemId])
GO
ALTER TABLE [dbo].[cms_TagsContentItems] CHECK CONSTRAINT [FK_cms_TagsContentItems_cms_ContentItems]
GO
/****** Object:  ForeignKey [FK_cms_TagsContentItems_cms_Tags]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_TagsContentItems]  WITH CHECK ADD  CONSTRAINT [FK_cms_TagsContentItems_cms_Tags] FOREIGN KEY([TagId])
REFERENCES [dbo].[cms_Tags] ([TagId])
GO
ALTER TABLE [dbo].[cms_TagsContentItems] CHECK CONSTRAINT [FK_cms_TagsContentItems_cms_Tags]
GO
/****** Object:  ForeignKey [FK_cms_Templates_cms_Sites]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Templates]  WITH CHECK ADD  CONSTRAINT [FK_cms_Templates_cms_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[cms_Sites] ([SiteId])
GO
ALTER TABLE [dbo].[cms_Templates] CHECK CONSTRAINT [FK_cms_Templates_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_UserRoles_cms_Roles]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_cms_UserRoles_cms_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[cms_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[cms_UserRoles] CHECK CONSTRAINT [FK_cms_UserRoles_cms_Roles]
GO
/****** Object:  ForeignKey [FK_cms_UserRoles_cms_Users]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_cms_UserRoles_cms_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[cms_Users] ([UserId])
GO
ALTER TABLE [dbo].[cms_UserRoles] CHECK CONSTRAINT [FK_cms_UserRoles_cms_Users]
GO
/****** Object:  ForeignKey [FK_cms_Users_cms_Sites]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Users]  WITH CHECK ADD  CONSTRAINT [FK_cms_Users_cms_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[cms_Sites] ([SiteId])
GO
ALTER TABLE [dbo].[cms_Users] CHECK CONSTRAINT [FK_cms_Users_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_WidgetTypeDefaultSettings_cms_WidgetTypes]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_WidgetDefaultSettings]  WITH CHECK ADD  CONSTRAINT [FK_cms_WidgetTypeDefaultSettings_cms_WidgetTypes] FOREIGN KEY([WidgetTypeId])
REFERENCES [dbo].[cms_WidgetTypes] ([WidgetTypeId])
GO
ALTER TABLE [dbo].[cms_WidgetDefaultSettings] CHECK CONSTRAINT [FK_cms_WidgetTypeDefaultSettings_cms_WidgetTypes]
GO
/****** Object:  ForeignKey [FK_cms_Widgets_cms_Sites]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Widgets]  WITH CHECK ADD  CONSTRAINT [FK_cms_Widgets_cms_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[cms_Sites] ([SiteId])
GO
ALTER TABLE [dbo].[cms_Widgets] CHECK CONSTRAINT [FK_cms_Widgets_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_Widgets_cms_WidgetTypes]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_Widgets]  WITH CHECK ADD  CONSTRAINT [FK_cms_Widgets_cms_WidgetTypes] FOREIGN KEY([WidgetTypeId])
REFERENCES [dbo].[cms_WidgetTypes] ([WidgetTypeId])
GO
ALTER TABLE [dbo].[cms_Widgets] CHECK CONSTRAINT [FK_cms_Widgets_cms_WidgetTypes]
GO
/****** Object:  ForeignKey [FK_cms_WidgetSettings_cms_Widgets]    Script Date: 03/30/2010 13:10:42 ******/
ALTER TABLE [dbo].[cms_WidgetSettings]  WITH CHECK ADD  CONSTRAINT [FK_cms_WidgetSettings_cms_Widgets] FOREIGN KEY([WidgetId])
REFERENCES [dbo].[cms_Widgets] ([WidgetId])
GO
ALTER TABLE [dbo].[cms_WidgetSettings] CHECK CONSTRAINT [FK_cms_WidgetSettings_cms_Widgets]
GO






/* *************************************************************************************************** */
/*                                DEFAULT DATA                                                         */
/* *************************************************************************************************** */

INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (1, N'Arashi.Core',             1, 1, 2)
INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (2, N'Arashi.Core.Domain',      1, 1, 2)
INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (3, N'Arashi.Core.NHibernate',  1, 1, 2)
INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (10, N'Arashi.Services',        1, 1, 2)
INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (20, N'Arashi.Web',             1, 1, 2)
INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (21, N'Arashi.Web.Mvc',         1, 1, 2)
INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (30, N'Arashi.Web.Plugins',     1, 1, 2)
INSERT [dbo].[cms_Versions] ([versionid], [assembly], [major], [minor], [patch]) 	VALUES (31, N'Arashi.Web.Widgets',     1, 1, 2)

INSERT [dbo].[hibernate_unique_key] ([next_hi]) VALUES (100)

INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) VALUES (0, N'Content Management', 1, N'Comments', N'Manage the comments to your posts', N'/Resources/img/32x32/comments.png', N'/Resources/img/16x16/comments.png', NULL, N'AdminComment', N'Index', NULL)
INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) VALUES (1, N'Settings', 12, N'Themes', N'Change the appearance of the site', N'/Resources/img/32x32/themes.png', N'/Resources/img/16x16/themes.png', NULL, N'Themes', N'Index', NULL)
INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) VALUES (2, N'Settings', 10, N'Configuration', N'General settings', N'/Resources/img/32x32/settings.png', N'/Resources/img/16x16/settings.png', NULL, N'Site', N'Settings', NULL)
INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) VALUES (4, N'Settings', 20, N'Users', N'Manage users that log in the site', N'/Resources/img/32x32/users.png', N'/Resources/img/16x16/users.png', NULL, N'Users', N'Index', NULL)
INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) VALUES (5, N'Settings', 21, N'Groups', N'Manage of users group and their permissions', N'/Resources/img/32x32/group.png', N'/Resources/img/16x16/group.png', NULL, N'Roles', N'Index', NULL)
INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) VALUES (6, N'Content Management', 0, N'Posts', N'Create and edit posts', N'/Resources/img/32x32/word.png', N'/Resources/img/16x16/word.png', NULL, N'AdminPost', N'Index', NULL)
INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) VALUES (7, N'Settings', 11, N'SEO', N'Optimizes your site for Search Engines ', N'/Resources/img/32x32/seo.jpg', N'/Resources/img/16x16/seo.jpg', NULL, N'Seo', N'Index', NULL)
INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) VALUES (8, N'Content Management', 2, N'Pages', N'Manage the static pages of the site', N'/Resources/img/32x32/pages.png', N'/Resources/img/16x16/pages.png', NULL, N'AdminPage', N'Index', NULL)
INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) VALUES (9, N'Settings', 13, N'Widgets', N'Add or remove visual component to the pages', N'/Resources/img/32x32/widgets.png', N'/Resources/img/16x16/widgets.png', NULL, N'Widgets', N'Index', NULL)
INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) VALUES (10, N'Maintenance', 20, N'Rebuild Index', N'Rebuild the search index', N'/Resources/img/32x32/searchdb.png', N'/Resources/img/16x16/searchdb.png', NULL, N'SearchIndex', N'Index', NULL)
INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) VALUES (12, N'Content Management', 20, N'Media Manager', N'Manage your media files', N'/Resources/img/32x32/archive-32.png', N'/Resources/img/16x16/archive.png', NULL, N'MediaManager', N'Index', NULL)
INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) VALUES (13, N'Settings', 22, N'Messages', N'Notification Messages (Contacts, emails)', N'/Resources/img/32x32/mailbox.png', N'/Resources/img/16x16/mailbox.png', NULL, N'Messages', N'Index', NULL)


INSERT [dbo].[cms_Templates] ([TemplateId], [Name], [SiteId], [BasePath], [ThumbnailSrc]) VALUES (0, N'Default', NULL, N'~/Templates/default', N'screenshot.png')
INSERT [dbo].[cms_Templates] ([TemplateId], [Name], [SiteId], [BasePath], [ThumbnailSrc]) VALUES (1, N'Irresistible', NULL, N'~/Templates/irresistible', N'screenshot.png')

INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (0, N'Access Admin', N'Can access site administration', NULL, CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (1, N'Site Create', N'Can create a new site', N'Site', CAST(0x00009B9700A572AA AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (2, N'Site Delete', N'Can delete an existing site', N'Site', CAST(0x00009CD100000000 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (3, N'Dashboard Access', N'Can access Site Dashboard', NULL, CAST(0x00009CD101045F9B AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (10, N'Posts View', N'Can view existing posts', N'Posts', CAST(0x00009CCA00000000 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (11, N'Posts Edit', N'Can edit and create posts', N'Posts', CAST(0x00009CCA00000000 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (12, N'Posts Delete', N'Can delete posts', N'Posts', CAST(0x00009CCA00000000 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (20, N'Comments View', N'Can view existing comments', N'Comments', CAST(0x00009CCA00000000 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (21, N'Comments Edit', N'Can edit or create comments', N'Comments', CAST(0x00009CCA00000000 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (22, N'Comments Delete', N'Can delete comments', N'Comments', CAST(0x00009CCA00000000 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (30, N'Pages View', N'Can view existing pages', N'Pages', CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (31, N'Pages Edit', N'Can edit or create pages', N'Pages', CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (32, N'Pages Delete', N'Can delete pages', N'Pages', CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (50, N'Site Settings View', N'Can view the site settings', N'Site Settings', CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (51, N'Site Settings Edit', N'Can edit the site settings', N'Site Settings', CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (60, N'Templates View', N'Can view the available templates', N'Templates', CAST(0x00009B9700A572AA AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (61, N'Templates Change', N'Can change the current template for a site', N'Templates', CAST(0x00009B9700A572AA AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (70, N'Users View', N'Can view existing users', N'Users', CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (71, N'Users Edit', N'Can edit or create users', N'Users', CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (72, N'Users Delete', N'Can delete users', N'Users', CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (80, N'Roles View', N'Can view existing roles', N'Roles', CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (81, N'Roles Edit', N'Can edit or create roles', N'Roles', CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (82, N'Roles Delete', N'Can delete roles', N'Roles', CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (90, N'Files View', N'Can view existing files', N'Files', CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (91, N'Files Edit', N'Can edit files', N'Files', CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (92, N'Files Upload', N'Can upload new files', N'Files', CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (93, N'Files Delete', N'Can delete files', N'Files', CAST(0x00009B9700A572A9 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (94, N'Messages View', N'Can view notification messages', N'Messages', CAST(0x00009CCA00000000 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (95, N'Messages Edit', N'Can edit notification messages', N'Messages', CAST(0x00009CCA00000000 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (96, N'Messages Delete', N'Can delete notification messages', N'Messages', CAST(0x00009CCA00000000 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (4, N'System Configuration View', N'Can view the system configuration', 'System', GETDATE() )
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (5, N'System Configuration Edit', N'Can edit the system configuration', 'System', GETDATE() )



INSERT [dbo].[cms_WidgetTypes] ([WidgetTypeId], [Name], [AssemblyName], [ClassName]) VALUES (100, N'Archives', N'Arashi.Web.Widgets', N'Arashi.Web.Widgets.Archives.ArchivesWidgetComponent')
INSERT [dbo].[cms_WidgetTypes] ([WidgetTypeId], [Name], [AssemblyName], [ClassName]) VALUES (200, N'Categories', N'Arashi.Web.Widgets', N'Arashi.Web.Widgets.Categories.CategoriesWidgetComponent')
INSERT [dbo].[cms_WidgetTypes] ([WidgetTypeId], [Name], [AssemblyName], [ClassName]) VALUES (300, N'TagCloud', N'Arashi.Web.Widgets', N'Arashi.Web.Widgets.Tags.TagCloudWidgetComponent')
INSERT [dbo].[cms_WidgetTypes] ([WidgetTypeId], [Name], [AssemblyName], [ClassName]) VALUES (400, N'Meta', N'Arashi.Web.Widgets', N'Arashi.Web.Widgets.Meta.MetaWidgetComponent')

INSERT [dbo].[cms_WidgetDefaultSettings] ([WidgetDefaultSettingId], [WidgetTypeId], [Key], [Value]) VALUES (100, 100, N'type', N'monthly')
INSERT [dbo].[cms_WidgetDefaultSettings] ([WidgetDefaultSettingId], [WidgetTypeId], [Key], [Value]) VALUES (101, 100, N'format', N'html')
INSERT [dbo].[cms_WidgetDefaultSettings] ([WidgetDefaultSettingId], [WidgetTypeId], [Key], [Value]) VALUES (102, 100, N'show_post_count', N'true')
INSERT [dbo].[cms_WidgetDefaultSettings] ([WidgetDefaultSettingId], [WidgetTypeId], [Key], [Value]) VALUES (103, 100, N'before', N'')
INSERT [dbo].[cms_WidgetDefaultSettings] ([WidgetDefaultSettingId], [WidgetTypeId], [Key], [Value]) VALUES (104, 100, N'after', N'')
INSERT [dbo].[cms_WidgetDefaultSettings] ([WidgetDefaultSettingId], [WidgetTypeId], [Key], [Value]) VALUES (106, 200, N'show_count', N'0')
INSERT [dbo].[cms_WidgetDefaultSettings] ([WidgetDefaultSettingId], [WidgetTypeId], [Key], [Value]) VALUES (107, 300, N'smallest', N'8')
INSERT [dbo].[cms_WidgetDefaultSettings] ([WidgetDefaultSettingId], [WidgetTypeId], [Key], [Value]) VALUES (108, 300, N'largest', N'22')
INSERT [dbo].[cms_WidgetDefaultSettings] ([WidgetDefaultSettingId], [WidgetTypeId], [Key], [Value]) VALUES (109, 300, N'unit', N'pt')
INSERT [dbo].[cms_WidgetDefaultSettings] ([WidgetDefaultSettingId], [WidgetTypeId], [Key], [Value]) VALUES (110, 300, N'number', N'45')
INSERT [dbo].[cms_WidgetDefaultSettings] ([WidgetDefaultSettingId], [WidgetTypeId], [Key], [Value]) VALUES (111, 300, N'format', N'flat')
INSERT [dbo].[cms_WidgetDefaultSettings] ([WidgetDefaultSettingId], [WidgetTypeId], [Key], [Value]) VALUES (112, 300, N'orderby', N'name')
INSERT [dbo].[cms_WidgetDefaultSettings] ([WidgetDefaultSettingId], [WidgetTypeId], [Key], [Value]) VALUES (113, 300, N'order', N'ASC')
INSERT [dbo].[cms_WidgetDefaultSettings] ([WidgetDefaultSettingId], [WidgetTypeId], [Key], [Value]) VALUES (114, 300, N'taxonomy', N'post_tag')


