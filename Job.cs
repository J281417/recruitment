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
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public Contractor AssignedContractor { get; set; }
        public string AssignedContractorName { get; set; }
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Creates a new job with a title, description, date, and cost.
        /// The job starts as unassigned and not completed.
        /// </summary>
        /// <param name="title">The name or title of the job.</param>
        /// <param name="description">A short explanation of what the job involves.</param>
        /// <param name="date">The scheduled date of the job.</param>
        /// <param name="cost">The cost or price associated with the job.</param>
        public Job(string title, string description, DateTime date, decimal cost)
        {
            Title = title;
            Description = description;
            Date = date;
            Cost = cost;
            IsCompleted = false;
            AssignedContractor = null;
        }
    }
}
