using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PaulStovell.TrialBalance.MainWebsite.DomainModel;

namespace PaulStovell.TrialBalance.MainWebsite {
    public partial class GetBuild : System.Web.UI.Page {
        public const string BUILD_NUMBER_QUERY_STRING_KEY = "BuildNumber";
        private Build _build;

        protected void Page_Load(object sender, EventArgs e) {
            string buildNumber = this.Request.QueryString[BUILD_NUMBER_QUERY_STRING_KEY];
            if (buildNumber != null) {
                this.Build = DataProvider.Instance.GetBuild(buildNumber);
            }

            if (this.Build == null) {
                this.Response.Redirect("~/Builds.aspx");
            }
        }

        public Build Build {
            get { return _build; }
            set { _build = value; }
        }

        public void DownloadButton_Clicked(object sender, EventArgs e) {
            this.Build.Downloads++;
            DataProvider.Instance.SaveBuild(this.Build);
        }
    }
}
