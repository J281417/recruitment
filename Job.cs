using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace recruitment
{
    public class Job
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public Contractor AssignedContractor { get; set; }
        public string AssignedContractorName { get; set; }
        public bool IsCompleted { get; set; }

        public Job(string title, string description, decimal cost)
        {
            Title = title;
            Cost = cost;
            IsCompleted = false;
            AssignedContractor = null;
        }
    }
}
