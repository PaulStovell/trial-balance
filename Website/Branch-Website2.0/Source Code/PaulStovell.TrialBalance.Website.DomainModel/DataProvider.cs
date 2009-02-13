using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.Configuration;
using PaulStovell.TrialBalance.WebsiteCommon;

namespace PaulStovell.TrialBalance.Website.DomainModel {
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

        public BindingList<Build> GetBuilds(int limit) {
            return Helper.ExecuteSelectCommand<Build>(
                Helper.CreateCommand("[TrialBalanceBuildSystem].[GetBuilds]",
                    Helper.CreateParameter("@onlyPublicBuilds", !AuthenticationHelper.IsAuthenticated),
                    Helper.CreateParameter("@limit", limit)
                ));
        }

        public Build GetBuild(string buildNumber) {
            Build result = null;

            BindingList<Build> results = Helper.ExecuteSelectCommand<Build>(
                Helper.CreateCommand("[TrialBalanceBuildSystem].[GetBuild]",
                    Helper.CreateParameter("@buildNumber", buildNumber),
                    Helper.CreateParameter("@onlyPublicBuilds", !AuthenticationHelper.IsAuthenticated)
                ));
            if (results.Count > 0) {
                result = results[0];
            }

            return result;
        }

        public Build GetLatestStableBuild() {
            Build result = null;

            BindingList<Build> results = Helper.ExecuteSelectCommand<Build>(
                Helper.CreateCommand("[TrialBalanceBuildSystem].[GetLatestStableBuild]"
                ));
            if (results.Count > 0) {
                result = results[0];
            }

            return result;
        }

        public void SaveBuild(Build build) {
            Helper.ExecuteNonQuery(
                Helper.CreateCommand("[TrialBalanceBuildSystem].[SaveBuild]",
                    Helper.CreateParameter("@buildNumber", build.BuildNumber),
                    Helper.CreateParameter("@buildDate", build.BuildDate),
                    Helper.CreateParameter("@isSuccessful", build.IsSuccessful),
                    Helper.CreateParameter("@buildStatus", build.BuildStatus),
                    Helper.CreateParameter("@releaseNotes", build.ReleaseNotes),
                    Helper.CreateParameter("@majorVersion", build.MajorVersion),
                    Helper.CreateParameter("@minorVersion", build.MinorVersion),
                    Helper.CreateParameter("@buildVersion", build.BuildVersion),
                    Helper.CreateParameter("@revisionVersion", build.RevisionVersion),
                    Helper.CreateParameter("@downloads", build.Downloads),
                    Helper.CreateParameter("@isPublic", build.IsPublic),
                    Helper.CreateParameter("@sourceCodeUrl", build.SourceCodeUrl),
                    Helper.CreateParameter("@installerUrl", build.InstallerUrl),
                    Helper.CreateParameter("@clickOnceUrl", build.ClickOnceUrl)
                ));
        }

        public BindingList<Screenshot> GetScreenshots() {
            return Helper.ExecuteSelectCommand<Screenshot>(
                Helper.CreateCommand("[TrialBalanceMedia].[GetScreenshots]"));
        }

        public Screenshot GetScreenshot(int screenshotID) {
            Screenshot result = null;
            BindingList<Screenshot> results = Helper.ExecuteSelectCommand<Screenshot>(
                Helper.CreateCommand("[TrialBalanceMedia].[GetScreenshot]",
                    Helper.CreateParameter("@screenshotID", screenshotID)));
            if (results.Count > 0) {
                result = results[0];
            }
            return result;
        }

        public void SaveScreenshot(Screenshot screenshot) {

            SqlParameter p = Helper.CreateOutParameter("@screenshotID", screenshot.ScreenshotID);
            Helper.ExecuteNonQuery(
                Helper.CreateCommand("[TrialBalanceMedia].[SaveScreenshot]",
                    p,
                    Helper.CreateParameter("@dateTaken", screenshot.DateTaken),
                    Helper.CreateParameter("@caption", screenshot.Caption)));
            screenshot.ScreenshotID = (int)p.Value;
        }

        public void DeleteScreenshot(Screenshot screenshot) {
            Helper.ExecuteNonQuery(
                Helper.CreateCommand("[TrialBalanceMedia].[DeleteScreenshot]",
                    Helper.CreateParameter("@screenshotID", screenshot.ScreenshotID)));
        }

        public string GetNextBuildNumber() {
            SqlParameter nextMajor = Helper.CreateOutParameter("@nextMajorVersion", 1);
            SqlParameter nextMinor = Helper.CreateOutParameter("@nextMinorVersion", 1);
            SqlParameter nextBuild = Helper.CreateOutParameter("@nextBuildVersion", 1);
            SqlParameter nextRevision = Helper.CreateOutParameter("@nextRevisionVersion", 1);
            Helper.ExecuteNonQuery(
                Helper.CreateCommand("[TrialBalanceBuildSystem].[GetNextBuildNumber]",
                    nextMajor, nextMinor, nextBuild, nextRevision));
            return nextMajor.Value.ToString() + "." + nextMinor.Value.ToString() + "." + nextBuild.Value.ToString() + "." + nextRevision.Value.ToString();
        }

        public void AppendFeatureUsageRecord(string applicationName, string applicationVersion, string featureGroup, string featureName, string featureDetails, string operatingSystem, string clientIPAddress, DateTime dateRecorded) {
            Helper.ExecuteNonQuery(
                Helper.CreateCommand("[TrialBalanceReporting].[AppendFeatureUsageRecord]",
                    Helper.CreateParameter("@applicationName", applicationName),
                    Helper.CreateParameter("@applicationVersion",  applicationVersion),
                    Helper.CreateParameter("@featureGroup", featureGroup),
                    Helper.CreateParameter("@featureName", featureName),
                    Helper.CreateParameter("@featureDetails", featureDetails),
                    Helper.CreateParameter("@operatingSystem", operatingSystem),
                    Helper.CreateParameter("@clientIPAddress", clientIPAddress),
                    Helper.CreateParameter("@dateRecorded", dateRecorded)
                ));
        }
    }
}