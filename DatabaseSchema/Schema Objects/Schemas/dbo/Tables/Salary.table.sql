﻿CREATE TABLE [dbo].[Salary]
(
	SalaryID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	SalaryTypeID INT NOT NULL REFERENCES SalaryType(SalaryTypeID),
	SalaryName VARCHAR(255) NOT NULL,
	Valid BIT NOT NULL DEFAULT(1)
)