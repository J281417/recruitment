using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace recruitment
{
    public partial class MainWindow : Window
    {        
        public RecruitmentSystem recruitmentSystem;

        public MainWindow()
        {
            InitializeComponent();
            recruitmentSystem = new RecruitmentSystem();

            // Seed some sample data
            SeedSampleData();
            RefreshContractors();
            RefreshJobs();
        }

        #region Sample Data (for testing/demo)
        private void SeedSampleData()
        {
            recruitmentSystem.AddContractor(new Contractor("Peter", "Parker", DateTime.Now.AddMonths(-12), 45));
            recruitmentSystem.AddContractor(new Contractor("Tony", "Stark", DateTime.Now.AddMonths(-24), 120));
            recruitmentSystem.AddContractor(new Contractor("Natasha", "Romanoff", DateTime.Now.AddMonths(-18), 95));
            recruitmentSystem.AddContractor(new Contractor("Steve", "Rogers", DateTime.Now.AddMonths(-36), 100));
            recruitmentSystem.AddContractor(new Contractor("Bruce", "Banner", DateTime.Now.AddMonths(-20), 90));
            recruitmentSystem.AddContractor(new Contractor("Thor", "Odinson", DateTime.Now.AddMonths(-15), 110));
            recruitmentSystem.AddContractor(new Contractor("Wanda", "Maximoff", DateTime.Now.AddMonths(-10), 85));
            recruitmentSystem.AddContractor(new Contractor("Clint", "Barton", DateTime.Now.AddMonths(-22), 80));
            recruitmentSystem.AddContractor(new Contractor("Stephen", "Strange", DateTime.Now.AddMonths(-8), 105));
            recruitmentSystem.AddContractor(new Contractor("Sam", "Wilson", DateTime.Now.AddMonths(-12), 75));
            recruitmentSystem.AddContractor(new Contractor("Peter", "Quill", DateTime.Now.AddMonths(-9), 70));
            recruitmentSystem.AddContractor(new Contractor("Gamora", "Zen", DateTime.Now.AddMonths(-7), 85));
            recruitmentSystem.AddContractor(new Contractor("Rocket", "Raccoon", DateTime.Now.AddMonths(-6), 65));
            recruitmentSystem.AddContractor(new Contractor("Drax", "Destroyer", DateTime.Now.AddMonths(-5), 60));
            recruitmentSystem.AddContractor(new Contractor("Vision", "Synth", DateTime.Now.AddMonths(-14), 95));

            recruitmentSystem.AddJob(new Job("Kitchen Renovation", "Complete kitchen remodeling", DateTime.Now.AddMonths(-12), 5000));
            recruitmentSystem.AddJob(new Job("Bathroom Upgrade", "Install new tiles and fixtures", DateTime.Now.AddMonths(-12), 3000));
            recruitmentSystem.AddJob(new Job("Living Room Paint", "Paint walls and ceiling", DateTime.Now.AddMonths(-12), 1200));
            recruitmentSystem.AddJob(new Job("Roof Repair", "Fix leaks and replace damaged shingles", DateTime.Now.AddMonths(-12), 4500));
            recruitmentSystem.AddJob(new Job("Flooring Installation", "Install hardwood floors in living room", DateTime.Now.AddMonths(-12), 3500));
            recruitmentSystem.AddJob(new Job("Garage Renovation", "Organize space and add storage cabinets", DateTime.Now.AddMonths(-12), 2800));
            recruitmentSystem.AddJob(new Job("Basement Waterproofing", "Seal basement walls and floor", DateTime.Now.AddMonths(-12), 6000));
            recruitmentSystem.AddJob(new Job("Patio Construction", "Build stone patio with seating area", DateTime.Now.AddMonths(-12), 4000));
            recruitmentSystem.AddJob(new Job("Window Replacement", "Replace old windows with energy-efficient ones", DateTime.Now.AddMonths(-12), 2500));
            recruitmentSystem.AddJob(new Job("Door Installation", "Install front and back doors", DateTime.Now.AddMonths(-12), 1800));
            recruitmentSystem.AddJob(new Job("Deck Construction", "Build wooden deck in backyard", DateTime.Now.AddMonths(-12), 4200));
            recruitmentSystem.AddJob(new Job("Fence Installation", "Install wooden fence around property", DateTime.Now.AddMonths(-12), 3200));
            recruitmentSystem.AddJob(new Job("Lighting Upgrade", "Replace indoor lighting with LED fixtures", DateTime.Now.AddMonths(-12), 1500));
            recruitmentSystem.AddJob(new Job("HVAC Maintenance", "Service and repair heating/cooling system", DateTime.Now.AddMonths(-12), 2200));
            recruitmentSystem.AddJob(new Job("Exterior Painting", "Paint house exterior and trim", DateTime.Now.AddMonths(-12), 3800));

        }
        #endregion


        #region Contractor management

        /// <summary>
        /// Handles the Add Contractor button click event.
        /// Validates the user's input from the UI, creates a new contractor,
        /// adds it to the recruitment system, and updates the UI.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private void BtnAddContractor_Click(object sender, RoutedEventArgs e)
        {
            // Validate input
            string first = TxtFirstName.Text?.Trim() ?? string.Empty;
            string last = TxtLastName.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(last) )
            {
                MessageBox.Show("First name and last name are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!first.All(char.IsLetter) || !last.All(char.IsLetter))
            {
                MessageBox.Show("First and last names must contain only letters.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtHourlyWage.Text, out decimal wage) || wage <= 0 )
            {
                MessageBox.Show("Please enter a valid hourly wage.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            first = char.ToUpper(first[0]) + first.Substring(1).ToLower();
            last = char.ToUpper(last[0]) + last.Substring(1).ToLower();
            DateTime startDate = DpStartDate.SelectedDate ?? DateTime.Now;


            recruitmentSystem.AddContractor(new Contractor(first, last, startDate, wage));

            MessageBox.Show("Contractor added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Clear inputs
            TxtFirstName.Clear();
            TxtLastName.Clear();
            TxtHourlyWage.Clear();
            DpStartDate.SelectedDate = DateTime.Now;

            RefreshContractors();
            RefreshJobs();

        }

        /// <summary>
        /// Refreshes the contractors ListView on the UI.
        /// Shows either all contractors or only available contractors
        /// depending on the state of the "Available" checkbox.
        /// </summary>
        private void RefreshContractors()
        {
            LvContractors.ItemsSource = null;
            var contractorsToShow = ChkShowAvailable.IsChecked == true ? recruitmentSystem.GetAvailableContractors() : recruitmentSystem.GetContractors();
            LvContractors.ItemsSource = contractorsToShow;
        }

        /// <summary>
        /// Handles the Checked and Unchecked events of the "Available" checkbox.
        /// Updates the contractors ListView to show either all contractors
        /// or only available contractors depending on the checkbox state.
        /// </summary>
        /// <param name="sender">The checkbox that triggered the event.</param>
        /// <param name="e">Event arguments for the checked/unchecked event.</param>
        private void ChkShowAvailable_Changed(object sender, RoutedEventArgs e)
        {
            RefreshContractors();
        }

        /// <summary>
        /// Refreshes the jobs ListView on the UI.
        /// Shows either all jobs or only unassigned jobs depending on 
        /// the state of the "Unassigned" checkbox.
        /// </summary>
        private void RefreshJobs()
        {
            LvJobs.ItemsSource = null;
            var jobsToShow = ChkShowUnassignedJobs.IsChecked == true ? recruitmentSystem.GetUnassignedJobs() : recruitmentSystem.GetJobs();
            LvJobs.ItemsSource = jobsToShow;
        }

        /// <summary>
        /// Handles the Checked and Unchecked events of the "Unassigned" checkbox.
        /// Updates the jobs ListView to show either all jobs or only unassigned jobs
        /// depending on the checkbox state.
        /// </summary>
        /// <param name="sender">The checkbox that triggered the event.</param>
        /// <param name="e">Event arguments for the checked/unchecked event.</param>
        private void ChkShowUnassignedJobs_Changed(object sender, RoutedEventArgs e)
        {
            RefreshJobs();
        }


        /// <summary>
        /// Handles the click event for the "Remove Contractor" button.
        /// Validates that a contractor is selected and not currently assigned to a job,
        /// confirms the removal with the user, then removes the contractor from the system
        /// and refreshes the UI.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private void BtnRemoveContractor_Click(object sender, RoutedEventArgs e)
        {
            Contractor selected = (Contractor)LvContractors.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Select a contractor to remove.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Prevent removing a contractor who is assigned to a job.
            if (selected.IsAssigned)
            {
                MessageBox.Show("Cannot remove a contractor who is currently assigned to a job.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Remove contractor {selected.FirstName} {selected.LastName}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                recruitmentSystem.RemoveContractor(selected);
            }

            MessageBox.Show("Contractor added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            RefreshContractors();
            RefreshJobs();
        }

        #endregion

        #region Job management
        /// <summary>
        /// Handles the click event for the "Create Job" button.
        /// Validates the user's input from the UI, creates a new job,
        /// adds it to the recruitment system, and updates the UI.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private void BtnCreateJob_Click(object sender, RoutedEventArgs e)
       {
           string title = TxtJobTitle.Text?.Trim() ?? string.Empty;
           string desc = TxtJobDescription.Text?.Trim() ?? string.Empty;
           DateTime date = DpStartDate.SelectedDate ?? DateTime.Now;
           string costText = TxtJobCost.Text?.Trim() ?? string.Empty;

           if (string.IsNullOrWhiteSpace(title))
           {
               MessageBox.Show("Job title is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
               return;
           }

           if (!decimal.TryParse(costText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal cost) || cost < 0)
           {
               MessageBox.Show("Job cost must be a non-negative number (use dot for decimals).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
               return;
           }

            recruitmentSystem.AddJob(new Job(title, desc, date, cost));

            MessageBox.Show("Job added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            TxtJobTitle.Text = "";
            TxtJobDescription.Text = "";
            TxtJobCost.Text = "";

            RefreshContractors();
            RefreshJobs();

        }

        /// <summary>
        /// Handles the click event for the "Assign Contractor to Job" button.
        /// Validates that both a contractor and a job are selected, checks that
        /// the contractor is available and the job is unassigned, then assigns
        /// the contractor to the job and updates the UI.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private void BtnAssign_Click(object sender, RoutedEventArgs e)
       {
           Contractor selectedContractor = (Contractor)LvContractors.SelectedItem;
           Job selectedJob = (Job)LvJobs.SelectedItem;

           if (selectedContractor == null || selectedJob == null)
           {
               MessageBox.Show("Please select both a contractor and a job to assign.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
               return;
           }

           if (selectedContractor.IsAssigned)
           {
               MessageBox.Show("Selected contractor is already assigned.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
               return;
           }

           if (selectedJob.AssignedContractor != null)
           {
               MessageBox.Show("Selected job already has an assigned contractor.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
               return;
           }

            recruitmentSystem.AssignJob(
                    selectedJob,
                    selectedContractor
                );

            MessageBox.Show("Job assigned successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            RefreshContractors();
            RefreshJobs();
        }

        /// <summary>
        /// Handles the click event for the "Complete Selected Job" button.
        /// Validates that a job is selected, has an assigned contractor, and is not already completed.
        /// Marks the job as completed, frees up the contractor, and updates the UI.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private void BtnCompleteJob_Click(object sender, RoutedEventArgs e)
       {
           Job selectedJob = (Job)LvJobs.SelectedItem;

           if (selectedJob == null)
           {
               MessageBox.Show("Select a job to complete.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
               return;
           }

           if (selectedJob.AssignedContractor == null)
           {
               MessageBox.Show("This job has not been assigned.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
               return;
           }

           if (selectedJob.IsCompleted)
           {
               MessageBox.Show("This job is already complete.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
               return;
           }

           if (selectedJob.IsCompleted)
           {
               MessageBox.Show("This job is already completed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
               return;
           }

            recruitmentSystem.CompleteJob(
                    selectedJob,
                    selectedJob.AssignedContractor
                );

            MessageBox.Show("Job completed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            RefreshContractors();
            RefreshJobs();

       }
        #endregion

        #region Filters & Search




        /// <summary>
        /// Handles the TextChanged event for the contractor search TextBox.
        /// Filters the contractor list based on the user's search input, 
        /// matching against first and last names, and updates the ListView.
        /// </summary>
        /// <param name="sender">The TextBox where text was changed.</param>
        /// <param name="e">Event arguments for the text changed event.</param>
        private void TxtSearchContractors_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
       {

            
           string search = TxtSearchContractors.Text?.Trim().ToLowerInvariant() ?? string.Empty;

           if (string.IsNullOrWhiteSpace(search))
            {
                RefreshContractors();
            }

            var contractorsToShow = recruitmentSystem.GetContractors()
                .Where(c => string.IsNullOrEmpty(search) ||
                            c.FirstName.ToLower().Contains(search) ||
                            c.LastName.ToLower().Contains(search)).ToList();

            LvContractors.ItemsSource = contractorsToShow;

       }

        /// <summary>
        /// Handles the TextChanged event for the job search TextBox.
        /// Filters the job list based on the user's search input by matching
        /// against job titles, and updates the ListView.
        /// </summary>
        /// <param name="sender">The TextBox where text was changed.</param>
        /// <param name="e">Event arguments for the text changed event.</param>
        private void TxtSearchJobs_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
       {
           string search = TxtSearchJobs.Text?.Trim().ToLowerInvariant() ?? string.Empty;

           if (string.IsNullOrWhiteSpace(search))
            {
                RefreshJobs();
            }

            var jobsToShow = recruitmentSystem.GetJobs()
                .Where(j => string.IsNullOrEmpty(search) ||
                            j.Title.ToLower().Contains(search)).ToList();

            LvJobs.ItemsSource = jobsToShow;

       }


        /// <summary>
        /// Handles the click event for the "Search" button in the cost filter section.
        /// Retrieves the minimum and maximum cost values from the UI, validates them,
        /// and displays jobs within the specified cost range in the report ListView.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private void BtnSearchCost_Click(object sender, RoutedEventArgs e)
       {
           LvReports.ItemsSource = null;

           string minText = TxtCostMin.Text?.Trim() ?? string.Empty;
           string maxText = TxtCostMax.Text?.Trim() ?? string.Empty;

           bool minOk = decimal.TryParse(minText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal minVal);
           bool maxOk = decimal.TryParse(maxText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal maxVal);

           // If either blank, treat as open range
           if (!minOk) minVal = decimal.MinValue;
           if (!maxOk) maxVal = decimal.MaxValue;

           if (minVal > maxVal)
           {
               MessageBox.Show("Minimum cannot be greater than maximum.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
               return;
           }

            // Show search result
            var searchResult = recruitmentSystem.GetJobByCost(
                    minVal,
                    maxVal
                );
            if (searchResult.Count == 0)
            {
                MessageBox.Show("No jobs found in that cost range.", "Report", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            LvReports.ItemsSource = searchResult;

       }
       #endregion

    }
}