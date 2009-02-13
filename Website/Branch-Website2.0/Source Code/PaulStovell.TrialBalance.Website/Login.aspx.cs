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

namespace PaulStovell.TrialBalance.Website {
    public partial class Login : ApplicationPage {
        protected void Page_Load(object sender, EventArgs e) {
            
        }

        protected void Login_Authenticate(object sender, AuthenticateEventArgs e) {
            e.Authenticated = true;
        }
    }
}
