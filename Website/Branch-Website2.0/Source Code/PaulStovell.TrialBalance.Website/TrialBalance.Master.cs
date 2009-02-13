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
using System.Collections.ObjectModel;

namespace PaulStovell.TrialBalance.Website {
    public partial class TrialBalance : System.Web.UI.MasterPage {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected override void OnPreRender(EventArgs e) {
            // Render any non-critical exceptions
            ApplicationPage applicationPage = this.Page as ApplicationPage;
            if (applicationPage != null) {
                ReadOnlyCollection<Exception> nonCriticalPageExceptions = applicationPage.GetNonCriticalPageExceptions();
                if (nonCriticalPageExceptions.Count > 0) {
                    _exceptionRepeater.Visible = true;
                    _exceptionRepeater.DataSource = nonCriticalPageExceptions;
                    _exceptionRepeater.DataBind();
                } else {
                    _exceptionRepeater.Visible = false;
                }
            }
            
            base.OnPreRender(e);
        }
    }
}
