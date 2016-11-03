﻿CREATE TABLE [dbo].[Members]
(
	MemberID INT IDENTITY(1,1) PRIMARY KEY,
	SiteID   INT NOT NULL REFERENCES Sites(SiteID),
	Username NVARCHAR(255) NOT NULL,
	Password NVARCHAR(255) NOT NULL,
	Title	 NVARCHAR(100),
	FirstName NVARCHAR(255) NOT NULL,
	Surname NVARCHAR(255) NOT NULL,
	EmailAddress VARCHAR(255) NOT NULL,
	Company VARCHAR(255),
	Position VARCHAR(255),
	HomePhone CHAR(40),
	WorkPhone CHAR(40),
	MobilePhone CHAR(40),
	Fax CHAR(40),
	Address1 NVARCHAR(500),
	Address2 NVARCHAR(500),
	LocationID		INT NOT NULL REFERENCES Location(LocationID),
	AreaID			INT NOT NULL REFERENCES Area(AreaID),
	CountryID		INT NOT NULL REFERENCES Countries(CountryID),
	EducationID		INT REFERENCES Educations(EducationID),
	PreferredCategoryID		INT,
	PreferredSubCategoryID	INT,
	PreferredSalaryID   INT,
	Subscribed BIT NOT NULL,
	MonthlyUpdate BIT NOT NULL,
	ReferringMemberID INT,
	LastModifiedDate DATETIME DEFAULT(GETDATE()),
	Valid BIT NOT NULL,
	EmailFormat INT NOT NULL REFERENCES EmailFormats(EmailFormatID),
	LastLogon DATETIME DEFAULT(GETDATE()),
	DateOfBirth SMALLDATETIME,
	Gender CHAR(1),
	Tags NVARCHAR(1000),
	Validated BIT NOT NULL,
	ValidateGUID UNIQUEIDENTIFIER,
	[SearchField] NVARCHAR(MAX) NULL,
	WebsiteURL VARCHAR(500),
	AvailabilityID INT,
	AvailabilityFromDate SMALLDATETIME,
	MySpaceHeading NTEXT,
	MySpaceContent NTEXT,
	URLReferrer VARCHAR(500),
	RequiredPasswordChange BIT,
	PreferredJobTitle NVARCHAR(500),
	PreferredAvailability VARCHAR(500),
	PreferredAvailabilityType INT,
	PreferredSalaryFrom VARCHAR(100),
	PreferredSalaryTo VARCHAR(100),
	CurrentSalaryFrom VARCHAR(100),
	CurrentSalaryTo VARCHAR(100),
	LookingFor NVARCHAR(500),
	Experience NVARCHAR(MAX),
	Skills	   NVARCHAR(MAX),
	Reasons	   NVARCHAR(MAX),
	Comments   NVARCHAR(MAX),
	ProfileType VARCHAR(255),
)