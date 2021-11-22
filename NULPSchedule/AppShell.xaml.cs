using System;
using System.Collections.Generic;
using NULPSchedule.ViewModels;
using NULPSchedule.View;
using Xamarin.Forms;

namespace NULPSchedule
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ScheduleView), typeof(ScheduleView));
            Routing.RegisterRoute(nameof(MapPageView), typeof(MapPageView));
            Routing.RegisterRoute(nameof(HomeWorkView), typeof(HomeWorkView));
            Routing.RegisterRoute(nameof(SettingsView), typeof(SettingsView));
            Routing.RegisterRoute(nameof(RequestsView), typeof(RequestsView));
        }

    }
}
