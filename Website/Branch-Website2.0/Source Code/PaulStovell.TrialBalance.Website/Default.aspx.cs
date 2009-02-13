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
    public partial class Default : ApplicationPage {
        private Screenshot _mainScreenshot;

        protected void Page_Load(object sender, EventArgs e) {
            BindingList<Screenshot> screenshots = DataProvider.Instance.GetScreenshots();
            if (screenshots.Count > 0) {
                Random r = new Random(Guid.NewGuid().ToByteArray()[1]);
                int ix = r.Next(0, screenshots.Count - 1);
                MainScreenshot = screenshots[ix];
            }
        }

        public Screenshot MainScreenshot {
            get { return _mainScreenshot; }
            set { _mainScreenshot = value; }
        }
    }
}
