using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using PaulStovell.TrialBalance.Website.DomainModel;
using System.Text;

namespace PaulStovell.TrialBalance.Website {
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BuildsRss : IHttpHandler {

        public void ProcessRequest(HttpContext context) {
            context.Response.ContentType = "application/rss+xml";

            WriteHeader();
            foreach (Build build in DataProvider.Instance.GetBuilds(20)) {
                StringBuilder description = new StringBuilder();
                description.Append("<p>TrialBalance version <b>");
                description.Append(build.Version.ToString());
                description.Append(" </b>was uploaded on ");
                description.Append(build.BuildDate.ToLongDateString());
                description.Append(". ");
                if (build.IsSuccessful) {
                    description.Append("This was a successful build ");
                } else {
                    description.Append("This was an unsuccessful build ");
                }

                description.Append("and has a status of: \"" + build.BuildStatus + "\". ");

                description.Append("</p>");

                if (build.HasInstaller) {
                    description.Append("<p><a href=\"");
                    description.Append(build.InstallerUrl);
                    description.Append("\">Download Installer</a></p>");
                }
                if (build.HasSourceCode) {
                    description.Append("<p><a href=\"");
                    description.Append(build.SourceCodeUrl);
                    description.Append("\">Download Source Code</a></p>");
                }
                if (build.HasClickOnce) {
                    description.Append("<p><a href=\"");
                    description.Append(build.ClickOnceUrl);
                    description.Append("\">Try Online</a></p>");
                }

                if (build.ReleaseNotes != null && build.ReleaseNotes != "") {
                    description.Append("<p>");
                    description.Append("<b>Release Notes:</b>");
                    description.Append("</p>");
                    description.Append(build.ReleaseNotes);
                }

                WriteLine("<item>");
                WriteLine("<title>TrialBalance version " + build.Version.ToString() + " released</title>");
                WriteLine("<link>http://trialbalance.net.au" + build.DownloadLink + "</link>");
                WriteLine("<pubDate>" + FormatDate(build.BuildDate) + "</pubDate>");
                WriteLine("<dc:creator>Paul Stovell</dc:creator>");
                WriteLine("<category>Build Notifications</category>");
                WriteLine("<guid isPermaLink=\"false\">http://trialbalance.net.au" + build.DownloadLink + "</guid>");
                WriteLine("<description><![CDATA[" + description.ToString() +"]]></description>");
                WriteLine("</item>");
            }
            WriteFooter();
        }

        public bool IsReusable {
            get {
                return false;
            }
        }

        private string FormatDate(DateTime d) {
            return d.ToUniversalTime().ToString("ddd, dd MM yyyy hh:mm:ss") + " GMT";
        }

        private void WriteHeader() {
            string pubDate = FormatDate(DateTime.Now);
            WriteLine(
            @"<?xml version=""1.0"" encoding=""utf-8""?>
            <rss version=""2.0"" 
                xmlns:content=""http://purl.org/rss/1.0/modules/content/""
                xmlns:wfw=""http://wellformedweb.org/CommentAPI/""
                xmlns:dc=""http://purl.org/dc/elements/1.1/"">
                <channel>
	                <title>TrialBalance Builds</title>
	                <link>http://www.trialbalance.net.au</link>
	                <description>TrialBalance Builds</description>
	                <pubDate>" + pubDate + @"</pubDate>
	                <generator>http://www.trialbalance.net.au/BuildsRss.ashx</generator>
	                <language>en</language>");
        }

        private void WriteFooter() {
            WriteLine("</channel></rss>");
        }

        private void WriteLine(string s) {
            HttpContext.Current.Response.Write(s + Environment.NewLine);
        }
    }
}
