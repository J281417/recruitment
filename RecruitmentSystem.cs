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

        /// <summary>
        /// Initializes a new instance of the <see cref="RecruitmentSystem"/> class,
        /// creating empty lists for contractors and jobs.
        /// </summary>
        public RecruitmentSystem()
        {
            Contractors = new List<Contractor>();
            Jobs = new List<Job>();
        }

        /// <summary>
        /// Adds a contractor to the system after validating that required fields are provided.
        /// Throws an exception if mandatory information is missing.
        /// </summary>
        /// <param name="person">The contractor object to be added.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the contractor's first name or last name is missing or contains only whitespace.
        /// </exception>
        public void AddContractor(Contractor person) {

            if (string.IsNullOrWhiteSpace(person.FirstName))
                throw new ArgumentException("First name required.");

            if (string.IsNullOrWhiteSpace(person.LastName))
                throw new ArgumentException("Last name required.");

            Contractors.Add(person);

        }
        /// <summary>
        /// Removes the specified contractor from the system's contractor list.
        /// </summary>
        /// <param name="contractor">The contractor to remove.</param>
        public void RemoveContractor(Contractor contractor)
        {
            Contractors.Remove(contractor);
        }

        /// <summary>
        /// Returns the full list of contractors in the system.
        /// </summary>
        /// <returns>A list containing all contractors.</returns>
        public List<Contractor> GetContractors()
        {
            return Contractors;
        }


        /// <summary>
        /// Returns a list of contractors who are not currently assigned 
        /// to any active (incomplete) job.
        /// </summary>
        /// <returns>
        /// A list of contractors who are available for new job assignments.
        /// </returns>
        public List<Contractor> GetAvailableContractors()
        {
            return Contractors
                .Where(c => !Jobs.Any(j => j.AssignedContractor == c && !j.IsCompleted))
                .ToList();
        }

        /// <summary>
        /// Adds a new job to the job list.
        /// </summary>
        /// <param name="job">The job to be added.</param>
        public void AddJob(Job job)
        {
            Jobs.Add(job);
        }

        /// <summary>
        /// Returns the full list of jobs.
        /// </summary>
        /// <returns>A list containing all jobs.</returns>
        public List<Job> GetJobs()
        {
            return Jobs;
        }

        /// <summary>
        /// Returns a list of jobs that have no contractor assigned 
        /// and are not yet completed.
        /// </summary>
        /// <returns>
        /// A list of unassigned and active (incomplete) jobs.
        /// </returns>
        public List<Job> GetUnassignedJobs()
        {
            return Jobs.Where(j => j.AssignedContractor == null && !j.IsCompleted).ToList();
        }

        /// <summary>
        /// Assigns a contractor to a job after verifying that both the job
        /// and contractor are valid and that the contractor is available.
        /// </summary>
        /// <param name="job">The job to assign a contractor to.</param>
        /// <param name="contractor">The contractor being assigned to the job.</param>
        /// <exception cref="Exception">
        /// Thrown when the job does not exist in the system or 
        /// when the contractor is not available for assignment.
        /// </exception>
        public void AssignJob(Job job, Contractor contractor)
        {
            if (job != null && contractor != null && !Jobs.Contains(job))
                throw new Exception("Job does not exist in the system.");

            if (!GetAvailableContractors().Contains(contractor))
                throw new Exception("Contractor is not available.");

            job.AssignedContractor = contractor;
            job.AssignedContractorName = $"{contractor.FirstName} {contractor.LastName}";
            contractor.IsAssigned = true;
        }


        /// <summary>
        /// Marks the specified job as completed and frees up the assigned contractor.
        /// </summary>
        /// <param name="job">The job to mark as completed.</param>
        /// <param name="contractor">The contractor who was assigned to the job.</param>
        /// <exception cref="Exception">
        /// Thrown if the job has no contractor assigned.
        /// </exception>
        public void CompleteJob(Job job, Contractor contractor)
        {
            if (job.AssignedContractor == null)
                throw new Exception("Cannot complete an unassigned job.");

            job.IsCompleted = true;
            contractor.IsAssigned = false;
        }

        /// <summary>
        /// Returns a list of jobs whose cost falls within the specified range, inclusive.
        /// </summary>
        /// <param name="minCost">The minimum cost to filter jobs.</param>
        /// <param name="maxCost">The maximum cost to filter jobs.</param>
        /// <returns>A list of jobs with costs between <paramref name="minCost"/> and <paramref name="maxCost"/>.</returns>
        public List<Job> GetJobByCost(decimal minCost, decimal maxCost)
        {
            return Jobs.Where(j => j.Cost >= minCost && j.Cost <= maxCost).ToList();

        }




    }

}
