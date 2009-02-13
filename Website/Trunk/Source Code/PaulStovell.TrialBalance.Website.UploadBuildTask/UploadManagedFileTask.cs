using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;

namespace PaulStovell.TrialBalance.Website.UploadBuildTask {
    public class UploadManagedFileTask : Task {
        private string _webServiceUrl = "http://download.trialbalance.net.au/Services/FilePublishingService.asmx";
        private string _username;
        private string _password;
        private string _fileName;
        private string _contentType;
        private string _filePath;
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
        public string FileName {
            get { return _fileName; }
            set { _fileName = value; }
        }

        [Required]
        public string ContentType {
            get { return _contentType; }
            set { _contentType = value; }
        }

        [Required]
        public string FilePath {
            get { return _filePath; }
            set { _filePath = value; }
        }

        [Output]
        public string Url {
            get { return _url; }
        }

        public override bool Execute() {
            byte[] file = File.ReadAllBytes(this.FilePath);
            FilePublishingServiceProxy.FilePublishingService service = new PaulStovell.TrialBalance.Website.UploadBuildTask.FilePublishingServiceProxy.FilePublishingService();
            service.Url = this.WebServiceUrl;
            string result = service.UploadManagedFile(this.Username, this.Password, this.FileName, this.ContentType, file);
            this._url = result;
            return true;
        }
    }
}
