using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NULPSchedule.Models.Mocks;
using ShceduleParser.Models;
using ShceduleParser.Models.Mocks;

namespace NULPSchedule.Models.Interfaces
{
    public interface IScheduleManager
    {
        Task<List<Lesson>> GetLessons(Request request);
    }
}
