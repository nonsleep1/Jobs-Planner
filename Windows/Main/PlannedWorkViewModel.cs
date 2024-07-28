using Jobs_Planner.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Jobs_Planner.Windows.Main
{
    public class PlannedWorkViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<PlannedWork> _plannedWorkList;
        private ObservableCollection<Locations> _locationsList;
        private ObservableCollection<Devices> _devicesList;
        private ObservableCollection<Devices> _filteredDevicesList;
        private readonly DatabaseService _databaseService;

        

        private Locations _selectedLocation;

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
                FilterDevices();
            }
        }

        public ObservableCollection<Devices> FilteredDevices_List
        {
            get => _filteredDevicesList;
            set
            {
                _filteredDevicesList = value;
                OnPropertyChanged(nameof(FilteredDevices_List));
            }
        }

        public Locations SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
                MessageBox.Show($"SelectedLocation changed to: {SelectedLocation?.Name}");
                FilterDevices();
            }
        }

        public PlannedWorkViewModel()
        {

            string dbPath = ConfigurationManager.AppSettings["DatabasePath"];
            if( dbPath == null ) { throw new NullReferenceException(nameof(dbPath)); }
            dbPath = Environment.ExpandEnvironmentVariables(dbPath);
            // Load data into the collections
            _databaseService = new DatabaseService(dbPath);

            PlannedWork_List = new ObservableCollection<PlannedWork>();
            Locations_List = new ObservableCollection<Locations>();
            Devices_List = new ObservableCollection<Devices>();
            FilteredDevices_List = new ObservableCollection<Devices>();


            LoadData();

            

            
            
        }


       

        private void LoadData()
        {
           
            try
            {
                using (var connection = _databaseService.GetConnection())
                {
                    var devices = connection.Table<Devices>().Where(p => !p.IsDeleted).ToList();
                    Devices_List = new ObservableCollection<Devices>(devices);

                    var plannedWork = connection.Table<PlannedWork>().Where(p => !p.IsDeleted).ToList();
                    PlannedWork_List = new ObservableCollection<PlannedWork>(plannedWork);

                    var locations = connection.Table<Locations>().Where(p => !p.IsDeleted).ToList();
                    Locations_List = new ObservableCollection<Locations>(locations);

                    PlannedWork.LocationsList = locations;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }




        }

        private void FilterDevices()
        {
            //MessageBox.Show("FilterDevices method called");

            if (FilteredDevices_List == null)
            {
                
                FilteredDevices_List = new ObservableCollection<Devices>();
            }



            if (SelectedLocation != null)
            {
                MessageBox.Show($"Filtering devices for location: {SelectedLocation.Name}");
                var filteredDevices = Devices_List.Where(d => d.LocationId == SelectedLocation.Id).ToList();
                FilteredDevices_List.Clear();
                foreach (var device in filteredDevices)
                {
                    FilteredDevices_List.Add(device);
                }
            }
            else
            {
                FilteredDevices_List.Clear();
            }
        }






        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
           // MessageBox.Show($"PropertyChanged: {propertyName}");

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
