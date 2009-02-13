using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaulStovell.TrialBalance.DomainModel;

namespace PaulStovell.TrialBalance.Tests.DomainModel {
    [TestClass]
    public class BalanceTests {

        [TestMethod]
        public void AddTwoDebitBalancesTest() {
            Balance b1 = new Balance(100M, BalanceType.Debit);
            Balance b2 = new Balance(50M, BalanceType.Debit);

            Balance b3 = b1 + b2;
            Assert.AreEqual(b3.Magnitude, 150M);
            Assert.AreEqual(b3.BalanceType, BalanceType.Debit);
        }

        [TestMethod]
        public void AddTwoCreditBalancesTest() {
            Balance b1 = new Balance(100M, BalanceType.Credit);
            Balance b2 = new Balance(50M, BalanceType.Credit);

            Balance b3 = b1 + b2;
            Assert.AreEqual(b3.Magnitude, 150M);
            Assert.AreEqual(b3.BalanceType, BalanceType.Credit);
        }

        [TestMethod]
        public void AddDebitCreditBalancesTest() {
            Balance b1 = new Balance(100M, BalanceType.Debit);
            Balance b2 = new Balance(50M, BalanceType.Credit);

            Balance b3 = b1 + b2;
            Assert.AreEqual(b3.Magnitude, 50M);
            Assert.AreEqual(b3.BalanceType, BalanceType.Debit);
        }

        [TestMethod]
        public void AddCreditDebitBalancesTest() {
            Balance b1 = new Balance(100M, BalanceType.Credit);
            Balance b2 = new Balance(50M, BalanceType.Debit);

            Balance b3 = b1 + b2;
            Assert.AreEqual(b3.Magnitude, 50M);
            Assert.AreEqual(b3.BalanceType, BalanceType.Credit);
        }

        [TestMethod]
        public void AddDebitCreditBalancesSecondLargerTest() {
            Balance b1 = new Balance(50M, BalanceType.Debit);
            Balance b2 = new Balance(100M, BalanceType.Credit);

            Balance b3 = b1 + b2;
            Assert.AreEqual(b3.Magnitude, 50M);
            Assert.AreEqual(b3.BalanceType, BalanceType.Credit);
        }

        [TestMethod]
        public void AddCreditDebitBalancesSecondLargerTest() {
            Balance b1 = new Balance(50M, BalanceType.Credit);
            Balance b2 = new Balance(100M, BalanceType.Debit);

            Balance b3 = b1 + b2;
            Assert.AreEqual(b3.Magnitude, 50M);
            Assert.AreEqual(b3.BalanceType, BalanceType.Debit);
        }

        [TestMethod]
        public void SubtractTwoDebitBalancesTest() {
            Balance b1 = new Balance(100M, BalanceType.Debit);
            Balance b2 = new Balance(50M, BalanceType.Debit);

            Balance b3 = b1 - b2;
            Assert.AreEqual(b3.Magnitude, 50M);
            Assert.AreEqual(b3.BalanceType, BalanceType.Debit);
        }

        [TestMethod]
        public void SubtractTwoCreditBalancesTest() {
            Balance b1 = new Balance(100M, BalanceType.Credit);
            Balance b2 = new Balance(50M, BalanceType.Credit);

            Balance b3 = b1 - b2;
            Assert.AreEqual(b3.Magnitude, 50M);
            Assert.AreEqual(b3.BalanceType, BalanceType.Credit);
        }

        [TestMethod]
        public void SubtractDebitCreditBalancesTest() {
            Balance b1 = new Balance(100M, BalanceType.Debit);
            Balance b2 = new Balance(50M, BalanceType.Credit);

            Balance b3 = b1 - b2;
            Assert.AreEqual(b3.Magnitude, 150M);
            Assert.AreEqual(b3.BalanceType, BalanceType.Debit);
        }

        [TestMethod]
        public void SubtractCreditDebitBalancesTest() {
            Balance b1 = new Balance(100M, BalanceType.Credit);
            Balance b2 = new Balance(50M, BalanceType.Debit);

            Balance b3 = b1 - b2;
            Assert.AreEqual(b3.Magnitude, 150M);
            Assert.AreEqual(b3.BalanceType, BalanceType.Credit);
        }

        [TestMethod]
        public void SubtractDebitCreditBalancesSecondLargerTest() {
            Balance b1 = new Balance(50M, BalanceType.Debit);
            Balance b2 = new Balance(100M, BalanceType.Credit);

            Balance b3 = b1 - b2;
            Assert.AreEqual(b3.Magnitude, 50M);
            Assert.AreEqual(b3.BalanceType, BalanceType.Credit);
        }

        [TestMethod]
        public void SubtractCreditDebitBalancesSecondLargerTest() {
            Balance b1 = new Balance(50M, BalanceType.Credit);
            Balance b2 = new Balance(100M, BalanceType.Debit);

            Balance b3 = b1 - b2;
            Assert.AreEqual(b3.Magnitude, 50M);
            Assert.AreEqual(b3.BalanceType, BalanceType.Debit);
        }

        [TestMethod]
        public void EqualsTrueTest() {
            Balance b1 = new Balance(100M, BalanceType.Debit);
            Balance b2 = new Balance(100M, BalanceType.Debit);

            bool result = b1 == b2;
            Assert.IsTrue(result);

            Balance b3 = new Balance(100M, BalanceType.Credit);
            Balance b4 = new Balance(100M, BalanceType.Credit);

            result = b3 == b4;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EqualsFalseTest() {
            Balance b1 = new Balance(100M, BalanceType.Debit);
            Balance b2 = new Balance(100M, BalanceType.Credit);

            bool result = b1 == b2;
            Assert.IsFalse(result);

            Balance b3 = new Balance(100M, BalanceType.Credit);
            Balance b4 = new Balance(50M, BalanceType.Credit);

            result = b3 == b4;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NotEqualsFalseTest() {
            Balance b1 = new Balance(100M, BalanceType.Debit);
            Balance b2 = new Balance(100M, BalanceType.Debit);

            bool result = b1 != b2;
            Assert.IsFalse(result);

            Balance b3 = new Balance(100M, BalanceType.Credit);
            Balance b4 = new Balance(100M, BalanceType.Credit);

            result = b3 != b4;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NotEqualsTrueTest() {
            Balance b1 = new Balance(100M, BalanceType.Debit);
            Balance b2 = new Balance(100M, BalanceType.Credit);

            bool result = b1 != b2;
            Assert.IsTrue(result);

            Balance b3 = new Balance(100M, BalanceType.Credit);
            Balance b4 = new Balance(50M, BalanceType.Credit);

            result = b3 != b4;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EqualsMethodTest() {
            Balance b1 = new Balance(100M, BalanceType.Debit);
            Balance b2 = new Balance(100M, BalanceType.Debit);

            bool result = b1.Equals(b2);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ParseValidTest() {
            Assert.AreEqual(new Balance(1000, BalanceType.Debit), Balance.Parse("$1000 DR"));
            Assert.AreEqual(new Balance(123, BalanceType.Debit), Balance.Parse("$123DR"));
            Assert.AreEqual(new Balance(1000, BalanceType.Debit), Balance.Parse("1000dr"));
            Assert.AreEqual(new Balance(9999999999999999, BalanceType.Credit), Balance.Parse("9999999999999999cr"));
            Assert.AreEqual(new Balance(999999999999, BalanceType.Credit), Balance.Parse("    999,999,999,999   CR   "));
        }

        [TestMethod]
        public void ParseInvalidThrowsFormatExceptionTest() {
            try { Balance.Parse("$1000"); Assert.Fail(); } catch (FormatException) { } catch { Assert.Fail(); }
            try { Balance.Parse("DR"); Assert.Fail(); } catch (FormatException) { } catch { Assert.Fail(); }
            try { Balance.Parse("abcdr"); Assert.Fail(); } catch (FormatException) { } catch { Assert.Fail(); }
            try { Balance.Parse("1,000"); Assert.Fail(); } catch (FormatException) { } catch { Assert.Fail(); }
            try { Balance.Parse("cr 1000"); Assert.Fail(); } catch (FormatException) { } catch { Assert.Fail(); }
        }

        [TestMethod]
        public void ParseIgnoreNegativeTest() {
            Assert.AreEqual(new Balance(1000, BalanceType.Debit), Balance.Parse("-$1000 DR"));
            Assert.AreEqual(new Balance(123000, BalanceType.Debit), Balance.Parse("-123,000DR"));
            Assert.AreEqual(new Balance(1000, BalanceType.Credit), Balance.Parse("(1000)", BalanceType.Credit));
        }
    }
}
