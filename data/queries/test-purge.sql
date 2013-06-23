TRUNCATE TABLE dbo.ELMAH_Error
TRUNCATE TABLE dbo.cms_TrackingInfos


DELETE FROM dbo.cms_Posts
WHERE ContentItemId <> 8585216

DELETE FROM dbo.cms_Comments

DELETE FROM dbo.cms_ContentItems
WHERE ContentItemId <> 8585216


DELETE FROM dbo.cms_CategoriesContentItems
DELETE FROM dbo.cms_TagsContentItems
DELETE FROM dbo.cms_Tags
WHERE TagId <> 851968

DELETE FROM dbo.cms_Pages

DELETE FROM dbo.cms_Categories
