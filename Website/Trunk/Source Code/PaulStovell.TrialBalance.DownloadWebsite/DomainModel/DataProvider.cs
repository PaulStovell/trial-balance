using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PaulStovell.TrialBalance.WebsiteCommon;
using System.Web.Configuration;

namespace PaulStovell.TrialBalance.DownloadWebsite.DomainModel {
    public sealed class DataProvider {
        private static readonly DataProvider _instance = new DataProvider();
        private DataProviderHelper _helper;

        private DataProvider() {
            _helper = new DataProviderHelper(WebConfigurationManager.ConnectionStrings["Main"].ConnectionString);
        }

        private DataProviderHelper Helper {
            get { return _helper; }
        }

        public static DataProvider Instance {
            get { return _instance; }
        }

        public DownloadFile GetFileMetadata(int fileID) {
            return Helper.ExecuteSingleSelectCommand<DownloadFile>(
                Helper.CreateCommand("[TrialBalanceDownloadServer].[GetFileMetadata]",
                    Helper.CreateParameter("@FileID", fileID)));
        }

        public void UploadFileMetadata(DownloadFile downloadFile) {
            downloadFile.FileID = Convert.ToInt32(Helper.ExecuteScalar(
                Helper.CreateCommand("[TrialBalanceDownloadServer].[UploadFileMetadata]",
                    Helper.CreateParameter("@FileName", downloadFile.FileName),
                    Helper.CreateParameter("@ContentType", downloadFile.ContentType))));
        }

        public void RegisterFileDownload(int fileID) {
            Helper.ExecuteNonQuery(
                Helper.CreateCommand("[TrialBalanceDownloadServer].[RegisterFileDownload]",
                    Helper.CreateParameter("@FileID", fileID)));
        }
    }
}
