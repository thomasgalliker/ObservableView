﻿using System;

namespace ObservableView.Grouping
{
    public class MonthGroupAlgorithm : GroupKeyAlgorithm<DateTime?>
    {
        private readonly Func<string> nullString;

        public MonthGroupAlgorithm(Func<string> nullString = null)
        {
            if (nullString == null)
            {
                nullString = () => string.Empty;
            }

            this.nullString = nullString;
        }

        public override string GetGroupKey(DateTime? date)
        {
            if (!date.HasValue)
            {
                return this.nullString();
            }

            var dateTime = date.Value;
            var str = dateTime.ToString("MMMM");
            if (DateTime.Today.Year == dateTime.Year)
            {
                return str;
            }

            return str + " " + dateTime.Year;
        }
    }
}