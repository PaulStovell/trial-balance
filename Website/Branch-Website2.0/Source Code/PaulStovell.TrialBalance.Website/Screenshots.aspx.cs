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
    public partial class Screenshots : ApplicationPage {
        protected void Page_Load(object sender, EventArgs e) {
            BindingList<Screenshot> screenshots = DataProvider.Instance.GetScreenshots();

            _screenshotRepeater.DataSource = screenshots;
            _screenshotRepeater.DataBind();
        }
    }
}
