using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace BlazorScheduler.Components
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Starts the of week.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        public static DateTime StartOfWeek(this DateTime date)
        {
            var diff = date.DayOfWeek - DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;

            if (diff < 0)
            {
                diff += 7;
            }

            return date.AddDays(-diff).Date;
        }

        /// <summary>
        /// Starts the of month.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Ends the of month.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        public static DateTime EndOfMonth(this DateTime date)
        {
            return date.StartOfMonth().AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Ends the of week.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        public static DateTime EndOfWeek(this DateTime date)
        {
            return date.StartOfWeek().AddDays(6);
        }
    }
}
