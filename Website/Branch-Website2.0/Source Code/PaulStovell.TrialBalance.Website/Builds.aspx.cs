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
using PaulStovell.TrialBalance.Website.DomainModel;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.Website {
    public partial class Builds : ApplicationPage {
        private Build _latestStableBuild;

        /// <summary>
        /// Occurs when the page loads.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e) {
            _buildsDataGrid.Columns[6].Visible = AuthenticationHelper.IsAuthenticated;

            this.LatestStableBuild = DataProvider.Instance.GetLatestStableBuild();
            _downloadAreaPlaceholder.Visible = this.LatestStableBuild != null;

            BindingList<Build> builds = DataProvider.Instance.GetBuilds(20);
            _buildsDataGrid.DataSource = builds;
            _buildsDataGrid.DataBind();
        }

        public Build LatestStableBuild {
            get { return _latestStableBuild; }
            set { _latestStableBuild = value; }
        }

        protected Build GetBuild(object container) {
            return ((DataGridItem)container).DataItem as Build;
        }

        protected string GetSuccessfulImageAlt(object container) {
            string alt = "This build was successful";
            if (GetBuild(container).IsSuccessful == false) {
                alt = "There were errors in this build";
            }
            return alt;
        }

        protected string GetSuccessfulImageSource(object container) {
            string src = "~/Images/accept.png";
            if (GetBuild(container).IsSuccessful == false) {
                src = "~/Images/exclamation.png";
            }
            return src;
        }
    }
}
