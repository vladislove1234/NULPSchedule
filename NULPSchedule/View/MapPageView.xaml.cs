using System;
using System.Collections.Generic;
using NULPSchedule.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace NULPSchedule.View
{
    public partial class MapPageView : ContentPage
    {
        private MapViewModel _viewModel;
        private MapSpan _defaultMapSpan => new MapSpan(new Position(49.83517688105596, 24.014451053700174),0.01,0.01);
        public MapPageView(MapSpan mapSpan)
        {
            //InitializeComponent();
            _viewModel = new MapViewModel();
            var map = new Map(mapSpan)
            {
                IsShowingUser = true
            };
            foreach(var pin in _viewModel.CorpsPins)
                map.Pins.Add(pin);
            Content = map;
        }
        public MapPageView()
        {
            //InitializeComponent();
            _viewModel = new MapViewModel();
            var map = new Map(_defaultMapSpan)
            {
                IsShowingUser = true
            };
            foreach (var pin in _viewModel.CorpsPins)
                map.Pins.Add(pin);
            Content = map;
        }
    }
}
