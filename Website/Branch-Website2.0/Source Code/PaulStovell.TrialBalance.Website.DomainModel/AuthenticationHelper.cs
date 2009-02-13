using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace PaulStovell.TrialBalance.Website.DomainModel {
    public class AuthenticationHelper {
        public static bool IsAuthenticated {
            get { return HttpContext.Current.User.Identity.IsAuthenticated; }
        }
    }
}
