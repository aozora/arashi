USE [Arashi]
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Comments_CommentStatus]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Comments]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Comments_CommentStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Comments] DROP CONSTRAINT [DF_cms_Comments_CommentStatus]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Comments_CommentType]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Comments]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Comments_CommentType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Comments] DROP CONSTRAINT [DF_cms_Comments_CommentType]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_ContentItems_AllowComments]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentItems]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_ContentItems_AllowComments]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_ContentItems] DROP CONSTRAINT [DF_cms_ContentItems_AllowComments]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_ContentItems_AllowPings]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentItems]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_ContentItems_AllowPings]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_ContentItems] DROP CONSTRAINT [DF_cms_ContentItems_AllowPings]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__cms_Conte__Enabl__09202D14]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentItems]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__cms_Conte__Enabl__09202D14]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_ContentItems] DROP CONSTRAINT [DF__cms_Conte__Enabl__09202D14]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__cms_Conte__IsLog__3CD4DB44]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentItems]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__cms_Conte__IsLog__3CD4DB44]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_ContentItems] DROP CONSTRAINT [DF__cms_Conte__IsLog__3CD4DB44]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__cms_Messa__Attem__31632898]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Messages]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__cms_Messa__Attem__31632898]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Messages] DROP CONSTRAINT [DF__cms_Messa__Attem__31632898]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__cms_Right__Creat__11AA861D]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Rights]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__cms_Right__Creat__11AA861D]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Rights] DROP CONSTRAINT [DF__cms_Right__Creat__11AA861D]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_SeoSettings_RewriteTitles]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_SeoSettings]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_SeoSettings_RewriteTitles]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_RewriteTitles]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_SeoSettings_UseCategoriesForMeta]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_SeoSettings]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_SeoSettings_UseCategoriesForMeta]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_UseCategoriesForMeta]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_SeoSettings_GenerateKeywordsForPost]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_SeoSettings]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_SeoSettings_GenerateKeywordsForPost]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_GenerateKeywordsForPost]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_SeoSettings_UseNoIndexForCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_SeoSettings]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_SeoSettings_UseNoIndexForCategories]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_UseNoIndexForCategories]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_SeoSettings_UseNoIndexForArchives]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_SeoSettings]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_SeoSettings_UseNoIndexForArchives]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_UseNoIndexForArchives]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_SeoSettings_UseNoIndexForTags]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_SeoSettings]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_SeoSettings_UseNoIndexForTags]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_UseNoIndexForTags]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_SeoSettings_GenerateDescriptions]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_SeoSettings]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_SeoSettings_GenerateDescriptions]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_GenerateDescriptions]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_SeoSettings_CapitalizeCategoryTitles]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_SeoSettings]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_SeoSettings_CapitalizeCategoryTitles]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [DF_cms_SeoSettings_CapitalizeCategoryTitles]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__cms_SiteH__IsDef__1ADEEA9C]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_SiteHosts]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__cms_SiteH__IsDef__1ADEEA9C]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_SiteHosts] DROP CONSTRAINT [DF__cms_SiteH__IsDef__1ADEEA9C]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__cms_sites__allow__1273C1CD]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__cms_sites__allow__1273C1CD]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF__cms_sites__allow__1273C1CD]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__cms_sites__allow__1367E606]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__cms_sites__allow__1367E606]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF__cms_sites__allow__1367E606]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__cms_sites__enabl__145C0A3F]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__cms_sites__enabl__145C0A3F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF__cms_sites__enabl__145C0A3F]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Sites_TimeZone]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Sites_TimeZone]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_TimeZone]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__cms_sites__statu__173876EA]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__cms_sites__statu__173876EA]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF__cms_sites__statu__173876EA]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Sites_FeedUseSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Sites_FeedUseSummary]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_FeedUseSummary]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Sites_AllowPings]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Sites_AllowPings]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_AllowPings]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Sites_AllowComments]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Sites_AllowComments]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_AllowComments]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Sites_AllowCommentsOnlyForRegisteredUsers]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Sites_AllowCommentsOnlyForRegisteredUsers]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_AllowCommentsOnlyForRegisteredUsers]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Sites_SortCommentsFromOlderToNewest]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Sites_SortCommentsFromOlderToNewest]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_SortCommentsFromOlderToNewest]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Sites_SendEmailForNewComment]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Sites_SendEmailForNewComment]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_SendEmailForNewComment]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Sites_SendEmailForNewModeration]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Sites_SendEmailForNewModeration]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_SendEmailForNewModeration]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Sites_ShowAvatars]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Sites_ShowAvatars]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_ShowAvatars]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Sites_MaxLinksInComments]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Sites_MaxLinksInComments]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_MaxLinksInComments]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Sites_EnableCaptchaForComments]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Sites_EnableCaptchaForComments]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [DF_cms_Sites_EnableCaptchaForComments]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Users_TimeZone]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Users]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Users_TimeZone]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Users] DROP CONSTRAINT [DF_cms_Users_TimeZone]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[AddDateDflt]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Users]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[AddDateDflt]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Users] DROP CONSTRAINT [AddDateDflt]
END


End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_ELMAH_Error_ErrorId]') AND parent_object_id = OBJECT_ID(N'[dbo].[ELMAH_Error]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ELMAH_Error_ErrorId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ELMAH_Error] DROP CONSTRAINT [DF_ELMAH_Error_ErrorId]
END


End
GO
/****** Object:  ForeignKey [FK_cms_Categories_cms_Categories]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Categories_cms_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Categories]'))
ALTER TABLE [dbo].[cms_Categories] DROP CONSTRAINT [FK_cms_Categories_cms_Categories]
GO
/****** Object:  ForeignKey [FK_cms_Categories_cms_Sites]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Categories_cms_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Categories]'))
ALTER TABLE [dbo].[cms_Categories] DROP CONSTRAINT [FK_cms_Categories_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_CategoryContentItems_cms_Categories]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_CategoryContentItems_cms_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_CategoriesContentItems]'))
ALTER TABLE [dbo].[cms_CategoriesContentItems] DROP CONSTRAINT [FK_cms_CategoryContentItems_cms_Categories]
GO
/****** Object:  ForeignKey [FK_cms_CategoryContentItems_cms_ContentItems]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_CategoryContentItems_cms_ContentItems]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_CategoriesContentItems]'))
ALTER TABLE [dbo].[cms_CategoriesContentItems] DROP CONSTRAINT [FK_cms_CategoryContentItems_cms_ContentItems]
GO
/****** Object:  ForeignKey [FK_cms_Comments_cms_ContentItems_ContentItemId]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Comments_cms_ContentItems_ContentItemId]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Comments]'))
ALTER TABLE [dbo].[cms_Comments] DROP CONSTRAINT [FK_cms_Comments_cms_ContentItems_ContentItemId]
GO
/****** Object:  ForeignKey [FK_cms_Comments_cms_Users_UserId]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Comments_cms_Users_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Comments]'))
ALTER TABLE [dbo].[cms_Comments] DROP CONSTRAINT [FK_cms_Comments_cms_Users_UserId]
GO
/****** Object:  ForeignKey [FK_cms_ContentItems_cms_Sites]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentItems_cms_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentItems]'))
ALTER TABLE [dbo].[cms_ContentItems] DROP CONSTRAINT [FK_cms_ContentItems_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_Messages_cms_Sites]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Messages_cms_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Messages]'))
ALTER TABLE [dbo].[cms_Messages] DROP CONSTRAINT [FK_cms_Messages_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_Pages_cms_ContentItems]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Pages_cms_ContentItems]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Pages]'))
ALTER TABLE [dbo].[cms_Pages] DROP CONSTRAINT [FK_cms_Pages_cms_ContentItems]
GO
/****** Object:  ForeignKey [FK_cms_Posts_cms_ContentItems]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Posts_cms_ContentItems]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Posts]'))
ALTER TABLE [dbo].[cms_Posts] DROP CONSTRAINT [FK_cms_Posts_cms_ContentItems]
GO
/****** Object:  ForeignKey [FK_cms_QuotesOfTheDay_cms_Sites]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_QuotesOfTheDay_cms_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_QuotesOfTheDay]'))
ALTER TABLE [dbo].[cms_QuotesOfTheDay] DROP CONSTRAINT [FK_cms_QuotesOfTheDay_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_Roles_cms_Sites]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Roles_cms_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Roles]'))
ALTER TABLE [dbo].[cms_Roles] DROP CONSTRAINT [FK_cms_Roles_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_SeoSettings_cms_Sites]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_SeoSettings_cms_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_SeoSettings]'))
ALTER TABLE [dbo].[cms_SeoSettings] DROP CONSTRAINT [FK_cms_SeoSettings_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_SiteHosts_cms_Sites]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_SiteHosts_cms_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_SiteHosts]'))
ALTER TABLE [dbo].[cms_SiteHosts] DROP CONSTRAINT [FK_cms_SiteHosts_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_SiteOptions_cms_Sites]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_SiteOptions_cms_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_SiteOptions]'))
ALTER TABLE [dbo].[cms_SiteOptions] DROP CONSTRAINT [FK_cms_SiteOptions_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_Sites_cms_Themes]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Sites_cms_Themes]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sites]'))
ALTER TABLE [dbo].[cms_Sites] DROP CONSTRAINT [FK_cms_Sites_cms_Themes]
GO
/****** Object:  ForeignKey [FK_cms_Tags_cms_Sites]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Tags_cms_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Tags]'))
ALTER TABLE [dbo].[cms_Tags] DROP CONSTRAINT [FK_cms_Tags_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_TagsContentItems_cms_ContentItems]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_TagsContentItems_cms_ContentItems]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_TagsContentItems]'))
ALTER TABLE [dbo].[cms_TagsContentItems] DROP CONSTRAINT [FK_cms_TagsContentItems_cms_ContentItems]
GO
/****** Object:  ForeignKey [FK_cms_TagsContentItems_cms_Tags]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_TagsContentItems_cms_Tags]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_TagsContentItems]'))
ALTER TABLE [dbo].[cms_TagsContentItems] DROP CONSTRAINT [FK_cms_TagsContentItems_cms_Tags]
GO
/****** Object:  ForeignKey [FK_cms_UserRoles_cms_Roles]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_UserRoles_cms_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_UserRoles]'))
ALTER TABLE [dbo].[cms_UserRoles] DROP CONSTRAINT [FK_cms_UserRoles_cms_Roles]
GO
/****** Object:  ForeignKey [FK_cms_UserRoles_cms_Users]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_UserRoles_cms_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_UserRoles]'))
ALTER TABLE [dbo].[cms_UserRoles] DROP CONSTRAINT [FK_cms_UserRoles_cms_Users]
GO
/****** Object:  ForeignKey [FK_cms_Users_cms_Sites]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Users_cms_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Users]'))
ALTER TABLE [dbo].[cms_Users] DROP CONSTRAINT [FK_cms_Users_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_WidgetTypeDefaultSettings_cms_WidgetTypes]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_WidgetTypeDefaultSettings_cms_WidgetTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_WidgetDefaultSettings]'))
ALTER TABLE [dbo].[cms_WidgetDefaultSettings] DROP CONSTRAINT [FK_cms_WidgetTypeDefaultSettings_cms_WidgetTypes]
GO
/****** Object:  ForeignKey [FK_cms_Widgets_cms_Sites]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Widgets_cms_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Widgets]'))
ALTER TABLE [dbo].[cms_Widgets] DROP CONSTRAINT [FK_cms_Widgets_cms_Sites]
GO
/****** Object:  ForeignKey [FK_cms_Widgets_cms_WidgetTypes]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Widgets_cms_WidgetTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Widgets]'))
ALTER TABLE [dbo].[cms_Widgets] DROP CONSTRAINT [FK_cms_Widgets_cms_WidgetTypes]
GO
/****** Object:  ForeignKey [FK_cms_WidgetSettings_cms_Widgets]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_WidgetSettings_cms_Widgets]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_WidgetSettings]'))
ALTER TABLE [dbo].[cms_WidgetSettings] DROP CONSTRAINT [FK_cms_WidgetSettings_cms_Widgets]
GO
/****** Object:  Table [dbo].[cms_CategoriesContentItems]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_CategoriesContentItems]') AND type in (N'U'))
DROP TABLE [dbo].[cms_CategoriesContentItems]
GO
/****** Object:  Table [dbo].[cms_Comments]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Comments]') AND type in (N'U'))
DROP TABLE [dbo].[cms_Comments]
GO
/****** Object:  Table [dbo].[cms_Pages]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Pages]') AND type in (N'U'))
DROP TABLE [dbo].[cms_Pages]
GO
/****** Object:  Table [dbo].[cms_Posts]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Posts]') AND type in (N'U'))
DROP TABLE [dbo].[cms_Posts]
GO
/****** Object:  Table [dbo].[cms_TagsContentItems]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_TagsContentItems]') AND type in (N'U'))
DROP TABLE [dbo].[cms_TagsContentItems]
GO
/****** Object:  Table [dbo].[cms_UserRoles]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_UserRoles]') AND type in (N'U'))
DROP TABLE [dbo].[cms_UserRoles]
GO
/****** Object:  Table [dbo].[cms_WidgetSettings]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_WidgetSettings]') AND type in (N'U'))
DROP TABLE [dbo].[cms_WidgetSettings]
GO
/****** Object:  Table [dbo].[cms_Users]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Users]') AND type in (N'U'))
DROP TABLE [dbo].[cms_Users]
GO
/****** Object:  Table [dbo].[cms_Messages]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Messages]') AND type in (N'U'))
DROP TABLE [dbo].[cms_Messages]
GO
/****** Object:  Table [dbo].[cms_Widgets]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Widgets]') AND type in (N'U'))
DROP TABLE [dbo].[cms_Widgets]
GO
/****** Object:  Table [dbo].[cms_QuotesOfTheDay]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_QuotesOfTheDay]') AND type in (N'U'))
DROP TABLE [dbo].[cms_QuotesOfTheDay]
GO
/****** Object:  Table [dbo].[cms_Categories]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Categories]') AND type in (N'U'))
DROP TABLE [dbo].[cms_Categories]
GO
/****** Object:  Table [dbo].[cms_Tags]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Tags]') AND type in (N'U'))
DROP TABLE [dbo].[cms_Tags]
GO
/****** Object:  Table [dbo].[cms_ContentItems]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentItems]') AND type in (N'U'))
DROP TABLE [dbo].[cms_ContentItems]
GO
/****** Object:  Table [dbo].[cms_Roles]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Roles]') AND type in (N'U'))
DROP TABLE [dbo].[cms_Roles]
GO
/****** Object:  Table [dbo].[cms_SeoSettings]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_SeoSettings]') AND type in (N'U'))
DROP TABLE [dbo].[cms_SeoSettings]
GO
/****** Object:  Table [dbo].[cms_SiteHosts]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_SiteHosts]') AND type in (N'U'))
DROP TABLE [dbo].[cms_SiteHosts]
GO
/****** Object:  Table [dbo].[cms_SiteOptions]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_SiteOptions]') AND type in (N'U'))
DROP TABLE [dbo].[cms_SiteOptions]
GO
/****** Object:  Table [dbo].[cms_Sites]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sites]') AND type in (N'U'))
DROP TABLE [dbo].[cms_Sites]
GO
/****** Object:  Table [dbo].[cms_WidgetDefaultSettings]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_WidgetDefaultSettings]') AND type in (N'U'))
DROP TABLE [dbo].[cms_WidgetDefaultSettings]
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorsXml]    Script Date: 04/05/2011 21:09:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ELMAH_GetErrorsXml]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ELMAH_GetErrorsXml]
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorXml]    Script Date: 04/05/2011 21:09:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ELMAH_GetErrorXml]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ELMAH_GetErrorXml]
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_LogError]    Script Date: 04/05/2011 21:09:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ELMAH_LogError]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ELMAH_LogError]
GO
/****** Object:  Table [dbo].[hibernate_unique_key]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hibernate_unique_key]') AND type in (N'U'))
DROP TABLE [dbo].[hibernate_unique_key]
GO
/****** Object:  Table [dbo].[cms_Versions]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Versions]') AND type in (N'U'))
DROP TABLE [dbo].[cms_Versions]
GO
/****** Object:  Table [dbo].[cms_Themes]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Themes]') AND type in (N'U'))
DROP TABLE [dbo].[cms_Themes]
GO
/****** Object:  Table [dbo].[cms_TrackingInfos]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_TrackingInfos]') AND type in (N'U'))
DROP TABLE [dbo].[cms_TrackingInfos]
GO
/****** Object:  Table [dbo].[cms_WidgetTypes]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_WidgetTypes]') AND type in (N'U'))
DROP TABLE [dbo].[cms_WidgetTypes]
GO
/****** Object:  Table [dbo].[ELMAH_Error]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ELMAH_Error]') AND type in (N'U'))
DROP TABLE [dbo].[ELMAH_Error]
GO
/****** Object:  Table [dbo].[cms_SystemConfiguration]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_SystemConfiguration]') AND type in (N'U'))
DROP TABLE [dbo].[cms_SystemConfiguration]
GO
/****** Object:  Table [dbo].[cms_ControlPanelItems]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ControlPanelItems]') AND type in (N'U'))
DROP TABLE [dbo].[cms_ControlPanelItems]
GO
/****** Object:  Table [dbo].[cms_ContentItemCustomFields]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentItemCustomFields]') AND type in (N'U'))
DROP TABLE [dbo].[cms_ContentItemCustomFields]
GO
/****** Object:  Table [dbo].[cms_ContentItemRoles]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentItemRoles]') AND type in (N'U'))
DROP TABLE [dbo].[cms_ContentItemRoles]
GO
/****** Object:  Table [dbo].[cms_Rights]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Rights]') AND type in (N'U'))
DROP TABLE [dbo].[cms_Rights]
GO
/****** Object:  Table [dbo].[cms_RoleRights]    Script Date: 04/05/2011 21:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_RoleRights]') AND type in (N'U'))
DROP TABLE [dbo].[cms_RoleRights]
GO
