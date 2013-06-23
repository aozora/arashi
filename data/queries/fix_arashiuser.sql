--USE MIODBNAME
GO
SELECT 'Lists of users and corresponding security identifiers (SID) in the current database that are not linked to any login:'
EXEC sp_change_users_login 'Report'; -- lista gli utenti con SID non in sync

-- FIRST YOU NEED TO RECREATE THE unsync user at SERVER level!
-- THAN execute this script
EXEC sp_change_users_login 'Auto_Fix' 
                          ,'tripper2'
GO



