using System;
using System.Collections.Generic;
using System.Globalization;
using NULPSchedule.Models.Mocks;

namespace NULPSchedule.Models
{
    public static class DayManager
    {
        public static DateTime Today => DateTime.Today;
        public static bool IsZnam()
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;;
            var _calendar = dfi.Calendar;
            DateTime firstStudyDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            for(int i = 1; i < 5; i++)
            {
                if (WorkingDays.Contains(firstStudyDay.DayOfWeek))
                    break;
                else
                    firstStudyDay.AddDays(1);
            }
            return _calendar.GetWeekOfYear(firstStudyDay, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) % 2
                !=_calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) % 2;
        }
        public static List<DayOfWeek> WorkingDays => new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        public static DateTime GetClosestWorkingDay(DateTime time)
        {
            if (time.DayOfWeek == DayOfWeek.Saturday)
                return time.AddDays(2);
            else return time.AddDays(1);
        }
        public static bool ThisWeekLesson(Lesson lesson)
        {
            return lesson.RepeatableType.ToString().ToLower().Contains("znam") == IsZnam();
        }
        public static bool PairWeekToday()
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo; ;
            var _calendar = dfi.Calendar;
            return _calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) % 2 == 0;
        }
        public static DateTime AddDay(DateTime day)
        {
            if (day.DayOfWeek != DayOfWeek.Friday)
                return day.AddDays(1);
            else return day.AddDays(3);
        }
        public static DateTime MinusDay(DateTime day)
        {
            if (day.DayOfWeek != DayOfWeek.Monday)
                return day.AddDays(-1);
            else return day.AddDays(-3);
        }
        public static int GetDifferenceInDates(DateTime a, DateTime b)
        {
            int res = Convert.ToInt32((a - b).TotalDays); 
            return res;
        }
    }
}
