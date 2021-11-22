using System;
using Xamarin.Essentials;
namespace NULPSchedule.Models
{
    public static class Settings
    {
        public static bool ShowNotifications
        {
            get
            {
                return Preferences.Get(nameof(ShowNotifications), false);
            }
            set
            {
                Preferences.Set(nameof(ShowNotifications), value);
            }
        }
        public static int ChosenRequest
        {
            get
            {
                return Preferences.Get(nameof(ChosenRequest), 1);
            }
            set
            {
                Preferences.Set(nameof(ChosenRequest), value);
            }
        }
    }
}
