using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyEconomy.Services;

namespace Tests.Services
{
    [TestClass]
    public class RecurrenceServiceTests
    {
        [TestMethod]
        public void GetSubsequentDateSingleMonth()
        {
            DateTime initialDate = new DateTime(2018, 01, 01);
            RecurrenceService.IntervalLength interval = RecurrenceService.IntervalLength.Month;
            DateTime subsequentDate = RecurrenceService.GetSubsequentDate(initialDate, interval);

            DateTime expectedDate = new DateTime(2018, 02, 01);
            Assert.AreEqual(expectedDate, subsequentDate);
        }

        [TestMethod]
        public void GetSubsequentDateTwoSubsequentMonths()
        {
            DateTime initialDate = new DateTime(2018, 01, 31);
            RecurrenceService.IntervalLength interval = RecurrenceService.IntervalLength.Month;
            DateTime firstSubsequentDate = RecurrenceService.GetSubsequentDate(initialDate, interval);
            DateTime secondSubsequentDate = RecurrenceService.GetSubsequentDate(firstSubsequentDate, interval);

            DateTime expectedDate = new DateTime(2018, 03, 31);
            Assert.AreEqual(expectedDate, secondSubsequentDate);
        }
    }
}
