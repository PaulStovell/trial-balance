#include "isxdl.iss"

[Setup]
AppName=Download Demo
AppVerName=Download Demo 1
DisableProgramGroupPage=true
Uninstallable=false
DirExistsWarning=no
CreateAppDir=false
OutputDir=.
OutputBaseFilename=demo1
SourceDir=.

[Tasks]
Name: isdownload; Description: Inno Setup; Flags: unchecked; GroupDescription: Download and install:
Name: istooldownload; Description: ISTool; Flags: unchecked; GroupDescription: Download and install:

[Files]
Source: ..\languages\norwegian.ini; Flags: dontcopy

[Code]
const
	// Inno Setup
	url1 = 'http://www.jrsoftware.org/download.php/is.exe';
	// ISTool
	url2 = 'http://www.istool.org/getistool.aspx';

function NextButtonClick(CurPage: Integer): Boolean;
var
  hWnd: Integer;
  sFileName: String;
  sTasks: String;
  nCode: Integer;
begin
  Result := true;

  if CurPage = wpReady then begin
    hWnd := StrToInt(ExpandConstant('{wizardhwnd}'));

    sTasks := WizardSelectedTasks(false);

    isxdl_ClearFiles;

    if Pos('isdownload',sTasks)>0 then begin
      sFileName := ExpandConstant('{tmp}\isetup.exe');
      isxdl_AddFile(url1,sFileName);
    end;
    if Pos('istooldownload',sTasks)>0 then begin
      sFileName := ExpandConstant('{tmp}\istool.exe');
      isxdl_AddFile(url2,sFileName);
    end;

    if isxdl_DownloadFiles(hWnd) <> 0 then begin
      sFileName := ExpandConstant('{tmp}\isetup.exe');
      if FileExists(sFileName) then Exec(sFileName,'','',SW_SHOW,ewWaitUntilTerminated,nCode)
      sFileName := ExpandConstant('{tmp}\istool.exe');
      if FileExists(sFileName) then Exec(sFileName,'','',SW_SHOW,ewWaitUntilTerminated,nCode)
    end else
      Result := false;
  end;
end;

function InitializeSetup: Boolean;
begin
  ExtractTemporaryFile('norwegian.ini');
  isxdl_SetOption('language',ExpandConstant('{tmp}\norwegian.ini'));
  isxdl_SetOption('title','Setup - Download Demo');
  //isxdl_SetOption('label','Some label...');
  //isxdl_SetOption('description','Some description...');
  //isxdl_SetOption('resume','false');
  //isxdl_SetOption('smallwizardimage','F:\UTVK\MISC\isxdl\WizModernSmallImage-IS.bmp');

  Result := true;
end;


