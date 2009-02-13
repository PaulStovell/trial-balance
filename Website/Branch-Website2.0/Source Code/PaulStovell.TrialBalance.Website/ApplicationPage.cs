using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.UI.MobileControls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PaulStovell.TrialBalance.Website {
    public class ApplicationPage : System.Web.UI.Page {
        private List<Exception> _nonCriticalPageExceptions;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationPage() {
            _nonCriticalPageExceptions = new List<Exception>();
        }

        protected override void OnLoad(EventArgs e) {
            try {
                base.OnLoad(e);
            } catch (ApplicationException ex) {
                _nonCriticalPageExceptions.Add(ex);
            }
        }

        protected override void OnInit(EventArgs e) {
            try {
                base.OnInit(e);
            } catch (ApplicationException ex) {
                _nonCriticalPageExceptions.Add(ex);
            }
        }

        protected override void OnPreInit(EventArgs e) {
            try {
                base.OnPreInit(e);
            } catch (ApplicationException ex) {
                _nonCriticalPageExceptions.Add(ex);
            }
        }

        public ReadOnlyCollection<Exception> GetNonCriticalPageExceptions() {
            return _nonCriticalPageExceptions.AsReadOnly();
        }
    }
}
