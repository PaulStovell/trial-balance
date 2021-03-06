; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
AppName=TrialBalance
AppVerName=TrialBalance 1.0
AppPublisher=Paul Stovell
AppPublisherURL=http://www.trialbalance.net.au
AppSupportURL=http://www.trialbalance.net.au
AppUpdatesURL=http://www.trialbalance.net.au
DefaultDirName={pf}\TrialBalance
DefaultGroupName=TrialBalance
DisableProgramGroupPage=yes
OutputBaseFilename=TrialBalanceSetup
Compression=lzma
SolidCompression=yes
WizardImageFile=dlgbmp.bmp
LicenseFile=TrialBalanceCode\Documentation\License.txt

[LangOptions]
DialogFontName=Tahoma
DialogFontSize=8
WelcomeFontName=Tahoma
WelcomeFontSize=12
TitleFontName=Tahoma
TitleFontSize=29
CopyrightFontName=Tahoma
CopyrightFontSize=8

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: isxdl.dll; Flags: dontcopy
Source: "TrialBalanceCode\Bin\*"; DestDir: "{app}\Bin"; Flags: ignoreversion
Source: "TrialBalanceCode\Documentation\*"; DestDir: "{app}\Documentation"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\TrialBalance"; Filename: "{app}\Bin\TrialBalance.exe"
Name: "{commondesktop}\TrialBalance"; Filename: "{app}\Bin\TrialBalance.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\Bin\TrialBalance.exe"; Description: "{cm:LaunchProgram,TrialBalance}"; Flags: nowait postinstall skipifsilent

[Code]
var
  dotnetRedistPath: string;
  downloadNeeded: boolean;
  dotNetNeeded: boolean;
  memoDependenciesNeeded: string;

procedure isxdl_AddFile(URL, Filename: PChar);
external 'isxdl_AddFile@files:isxdl.dll stdcall';
function isxdl_DownloadFiles(hWnd: Integer): Integer;
external 'isxdl_DownloadFiles@files:isxdl.dll stdcall';
function isxdl_SetOption(Option, Value: PChar): Integer;
external 'isxdl_SetOption@files:isxdl.dll stdcall';


const
  dotnetRedistURL = 'http://www.microsoft.com/downloads/info.aspx?na=90&p=&SrcDisplayLang=en&SrcCategoryId=&SrcFamilyId=10CC340B-F857-4A14-83F5-25634C3BF043&u=http%3a%2f%2fdownload.microsoft.com%2fdownload%2f4%2fd%2fa%2f4da3a5fa-ee6a-42b8-8bfa-ea5c4a458a7d%2fdotnetfx3setup.exe';
  // local system for testing...
  // dotnetRedistURL = 'http://192.168.1.1/dotnetfx.exe';

function InitializeSetup(): Boolean;

begin
  Result := true;
  dotNetNeeded := false;


  // Check for required netfx installation
  //if (not GetUserDefaultLangID() = 'English') then begin

        //msgbox('Language Is Not English');

  //end;

  if (not RegKeyExists(HKLM, 'Software\Microsoft\Net Framework Setup\NDP\v3.0')) then begin
    dotNetNeeded := true;
    if (not IsAdminLoggedOn()) then begin
      MsgBox('TrialBalance requires the Microsoft .NET Framework 3.0 to be installed by an Administrator', mbInformation, MB_OK);
      Result := false;
    end else begin
      memoDependenciesNeeded := memoDependenciesNeeded + '      .NET Framework 3.0' #13;
      dotnetRedistPath := ExpandConstant('{src}\dotnetfx3setup.exe.exe');
      if not FileExists(dotnetRedistPath) then begin
        dotnetRedistPath := ExpandConstant('{tmp}\dotnetfx3setup.exe.exe');
        if not FileExists(dotnetRedistPath) then begin
          isxdl_AddFile(dotnetRedistURL, dotnetRedistPath);
          downloadNeeded := true;
        end;
      end;
      SetIniString('install', 'dotnetRedist', dotnetRedistPath, ExpandConstant('{tmp}\dep.ini'));
    end;
  end;

end;

function NextButtonClick(CurPage: Integer): Boolean;
var
  hWnd: Integer;
  ResultCode: Integer;

begin
  Result := true;

  if CurPage = wpReady then begin

    hWnd := StrToInt(ExpandConstant('{wizardhwnd}'));

    // don't try to init isxdl if it's not needed because it will error on < ie 3
    if downloadNeeded then begin

      isxdl_SetOption('label', 'Downloading Microsoft .NET Framework 3.0');
      isxdl_SetOption('description', 'TrialBalance Setup needs to install the Microsoft .NET Framework 2.0. Please wait while Setup is downloading extra files to your computer.');
      if isxdl_DownloadFiles(hWnd) = 0 then Result := false;
    end;
    if (Result = true) and (dotNetNeeded = true) then begin
      if Exec(ExpandConstant(dotnetRedistPath), '', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then begin
        // handle success if necessary; ResultCode contains the exit code
        if not (ResultCode = 0) then begin
          Result := false;
        end;
      end else begin
        // handle failure if necessary; ResultCode contains the error code
        Result := false;
      end;
    end;
  end;
end;

function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo, MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
var
  s: string;

begin
  if memoDependenciesNeeded <> '' then s := s + 'Dependencies that will be automatically downloaded And installed:' + NewLine + memoDependenciesNeeded + NewLine;
  s := s + MemoDirInfo + NewLine + NewLine;

  Result := s
end;

