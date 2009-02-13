using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaulStovell.TrialBalance.DomainModel;
using PaulStovell.TrialBalance.Tests.DataLayer.MockObjects;

namespace PaulStovell.TrialBalance.Tests.DomainModel {
    [TestClass]
    public class TransactionTests {

        //[TestMethod]
        //public void PlainTransactionTest() {
        //    Asset a = DataProvider.Current.CreateAsset();
        //    a.Name = "Cash at Bank";
        //    a.Description = "The cash in the bank.";

        //    Liability l = DataProvider.Current.CreateLiability();
        //    l.Name = "Mortgage";
        //    l.Description = "Mortgage for factory";

        //    Transaction t = DataProvider.Current.CreateTransaction();
        //    t.AddLine(a, new Balance(700M, BalanceType.Credit));
        //    t.AddLine(l, new Balance(700M, BalanceType.Debit));
        //    Assert.AreEqual(700M, t.Magnitude);
        //}

        //[TestMethod]
        //public void TransactionDoubleEntryPolicyNotEnforcedTest() {
        //    Asset a = DataProvider.Current.CreateAsset();
        //    a.Name = "Cash at Bank";
        //    a.Description = "The cash in the bank.";

        //    Liability l = Ledger.Instance.CreateLiability();
        //    l.Name = "Mortgage";
        //    l.Description = "Mortgage for factory";

        //    Transaction t = TransactionRegister.Instance.CreateTransaction(TransactionalIntegrityPolicy.NotEnforced);
        //    t.Date = DateTime.Now;
        //    t.Particulars = "Paying off mortgage.";
            
        //    t.AddLine(a, new Balance(700M, BalanceType.Credit));
        //    t.AddLine(l, new Balance(650M, BalanceType.Debit));
        //    Assert.AreEqual(true, t.IsValid);
        //}

        //[TestMethod]
        //public void TransactionDoubleEntryPolicyEnforcedTest() {
        //    Asset a = DataProvider.Current.CreateAsset();
        //    a.Name = "Cash at Bank";
        //    a.Description = "The cash in the bank.";

        //    Liability l = Ledger.Instance.CreateLiability();
        //    l.Name = "Mortgage";
        //    l.Description = "Mortgage for factory";

        //    Transaction t = TransactionRegister.Instance.CreateTransaction(TransactionalIntegrityPolicy.Enforced);
        //    t.Date = DateTime.Now;
        //    t.Particulars = "Paying off mortgage.";

        //    t.AddLine(a, new Balance(700M, BalanceType.Credit));
        //    t.AddLine(l, new Balance(650M, BalanceType.Debit));
        //    Assert.AreEqual(false, t.IsValid);
        //}

        //[TestMethod]
        //public void TransactionSaveTest() {
        //    using (new TemporaryAccessDatabase()) {
        //        Asset a = DataProvider.Current.CreateAsset();
        //        a.Category = AssetCategory.Current;
        //        a.Description = "The cash in the bank";
        //        a.Name = "Cash at Bank";

        //        Ledger.Instance.Persist(a);

        //        Transaction t = TransactionRegister.Instance.CreateTransaction(TransactionalIntegrityPolicy.NotEnforced);
        //        t.Particulars = "A simple transaction whose lines will cancel each other out.";
        //        t.Lines.Add(a, new Balance(1000M, BalanceType.Credit));
        //        t.Lines.Add(a, new Balance(1000M, BalanceType.Debit));

        //        TransactionRegister.Instance.PersistTransaction(t);

        //        Transaction t2 = TransactionRegister.Instance.FetchTransaction(t.TransactionID);
        //        Assert.AreEqual(1000M, t2.Magnitude);
        //        Assert.AreEqual(2, t2.Lines.Count);
        //        Assert.AreEqual(a.AccountID, t2.Lines[0].Account.AccountID);
        //        Assert.AreEqual(a.AccountID, t2.Lines[1].Account.AccountID);
        //    }
        //}
    }
}
