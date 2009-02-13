using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;

namespace PaulStovell.TrialBalance.Website.UploadBuildTask {
    public class UploadUnmanagedFileTask : Task {
        private string _webServiceUrl = "http://download.trialbalance.net.au/Services/FilePublishingService.asmx";
        private string _username;
        private string _password;
        private ITaskItem[] _localFilePaths;
        private ITaskItem[] _remoteVirtualFilePaths;
        private string _url;

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
        public ITaskItem[] LocalFilePaths {
            get { return _localFilePaths; }
            set { _localFilePaths = value; }
        }

        [Required]
        public ITaskItem[] RemoteVirtualFilePaths {
            get { return _remoteVirtualFilePaths; }
            set { _remoteVirtualFilePaths = value; }
        }

        public override bool Execute() {
            FilePublishingServiceProxy.FilePublishingService service = new PaulStovell.TrialBalance.Website.UploadBuildTask.FilePublishingServiceProxy.FilePublishingService();
            service.Url = this.WebServiceUrl;
            for (int ixFile = 0; ixFile < LocalFilePaths.Length; ixFile++) {
                this.Log.LogMessage(MessageImportance.Normal, "Uploading file \"{0}\" as \"{1}\"...", this.LocalFilePaths[ixFile].ItemSpec, this.RemoteVirtualFilePaths[ixFile].ItemSpec);
                byte[] fileContent = File.ReadAllBytes(this.LocalFilePaths[ixFile].ItemSpec);
                service.UploadUnmanagedFile(this.Username, this.Password, this.RemoteVirtualFilePaths[ixFile].ItemSpec, fileContent);
            }
            return true;
        }
    }
}
