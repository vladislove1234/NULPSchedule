using System;
using System.Collections.Generic;
using NULPSchedule.Models.Enums;
using NULPSchedule.Models.Mocks;

namespace ShceduleParser.Models.Mocks
{
    public class Request
    {
        public int ID { get; set; }
        public string Institute { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string Semester { get; set; }
        public DateTime LastUpdated { get; set; }
        public string URL => "https://student.lpnu.ua/students_schedule?departmentparent_abbrname_selective=" + Institute + "&studygroup_abbrname_selective=" + Group + "&semestrduration=1";
        public List<Lesson> Lessons { get; set; } = new List<Lesson>();
        public string Description => "Інститут: " + Institute + "  "+ "Група: " + Group; 
    }
}
