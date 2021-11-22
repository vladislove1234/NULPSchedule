using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NULPSchedule.Models;
using NULPSchedule.Services;
using ShceduleParser.Models.Mocks;
using Xamarin.Forms;

namespace NULPSchedule.ViewModels
{
    public class AddEditRequestViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Institute { get; set; }
        public string Group { get; set; }
        public string Title { get; set; }
        public ICommand SaveRequestCommand { get; }
        private Request _editedRequest;
        public AddEditRequestViewModel(Request request)
        {
            SaveRequestCommand = new Command(() => UpdateRequest(_editedRequest));
            _editedRequest = request;
            Name = request.Name;
            Institute = request.Institute;
            Group = request.Group;
            Title = "Редагування розкладу";
        }
        public AddEditRequestViewModel()
        {
            SaveRequestCommand = new Command(SaveRequest);
            Title = "Додавання розкладу";
        }

        private void SaveRequest()
        {
            using(var dbCotext = new LessonsDBContext())
            {
                var request = new Request() { Name = this.Name, Institute = this.Institute, Group = this.Group};
                dbCotext.Add(request);
                dbCotext.SaveChanges();
                Settings.ChosenRequest = request.ID;
            }
            Shell.Current.Navigation.PopModalAsync();
        }
        private void UpdateRequest(Request newRequest)
        {
            using (var dbCotext = new LessonsDBContext())
            {
                var request = dbCotext.Requests.Where(x => x.ID == newRequest.ID).FirstOrDefault();
                if (request == null)
                    throw new Exception("Failed to update request");
                request.Name = Name; 
                request.Institute = Institute;
                request.Group = Group;
                dbCotext.SaveChanges();
            }
            Shell.Current.Navigation.PopAsync();
        }
    }
}
