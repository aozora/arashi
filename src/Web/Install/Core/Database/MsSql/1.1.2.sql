/* CONTENTITEM CUSTOM FIELDS
============================================ */

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




/* ADD NEW ROLES
============================================ */
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (94, N'Messages View', N'Can view notification messages', N'Messages', CAST(0x00009CCA00000000 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (95, N'Messages Edit', N'Can edit notification messages', N'Messages', CAST(0x00009CCA00000000 AS DateTime))
INSERT [dbo].[cms_Rights] ([RightId], [Name], [Description], [RightGroup], [CreatedDate]) VALUES (96, N'Messages Delete', N'Can delete notification messages', N'Messages', CAST(0x00009CCA00000000 AS DateTime))


/* ADD CONTROLPANEL ICON
============================================ */
INSERT [dbo].[cms_ControlPanelItems] ([ControlPanelItemId], [Category], [ViewOrder], [Text], [Description], [ImageSrc], [LittleImageSrc], [ImageAlt], [Controller], [Action], [Parameters]) 
   VALUES (13, N'Settings', 22, N'Messages', N'Notification Messages (Contacts, emails)', N'/Resources/img/32x32/mailbox.png', N'/Resources/img/16x16/mailbox.png', NULL, N'Messages', N'Index', NULL)


/* UPGRADE VERSIONS 
============================================ */

-- update core assemblies version
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 2 WHERE [assembly] = N'Arashi.Core'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 2 WHERE [assembly] = N'Arashi.Core.Domain'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 2 WHERE [assembly] = N'Arashi.Core.NHibernate'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 2 WHERE [assembly] = N'Arashi.Services'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 2 WHERE [assembly] = N'Arashi.Web'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 2 WHERE [assembly] = N'Arashi.Web.Mvc'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 2 WHERE [assembly] = N'Arashi.Web.Plugins'
UPDATE [dbo].[cms_Versions] SET [major] = 1, [minor] = 1, [patch] = 2 WHERE [assembly] = N'Arashi.Web.Widgets'
GO


SET ANSI_PADDING OFF
GO
