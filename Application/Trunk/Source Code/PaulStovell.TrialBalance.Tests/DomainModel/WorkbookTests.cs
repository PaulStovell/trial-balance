using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaulStovell.TrialBalance.DomainModel;
using PaulStovell.TrialBalance.Tests.DataLayer.MockObjects;

namespace PaulStovell.TrialBalance.Tests.DomainModel
{
    class TemporaryWorkbook : Workbook, IDisposable
    {
        public TemporaryWorkbook() : base (new TemporaryPackageDataProvider())
        {

        }

        public void Dispose()
        {
            ((TemporaryPackageDataProvider)this.DataProvider).Dispose();
        }
    }

    [TestClass]
    public class WorkbookTests
    {

        [TestMethod]
        public void CreateAccountsReferencesEqualTest()
        {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook())
            {
                ChangeCoordinator<Asset> a1 = workbook.CreateAsset();
                Assert.AreNotEqual(a1.EditableItem.AccountID, Guid.Empty);

                Asset a2 = workbook.FetchAsset(a1.EditableItem.AccountID);
                Assert.AreEqual(a1.EditableItem.AccountID, a2.AccountID);
                Assert.IsTrue(object.ReferenceEquals(a1.EditableItem, a2));
            }
        }
    }
}
