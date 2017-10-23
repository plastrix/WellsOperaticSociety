ALTER ROLE [db_owner] ADD MEMBER [tempuser];


GO
ALTER ROLE [db_owner] ADD MEMBER [IIS APPPOOL\test];


GO
ALTER ROLE [db_securityadmin] ADD MEMBER [littletheatre];


GO
ALTER ROLE [db_ddladmin] ADD MEMBER [littletheatre];


GO
ALTER ROLE [db_backupoperator] ADD MEMBER [littletheatre];


GO
ALTER ROLE [db_datareader] ADD MEMBER [littletheatre];


GO
ALTER ROLE [db_datawriter] ADD MEMBER [littletheatre];

