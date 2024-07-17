using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.ComponentModel;

namespace Jobs_Planner.Database
{
    public class Workers : INotifyPropertyChanged
    {
        private string _name;
        private string _surname;
        private bool _isdeleted;


        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name 
        {

            get => _name;
            set 
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            } 
        
        }
        public string Surname 
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));

            } 
        }
        public bool IsDeleted 
        {
            get => _isdeleted;
            set
            {
                _isdeleted = value;
                OnPropertyChanged(nameof(IsDeleted));
            } 
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
