using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobs_Planner.Database
{
    public class PlannedWork : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private int _locationId;
        [Indexed]
        public int LocationId
        {
            get => _locationId;
            set
            {
                if (_locationId != value)
                {
                    _locationId = value;
                    OnPropertyChanged(nameof(LocationId));
                    // Update SelectedLocation whenever LocationId is changed
                    SelectedLocation = LocationsList?.FirstOrDefault(loc => loc.Id == value);
                }
            }
        }

        private Locations _selectedLocation;
        [Ignore]
        public Locations SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                if (_selectedLocation != value)
                {
                    _selectedLocation = value;
                    OnPropertyChanged(nameof(SelectedLocation));
                    // Update LocationId whenever SelectedLocation is changed
                    if (value != null && _locationId != value.Id)
                    {
                        LocationId = value.Id;
                    }
                }
            }
        }

        public int DeviceId { get; set; }
        public bool IsDeleted { get; set; }

        public static List<Locations> LocationsList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
