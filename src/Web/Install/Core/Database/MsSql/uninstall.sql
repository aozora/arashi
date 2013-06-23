ALTER TABLE [dbo].[cms_Comments] DROP CONSTRAINT [DF_cms_Comments_CommentStatus]
GO
ALTER TABLE [dbo].[cms_Comments] DROP CONSTRAINT [DF_cms_Comments_CommentType]
GO
ALTER TABLE [dbo].[cms_ContentItems] DROP CONSTRAINT [DF_cms_ContentItems_AllowComments]
GO
ALTER TABLE [dbo].[cms_ContentItems] DROP CONSTRAINT [DF_cms_ContentItems_AllowPings]
GO
ALTER TABLE [dbo].[cms_Rights] DROP CONSTRAINT [DF__cms_Right__Creat__11AA861D]
GO
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_RewriteTitles]
GO
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_UseCategoriesForMeta]
GO
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_GenerateKeywordsForPost]
GO
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_UseNoIndexForCategories]
GO
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_UseNoIndexForArchives]
GO
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_UseNoIndexForTags]
GO
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_GenerateDescriptions]
GO
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_CapitalizeCategoryTitles]
GO
ALTER TABLE [dbo].[cms_SiteHosts] DROP CONSTRAINT [DF__cms_SiteH__IsDef__1ADEEA9C]
GO
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF__cms_sites__allow__1273C1CD]
GO
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF__cms_sites__allow__1367E606]
GO
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF__cms_sites__enabl__145C0A3F]
GO
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_TimeZone]
GO
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF__cms_sites__statu__173876EA]
GO
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_FeedUseSummary]
GO
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_AllowPings]
GO
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_AllowComments]
GO
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_AllowCommentsOnlyForRegisteredUsers]
GO
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_SortCommentsFromOlderToNewest]
GO
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_SendEmailForNewComment]
GO
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_SendEmailForNewModeration]
GO
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_ShowAvatars]
GO
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_MaxLinksInComments]
GO
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_EnableCaptchaForComments]
GO
ALTER TABLE [dbo].[cms_Users] DROP CONSTRAINT [DF_cms_Users_TimeZone]
GO
ALTER TABLE [dbo].[cms_Users] DROP CONSTRAINT [AddDateDflt]
GO
ALTER TABLE [dbo].[ELMAH_Error] DROP CONSTRAINT [DF_ELMAH_Error_ErrorId]
GO
/****** Object:  ForeignKey [FK_cms_Categories_cms_Categories]    Script Date: 12/18/2009 13:00:30 ******/
ALTER TABLE [dbo].[cms_Categories] DROP CONSTRAINT [FK_cms_Categories_cms_Categories]
GO
/****** Object:  ForeignKey [FK_cms_Categories_cms_Sites]    Script Date: 12/18/2009 13:00:30 ******/
ALTER TABLE [dbo].[cms_Categories] DROP CONSTRAINT [FK_cms_Categories_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_CategoryContentItems_cms_Categories]    Script Date: 12/18/2009 13:00:30 ******/
ALTER TABLE [dbo].[cms_CategoriesContentItems] DROP CONSTRAINT [FK_cms_CategoryContentItems_cms_Categories]
GO
/****** Object:  ForeignKey [FK_cms_CategoryContentItems_cms_ContentItems]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_CategoriesContentItems] DROP CONSTRAINT [FK_cms_CategoryContentItems_cms_ContentItems]
GO
/****** Object:  ForeignKey [FK_cms_Comments_cms_ContentItems_ContentItemId]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_Comments] DROP CONSTRAINT [FK_cms_Comments_cms_ContentItems_ContentItemId]
GO
/****** Object:  ForeignKey [FK_cms_Comments_cms_Users_UserId]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_Comments] DROP CONSTRAINT [FK_cms_Comments_cms_Users_UserId]
GO
/****** Object:  ForeignKey [FK_cms_ContentItems_cms_Sites]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_ContentItems] DROP CONSTRAINT [FK_cms_ContentItems_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_Pages_cms_ContentItems]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_Pages] DROP CONSTRAINT [FK_cms_Pages_cms_ContentItems]
GO
/****** Object:  ForeignKey [FK_cms_Posts_cms_ContentItems]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_Posts] DROP CONSTRAINT [FK_cms_Posts_cms_ContentItems]
GO
/****** Object:  ForeignKey [FK_cms_QuotesOfTheDay_cms_Sites]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_QuotesOfTheDay] DROP CONSTRAINT [FK_cms_QuotesOfTheDay_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_Roles_cms_Sites]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_Roles] DROP CONSTRAINT [FK_cms_Roles_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_SeoSettings_cms_Sites]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [FK_cms_SeoSettings_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_SiteHosts_cms_Sites]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_SiteHosts] DROP CONSTRAINT [FK_cms_SiteHosts_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_Sites_cms_Templates]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [FK_cms_Sites_cms_Templates]
GO
/****** Object:  ForeignKey [FK_cms_Tags_cms_Sites]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_Tags] DROP CONSTRAINT [FK_cms_Tags_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_TagsContentItems_cms_ContentItems]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_TagsContentItems] DROP CONSTRAINT [FK_cms_TagsContentItems_cms_ContentItems]
GO
/****** Object:  ForeignKey [FK_cms_TagsContentItems_cms_Tags]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_TagsContentItems] DROP CONSTRAINT [FK_cms_TagsContentItems_cms_Tags]
GO
/****** Object:  ForeignKey [FK_cms_Templates_cms_Sites]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_Templates] DROP CONSTRAINT [FK_cms_Templates_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_UserRoles_cms_Roles]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_UserRoles] DROP CONSTRAINT [FK_cms_UserRoles_cms_Roles]
GO
/****** Object:  ForeignKey [FK_cms_UserRoles_cms_Users]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_UserRoles] DROP CONSTRAINT [FK_cms_UserRoles_cms_Users]
GO
/****** Object:  ForeignKey [FK_cms_Users_cms_Sites]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_Users] DROP CONSTRAINT [FK_cms_Users_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_WidgetTypeDefaultSettings_cms_WidgetTypes]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_WidgetDefaultSettings] DROP CONSTRAINT [FK_cms_WidgetTypeDefaultSettings_cms_WidgetTypes]
GO
/****** Object:  ForeignKey [FK_cms_Widgets_cms_Sites]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_Widgets] DROP CONSTRAINT [FK_cms_Widgets_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_Widgets_cms_WidgetTypes]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_Widgets] DROP CONSTRAINT [FK_cms_Widgets_cms_WidgetTypes]
GO
/****** Object:  ForeignKey [FK_cms_WidgetSettings_cms_Widgets]    Script Date: 12/18/2009 13:00:31 ******/
ALTER TABLE [dbo].[cms_WidgetSettings] DROP CONSTRAINT [FK_cms_WidgetSettings_cms_Widgets]
GO

DROP TABLE [dbo].[cms_ContentItemCustomFields]
GO

/****** Object:  Table [dbo].[cms_WidgetDefaultSettings]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_WidgetDefaultSettings]
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorsXml]    Script Date: 12/18/2009 13:00:24 ******/
DROP PROCEDURE [dbo].[ELMAH_GetErrorsXml]
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorXml]    Script Date: 12/18/2009 13:00:24 ******/
DROP PROCEDURE [dbo].[ELMAH_GetErrorXml]
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_LogError]    Script Date: 12/18/2009 13:00:24 ******/
DROP PROCEDURE [dbo].[ELMAH_LogError]
GO
/****** Object:  Table [dbo].[hibernate_unique_key]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[hibernate_unique_key]
GO
/****** Object:  Table [dbo].[cms_Versions]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_Versions]
GO
/****** Object:  Table [dbo].[cms_WidgetTypes]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_WidgetTypes]
GO
/****** Object:  Table [dbo].[cms_Widgets]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_Widgets]
GO
/****** Object:  Table [dbo].[ELMAH_Error]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[ELMAH_Error]
GO
/****** Object:  Table [dbo].[cms_Rights]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_Rights]
GO
/****** Object:  Table [dbo].[cms_RoleRights]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_RoleRights]
GO
/****** Object:  Table [dbo].[cms_ContentItemRoles]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_ContentItemRoles]
GO
/****** Object:  Table [dbo].[cms_ControlPanelItems]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_ControlPanelItems]
GO
/****** Object:  Table [dbo].[cms_TrackingInfos]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_TrackingInfos]
GO
/****** Object:  Table [dbo].[cms_UserRoles]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_UserRoles]
GO
/****** Object:  Table [dbo].[cms_Users]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_Users]
GO
/****** Object:  Table [dbo].[cms_Comments]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_Comments]
GO
/****** Object:  Table [dbo].[cms_Pages]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_Pages]
GO
/****** Object:  Table [dbo].[cms_Posts]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_Posts]
GO
/****** Object:  Table [dbo].[cms_QuotesOfTheDay]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_QuotesOfTheDay]
GO
/****** Object:  Table [dbo].[cms_ContentItems]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_ContentItems]
GO
/****** Object:  Table [dbo].[cms_CategoriesContentItems]    Script Date: 12/18/2009 13:00:30 ******/
DROP TABLE [dbo].[cms_CategoriesContentItems]
GO
/****** Object:  Table [dbo].[cms_TagsContentItems]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_TagsContentItems]
GO
/****** Object:  Table [dbo].[cms_Categories]    Script Date: 12/18/2009 13:00:30 ******/
DROP TABLE [dbo].[cms_Categories]
GO
/****** Object:  Table [dbo].[cms_Roles]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_Roles]
GO
/****** Object:  Table [dbo].[cms_SeoSettings]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_SeoSettings]
GO
/****** Object:  Table [dbo].[cms_SiteHosts]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_SiteHosts]
GO
/****** Object:  Table [dbo].[cms_Sites]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_Sites]
GO
/****** Object:  Table [dbo].[cms_Tags]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_Tags]
GO
/****** Object:  Table [dbo].[cms_Templates]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_Templates]
GO
/****** Object:  Table [dbo].[cms_WidgetSettings]    Script Date: 12/18/2009 13:00:31 ******/
DROP TABLE [dbo].[cms_WidgetSettings]
GO
DROP TABLE [dbo].[cms_SystemConfiguration]
GO
