﻿CREATE TABLE AdminRoles
(
	AdminRoleID	 INT IDENTITY(1,1) PRIMARY KEY,
	RoleName VARCHAR(255) NOT NULL
)
GO

/*
GRANT SELECT ON AdminRoles TO PUBLIC

GO
*/