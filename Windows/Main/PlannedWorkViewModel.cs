using Jobs_Planner.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobs_Planner.Windows.Main
{
    public class PlannedWorkViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<PlannedWork> _plannedWorkList;
        private ObservableCollection<Locations> _locationsList;
        private ObservableCollection<Devices> _devicesList;


        public ObservableCollection<PlannedWork> PlannedWork_List
        {
            get => _plannedWorkList;
            set
            {
                _plannedWorkList = value;
                OnPropertyChanged(nameof(PlannedWork_List));
            }
        }

        public ObservableCollection<Locations> Locations_List
        {
            get => _locationsList;
            set
            {
                _locationsList = value;
                OnPropertyChanged(nameof(Locations_List));
            }
        }

        public ObservableCollection<Devices> Devices_List
        {
            get => _devicesList;
            set
            {
                _devicesList = value;
                OnPropertyChanged(nameof(Devices_List));
            }
        }








        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
