using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using PaulStovell.TrialBalance.Website.Common;
using PaulStovell.TrialBalance.DownloadWebsite.DomainModel;

namespace PaulStovell.TrialBalance.DownloadWebsite {
    /// <summary>
    /// An HTTP Handler to retrieve files from the file server. 
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetFileHandler : IHttpHandler {
        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The HttpContext to retrieve the request info from and to write the response to.</param>
        public void ProcessRequest(HttpContext context) {
            DownloadFile downloadFile = null;
            if (context.Request.QueryString["FileID"] != null) {
                int fileID = Convert.ToInt32(context.Request.QueryString["FileID"]);
                downloadFile = DataProvider.Instance.GetFileMetadata(fileID);
            }

            if (downloadFile == null) {
                context.Response.StatusCode = 404;
            } else {
                try {
                    context.Response.ContentType = downloadFile.ContentType;
                    context.Response.WriteFile(downloadFile.ServerFilePath);

                    DataProvider.Instance.RegisterFileDownload(downloadFile.FileID);
                } catch (Exception ex) {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        public bool IsReusable {
            get {
                return true;
            }
        }
    }
}
