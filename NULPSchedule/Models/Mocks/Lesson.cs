using System;
using NULPSchedule.Models.Enums;
using ShceduleParser.Models.Mocks;

namespace NULPSchedule.Models.Mocks
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DayOfWeek Day {get;set;}
        public RepeatableType RepeatableType { get; set; }
        public string Type { get; set; }
        public int Number { get; set; }
        public bool PairWeek { get; set; }
        public int RequestId { get; set; }
        public string FullDescription => ToString();

        //public Request Request { get; set; }

        public override string ToString()
        {
            return $"{Day} | {Number} | {RepeatableType} |{Name} | {Description} | {Type} \n";
        }
        public string TextRepeatableType => RepeatableType.ToString().ToLower();
    }
}
