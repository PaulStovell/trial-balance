create schema [TrialBalanceBuildSystem]
go

create schema [TrialBalanceMedia]
go

-- Builds table
create table [TrialBalanceBuildSystem].[Builds] (
	BuildNumber varchar(50) not null constraint [PK_Builds_BuildNumber] primary key nonclustered,
	BuildDate datetime not null,
	IsSuccessful bit not null,
	IsStable bit not null,
	ReleaseNotes text not null,
	MajorVersion smallint not null,
	MinorVersion smallint not null,
	BuildVersion smallint not null,
	RevisionVersion smallint not null,
	Downloads int not null,
	IsPublic bit not null
)
go
create clustered index [IX_Builds_BuildDate] on [TrialBalanceBuildSystem].Builds(BuildDate)
go

-- Build Settings table
create table [TrialBalanceBuildSystem].[BuildSettings] (
	[SettingName] varchar(50) not null constraint [PK_Settings_SettingName] primary key clustered,
	[SettingValue] varchar(50) not null
)
go

-- Scereenshots table
create table [TrialBalanceMedia].[Screenshots] (
	ScreenshotID int not null identity(1, 1) constraint [PK_Screenshot_ScreenshotID] primary key clustered,
	DateTaken datetime not null,
	Caption varchar(400)	
)
go