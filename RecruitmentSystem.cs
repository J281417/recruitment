using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Reflection.Metadata.BlobBuilder;

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
        public void RemoveContractor(Contractor contractor)
        {
            Contractors.Remove(contractor);
        }

        public List<Contractor> GetContractors()
        {
            return Contractors;
        }



        public List<Contractor> GetAvailableContractors()
        {
            return Contractors
                .Where(c => !Jobs.Any(j => j.AssignedContractor == c && !j.IsCompleted))
                .ToList();
        }

        public void AddJob(Job job)
        {
            Jobs.Add(job);
        }

        public List<Job> GetJobs()
        {
            return Jobs;
        }

        public List<Job> GetUnassignedJobs()
        {
            return Jobs.Where(j => j.AssignedContractor == null && !j.IsCompleted).ToList();
        }

        public void AssignJob(Job job, Contractor contractor)
        {
            if (job != null && contractor != null && !Jobs.Contains(job))
                throw new Exception("Job does not exist in the system.");

            if (!GetAvailableContractors().Contains(contractor))
                throw new Exception("Contractor is not available.");

            job.AssignedContractor = contractor;
            job.AssignedContractorName = $"{contractor.FirstName} {contractor.LastName}"; ;
            contractor.IsAssigned = true;
        }
        public void CompleteJob(Job job, Contractor contractor)
        {
            if (job.AssignedContractor == null)
                throw new Exception("Cannot complete an unassigned job.");

            job.IsCompleted = true;
            contractor.IsAssigned = false;
        }

        public List<Job> GetJobByCost(decimal minCost, decimal maxCost)
        {
            return Jobs.Where(j => j.Cost >= minCost && j.Cost <= maxCost).ToList();

        }




    }

}
