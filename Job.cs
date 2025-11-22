using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recruitment
{
    class Job
    {
        private Guid _id;
        private string _title;
        private string _description;
        private decimal _cost;
        private Contractor _assignedContractor;
        private DateTime? _completedAt;
        private DateTime _jobDate;

        /// Initializes a new job instance.
        public Job()
        {
            _id = Guid.NewGuid();
            _jobDate = DateTime.Now;
            _completedAt = null;
        }

        /// Unique id for the job.
        public Guid Id => _id;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        public decimal Cost
        {
            get => _cost;
            set { _cost = value; OnPropertyChanged(nameof(Cost)); }
        }

        /// The contractor assigned to this job. Null if unassigned.
        public Contractor AssignedContractor
        {
            get => _assignedContractor;
            set
            {
                _assignedContractor = value;
                OnPropertyChanged(nameof(AssignedContractor));
                OnPropertyChanged(nameof(IsAssigned)); 
                OnPropertyChanged(nameof(AssignedContractorName));
            }
        }

        /// Date/time job was completed. Null if not completed yet.
        public DateTime? CompletedAt
        {
            get => _completedAt;
            set { _completedAt = value; OnPropertyChanged(nameof(CompletedAt)); OnPropertyChanged(nameof(IsCompleted)); }
        }

        /// The date this job was created or started.
        public DateTime JobDate
        {
            get => _jobDate;
            set => _jobDate = value;
        }

        /// True if a contractor is assigned.
        public bool IsAssigned => AssignedContractor != null;

        /// True if job has been completed.
        public bool IsCompleted => CompletedAt.HasValue;

        /// Friendly name for assigned contractor.
        public string AssignedContractorName => AssignedContractor?.DisplayName ?? "(none)";

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion
    }
}
