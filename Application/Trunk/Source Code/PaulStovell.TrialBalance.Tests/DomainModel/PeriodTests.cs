using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaulStovell.TrialBalance.DomainModel;
using PaulStovell.TrialBalance.Tests.DataLayer.MockObjects;

namespace PaulStovell.TrialBalance.Tests.DomainModel
{
    [TestClass]
    public class PeriodTests
    {
        [TestMethod]
        public void CurrentPeriodStartEndDateMonthlyTest()
        {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook())
            {
                workbook.PeriodLength = PeriodLength.Monthly;
                workbook.PeriodStartDate = DateTime.Now.Date;

                Assert.AreEqual(DateTime.Now.Date, workbook.CurrentPeriod.StartDate);
                Assert.AreEqual(DateTime.Now.Date.AddMonths(1).AddDays(-1), workbook.CurrentPeriod.EndDate);
            }
        }

        [TestMethod]
        public void CurrentPeriodStartEndDateQuarterlyTest()
        {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook())
            {
                workbook.PeriodLength = PeriodLength.Quarterly;
                workbook.PeriodStartDate = DateTime.Now.Date;

                Assert.AreEqual(DateTime.Now.Date, workbook.CurrentPeriod.StartDate);
                Assert.AreEqual(DateTime.Now.Date.AddMonths(3).AddDays(-1), workbook.CurrentPeriod.EndDate);
            }
        }

        [TestMethod]
        public void CurrentPeriodStartEndDateHalfYearlyTest()
        {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook())
            {
                workbook.PeriodLength = PeriodLength.HalfYearly;
                workbook.PeriodStartDate = DateTime.Now.Date;

                Assert.AreEqual(DateTime.Now.Date, workbook.CurrentPeriod.StartDate);
                Assert.AreEqual(DateTime.Now.Date.AddMonths(6).AddDays(-1), workbook.CurrentPeriod.EndDate);
            }
        }

        [TestMethod]
        public void CurrentPeriodStartEndDateYearlyTest()
        {
            using (TemporaryWorkbook workbook = new TemporaryWorkbook())
            {
                workbook.PeriodLength = PeriodLength.Yearly;
                workbook.PeriodStartDate = DateTime.Now.Date;

                Assert.AreEqual(DateTime.Now.Date, workbook.CurrentPeriod.StartDate);
                Assert.AreEqual(DateTime.Now.Date.AddYears(1).AddDays(-1), workbook.CurrentPeriod.EndDate);
            }
        }
    }
}
