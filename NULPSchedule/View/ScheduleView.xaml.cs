using System;
using System.Collections.Generic;
using System.ComponentModel;
using NULPSchedule.Models;
using NULPSchedule.Models.Mocks;
using NULPSchedule.ViewModels;
using ShceduleParser.Models.Mocks;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.PancakeView;
using CommonServiceLocator;

namespace NULPSchedule.View
{
    public partial class ScheduleView : ContentPage
    {
        private ScheduleViewModel _viewModel = ServiceLocator.Current.GetInstance<ScheduleViewModel>();
        public ScheduleView()
        {
            InitializeComponent();
            _viewModel.PropertyChanged += RefreshList;
            //Content = GeneratePage();
        }

        private void RefreshList(object sender, PropertyChangedEventArgs e)
        {
            Content = GeneratePage();
            
        }
        private StackLayout GeneratePage()
        {
            if(_viewModel.CurrentRequest == null)
            {
                return new StackLayout()
                {
                    Children =
                    {
                        new Label()
                        {
                            Text = "Вибреіть розклад",
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
                };
            }
            var lessons = _viewModel.GetCurrentDayLessons();
            if (lessons.Count == 0)
                return new StackLayout()
                {
                    Children = {
                        new Label()
                        {
                            Text = "Відсутні дані",
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
                };

            var mainStack = new StackLayout();
            var scrollLessonsView = new ScrollView();
            var LessonsStack = new StackLayout();
            foreach (var children in BlocksGenerator.GetFrames(lessons))
            {
                LessonsStack.Children.Add(children);
            }
            var daysGrid = new Grid()
            {
                ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    new ColumnDefinition() { Width = GridLength.Star },
                    new ColumnDefinition() { Width = GridLength.Star },
                    new ColumnDefinition() { Width = GridLength.Star }
                },
                MinimumHeightRequest = 50,
                Margin = 8
            };
            daysGrid.Children.Add(new Button() { Text = "<", Command = _viewModel.MinusDayComand, TextColor = Color.Black, BackgroundColor = Color.White }, 0, 0);
            daysGrid.Children.Add(new Frame() { Padding = 0,Content = new Label() { Text = _viewModel.CurrentDay.DayOfWeek.ToString(), HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center }, CornerRadius = 24 }, 1, 0);
            daysGrid.Children.Add(new Button() { Text = ">", Command = _viewModel.AddDayComand, TextColor = Color.Black, BackgroundColor = Color.White}, 2, 0);
            scrollLessonsView.Content = LessonsStack;
            if (DayManager.GetDifferenceInDates(DateTime.Now, _viewModel.CurrentRequest.LastUpdated) > 1)
                mainStack.Children.Add(NotUpdatedLabel);
            mainStack.Children.Add(daysGrid);
            mainStack.Children.Add(scrollLessonsView);
            return mainStack;
           
        }
        private Label NotUpdatedLabel => new Label()
            {
                Text = "Дані не оновлені, можливі зміни в розкладі!",
                BackgroundColor = Color.Yellow,
                HorizontalOptions = LayoutOptions.Fill
            };
        protected override void OnAppearing()
        {
            _viewModel.RefreshSchedule();
            ToolbarItems.Clear();
            ToolbarItems.Add(new ToolbarItem() { Text = _viewModel.CurrentRequest == null ? "Вибрати розклад" : _viewModel.CurrentRequest.Name, Command = _viewModel.ChooseRequestCommand });
            Content = GeneratePage();
        }
    }
}
