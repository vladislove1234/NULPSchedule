using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using NULPScgedule.Models.Schedule_Managers;
using NULPSchedule.Models;
using NULPSchedule.Models.Interfaces;
using NULPSchedule.Models.Mocks;
using NULPSchedule.Services;
using NULPSchedule.View;
using ShceduleParser.Models;
using ShceduleParser.Models.Mocks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NULPSchedule.ViewModels
{
    public class ScheduleViewModel : BaseViewModel
    {
        public ICommand ChooseRequestCommand { get; private set; }
        public ICommand AddDayComand { get; private set; }
        public ICommand MinusDayComand { get; private set; }
        public Request CurrentRequest { get; private set; }

        public DateTime _currentDay { get; private set; }
        public  DateTime CurrentDay { get
            {
                return _currentDay;
            } private set
            {
                _currentDay = value;
                OnPropertyChanged();
            } }

        private List<Lesson> _lessons;
        private IScheduleManager _scheduleManager;

        public ScheduleViewModel()
        {
            _scheduleManager = new ParserShceduleManager();
            AddDayComand = new Command(() => CurrentDay = DayManager.AddDay(CurrentDay));
            MinusDayComand = new Command(() => CurrentDay = DayManager.MinusDay(CurrentDay));
            ChooseRequestCommand = new Command(async() => await Shell.Current.Navigation.PushAsync(new RequestsView()));
            CurrentDay = DayManager.WorkingDays.Contains(DateTime.Today.DayOfWeek) ? DateTime.Now : DayManager.GetClosestWorkingDay(DateTime.Now);
            CurrentRequest = GetRequest();
        }

        public void RefreshSchedule()
        {
            var request = GetRequest();
            if(request != CurrentRequest)
            {
                CurrentRequest = request;
                SetLessons();
            }
        }

        private Request GetRequest()
        {
            using(var dbContext = new LessonsDBContext())
            {
               var request = dbContext.Requests.Where(x => x.ID == Settings.ChosenRequest).FirstOrDefault();
               return request;
            }
        }

        public List<Lesson> GetCurrentDayLessons()
        {
            return GetLessonsForDay(CurrentDay);
        }

        public List<Lesson> AllLessons
        {
            get
            {
                if (_lessons == null)
                SetLessons();
                return _lessons;
            }
        }
        public void SetLessons()
        {
            if (CurrentRequest == null)
            {
                _lessons = new List<Lesson>();
                
                return;
            }
            using (var dbContext = new LessonsDBContext())
            {
                _lessons = dbContext.Lessons.Where(x => x.RequestId == CurrentRequest.ID).ToList();
            }
            if (Connectivity.NetworkAccess == NetworkAccess.Internet && DayManager.GetDifferenceInDates(DateTime.Now, CurrentRequest.LastUpdated) > 1)
            {
                SetLessonsFromSite();
            }
        }
        void SetLessonsFromSite()
        {
            Task.Run(() =>
            {
                var newLessons = _scheduleManager.GetLessons(CurrentRequest).Result;
                _lessons = newLessons;
                UpdateLessons(CurrentRequest, newLessons);
            });
            Task.WaitAll();
            OnPropertyChanged();
        }
        private void UpdateLessons(Request request, List<Lesson> newlessons)
        {
            using (var dbContext = new LessonsDBContext())
            {
                var requestLessons = dbContext.Lessons.Where(x => x.RequestId == request.ID).ToList();
                dbContext.Lessons.RemoveRange(requestLessons);
                dbContext.Lessons.AddRange(newlessons);
                var requestToUpdate = dbContext.Requests.Where(x => x.ID == request.ID).FirstOrDefault();
                requestToUpdate.LastUpdated = DateTime.Now;
                request.LastUpdated = DateTime.Now;
                dbContext.SaveChanges();
            }
        }

        public List<Lesson> GetLessonsForDay(DateTime date)
        {
            using (var dbContext = new LessonsDBContext())
            {
                var lessons = AllLessons.Where(x => x.Day == date.DayOfWeek);
                return lessons.ToList();

            }
        }
    }
}
