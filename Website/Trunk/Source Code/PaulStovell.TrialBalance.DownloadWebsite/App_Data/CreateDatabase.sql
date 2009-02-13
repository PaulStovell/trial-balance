create database TBDS_Test2
go
use TBDS_Test2
go

------------------------------
-- Database creation 
------------------------------

create schema TrialBalanceDownloadServer
go


create table TrialBalanceDownloadServer.FileMetadata (
	FileID int identity (505, 1) not null constraint PK_Files_FileID primary key,
	[FileName] varchar(255) not null,
	ContentType varchar(50) not null,
	Downloads int not null,
	UploadedDate datetime not null
)
go

create procedure TrialBalanceDownloadServer.UploadFileMetadata(
	@FileName varchar(255),
	@ContentType varchar(255)
) as
begin
	insert into TrialBalanceDownloadServer.FileMetadata ([FileName], ContentType, Downloads, UploadedDate)	
		values (@FileName, @ContentType, 0, getdate());
	select @@identity;
end
go

create procedure TrialBalanceDownloadServer.GetFileMetadata(
	@FileID int
) as 
begin
	select FileID, [FileName], ContentType, Downloads, UploadedDate
		from TrialBalanceDownloadServer.FileMetadata
		where FileID=@FileID;
end
go

create procedure TrialBalanceDownloadServer.RegisterFileDownload( 
	@FileID int
) as 
begin
	update TrialBalanceDownloadServer.FileMetadata set Downloads = Downloads + 1 where FileID=@FileID;
end
go


exec TrialBalanceDownloadServer.UploadFileMetadata 'Paul.zip', 'application/zip'

select * from TrialBalanceDownloadServer.FileMetadata

------------------------------
-- End database creation 
------------------------------

use Master
go
drop database TBDS_Test2
go