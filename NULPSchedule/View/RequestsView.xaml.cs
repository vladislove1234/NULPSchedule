using System;
using System.Collections.Generic;
using System.ComponentModel;
using NULPSchedule.Services;
using NULPSchedule.ViewModels;
using ShceduleParser.Models.Mocks;
using Xamarin.Forms;

namespace NULPSchedule.View
{
    public partial class RequestsView : ContentPage
    {
        private RequestsViewModel _viewModel;
        public RequestsView()
        {
            //InitializeComponent();
            _viewModel = new RequestsViewModel();
            ToolbarItems.Add(new ToolbarItem() { Text = "+", Command = _viewModel.AddRequestCommand });
            Content = GenerateView();
            _viewModel.PropertyChanged += RefreshPage;
        }

        private void RefreshPage(object sender, PropertyChangedEventArgs e)
        {
            Content = GenerateView();
        }

        private ScrollView GenerateView()
        {
            var requests = GetRequests();
            if (requests.Count == 0)
                return new ScrollView()
                {
                    Content = new Label()
                    {
                        Text = "Розкладів немає!",
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Center
                    }
                };
            var scrollView = new ScrollView();
            var requestsStackLayout = new StackLayout();
            foreach(var request in requests)
            {
                requestsStackLayout.Children.Add
                    (
                        new SwipeView()
                        {
                            Content =
                                new Frame()
                                {
                                    Content =
                                        new Grid()
                                        {
                                            RowDefinitions = new RowDefinitionCollection()
                                            {
                                                new RowDefinition(){ Height = GridLength.Star},
                                                new RowDefinition(){ Height = new GridLength(0.5,GridUnitType.Star)}
                                            },
                                            Children =
                                            {
                                                new Label(){Text = request.Name}
                                            }
                                        },
                                    GestureRecognizers = {
                                        new TapGestureRecognizer()
                                        {
                                            Command = _viewModel.ChooseRequestCommand,
                                            CommandParameter = request,
                                            NumberOfTapsRequired = 1
                                        }
                                    }
                                },
                            LeftItems =
                                new SwipeItems()
                                {
                                    new SwipeItem()
                                    {
                                        BackgroundColor = Color.Red,
                                        Text = "Видалити",
                                        Command = _viewModel.DeleteRequestCommand,
                                        CommandParameter = request
                                    }
                                },
                            RightItems =
                                new SwipeItems()
                                {
                                    new SwipeItem()
                                    {
                                        BackgroundColor = Color.Yellow,
                                        Text = "Редагувати",
                                        Command = _viewModel.EditRequestCommand,
                                        CommandParameter = request
                                    }
                                }
                        }
                    ) ;
            }
            scrollView.Content = requestsStackLayout;
            return scrollView;
        }
        private List<Request> GetRequests()
        {
            using(var context = new LessonsDBContext())
            {
                return new List<Request>(context.Requests);
            }
        }
    }
}
