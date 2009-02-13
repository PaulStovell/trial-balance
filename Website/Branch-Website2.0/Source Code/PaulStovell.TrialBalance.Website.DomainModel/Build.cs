using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.IO;

namespace PaulStovell.TrialBalance.Website.DomainModel {
    public class Build {
        private string _buildNumber;
        private DateTime _buildDate;
        private bool _isSuccessful;
        private string _buildStatus;
        private bool _isPublic;
        private string _releaseNotes;
        private short _majorVersion;
        private short _minorVersion;
        private short _buildVersion;
        private short _revisionVersion;
        private int _downloads;
        private string _sourceCodeUrl;
        private string _installerUrl;
        private string _clickOnceUrl;

        public string BuildNumber { get { return _buildNumber; } set { _buildNumber = value; } }
        public DateTime BuildDate { get { return _buildDate; } set { _buildDate = value; } }
        public bool IsSuccessful { get { return _isSuccessful; } set { _isSuccessful = value; } }
        public string BuildStatus { get { return _buildStatus; } set { _buildStatus = value; } }
        public bool IsPublic { get { return _isPublic; } set { _isPublic = value; } }
        public string ReleaseNotes { get { return _releaseNotes; } set { _releaseNotes = value; } }
        public short MajorVersion { get { return _majorVersion; } set { _majorVersion = value; } }
        public short MinorVersion { get { return _minorVersion; } set { _minorVersion = value; } }
        public short BuildVersion { get { return _buildVersion; } set { _buildVersion = value; } }
        public short RevisionVersion { get { return _revisionVersion; } set { _revisionVersion = value; } }
        public int Downloads { get { return _downloads; } set { _downloads = value; } }
        public string SourceCodeUrl { get { return _sourceCodeUrl ?? string.Empty; } set { _sourceCodeUrl = value; } }
        public string InstallerUrl { get { return _installerUrl ?? string.Empty; } set { _installerUrl = value; } }
        public string ClickOnceUrl { get { return _clickOnceUrl ?? string.Empty; } set { _clickOnceUrl = value; } }

        public string DownloadLink {
            get {
                return "/GetBuild.aspx?BuildNumber=" + this.BuildNumber;
            }
        }

        public string EditLink {
            get {
                return "/Administration/EditBuild.aspx?BuildNumber=" + this.BuildNumber;
            }
        }

        public Version Version {
            get {
                return new Version(this.MajorVersion, this.MinorVersion, this.BuildVersion, this.RevisionVersion);
            }
            set {
                this.MajorVersion = (short)value.Major;
                this.MinorVersion = (short)value.Minor;
                this.BuildVersion = (short)value.Build;
                this.RevisionVersion = (short)value.Revision;
            }
        }

        public bool HasInstaller {
            get { return (this.InstallerUrl ?? string.Empty).Length > 0; }
        }

        public bool HasSourceCode {
            get { return (this.SourceCodeUrl ?? string.Empty).Length > 0; }
        }

        public bool HasClickOnce {
            get { return (this.ClickOnceUrl ?? string.Empty).Length > 0; }
        }


    }
}
