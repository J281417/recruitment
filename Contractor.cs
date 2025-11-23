using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recruitment
{

    /// <summary>
    /// A Contractor
    /// </summary>
    public class Contractor
    {

        /// <summary>
        /// Set 
        /// </summary>
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAssigned { get; set; }
        public DateTime StartDate { get; set; }
        public decimal HourlyWage { get; set; }

        public Contractor(string firstName, string lastName, DateTime startDate, decimal hourlyWage)
        {
            FirstName = firstName;
            LastName = lastName;
            StartDate = startDate;
            HourlyWage = hourlyWage;
        }

    }

}
