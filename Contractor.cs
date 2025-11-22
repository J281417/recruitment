using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recruitment
{
    class Contractor
    {
        
        private Guid _id;
        private string _firstName;
        private string _lastName;
        private bool _isAssigned;
        private DateTime _startDate;
        private decimal _hourlyWage;

        /// Initializes a new instance of Contractor.
        public Contractor()
        {
            _id = Guid.NewGuid();
            _startDate = DateTime.Now; // Default to today
            _hourlyWage = 0.0m;        // Default wage
        }

        /// Unique identifier for the contractor.
        public Guid Id => _id;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(nameof(FirstName)); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(nameof(LastName)); }
        }

        /// True if contractor is currently assigned to a job.
        public bool IsAssigned
        {
            get => _isAssigned;
            set { _isAssigned = value; OnPropertyChanged(nameof(IsAssigned)); }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(nameof(StartDate)); }
        }

        public decimal HourlyWage
        {
            get => _hourlyWage;
            set { _hourlyWage = value; OnPropertyChanged(nameof(HourlyWage)); }
        }

        /// Display full name
        public string DisplayName => $"{FirstName} {LastName}";

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion
    }

}
