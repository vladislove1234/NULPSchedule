using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using NULPSchedule.Models.Mocks;
using ShceduleParser.Models;
using NULPSchedule.Models.Enums;
using NULPSchedule.Models.Interfaces;
using ShceduleParser.Models.Mocks;
using System.Globalization;

namespace NULPScgedule.Models.Schedule_Managers
{
    public class ParserShceduleManager : IScheduleManager
    {
        public async Task<List<Lesson>> GetLessons(Request request)
        {
            List<Lesson> lessons = new List<Lesson>();
            var web = new HtmlWeb();
            int currentNumber = 1;
            DayOfWeek currentDay = DayOfWeek.Monday;
            try
            {
                var document = web.Load(request.URL);
                foreach (var day in document.DocumentNode.QuerySelectorAll(".view-grouping"))
                {
                    currentDay = StringToDay(day.QuerySelector(".view-grouping-header").InnerText.Trim());
                    foreach (var element in day.QuerySelectorAll(".view-grouping-content"))
                    {
                        foreach (var child in element.Children())
                        {
                            if (child.HasClass("stud_schedule"))
                            {
                                
                                foreach (var item in child.Children())
                                {
                                    var id = item.GetAttributeValue("id", string.Empty);
                                    var _class = item.GetAttributeValue("class", string.Empty);
                                    if (id != string.Empty)
                                    {
                                        
                                        Lesson currentLesson = new Lesson();
                                        currentLesson.Number = currentNumber;
                                        currentLesson.Day = currentDay;
                                        currentLesson.RepeatableType = StringToType(id);
                                        currentLesson.PairWeek = SetWeek(currentLesson, _class.Contains("week"));
                                        //currentLesson.Request = request;
                                        currentLesson.RequestId = request.ID;
                                        var namediv = item.Children().Where(x => x.HasClass("group_content")).FirstOrDefault();
                                        if (namediv != null)
                                        {
                                            var array = namediv.InnerHtml.Split(new string[] { "<br>" }, StringSplitOptions.None);
                                            currentLesson.Name = array[0];
                                            currentLesson.Description = array[1];
                                            currentLesson.Type = array[2];
                                            lessons.Add(currentLesson);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var number = Regex.Replace(child.InnerText.Trim(), @"[\p{L}]", "");
                                if (number != string.Empty)
                                    currentNumber = Convert.ToInt32(number);
                            }
                        }
                    }
                }
            }
            catch(HtmlWebException e)
            {
                return null;
            }
            return lessons;
        }

        private bool SetWeek(Lesson currentLesson, bool isToday)
        {
            string type = currentLesson.TextRepeatableType;
            if (type.Contains("znam") || type.Contains("chys"))
            {
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo; 
                var _calendar = dfi.Calendar;

                return isToday ?
                    _calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) % 2 == 0
                    : _calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) % 2 != 0;
            }
            return true;
        }

        private RepeatableType StringToType(string type)
        {
            switch (type)
            {
                case "group_full":
                    return RepeatableType.Full;
                case "group_chys":
                    return RepeatableType.Full_chys;
                case "group_znam":
                    return RepeatableType.Full_znam;
                case "sub_1_znam":
                    return RepeatableType.Group1_znam;
                case "sub_1_chys":
                    return RepeatableType.Group1_chys;
                case "sub_2_znam":
                    return RepeatableType.Group2_znam;
                case "sub_2_chys":
                    return RepeatableType.Group2_chys;
                case "sub_1_full":
                    return RepeatableType.Group1_full;
                case "sub_2_full":
                    return RepeatableType.Group2_full;
                default:
                    throw new Exception("Failed to parse RepeatableType");
            }
        }
        private DayOfWeek StringToDay(string day)
        {
            switch (day)
            {
                case "Пн":
                    return DayOfWeek.Monday;
                case "Вт":
                    return DayOfWeek.Tuesday;
                case "Ср":
                    return DayOfWeek.Wednesday;
                case "Чт":
                    return DayOfWeek.Thursday;
                case "Пт":
                    return DayOfWeek.Friday;
                default:
                    throw new Exception("Failed to parse Day");
            }
        }
    }
}
