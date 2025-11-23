using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
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
        // Collections that hold the data model instances.
        /* private ObservableCollection<Contractor> _contractors = new ObservableCollection<Contractor>();
         private ObservableCollection<Contractor> _availableContractors = new ObservableCollection<Contractor>();
         private ObservableCollection<Job> _jobs = new ObservableCollection<Job>();
         private ObservableCollection<Job> _unassignedJobs = new ObservableCollection<Job>();
        */
        
        private RecruitmentSystem recruitmentSystem;

        // Views used for filtering and binding to UI elements.
        //private ICollectionView _contractorsView;
        //private ICollectionView _jobsView;


        public MainWindow()
        {
            InitializeComponent();

            recruitmentSystem = new RecruitmentSystem();

            // Create collection views to support filtering.
            /*_contractorsView = CollectionViewSource.GetDefaultView(_availableContractors);
            _jobsView = CollectionViewSource.GetDefaultView(_unassignedJobs);


            // Setup bindings
            LvContractors.ItemsSource = _contractorsView;
            LvJobs.ItemsSource = _jobsView;*/


            // Seed some sample data (optional - helpful for testing)
            SeedSampleData();
            RefreshUI();
            //RefreshAvailableContractors();
            //RefreshUnassignedJobs()
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
        /// Add a contractor from the UI inputs after validating user input.
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

            if (!decimal.TryParse(TxtHourlyWage.Text, out decimal wage))
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

        private void RefreshUI()
        {
            LvContractors.ItemsSource = null;
            LvContractors.ItemsSource = recruitmentSystem.GetContractors();

            LvContractors.ItemsSource = null;
            LvContractors.ItemsSource = recruitmentSystem.GetContractors();

            LvJobs.ItemsSource = null;
            LvJobs.ItemsSource = recruitmentSystem.GetJobs();

            LvJobs.ItemsSource = null;
            LvJobs.ItemsSource = recruitmentSystem.GetUnassignedJobs();
        }
        
        private void RefreshContractors()
        {
            LvContractors.ItemsSource = null;

            // If checkbox is checked, filter only unassigned contractors
            var contractorsToShow = ChkShowAvailable.IsChecked == true ? recruitmentSystem.GetAvailableContractors() : recruitmentSystem.GetContractors();

            LvContractors.ItemsSource = contractorsToShow;

        }
        
        private void ChkShowAvailable_Changed(object sender, RoutedEventArgs e)
        {


            RefreshContractors();

        }
        
        private void RefreshJobs()
        {
            LvJobs.ItemsSource = null;

            // If checkbox is checked, filter only unassigned contractors
            var jobsToShow = ChkShowUnassignedJobs.IsChecked == true ? recruitmentSystem.GetUnassignedJobs() : recruitmentSystem.GetJobs();

            LvJobs.ItemsSource = jobsToShow;
        }

        private void ChkShowUnassignedJobs_Changed(object sender, RoutedEventArgs e)
        {
            RefreshJobs();
        }

        
        /// Remove selected contractor if they are not currently assigned to a job.
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
        /// Create a new job from UI inputs. Includes validation.
        /// </summary>
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


            // clear inputs
            TxtJobTitle.Text = "";
            TxtJobDescription.Text = "";
            TxtJobCost.Text = "";

            RefreshContractors();
            RefreshJobs();

        }
        
       /// Assign selected contractor to selected job.
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

            // assign contractor to job

            recruitmentSystem.AssignJob(
                    selectedJob,
                    selectedContractor
                );

            MessageBox.Show("Job assigned successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            RefreshContractors();
            RefreshJobs();
        }
       /// Set selected job as completed and return contractor to unassigned contractors.
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

            // complete job

            recruitmentSystem.CompleteJob(
                    selectedJob,
                    selectedJob.AssignedContractor
                );

            MessageBox.Show("Job completed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            RefreshContractors();
            RefreshJobs();

       }
        #endregion
        /*
       #region Filters & Search
       private void ChkShowOnlyAvailable_Checked(object sender, RoutedEventArgs e)
       {
           if (ChkShowAvailable.IsChecked == true)
           {
               _contractorsView.Filter = (obj) =>
               {
                   if (obj is Contractor c)
                       return !c.IsAssigned;
                   return false;
               };
           }
           else
           {
               _contractorsView.Filter = null;
           }
       }

       private void ChkShowOnlyUnassigned_Checked(object sender, RoutedEventArgs e)
       {
           if (ChkShowUnassignedJobs.IsChecked == true)
           {
               _jobsView.Filter = (obj) =>
               {
                   if (obj is Job j)
                       return !j.IsAssigned && !j.IsCompleted;
                   return false;
               };
           }
           else
           {
               _jobsView.Filter = null;
           }
       }

       /// Filter for contractor listbox.
       private void TxtSearchContractors_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
       {
           string search = TxtSearchContractors.Text?.Trim().ToLowerInvariant() ?? string.Empty;

           if (string.IsNullOrWhiteSpace(search))
           {
               _contractorsView.Filter = null;
               if (ChkShowAvailable.IsChecked == true)
                   RefreshAvailableContractors();
                   RefreshUnassignedJobs();
                   ChkShowOnlyAvailable_Checked(null, null);
               return;
           }

           _contractorsView.Filter = (obj) =>
           {
               if (obj is Contractor c)
               {
                   bool matches = c.DisplayName.ToLowerInvariant().Contains(search);
                   if (ChkShowAvailable.IsChecked == true)
                       matches &= !c.IsAssigned;
                   return matches;
               }
               return false;
           };
       }

       /// Filter for Jobs listbox.
       private void TxtSearchJobs_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
       {
           string search = TxtSearchJobs.Text?.Trim().ToLowerInvariant() ?? string.Empty;

           if (string.IsNullOrWhiteSpace(search))
           {
               _jobsView.Filter = null;
               if (ChkShowUnassignedJobs.IsChecked == true)
                   RefreshAvailableContractors();
                   RefreshUnassignedJobs();
                   ChkShowOnlyUnassigned_Checked(null, null);
               return;
           }

           _jobsView.Filter = (obj) =>
           {
               if (obj is Job j)
               {
                   bool matches = j.Title.ToLowerInvariant().Contains(search);
                   if (ChkShowUnassignedJobs.IsChecked == true)
                       matches &= !j.IsAssigned;
                   return matches;
               }
               return false;
           };
       }

       /// Search jobs by cost range (report).
       private void BtnSearchCost_Click(object sender, RoutedEventArgs e)
       {
           LbCostResults.Items.Clear();

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

           var results = _jobs.Where(j => j.Cost >= minVal && j.Cost <= maxVal).ToList();
           foreach (var job in results)
           {
               // Show title and cost in the listbox content
               LbCostResults.Items.Add(new { Title = $"{job.Title} — ${job.Cost:F2}", job.Id });
           }

           if (results.Count == 0)
           {
               MessageBox.Show("No jobs found in that cost range.", "Report", MessageBoxButton.OK, MessageBoxImage.Information);
           }
       }
       #endregion*/

    }
}