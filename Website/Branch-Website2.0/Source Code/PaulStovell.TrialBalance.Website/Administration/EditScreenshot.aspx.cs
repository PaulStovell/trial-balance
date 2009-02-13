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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace PaulStovell.TrialBalance.Website.Administration {
    public partial class EditScreenshot : System.Web.UI.Page {
        public const string SCREENSHOT_ID_QUERY_STRING_KEY = "ScreenshotID";
        private Screenshot _screenshot;

        protected void Page_Load(object sender, EventArgs e) {
            string screenshotIDString = this.Request.QueryString[SCREENSHOT_ID_QUERY_STRING_KEY];
            int screenshotID = 0;
            if (screenshotIDString != null && int.TryParse(screenshotIDString, out screenshotID)) {
                this.Screenshot = DataProvider.Instance.GetScreenshot(screenshotID);
            }

            if (this.Screenshot == null) {
                this.Screenshot = new Screenshot();
                this.Screenshot.DateTaken = DateTime.Now;
            }

            if (!this.IsPostBack) {
                _captionTextBox.Text = this.Screenshot.Caption;
            }
        }

        public Screenshot Screenshot {
            get { return _screenshot; }
            set { _screenshot = value; }
        }

        protected void SaveButton_Clicked(object sender, EventArgs e) {
            this.Screenshot.Caption = _captionTextBox.Text;
            DataProvider.Instance.SaveScreenshot(this.Screenshot);

            if (_imageFileUpload.HasFile) {
                _imageFileUpload.SaveAs(Server.MapPath(this.Screenshot.LargeImageLink));

                System.Drawing.Image b = System.Drawing.Image.FromStream(_imageFileUpload.FileContent);
                System.Drawing.Image i = b.GetThumbnailImage(400, 300, delegate { return false; }, IntPtr.Zero);
                i.Save(Server.MapPath(this.Screenshot.SmallImageLink), ImageFormat.Png);
            }

            this.Response.Redirect("~/Screenshots.aspx");
        }

        protected void DeleteButton_Clicked(object sender, EventArgs e) {
            DataProvider.Instance.DeleteScreenshot(this.Screenshot);

            this.Response.Redirect("~/Screenshots.aspx");
        }
    }
}
