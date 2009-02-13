using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using PaulStovell.TrialBalance.MainWebsite.DomainModel;
using System.IO;
using System.Security.Authentication;
using System.Web.Security;

namespace PaulStovell.TrialBalance.MainWebsite.Services {
    /// <summary>
    /// Summary description for UploadBuildService.
    /// </summary>
    [WebService(Namespace = "http://trialbalance.net.au/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class UploadBuildService : System.Web.Services.WebService {
        [WebMethod(EnableSession = true)]
        public void UploadBuild(string username, string password, string buildNumber, DateTime buildDate, bool isSuccessful, string buildStatus, string version, bool isPublic, string releaseNotes, 
            string sourceCodeUrl, 
            string installerUrl,
            string clickOnceUrl) {
            Authenticate(username, password);

            Build build = new Build();
            build.BuildNumber = buildNumber;
            build.BuildDate = buildDate;
            build.Downloads = 0;
            build.BuildStatus = buildStatus;
            build.SourceCodeUrl = sourceCodeUrl;
            build.InstallerUrl = installerUrl;
            build.ClickOnceUrl = clickOnceUrl;
            build.IsPublic = isPublic;
            build.IsSuccessful = isSuccessful;
            build.ReleaseNotes = releaseNotes;
            build.Version = new Version(version);

            DataProvider.Instance.SaveBuild(build);
        }

        [WebMethod(EnableSession=true)]
        public string GetNextBuildNumber(string username, string password) {
            Authenticate(username, password);
            return DataProvider.Instance.GetNextBuildNumber();
        }

        private void Authenticate(string username, string password) {
            if (!FormsAuthentication.Authenticate(username, password)) {
                throw new AuthenticationException();
            }
        }
    }
}
