using System;
namespace NULPSchedule.Models
{
    public static class LessonsTimeManager
    {
        public static string GetLessonTime(int number)
        {
            switch (number)
            {
                case 0:
                    return "7:00 - 8:30";
                case 1:
                    return "8:30 − 10:05";
                case 2:
                    return "10:20 − 11:55";
                case 3:
                    return "12:10 − 13:45";
                case 4:
                    return "14:15 − 15:50";
                case 5:
                    return "16:00 − 17:35";
                case 6:
                    return "17:40 − 19:15";
                case 7:
                    return "19:20 - 20:55";
                default:
                    return "";
            }
        }
    }
}
