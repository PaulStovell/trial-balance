------------------------------
-- Database creation 
------------------------------

create schema [TrialBalanceBuildSystem]
go

create schema [TrialBalanceMedia]
go

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

create table [TrialBalanceBuildSystem].[BuildSettings] (
	[SettingName] varchar(50) not null constraint [PK_Settings_SettingName] primary key clustered,
	[SettingValue] varchar(50) not null
)
go

create procedure [TrialBalanceBuildSystem].[SaveSetting] (
	@settingName varchar(50),
	@settingValue varchar(50)
)
as
begin
	if exists ( select SettingName from [TrialBalanceBuildSystem].[BuildSettings] where SettingName LIKE @settingName )
		update [TrialBalanceBuildSystem].[BuildSettings] set SettingValue=@settingValue where SettingName LIKE @settingName
	else
		insert into [TrialBalanceBuildSystem].[BuildSettings] (SettingName, SettingValue) values (@settingName, @settingValue)
end
go

create procedure [TrialBalanceBuildSystem].[GetSetting] (
	@settingName varchar(50),
	@settingValue varchar(50) out,
	@defaultValue varchar(50)
)
as
begin
	set @settingValue = (select [SettingValue] from [TrialBalanceBuildSystem].[BuildSettings] where SettingName LIKE @settingName)
	set @settingValue = isnull(@settingValue, @defaultValue)
end
go

create procedure [TrialBalanceBuildSystem].[SetNextBuildMajorMinorVersion] (
	@nextMajorVersion smallint,
	@nextMinorVersion smallint
) 
as 
begin
	execute [TrialBalanceBuildSystem].[SaveSetting] 'Build.NextMajorVersion', @nextMajorVersion
	execute [TrialBalanceBuildSystem].[SaveSetting] 'Build.NextMinorVersion', @nextMinorVersion
end
go

create procedure [TrialBalanceBuildSystem].[GetNextBuildNumber] (
	@nextMajorVersion smallint out,
	@nextMinorVersion smallint out,
	@nextBuildVersion smallint out,
	@nextRevisionVersion smallint out
)
as
begin
	-- The major and minor build numbers come from the Settings table (use 1.0 as default)
	declare @nextMajorVersionString varchar(50)
	declare @nextMinorVersionString varchar(50)
	execute [TrialBalanceBuildSystem].[GetSetting] 'Build.NextMajorVersion', @nextMajorVersionString out, '1'
	execute [TrialBalanceBuildSystem].[GetSetting] 'Build.NextMinorVersion', @nextMinorVersionString out, '0'
	set @nextMajorVersion = convert(smallint, @nextMajorVersionString)
	set @nextMinorVersion = convert(smallint, @nextMinorVersionString)

	-- The build number is based on the number of days since Jan 01 2000 (as per Microsofts recommendation)
	set @nextBuildVersion = convert(smallint, datediff(day, '2000-01-01', getdate()))
	
	-- And the revision will be unique
	declare @nextRevisionVersionString varchar(50)
	execute [TrialBalanceBuildSystem].[GetSetting] 'Build.NextRevisionVersion', @nextRevisionVersionString out, '0'
	set @nextRevisionVersion = convert(smallint, @nextRevisionVersionString) + 1
	execute [TrialBalanceBuildSystem].[SaveSetting] 'Build.NextRevisionVersion', @nextRevisionVersion
end 
go


create procedure [TrialBalanceBuildSystem].[SaveBuild] (
	@buildNumber varchar(50),
	@buildDate datetime,
	@isSuccessful bit,
	@isStable bit,
	@releaseNotes text,
	@majorVersion smallint,
	@minorVersion smallint, 
	@buildVersion smallint,
	@revisionVersion smallint,
	@downloads int = 0,
	@isPublic bit
)
as
begin
	if exists (select BuildNumber from [TrialBalanceBuildSystem].[Builds] where BuildNumber=@buildNumber) 
	begin
		update [TrialBalanceBuildSystem].[Builds] set
			BuildDate=@buildDate,
			IsSuccessful=@isSuccessful,
			IsStable=@isStable,
			ReleaseNotes=@releaseNotes,
			Downloads=@downloads,
			IsPublic=@isPublic
			where BuildNumber=@buildNumber
	end
	else
	begin
		insert into [TrialBalanceBuildSystem].[Builds] (
			BuildNumber, BuildDate, IsSuccessful, IsStable, ReleaseNotes, MajorVersion, MinorVersion, BuildVersion, RevisionVersion, Downloads, IsPublic
			) values (
			@buildNumber, @buildDate, @isSuccessful, @isStable, @releaseNotes, @majorVersion, @minorVersion, @buildVersion, @revisionVersion, 0, @isPublic
			)
	end
end
go

create procedure [TrialBalanceBuildSystem].[GetBuilds] (
	@onlyPublicBuilds bit,
	@limit int
)
as
begin
	select top (@limit) BuildNumber, BuildDate, IsSuccessful, IsStable, ReleaseNotes, MajorVersion, MinorVersion, BuildVersion, RevisionVersion, Downloads, IsPublic
		from [TrialBalanceBuildSystem].[Builds]
		where (@onlyPublicBuilds = 0 or IsPublic=1) 
		order by BuildDate desc
end
go

create procedure [TrialBalanceBuildSystem].[GetBuild] (
	@buildNumber varchar(50),
	@onlyPublicBuilds bit
)
as 
begin
	select BuildNumber, BuildDate, IsSuccessful, IsStable, ReleaseNotes, MajorVersion, MinorVersion, BuildVersion, RevisionVersion, Downloads, IsPublic
		from [TrialBalanceBuildSystem].[Builds]
		where BuildNumber like @buildNumber and (@onlyPublicBuilds = 0 or IsPublic=1) 
end
go

create procedure [TrialBalanceBuildSystem].[GetLatestStableBuild]
as
begin
	select top 1 BuildNumber, BuildDate, IsSuccessful, IsStable, ReleaseNotes, MajorVersion, MinorVersion, BuildVersion, RevisionVersion, Downloads, IsPublic
		from [TrialBalanceBuildSystem].[Builds]
		where IsPublic=1 and IsStable=1 and IsSuccessful=1
		order by BuildDate desc
end
go

create table [TrialBalanceMedia].[Screenshots] (
	ScreenshotID int not null identity(1, 1) constraint [PK_Screenshot_ScreenshotID] primary key clustered,
	DateTaken datetime not null,
	Caption varchar(400)	
)
go

create procedure [TrialBalanceMedia].[GetScreenshots] 
as 
begin
	select top 50 [ScreenshotID], [DateTaken], [Caption] 
		from [TrialBalanceMedia].[Screenshots]
		order by [DateTaken]
end
go

create procedure [TrialBalanceMedia].[GetScreenshot] (
	@screenshotID int
)
as 
begin
	select top 1 [ScreenshotID], [DateTaken], [Caption] 
		from [TrialBalanceMedia].[Screenshots]
		where [ScreenshotID] = @screenshotID
		order by [DateTaken]
end
go

create procedure [TrialBalanceMedia].[SaveScreenshot] (
	@screenshotID int output,
	@dateTaken datetime, 
	@caption varchar(400)
)
as 
begin
	if exists (select ScreenshotID from [TrialBalanceMedia].[Screenshots] where ScreenshotID=@screenshotID)
	begin	
		update [TrialBalanceMedia].[Screenshots]
			set DateTaken=@dateTaken, Caption=@caption
			where ScreenshotID=@screenshotID
	end
	else
	begin
		insert into [TrialBalanceMedia].[Screenshots]
			(DateTaken, Caption) VALUES
			(@dateTaken, @caption)
		set @screenshotID = @@identity
	end
end
go

create procedure [TrialBalanceMedia].[DeleteScreenshot] (
	@screenshotID int
)
as 
begin
	delete from [TrialBalanceMedia].[Screenshots] where ScreenshotID=@screenshotID
end
go

------------------------------
-- End database creation 
------------------------------