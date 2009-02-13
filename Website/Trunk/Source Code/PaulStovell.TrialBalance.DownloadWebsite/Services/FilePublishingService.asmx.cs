using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using PaulStovell.TrialBalance.DownloadWebsite.DomainModel;
using System.IO;
using System.Text;
using System.Web.Security;
using System.Security.Authentication;

namespace PaulStovell.TrialBalance.DownloadWebsite.Services {
    /// <summary>
    /// Summary description for FilePublishingService
    /// </summary>
    [WebService(Namespace = "http://download.trialbalance.net.au/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class FilePublishingService : System.Web.Services.WebService {
        [WebMethod]
        public string UploadManagedFile(string username, string password, string fileName, string contentType, byte[] fileContent) {
            Authenticate(username, password);
            string result = string.Empty;
            DownloadFile downloadFile = new DownloadFile();
            downloadFile.FileName = fileName;
            downloadFile.ContentType = contentType;
            DataProvider.Instance.UploadFileMetadata(downloadFile);

            // Now write the content away
            if (fileContent != null) {
                string serverPath = downloadFile.ServerFilePath;
                if (!Directory.Exists(Path.GetDirectoryName(serverPath))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(serverPath));
                }
                using (FileStream fileStream = new FileStream(serverPath, FileMode.Create)) {
                    fileStream.Write(fileContent, 0, fileContent.Length);
                }

                result = HttpContext.Current.Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
                result += "/DownloadFile.ashx?FileID=" + downloadFile.FileID;
            }

            return result;
        }

        [WebMethod]
        public void UploadUnmanagedFile(string username, string password, string filePath, byte[] fileContent) {
            Authenticate(username, password);
            // Now write the content away
            if (fileContent != null) {
                string serverPath = HttpContext.Current.Server.MapPath(filePath);
                if (!Directory.Exists(Path.GetDirectoryName(serverPath))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(serverPath));
                }
                using (FileStream fileStream = new FileStream(serverPath, FileMode.Create)) {
                    fileStream.Write(fileContent, 0, fileContent.Length);
                }
            }
        }

        private void Authenticate(string username, string password) {
            if (!FormsAuthentication.Authenticate(username, password)) {
                throw new AuthenticationException();
            }
        }
    }
}
