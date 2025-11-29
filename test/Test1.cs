
using recruitment;

namespace test
{
    [TestClass]
    public class Test1
    {

        // ------------------------------
        // Add Contractor
        // ------------------------------

        [TestMethod]
        public void AddContractor_ValidContractor_AddsToList()
        {
            RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
            var contractor = new Contractor("John", "Doe", DateTime.Now.AddMonths(-12), 45);

            recruitmentSystem.AddContractor(contractor);

            Assert.AreEqual(1, recruitmentSystem.Contractors.Count);
            Assert.AreEqual("John", recruitmentSystem.Contractors[0].FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddContractor_MissingFirstName_ThrowsException()
        {
            RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
            var contractor = new Contractor("", "Doe", DateTime.Now.AddMonths(-12), 45);

            recruitmentSystem.AddContractor(contractor);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddContractor_MissingLastName_ThrowsException()
        {
            RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
            var contractor = new Contractor("John", "", DateTime.Now.AddMonths(-12), 45);

            recruitmentSystem.AddContractor(contractor);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddContractor_InvalidWage_ThrowsException()
        {
            RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
            var contractor = new Contractor("John", "Doe", DateTime.Now.AddMonths(-12), -50);

            recruitmentSystem.AddContractor(contractor);

        }

        // ------------------------------
        // Remove Contractor
        // ------------------------------

        [TestMethod]
        public void RemoveContractor_RemovesFromList()
        {
            RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
            var contractor = new Contractor("John", "Doe", DateTime.Now.AddMonths(-12), 50);
            recruitmentSystem.AddContractor(contractor);

            recruitmentSystem.RemoveContractor(contractor);

            Assert.AreEqual(0, recruitmentSystem.Contractors.Count);
        }


        // ------------------------------
        // Create Job
        // ------------------------------

        [TestMethod]
        public void AddJob_AddsToList()
        {
            RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
            var job = new Job("Kitchen Renovation", "Complete kitchen remodeling", DateTime.Now.AddMonths(-12), 5000);

            recruitmentSystem.AddJob(job);

            Assert.AreEqual(1, recruitmentSystem.Jobs.Count);
        }


        // ------------------------------
        // View/filter jobs that do not have a contractor assigned.
        // ------------------------------

        [TestMethod]
        public void GetUnassignedJobs_ReturnsCorrectJobs()
        {
            RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
            var contractor = new Contractor("John", "Doe", DateTime.Now.AddMonths(-12), 50);
            recruitmentSystem.AddContractor(contractor);
            var job = new Job("Kitchen Renovation", "Complete kitchen remodeling", DateTime.Now.AddMonths(-12), 5000);
            var job2 = new Job("Kitchen Bathroom", "Complete bathroom remodeling", DateTime.Now.AddMonths(-12), 5000);

            recruitmentSystem.AddJob(job);
            recruitmentSystem.AddJob(job2);
            recruitmentSystem.AssignJob(job, contractor);

            var unassignedJob = recruitmentSystem.GetUnassignedJobs();

            Assert.AreEqual(1, unassignedJob.Count);
            Assert.AreEqual(job2, unassignedJob[0]);
        }

        // ------------------------------
        // Assign a contractor to a job
        // ------------------------------

        [TestMethod]
        public void AssignJob_ValidAssignment_SetsPropertiesCorrectly()
        {
            RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
            var contractor = new Contractor("John", "Doe", DateTime.Now.AddMonths(-12), 50);
            recruitmentSystem.AddContractor(contractor);
            var job = new Job("Kitchen Renovation", "Complete kitchen remodeling", DateTime.Now.AddMonths(-12), 5000);
            recruitmentSystem.AddJob(job);

            recruitmentSystem.AssignJob(job, contractor);

            Assert.AreEqual(contractor, job.AssignedContractor);
            Assert.AreEqual("John Doe", job.AssignedContractorName);
            Assert.IsTrue(contractor.IsAssigned);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AssignJob_ContractorNotAvailable_Throws()
        {
            RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
            var contractor = new Contractor("John", "Doe", DateTime.Now.AddMonths(-12), 50);
            var contractor2 = new Contractor("Jane", "Doe", DateTime.Now.AddMonths(-12), 50);
            recruitmentSystem.AddContractor(contractor);
            var job = new Job("Kitchen Renovation", "Complete kitchen remodeling", DateTime.Now.AddMonths(-12), 5000);

            recruitmentSystem.AddJob(job);
            recruitmentSystem.AssignJob(job, contractor);
            recruitmentSystem.AssignJob(job, contractor2);
        }


        // ------------------------------
        // View/filter available contractors to add to a job.
        // ------------------------------

        [TestMethod]
        public void GetAvailableContractors_ReturnsUnassignedContractors()
        {
            RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
            var contractor = new Contractor("John", "Doe", DateTime.Now.AddMonths(-12), 50);
            var contractor2 = new Contractor("Jane", "Doe", DateTime.Now.AddMonths(-12), 50);
            recruitmentSystem.AddContractor(contractor);
            recruitmentSystem.AddContractor(contractor2);
            var job = new Job("Kitchen Renovation", "Complete kitchen remodeling", DateTime.Now.AddMonths(-12), 5000);
            recruitmentSystem.AddJob(job);
            recruitmentSystem.AssignJob(job, contractor);

            var availableContractor = recruitmentSystem.GetAvailableContractors();

            Assert.AreEqual(1, availableContractor.Count);
            Assert.AreEqual(contractor2, availableContractor[0]);
        }


        // ------------------------------
        // Complete a job (returning the contractor to the available pool).
        // ------------------------------

        [TestMethod]
        public void CompleteJob_Valid_SetsJobCompleteAndContractorAvailable()
        {
            RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
            var contractor = new Contractor("John", "Doe", DateTime.Now.AddMonths(-12), 50);
            recruitmentSystem.AddContractor(contractor);
            var job = new Job("Kitchen Renovation", "Complete kitchen remodeling", DateTime.Now.AddMonths(-12), 5000);

            recruitmentSystem.AddJob(job);
            recruitmentSystem.AssignJob(job, contractor);
            recruitmentSystem.CompleteJob(job, contractor);

            Assert.IsTrue(job.IsCompleted);
            Assert.IsFalse(contractor.IsAssigned);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CompleteJob_UnassignedJob_Throws()
        {
            RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
            var contractor = new Contractor("John", "Doe", DateTime.Now.AddMonths(-12), 50);
            recruitmentSystem.AddContractor(contractor);
            var job = new Job("Kitchen Renovation", "Complete kitchen remodeling", DateTime.Now.AddMonths(-12), 5000);

            recruitmentSystem.AddJob(job);
            recruitmentSystem.CompleteJob(job, contractor);

        }

        // ------------------------------
        // Search for a job by cost within a given range.
        // ------------------------------

        [TestMethod]
        public void GetJobByCost_ReturnsCorrectRange()
        {
            RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
            var job = new Job("Kitchen Renovation", "Complete kitchen remodeling", DateTime.Now.AddMonths(-12), 1000);
            var job2 = new Job("Kitchen Renovation", "Complete kitchen remodeling", DateTime.Now.AddMonths(-12), 5000);
            var job3 = new Job("Kitchen Renovation", "Complete kitchen remodeling", DateTime.Now.AddMonths(-12), 10000);
            recruitmentSystem.AddJob(job);
            recruitmentSystem.AddJob(job2);
            recruitmentSystem.AddJob(job3);

            var filteredJob = recruitmentSystem.GetJobByCost(100, 4000);

            Assert.AreEqual(1, filteredJob.Count);
            Assert.AreEqual(job, filteredJob[0]);
        }
    }
}