using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace PaulStovell.TrialBalance.Website.UploadBuildTask {
    public class GetNextBuildVersionTask : Task {
        private string _webServiceUrl = "http://www.trialbalance.net.au/Services/UploadBuildService.asmx";
        private Version _nextVersion = new Version("0.0.0.0");
        private string _username;
        private string _password;

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

        [Output]
        public string NextVersion {
            get { return _nextVersion.ToString(); }
        }

        [Output]
        public string NextMajor {
            get { return _nextVersion.Major.ToString(); }
        }

        [Output]
        public string NextMinor {
            get { return _nextVersion.Minor.ToString(); }
        }

        [Output]
        public string NextBuild {
            get { return _nextVersion.Build.ToString(); }
        }
        [Output]
        public string NextRevision {
            get { return _nextVersion.Revision.ToString(); }
        }

        public override bool Execute() {
            UploadBuildServiceProxy.UploadBuildService service = new UploadBuildServiceProxy.UploadBuildService();
            service.Url = this.WebServiceUrl;
            string versionNumber = service.GetNextBuildNumber(this.Username, this.Password);
            _nextVersion = new Version(versionNumber);
            return true;
        }
    }
}
