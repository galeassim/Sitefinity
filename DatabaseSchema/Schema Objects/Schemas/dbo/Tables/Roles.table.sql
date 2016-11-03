﻿CREATE TABLE [dbo].[Roles]
(
	RoleID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	ProfessionID INT NOT NULL REFERENCES Profession(ProfessionID),
	RoleName VARCHAR(255) NOT NULL,
	Valid BIT NOT NULL DEFAULT(1),
	MetaTitle				VARCHAR(255) NOT NULL,
	MetaKeywords			VARCHAR(255) NOT NULL,
	MetaDescription			VARCHAR(512) NOT NULL
)