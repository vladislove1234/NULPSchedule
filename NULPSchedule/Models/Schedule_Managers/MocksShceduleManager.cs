using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NULPSchedule.Models.Interfaces;
using NULPSchedule.Models.Mocks;
using ShceduleParser.Models.Mocks;

namespace NULPSchedule.Models
{
    public class MocksShceduleManager : IScheduleManager
    {
        public async Task<List<Lesson>> GetLessons(Request request)
        {
            var list = new List<Lesson>()
            {
                new Lesson(){Name = "matem", RepeatableType = Enums.RepeatableType.Full, Number = 1},
                new Lesson(){Name = "фізика", RepeatableType = Enums.RepeatableType.Group1_chys, Number = 2},
                new Lesson(){Name = "фізика", RepeatableType = Enums.RepeatableType.Group2_znam, Number = 2}
            };
            return list;
        }
    }
}
