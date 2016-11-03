﻿CREATE TABLE [dbo].[Advertisers]
(
	AdvertiserID INT IDENTITY(1,1) PRIMARY KEY,
	SiteID					INT REFERENCES Sites(SiteID),
	AdvertiserAccountTypeID INT NOT NULL REFERENCES AdvertiserAccountType(AdvertiserAccountTypeID),
	AdvertiserBusinessTypeID INT NOT NULL REFERENCES AdvertiserBusinessType(AdvertiserBusinessTypeID),
	AdvertiserAccountStatusID INT NOT NULL REFERENCES AdvertiserAccountStatus(AdvertiserAccountStatusID),
	CompanyName		NVARCHAR(255),
	BusinessNumber NVARCHAR(255),
	StreetAddress1 NVARCHAR(255),
	StreetAddress2 NVARCHAR(255),
	LastModified	DATETIME NOT NULL DEFAULT(GETDATE()),
	LastModifiedBy INT NOT NULL REFERENCES AdminUsers(AdminUserID),
	PostalAddress1 NVARCHAR(255),
	PostalAddress2 NVARCHAR(255),
	WebAddress		VARCHAR(255),
	NoOfEmployees	VARCHAR(10),
	FirstApprovedDate SMALLDATETIME,
	Profile		  NTEXT,
	CharityNumber VARCHAR(50),
	SearchField	nvarchar(max),
	FreeTrialStartDate SMALLDATETIME,
	FreeTrialEndDate SMALLDATETIME,
	AccountsPayableEmail VARCHAR(255),
	RequireLogonForExternalApplication BIT NOT NULL DEFAULT(0),
	AdvertiserLogo IMAGE
)