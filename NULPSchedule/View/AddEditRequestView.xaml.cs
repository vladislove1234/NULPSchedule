using System;
using System.Collections.Generic;
using NULPSchedule.ViewModels;
using ShceduleParser.Models.Mocks;
using Xamarin.Forms;

namespace NULPSchedule.View
{
    public partial class AddEditRequestView : ContentPage
    {
        public AddEditRequestView(Request request)
        {
            InitializeComponent();
            BindingContext = new AddEditRequestViewModel(request);
        }
        public AddEditRequestView()
        {
            InitializeComponent();
            BindingContext = new AddEditRequestViewModel();
        }
    }
}
