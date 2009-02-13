using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using PaulStovell.TrialBalance.Website.DomainModel;
using System.Text;

namespace PaulStovell.TrialBalance.Website.Services {
    /// <summary>
    /// Web services used by the TrialBalance application.
    /// </summary>
    [WebService(Namespace = "http://www.trialbalance.net.au/Services/1.0")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class TrialBalanceApplicationServices : System.Web.Services.WebService {
        [WebMethod()]
        [SoapDocumentMethod(OneWay=true)]
        public void ReportFeatureUsage(string applicationName, string applicationVersion, string featureGroup, string featureName, string featureDetails, string operatingSystem) {
            DataProvider.Instance.AppendFeatureUsageRecord(applicationName, applicationVersion, featureGroup, featureName, featureDetails, operatingSystem,
                HttpContext.Current.Request.UserHostAddress, DateTime.Now);
            
        }

        [WebMethod()]
        [SoapDocumentMethod(OneWay = true)]
        public void ReportBug(string applicationName, string applicationVersion, string operatingSystem, string message, string exceptionType, string stackTrace) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html><body>");
            sb.AppendLine("<table>");
            BuildBugReportRow(sb, "Application Name", applicationName);
            BuildBugReportRow(sb, "Application Version", applicationVersion);
            BuildBugReportRow(sb, "Operating System", operatingSystem);
            BuildBugReportRow(sb, "Message", message);
            BuildBugReportRow(sb, "Exception Type", exceptionType);
            BuildBugReportRow(sb, "Stack Trace", stackTrace);
            BuildBugReportRow(sb, "Report Date", DateTime.Now.ToString());
            BuildBugReportRow(sb, "User IP", HttpContext.Current.Request.UserHostAddress);
            sb.AppendLine("<table>");
            sb.AppendLine("</body></html>");

            Message dispatchMessage = new Message();
            dispatchMessage.DateCreated = DateTime.Now;
            dispatchMessage.MessageTitle = "Bug Report";
            dispatchMessage.MessageBody = sb.ToString();

            MessageDispatcher.Dispatch(dispatchMessage);
        }

        private void BuildBugReportRow(StringBuilder sb, string key, string value) {
            sb.AppendLine("<tr>");
            sb.AppendLine("<td><span style='font-family: verdana; font-size: 10pt; font-weight: bold'>");
            sb.AppendLine(Server.HtmlEncode(key));
            sb.AppendLine("</span></td>");
            sb.AppendLine("<td><span style='font-family: verdana; font-size: 10pt; font-weight: bold'>");
            sb.AppendLine(Server.HtmlEncode(value).Replace("\n", "\n<br />"));
            sb.AppendLine("</span></td>");
            sb.AppendLine("</tr>");
        }
    }
}
