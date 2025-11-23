using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace recruitment
{
    public class RecruitmentSystem
    {
        public List<Contractor> Contractors;
        public List<Job> Jobs;

        public RecruitmentSystem()
        {
            Contractors = new List<Contractor>();
            Jobs = new List<Job>();
        }

        public void AddContractor(Contractor person) {

            if (string.IsNullOrWhiteSpace(person.FirstName))
                throw new ArgumentException("First name required.");

            if (string.IsNullOrWhiteSpace(person.LastName))
                throw new ArgumentException("Last name required.");

            Contractors.Add(person);

        }

        public List<Contractor> GetContractors()
        {
            return Contractors;
        }
        public void AddJob(Job job)
        {
            Jobs.Add(job);
        }

        public List<Contractor> GetAvailableContractors()
        {
            if (Contractors == null)
                Contractors = new List<Contractor>();

            if (Jobs == null)
                Jobs = new List<Job>();



            return Contractors
                .Where(c => !Jobs.Any(j => j.AssignedContractor == c && !j.IsCompleted))
                .ToList();
        }

    }

}
