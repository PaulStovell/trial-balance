using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PaulStovell.TrialBalance.DomainModel.PackageDataProvider;
using PaulStovell.TrialBalance.DomainModel;

namespace PaulStovell.TrialBalance.Tests.DataLayer.MockObjects {
    public class TemporaryPackageDataProvider : PackageDataProvider, IDisposable {

        public TemporaryPackageDataProvider()
            : base("C:\\Windows\\Temp\\" + "Temp-" + Guid.NewGuid().ToString() + ".tbdx")
        {
            CreateFile(this.FilePath);
        }

        #region IDisposable Members

        public void Dispose() {
            try {
                if (File.Exists(this.FilePath)) {
                    File.Delete(this.FilePath);
                }
            } catch {

            }
        }

        #endregion
    }
}
