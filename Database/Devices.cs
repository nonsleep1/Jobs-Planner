using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobs_Planner.Database
{
    public class Devices : INotifyPropertyChanged
    {

        private int _locationsId;
        private string _name;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name 
        { 
            get => _name;
            set
            {
                if (value == null)
                { throw new ArgumentNullException(nameof(value)); }
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string? Part { get; set; }
        public string? Manufactor { get; set; }
        public string? Type { get; set; }
        public string? Note { get; set; }
        public int LocationId { get; set; }
        //public int LocationId 
        //{
        //    get => _locationsId;
        //    set
        //    {
        //        if (_locationsId != value)
        //        {
        //            _locationsId = value;
        //            OnPropertyChanged(nameof(LocationId));
        //            Debug.WriteLine($"LocationsId updated to: {_locationsId}");
        //        }
        //    }
        //}
        public bool IsDeleted { get; set; }
        






        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
