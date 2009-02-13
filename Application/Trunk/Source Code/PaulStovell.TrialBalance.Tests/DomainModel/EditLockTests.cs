using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaulStovell.TrialBalance.DomainModel;

namespace PaulStovell.TrialBalance.Tests.DomainModel {
    [TestClass]
    public class ChangeCoordinatorTests {

        [TestMethod]
        public void EditWithinChangeCoordinatorWorks() {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook()) {
                ChangeCoordinator<Account> a = workbook.CreateAccount(AccountType.Asset);
                a.EditableItem.Name = "Fred";
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AccessViolationException))]
        public void EditOutsideChangeCoordinatorThrowsException() {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook()) {
                ChangeCoordinator<Account> a = workbook.CreateAccount(AccountType.Asset);
                a.EditableItem.Name = "Fred";
                Guid id = a.EditableItem.AccountID;
                a.PushChanges();

                Asset loaded = workbook.FetchAsset(id);
                Assert.IsNotNull(loaded);

                loaded.Name = "Fred";
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EditAfterPushChangesThrowsException() {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook()) {
                ChangeCoordinator<Account> a = workbook.CreateAccount(AccountType.Asset);
                a.EditableItem.Name = "Fred";
                Guid id = a.EditableItem.AccountID;
                a.PushChanges();

                a.EditableItem.Name = "Fred";
            }
        }


        [TestMethod]
        public void EditAfterFetchingWorks() {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook()) {
                ChangeCoordinator<Account> a = workbook.CreateAccount(AccountType.Asset);
                a.EditableItem.Name = "Fred";
                Guid id = a.EditableItem.AccountID;
                a.PushChanges();

                Asset loaded = workbook.FetchAsset(id);

                Assert.IsNotNull(loaded);
                Assert.AreEqual("Fred", loaded.Name);

                ChangeCoordinator<Asset> loadedLock = workbook.AcquireChangeCoordinator(loaded);
                loadedLock.EditableItem.Name = "Tom";
                loadedLock.PushChanges();
            }
        }

        [TestMethod]
        public void BeforePushChangesDontAppear() {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook()) {
                ChangeCoordinator<Account> a = workbook.CreateAccount(AccountType.Asset);
                a.EditableItem.Name = "Fred";
                Guid id = a.EditableItem.AccountID;
                a.PushChanges();

                Asset loaded = workbook.FetchAsset(id);

                Assert.IsNotNull(loaded);
                Assert.AreEqual("Fred", loaded.Name);

                ChangeCoordinator<Asset> loadedLock = workbook.AcquireChangeCoordinator(loaded);
                loadedLock.EditableItem.Name = "Tom";
                
                // Now, check that the original item ("loaded") is still "Fred" and not "Tom"
                Assert.AreEqual("Fred", loaded.Name);
                Assert.AreEqual("Tom", loadedLock.EditableItem.Name);

                loadedLock.PushChanges();
            }
        }

        [TestMethod]
        public void AfterPushChangesDoAppear() {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook()) {
                ChangeCoordinator<Account> a = workbook.CreateAccount(AccountType.Asset);
                a.EditableItem.Name = "Fred";
                Guid id = a.EditableItem.AccountID;
                a.PushChanges();

                Asset loaded = workbook.FetchAsset(id);

                Assert.IsNotNull(loaded);
                Assert.AreEqual("Fred", loaded.Name);

                ChangeCoordinator<Asset> loadedLock = workbook.AcquireChangeCoordinator(loaded);
                loadedLock.EditableItem.Name = "Tom";

                // Now, check that the original item ("loaded") is still "Fred" and not "Tom"
                Assert.AreEqual("Fred", loaded.Name);
                Assert.AreEqual("Tom", loadedLock.EditableItem.Name);

                loadedLock.PushChanges();

                Assert.AreEqual("Tom", loaded.Name);
            }
        }


    }
}
