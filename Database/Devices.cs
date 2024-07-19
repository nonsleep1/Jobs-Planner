using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobs_Planner.Database
{
    public class Devices : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Part { get; set; }
        public string? Manufactor { get; set; }
        public string? Type { get; set; }
        public string? Note { get; set; }
        public int LocationId { get; set; }
        public bool IsDeleted { get; set; }







        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
