using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using NULPSchedule.Models;
using NULPSchedule.Services;
using NULPSchedule.View;
using ShceduleParser.Models.Mocks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NULPSchedule.ViewModels
{
    public class RequestsViewModel : BaseViewModel
    {
        public ICommand AddRequestCommand { get; }
        public Command<Request> ChooseRequestCommand { get; }
        public Command<Request> EditRequestCommand { get; }
        public Command<Request> DeleteRequestCommand { get; }
        public ObservableCollection<Request> Requests { get
            {
                return GetRequests();
            } }
        public RequestsViewModel()
        {
            AddRequestCommand = new Command(() => Shell.Current.Navigation.PushModalAsync(new AddEditRequestView()));
            ChooseRequestCommand = new Command<Request>(ChooseRequest);
            EditRequestCommand = new Command<Request>(EditRequest);
            DeleteRequestCommand = new Command<Request>(DeleteRequest);
        }

        private void DeleteRequest(Request request)
        {
            using (var dbContext = new LessonsDBContext())
            {
                if (request != null)
                {
                    dbContext.Requests.Remove(request);
                    dbContext.SaveChanges();
                }
            }
            OnPropertyChanged(nameof(Requests));
        }

        private void EditRequest(Request request)
        {
            using (var dbContext = new LessonsDBContext())
            {
                if (request != null)
                    Shell.Current.Navigation.PushAsync(new AddEditRequestView(request));
            }
            OnPropertyChanged(nameof(Requests));
        }

        private void ChooseRequest(Request request)
        {
            Settings.ChosenRequest = request.ID;
            Shell.Current.Navigation.PopAsync();
        }
        private ObservableCollection<Request> GetRequests()
        {
            using(var dbContext = new LessonsDBContext())
            {
                return new ObservableCollection<Request>(dbContext.Requests);
            }
        }
    }
}
