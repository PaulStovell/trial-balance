---------------------------------------------------------------------------------------
-- In this block we're going to:
--   1) Drop the IsStable flag from the Builds table
--   2) Add a BuildStatus flag (a sring) and migrate data from the IsStable flag.
--   3) Add three columns to store the InstallerUrl, SourceCodeUrl and ClickOnceUrl
---------------------------------------------------------------------------------------

alter table [TrialBalanceBuildSystem].[Builds]  add 
	BuildStatus varchar(255) not null default('Unstable'),
	InstallerUrl varchar(255) not null default(''),	
	SourceCodeUrl varchar(255) not null default(''),	
	ClickOnceUrl varchar(255) not null default('')
go

-- Data migration
update [TrialBalanceBuildSystem].[Builds] 
	set SourceCodeUrl = 'http://www.trialbalance.net.au/Builds/' + BuildNumber + '/TrialBalance-'
		+ convert(varchar(10), MajorVersion) + '.' + convert(varchar(10), MinorVersion) + '.' + convert(varchar(10), BuildVersion) + '.' + convert(varchar(10), RevisionVersion) 
		+ '-Source.zip';
update [TrialBalanceBuildSystem].[Builds]
	set BuildStatus = 'Stable' where IsStable = 1;

-- Now we've migrated the data, drop the old column
alter table [TrialBalanceBuildSystem].[Builds] 
	drop column IsStable
go
