using System;
using System.Linq;

namespace apptab.Data
{
    public class Date
    {
        public static double GetDifference(DateTime? endDate, DateTime? startDate)
        {
            if (endDate == null || startDate == null)
            {
                return 0;
            }

            double difference = Math.Ceiling(((DateTime)endDate.Value.Date).Subtract((DateTime)startDate.Value.Date).TotalDays);

            double NoweekDay = Enumerable
                .Range(1, int.Parse(difference.ToString()))
                .Select(day => startDate.Value.Date.AddDays(day))
                .Count(day => day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday);

            return NoweekDay;
        }
    }
}
