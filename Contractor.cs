using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recruitment
{
    public class Contractor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAssigned { get; set; }
        public DateTime StartDate { get; set; }
        public decimal HourlyWage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contractor"/> class
        /// with the given personal and employment details.
        /// </summary>
        /// <param name="firstName">The contractor's first name.</param>
        /// <param name="lastName">The contractor's last name.</param>
        /// <param name="startDate">The date the contractor started work.</param>
        /// <param name="hourlyWage">The contractor's hourly wage.</param>
        public Contractor(string firstName, string lastName, DateTime startDate, decimal hourlyWage)
        {
            FirstName = firstName;
            LastName = lastName;
            StartDate = startDate;
            HourlyWage = hourlyWage;
        }

    }

}
