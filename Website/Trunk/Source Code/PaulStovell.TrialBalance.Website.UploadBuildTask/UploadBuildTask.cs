using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;

namespace PaulStovell.TrialBalance.Website.UploadBuildTask {
    public class UploadBuildTask : Task {
        private string _webServiceUrl = "http://www.trialbalance.net.au/Services/UploadBuildService.asmx";
        private string _version = "0.0.0.0";
        private string _username;
        private string _password;
        private string _buildNumber;
        private DateTime _buildDate = DateTime.Now;
        private bool _isSuccessful;
        private string _buildStatus = "Unstable";
        private bool _isPublic;
        private string _releaseNotesFilePath;
        private string _sourceCodeUrl;
        private string _installerUrl;
        private string _clickOnceUrl;

        public string WebServiceUrl {
            get { return _webServiceUrl; }
            set { _webServiceUrl = value; }
        }

        [Required]
        public string Username {
            get { return _username; }
            set { _username = value; }
        }

        [Required]
        public string Password {
            get { return _password; }
            set { _password = value; }
        }

        [Required]
        public string Version {
            get { return _version; }
            set { _version = value; }
        }

        public DateTime BuildDate {
            get { return _buildDate; }
            set { _buildDate = value; }
        }

        [Required]
        public string BuildNumber {
            get { return _buildNumber; }
            set { _buildNumber = value; }
        }

        public bool IsSuccessful {
            get { return _isSuccessful; }
            set { _isSuccessful = value; }
        }

        public string BuildStatus {
            get { return _buildStatus; }
            set { _buildStatus = value; }
        }

        public bool IsPublic {
            get { return _isPublic; }
            set { _isPublic = value; }
        }

        public string ReleaseNotesFilePath {
            get { return _releaseNotesFilePath ?? string.Empty; }
            set { _releaseNotesFilePath = value; }
        }

        public string SourceCodeUrl {
            get { return _sourceCodeUrl; }
            set { _sourceCodeUrl = value; }
        }

        public string InstallerUrl {
            get { return _installerUrl; }
            set { _installerUrl = value; }
        }

        public string ClickOnceUrl {
            get { return _clickOnceUrl; }
            set { _clickOnceUrl = value; }
        }

        public override bool Execute() {
            string releaseNotes = string.Empty;

            if (this.ReleaseNotesFilePath != string.Empty) {
                releaseNotes = File.ReadAllText(this.ReleaseNotesFilePath);
            }
            
            UploadBuildServiceProxy.UploadBuildService service = new UploadBuildServiceProxy.UploadBuildService();
            service.Url = _webServiceUrl;
            service.UploadBuild(this.Username, this.Password, this.BuildNumber, this.BuildDate, this.IsSuccessful, this.BuildStatus, this.Version, this.IsPublic,
                releaseNotes, this.SourceCodeUrl, this.InstallerUrl, this.ClickOnceUrl);
            return true;
        }
    }
}
