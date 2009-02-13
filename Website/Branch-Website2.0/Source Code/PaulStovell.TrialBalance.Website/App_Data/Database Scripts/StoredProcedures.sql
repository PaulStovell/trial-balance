---------------------------------------------------------------------------------------
-- This file contains all of the stored procedures. It will change for every schema 
-- change. 
-- 
-- Last updated for schema block: 0002
---------------------------------------------------------------------------------------

-- DROP
if exists (select * from sysobjects where name='SaveSetting' and xtype='P')
	drop procedure [TrialBalanceBuildSystem].[SaveSetting]
	
if exists (select * from sysobjects where name='GetSetting' and xtype='P')
	drop procedure [TrialBalanceBuildSystem].[GetSetting]
	
if exists (select * from sysobjects where name='SetNextBuildMajorMinorVersion' and xtype='P')
	drop procedure [TrialBalanceBuildSystem].[SetNextBuildMajorMinorVersion]
	
if exists (select * from sysobjects where name='GetNextBuildNumber' and xtype='P')
	drop procedure [TrialBalanceBuildSystem].[GetNextBuildNumber]
	
if exists (select * from sysobjects where name='SaveBuild' and xtype='P')
	drop procedure [TrialBalanceBuildSystem].[SaveBuild]
	
if exists (select * from sysobjects where name='GetBuilds' and xtype='P')
	drop procedure [TrialBalanceBuildSystem].[GetBuilds]
	
if exists (select * from sysobjects where name='GetBuild' and xtype='P')
	drop procedure [TrialBalanceBuildSystem].[GetBuild]
	
if exists (select * from sysobjects where name='SaveSetting' and xtype='P')
	drop procedure [TrialBalanceBuildSystem].[SaveSetting]
	
if exists (select * from sysobjects where name='GetLatestStableBuild' and xtype='P')
	drop procedure [TrialBalanceBuildSystem].[GetLatestStableBuild]
	
if exists (select * from sysobjects where name='GetScreenshots' and xtype='P')
	drop procedure [TrialBalanceMedia].[GetScreenshots]
	
if exists (select * from sysobjects where name='GetScreenshot' and xtype='P')
	drop procedure [TrialBalanceMedia].[GetScreenshot]
	
if exists (select * from sysobjects where name='SaveScreenshot' and xtype='P')
	drop procedure [TrialBalanceMedia].[SaveScreenshot]
	
if exists (select * from sysobjects where name='DeleteScreenshot' and xtype='P')
	drop procedure [TrialBalanceMedia].[DeleteScreenshot]
go	
-- CREATE

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
	@releaseNotes text,
	@majorVersion smallint,
	@minorVersion smallint, 
	@buildVersion smallint,
	@revisionVersion smallint,
	@downloads int = 0,
	@isPublic bit,
	@buildStatus varchar(255),
	@installerUrl varchar(255),
	@sourceCodeUrl varchar(255),
	@clickOnceUrl varchar(255)
)
as
begin
	if exists (select BuildNumber from [TrialBalanceBuildSystem].[Builds] where BuildNumber=@buildNumber) 
	begin
		update [TrialBalanceBuildSystem].[Builds] set
			BuildDate=@buildDate,
			IsSuccessful=@isSuccessful,
			ReleaseNotes=@releaseNotes,
			Downloads=@downloads,
			IsPublic=@isPublic,
			BuildStatus=@buildStatus,
			InstallerUrl=@installerUrl,
			ClickOnceUrl=@clickOnceUrl,
			SourceCodeUrl=@sourceCodeUrl
			where BuildNumber=@buildNumber
	end
	else
	begin
		insert into [TrialBalanceBuildSystem].[Builds] (
			BuildNumber, BuildDate, IsSuccessful, ReleaseNotes, MajorVersion, MinorVersion, BuildVersion, RevisionVersion, Downloads, IsPublic, BuildStatus, SourceCodeUrl, InstallerUrl, ClickOnceUrl
			) values (
			@buildNumber, @buildDate, @isSuccessful, @releaseNotes, @majorVersion, @minorVersion, @buildVersion, @revisionVersion, 0, @isPublic, @buildStatus, @sourceCodeUrl, @installerUrl, @clickOnceUrl
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
	select top (@limit) BuildNumber, BuildDate, IsSuccessful, ReleaseNotes, MajorVersion, MinorVersion, BuildVersion, RevisionVersion, Downloads, IsPublic, BuildStatus, SourceCodeUrl, InstallerUrl, ClickOnceUrl
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
	select BuildNumber, BuildDate, IsSuccessful, ReleaseNotes, MajorVersion, MinorVersion, BuildVersion, RevisionVersion, Downloads, IsPublic, BuildStatus, SourceCodeUrl, InstallerUrl, ClickOnceUrl
		from [TrialBalanceBuildSystem].[Builds]
		where BuildNumber like @buildNumber and (@onlyPublicBuilds = 0 or IsPublic=1) 
end
go

create procedure [TrialBalanceBuildSystem].[GetLatestStableBuild]
as
begin
	select top 1 BuildNumber, BuildDate, IsSuccessful, ReleaseNotes, MajorVersion, MinorVersion, BuildVersion, RevisionVersion, Downloads, IsPublic, BuildStatus, SourceCodeUrl, InstallerUrl, ClickOnceUrl
		from [TrialBalanceBuildSystem].[Builds]
		where IsPublic=1 and BuildStatus LIKE 'Stable%' and IsSuccessful=1
		order by BuildDate desc
end
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