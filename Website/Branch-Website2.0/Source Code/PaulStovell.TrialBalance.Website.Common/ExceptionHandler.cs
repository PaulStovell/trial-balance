using System;
using System.Data;
using System.Configuration;
using System.Web;

namespace PaulStovell.TrialBalance.Website.Common {
    public class ExceptionHandler {
        public static void HandleException(Exception ex) {
            throw ex;
        }
    }
}
