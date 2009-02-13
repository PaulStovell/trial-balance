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
    public class Screenshot {
        private int _screenshotID;
        private string _caption;
        private DateTime _dateTaken;

        public Screenshot() {

        }

        public int ScreenshotID { get { return _screenshotID; } set { _screenshotID = value; } }
        public string Caption { get { return _caption; } set { _caption = value; } }
        public DateTime DateTaken { get { return _dateTaken; } set { _dateTaken = value; } }

        public string LargeImageLink {
            get { return "/Screenshots/" + this.ScreenshotID.ToString() + ".png"; }
        }

        public string SmallImageLink {
            get { return "/Screenshots/" + this.ScreenshotID.ToString() + "-small.png"; }
        }

        public string EditScreenshotLink {
            get { return "/Administration/EditScreenshot.aspx?ScreenshotID=" + this.ScreenshotID; }
        }

        
    }
}
