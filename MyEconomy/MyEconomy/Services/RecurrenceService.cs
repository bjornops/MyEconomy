using MyEconomy.Models;
using System;
using System.Collections.Generic;

namespace MyEconomy.Services
{
    public static class RecurrenceService
    {
        public enum IntervalLength
        {
            Day,
            Week,
            Month,
            Quartal,
            Year
        }

        public static IntervalLength TranslateAbbreviationToInterval(string abbreviation)
        {
            switch (abbreviation.ToUpper())
            {
                case "D":
                    return IntervalLength.Day;
                case "W":
                    return IntervalLength.Week;
                case "M":
                    return IntervalLength.Month;
                case "Q":
                    return IntervalLength.Quartal;
                case "Y":
                    return IntervalLength.Year;
            }

            throw new ArgumentException(String.Format("Abbreviation {0} is not valid.", abbreviation));
        }

        public static DateTime GetSubsequentDate(DateTime date, IntervalLength intervalLength)
        {
            switch (intervalLength)
            {
                case IntervalLength.Day:
                    return date.AddDays(1);
                case IntervalLength.Week:
                    return date.AddDays(7);
                case IntervalLength.Month:
                    return date.AddMonths(1);
                case IntervalLength.Quartal:
                    return date.AddMonths(3);
                case IntervalLength.Year:
                    return date.AddYears(1);
            }

            throw new ArgumentException("Interval could not be applied to date");
        }
    }
}
