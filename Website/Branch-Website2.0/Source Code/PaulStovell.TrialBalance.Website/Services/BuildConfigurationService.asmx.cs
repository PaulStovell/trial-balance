using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.IO;
using System.Security.Authentication;
using System.Web.Security;
using PaulStovell.TrialBalance.Website.DomainModel;

namespace PaulStovell.TrialBalance.Website.Services {
    /// <summary>
    /// Summary description for BuildConfigurationService.
    /// </summary>
    [WebService(Namespace = "http://trialbalance.net.au/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class BuildConfigurationService : System.Web.Services.WebService {
        /// <summary>
        /// Uploads a build to the web server.
        /// </summary>
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

        /// <summary>
        /// Gets the next build number to use.
        /// </summary>
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
