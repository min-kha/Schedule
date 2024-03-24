using ScheduleService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleService.Utils
{
    public class Util
    {
        private Util()
        {
        }
        /// <summary>
        /// Tìm ngày gần nhất lớn hơn fromDate và có cùng thứ với dayOfWeek
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static DateTime FindNextDayOfWeek(DateTime fromDate, DayOfWeek dayOfWeek)
        {
            int daysToAdd = ((int)dayOfWeek - (int)fromDate.DayOfWeek + 7) % 7;
            return fromDate.AddDays(daysToAdd);
        }
        public static bool IsTimeSlotDouble(string input)
        {
            return Enum.TryParse(input, out TimeSlotDouble timeSlotDouble);
        }
    }
}
