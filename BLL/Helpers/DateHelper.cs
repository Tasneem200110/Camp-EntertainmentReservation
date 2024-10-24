using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Helpers
{
    public static class DateHelper
    {
        public static List<string> GetUpcoming7DaysLabels()
        {
            var upcomingDaysLabels = new List<string>();
            var currentDate = DateTime.Now;

            for (int i = 0; i < 7; i++)
            {
                var upcomingDate = currentDate.AddDays(i);
                upcomingDaysLabels.Add(upcomingDate.ToString("MMMM dd")); // Format the date as needed
            }

            return upcomingDaysLabels;
        }

        public static DateTime StartOfWeek(DateTime date)
        {
            var dayOfWeek = (int)date.DayOfWeek;
            return date.AddDays(-dayOfWeek).Date; // Set the start of the week to Sunday
        }
    }
}
