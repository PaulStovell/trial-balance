using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaulStovell.TrialBalance.DomainModel;
using PaulStovell.TrialBalance.Tests.DataLayer.MockObjects;

namespace PaulStovell.TrialBalance.Tests.DomainModel {
    [TestClass]
    public class TransactionCollectionViewTests {
        [TestMethod]
        public void AddToUnderlyingCollectionTest() {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook()) {

                #region Standard setup code
                // Create two test accounts
                ChangeCoordinator<Asset> cashAtBank = workbook.CreateAsset();
                ChangeCoordinator<Liability> mortgage = workbook.CreateLiability();

                // Create a view over the Cash at Bank transactions
                TransactionCollectionView credits = new TransactionCollectionView(cashAtBank.EditableItem, workbook.CurrentPeriod, BalanceType.Credit);
                TransactionCollectionView debits = new TransactionCollectionView(cashAtBank.EditableItem, workbook.CurrentPeriod, BalanceType.Debit);
                #endregion

                // Create a transaction
                ChangeCoordinator<Transaction> t1 = workbook.CreateTransaction();
                t1.EditableItem.CreditAccount = cashAtBank.EditableItem;
                t1.EditableItem.DebitAccount = mortgage.EditableItem;
                t1.EditableItem.Date = workbook.CurrentPeriod.EndDate.AddDays(-1);
                t1.EditableItem.Value = 1000M;
                t1.EditableItem.Particulars = "A test transaction";
                t1.PushChanges();

                Assert.AreEqual(1, cashAtBank.EditableItem.Transactions.Count, "The underlying transaction should have 1 item.");
                Assert.AreEqual(1, credits.Count, "The credits collection view should have 1 item");
                Assert.AreEqual(0, debits.Count, "The debits collection view should have 0 items");
            }
        }

        [TestMethod]
        public void RemoveAndAddTest() {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook()) {

                #region Standard setup code
                // Create two test accounts
                ChangeCoordinator<Asset> cashAtBank = workbook.CreateAsset();
                ChangeCoordinator<Liability> mortgage = workbook.CreateLiability();

                // Create a view over the Cash at Bank transactions
                TransactionCollectionView credits = new TransactionCollectionView(cashAtBank.EditableItem, workbook.CurrentPeriod, BalanceType.Credit);
                TransactionCollectionView debits = new TransactionCollectionView(cashAtBank.EditableItem, workbook.CurrentPeriod, BalanceType.Debit);
                #endregion

                // Create a transaction
                ChangeCoordinator<Transaction> t1 = workbook.CreateTransaction();
                t1.EditableItem.CreditAccount = cashAtBank.EditableItem;
                t1.EditableItem.DebitAccount = mortgage.EditableItem;
                t1.EditableItem.Date = workbook.CurrentPeriod.EndDate.AddDays(-1);
                t1.EditableItem.Value = 1000M;
                t1.EditableItem.Particulars = "A test transaction";
                t1.PushChanges();

                Assert.AreEqual(1, cashAtBank.EditableItem.Transactions.Count, "The underlying transaction should have 1 item.");
                Assert.AreEqual(1, credits.Count, "The credits collection view should have 1 item");
                Assert.AreEqual(0, debits.Count, "The debits collection view should have 0 items");

                // Now make it appear in the debits
                ChangeCoordinator<Transaction> t2 = workbook.AcquireChangeCoordinator(credits[0]);
                t2.EditableItem.DebitAccount = cashAtBank.EditableItem;
                t2.EditableItem.CreditAccount = mortgage.EditableItem;
                t2.PushChanges();

                Assert.AreEqual(1, cashAtBank.EditableItem.Transactions.Count, "The underlying transaction should have 1 item.");
                Assert.AreEqual(0, credits.Count, "The credits collection view should have 0 items");
                Assert.AreEqual(1, debits.Count, "The debits collection view should have 1 item");
            }
        }

        [TestMethod]
        public void ViewOverExistingCollectionTests() {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook()) {

                // Create two test accounts
                ChangeCoordinator<Asset> cashAtBank = workbook.CreateAsset();
                ChangeCoordinator<Liability> mortgage = workbook.CreateLiability();

                // Create a transaction
                ChangeCoordinator<Transaction> t1 = workbook.CreateTransaction();
                t1.EditableItem.CreditAccount = cashAtBank.EditableItem;
                t1.EditableItem.DebitAccount = mortgage.EditableItem;
                t1.EditableItem.Date = workbook.CurrentPeriod.EndDate.AddDays(-1);
                t1.EditableItem.Value = 1000M;
                t1.EditableItem.Particulars = "A test transaction";
                t1.PushChanges();

                // NOW create a view over the Cash at Bank transactions
                TransactionCollectionView credits = new TransactionCollectionView(cashAtBank.EditableItem, workbook.CurrentPeriod, BalanceType.Credit);
                TransactionCollectionView debits = new TransactionCollectionView(cashAtBank.EditableItem, workbook.CurrentPeriod, BalanceType.Debit);

                Assert.AreEqual(1, cashAtBank.EditableItem.Transactions.Count, "The underlying transaction should have 1 item.");
                Assert.AreEqual(1, credits.Count, "The credits collection view should have 1 item");
                Assert.AreEqual(0, debits.Count, "The debits collection view should have 0 items");
            }
        }

        [TestMethod]
        public void ClosingBalanceTest() {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook()) {

                // Create two test accounts
                ChangeCoordinator<Asset> cashAtBank = workbook.CreateAsset();
                ChangeCoordinator<Liability> mortgage = workbook.CreateLiability();

                // Create a transaction
                ChangeCoordinator<Transaction> t1 = workbook.CreateTransaction();
                t1.EditableItem.CreditAccount = cashAtBank.EditableItem;
                t1.EditableItem.DebitAccount = mortgage.EditableItem;
                t1.EditableItem.Date = workbook.CurrentPeriod.EndDate.AddDays(-1);
                t1.EditableItem.Value = 1000M;
                t1.EditableItem.Particulars = "A test transaction";
                t1.PushChanges();

                // Now create a view over the Cash at Bank transactions
                TransactionCollectionView balance = new TransactionCollectionView(cashAtBank.EditableItem, workbook.CurrentPeriod);

                Assert.AreEqual(1, cashAtBank.EditableItem.Transactions.Count, "The underlying transaction should have 1 item.");
                Assert.AreEqual(1, balance.Count, "The balance collection view should have 1 item");
                Assert.AreEqual(new Balance(0M, BalanceType.Debit), balance.OpeningBalance, "The opening balance should be 0 DR");
                Assert.AreEqual(new Balance(1000M, BalanceType.Credit), balance.ClosingBalance, "The closing balance should be 1000 CR");
            }
        }

        [TestMethod]
        public void TransactionNotInRangeNotShown() {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook()) {

                // Create two test accounts
                ChangeCoordinator<Asset> cashAtBank = workbook.CreateAsset();
                ChangeCoordinator<Liability> mortgage = workbook.CreateLiability();

                // Create a transaction
                ChangeCoordinator<Transaction> t1 = workbook.CreateTransaction();
                t1.EditableItem.CreditAccount = cashAtBank.EditableItem;
                t1.EditableItem.DebitAccount = mortgage.EditableItem;
                t1.EditableItem.Date = workbook.CurrentPeriod.StartDate.AddDays(-50);
                t1.EditableItem.Value = 1000M;
                t1.EditableItem.Particulars = "A test transaction";
                t1.PushChanges();

                // Now create a view over the Cash at Bank transactions
                TransactionCollectionView balance = new TransactionCollectionView(cashAtBank.EditableItem, workbook.CurrentPeriod);

                Assert.AreEqual(1, cashAtBank.EditableItem.Transactions.Count, "The underlying transaction should have 1 item.");
                Assert.AreEqual(0, balance.Count, "The balance collection view should have 0 items");
                Assert.AreEqual(new Balance(1000M, BalanceType.Credit), balance.OpeningBalance, "The opening balance should be 1000 CR");
                Assert.AreEqual(new Balance(1000M, BalanceType.Credit), balance.ClosingBalance, "The closing balance should be 1000 CR");
            }
        }

        [TestMethod]
        public void TransactionDateChangedTest() {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook()) {

                // Create two test accounts
                ChangeCoordinator<Asset> cashAtBank = workbook.CreateAsset();
                ChangeCoordinator<Liability> mortgage = workbook.CreateLiability();

                // Create a transaction
                ChangeCoordinator<Transaction> t1 = workbook.CreateTransaction();
                t1.EditableItem.CreditAccount = cashAtBank.EditableItem;
                t1.EditableItem.DebitAccount = mortgage.EditableItem;
                t1.EditableItem.Date = workbook.CurrentPeriod.StartDate.AddDays(-50);
                t1.EditableItem.Value = 1000M;
                t1.EditableItem.Particulars = "A test transaction";
                t1.PushChanges();

                // Now create a view over the Cash at Bank transactions
                TransactionCollectionView balance = new TransactionCollectionView(cashAtBank.EditableItem, workbook.CurrentPeriod);

                Assert.AreEqual(1, cashAtBank.EditableItem.Transactions.Count, "The underlying transaction should have 1 item.");
                Assert.AreEqual(0, balance.Count, "The balance collection view should have 0 items");
                Assert.AreEqual(new Balance(1000M, BalanceType.Credit), balance.OpeningBalance, "The opening balance should be 1000 CR");
                Assert.AreEqual(new Balance(1000M, BalanceType.Credit), balance.ClosingBalance, "The closing balance should be 1000 CR");

                // Now if we change the date, the transactions and balance should update
                ChangeCoordinator<Transaction> t2 = workbook.AcquireChangeCoordinator(cashAtBank.EditableItem.Transactions[0]);
                t2.EditableItem.Date = workbook.CurrentPeriod.EndDate.AddDays(-1);
                t2.PushChanges();

                Assert.AreEqual(1, cashAtBank.EditableItem.Transactions.Count, "The underlying transaction should have 1 item.");
                Assert.AreEqual(1, balance.Count, "The balance collection view should have 1 item");
                Assert.AreEqual(new Balance(0M, BalanceType.Debit), balance.OpeningBalance, "The opening balance should be 0 DR");
                Assert.AreEqual(new Balance(1000M, BalanceType.Credit), balance.ClosingBalance, "The closing balance should be 1000 CR");
            }
        }


    }

}