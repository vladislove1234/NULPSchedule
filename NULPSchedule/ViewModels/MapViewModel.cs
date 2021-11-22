using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NULPScgedule.Models.Schedule_Managers;
using NULPSchedule.Models.Mocks;
using ShceduleParser.Models.Mocks;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace NULPSchedule.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public MapViewModel()
        {

        }
        public ObservableCollection<Pin> CorpsPins => new ObservableCollection<Pin>()
        {
            new Pin() {Position = new Position(49.8365722302796, 24.034581023503453), Label = "Корпус 20", Address = "Князя Романа"}
        };
    }
}
