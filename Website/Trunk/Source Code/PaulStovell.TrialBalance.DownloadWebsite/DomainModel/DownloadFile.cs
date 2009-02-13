using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace PaulStovell.TrialBalance.DownloadWebsite.DomainModel {
    public class DownloadFile {
        private int _fileID;
        private string _fileName;
        private string _contentType;
        private int _downloads;
        private DateTime _uploadedDate;

        public int FileID {
            get { return _fileID; }
            set { _fileID = value; }
        }

        public string FileName {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string ContentType {
            get { return _contentType; }
            set { _contentType = value; }
        }

        public int Downloads {
            get { return _downloads; }
            set { _downloads = value; }
        }

        public DateTime UploadedDate {
            get { return _uploadedDate; }
            set { _uploadedDate = value; }
        }

        public string ServerFilePath {
            get {
                string result = string.Empty;

                if (HttpContext.Current != null) {
                    result = HttpContext.Current.Server.MapPath("~/Data/" + this.FileID + "." + this.FileName);
                    //result = @"C:\Users\paul.stovell\Downloads\SUB-11350.zip";
                }

                return result;
            }
        }
    }
}
