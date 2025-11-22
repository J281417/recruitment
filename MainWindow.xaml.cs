using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private ObservableCollection<Contractor> _contractors = new ObservableCollection<Contractor>();
        private ObservableCollection<Contractor> _availableContractors = new ObservableCollection<Contractor>();
        private ObservableCollection<Job> _jobs = new ObservableCollection<Job>();
        private ObservableCollection<Job> _unassignedJobs = new ObservableCollection<Job>();

        // Views used for filtering and binding to UI elements.
        private ICollectionView _contractorsView;
        private ICollectionView _jobsView;

        public MainWindow()
        {
            InitializeComponent();

            // Create collection views to support filtering.
            _contractorsView = CollectionViewSource.GetDefaultView(_availableContractors);
            _jobsView = CollectionViewSource.GetDefaultView(_unassignedJobs);

            // Setup bindings
            LvContractors.ItemsSource = _contractorsView;
            LvJobs.ItemsSource = _jobsView;

            // Seed some sample data (optional - helpful for testing)
            SeedSampleData();
            RefreshAvailableContractors();
            RefreshUnassignedJobs();
        }

        #region Sample Data (for testing/demo)
        private void SeedSampleData()
        {
            _contractors.Add(new Contractor { FirstName = "Peter", LastName = "Parker", StartDate = DateTime.Now.AddMonths(-12), HourlyWage = 45 });
            _contractors.Add(new Contractor { FirstName = "Tony", LastName = "Stark", StartDate = DateTime.Now.AddMonths(-24), HourlyWage = 120 });
            _contractors.Add(new Contractor { FirstName = "Natasha", LastName = "Romanoff", StartDate = DateTime.Now.AddMonths(-18), HourlyWage = 95 });
            _contractors.Add(new Contractor { FirstName = "Steve", LastName = "Rogers", StartDate = DateTime.Now.AddMonths(-36), HourlyWage = 100 });
            _contractors.Add(new Contractor { FirstName = "Bruce", LastName = "Banner", StartDate = DateTime.Now.AddMonths(-20), HourlyWage = 90 });
            _contractors.Add(new Contractor { FirstName = "Thor", LastName = "Odinson", StartDate = DateTime.Now.AddMonths(-15), HourlyWage = 110 });
            _contractors.Add(new Contractor { FirstName = "Wanda", LastName = "Maximoff", StartDate = DateTime.Now.AddMonths(-10), HourlyWage = 85 });
            _contractors.Add(new Contractor { FirstName = "Clint", LastName = "Barton", StartDate = DateTime.Now.AddMonths(-22), HourlyWage = 80 });
            _contractors.Add(new Contractor { FirstName = "Stephen", LastName = "Strange", StartDate = DateTime.Now.AddMonths(-8), HourlyWage = 105 });
            _contractors.Add(new Contractor { FirstName = "Sam", LastName = "Wilson", StartDate = DateTime.Now.AddMonths(-12), HourlyWage = 75 });
            _contractors.Add(new Contractor { FirstName = "Peter", LastName = "Quill", StartDate = DateTime.Now.AddMonths(-9), HourlyWage = 70 });
            _contractors.Add(new Contractor { FirstName = "Gamora", LastName = "Zen", StartDate = DateTime.Now.AddMonths(-7), HourlyWage = 85 });
            _contractors.Add(new Contractor { FirstName = "Rocket", LastName = "Raccoon", StartDate = DateTime.Now.AddMonths(-6), HourlyWage = 65 });
            _contractors.Add(new Contractor { FirstName = "Drax", LastName = "Destroyer", StartDate = DateTime.Now.AddMonths(-5), HourlyWage = 60 });
            _contractors.Add(new Contractor { FirstName = "Vision", LastName = "Synth", StartDate = DateTime.Now.AddMonths(-14), HourlyWage = 95 });

            _jobs.Add(new Job { Title = "Kitchen Renovation", Description = "Complete kitchen remodeling", Cost = 5000, JobDate = DateTime.Now.AddDays(-10) });
            _jobs.Add(new Job { Title = "Bathroom Upgrade", Description = "Install new tiles and fixtures", Cost = 3000, JobDate = DateTime.Now.AddDays(-8) });
            _jobs.Add(new Job { Title = "Living Room Paint", Description = "Paint walls and ceiling", Cost = 1200, JobDate = DateTime.Now.AddDays(-7) });
            _jobs.Add(new Job { Title = "Roof Repair", Description = "Fix leaks and replace damaged shingles", Cost = 4500, JobDate = DateTime.Now.AddDays(-15) });
            _jobs.Add(new Job { Title = "Flooring Installation", Description = "Install hardwood floors in living room", Cost = 3500, JobDate = DateTime.Now.AddDays(-12) });
            _jobs.Add(new Job { Title = "Garage Renovation", Description = "Organize space and add storage cabinets", Cost = 2800, JobDate = DateTime.Now.AddDays(-5) });
            _jobs.Add(new Job { Title = "Basement Waterproofing", Description = "Seal basement walls and floor", Cost = 6000, JobDate = DateTime.Now.AddDays(-20) });
            _jobs.Add(new Job { Title = "Patio Construction", Description = "Build stone patio with seating area", Cost = 4000, JobDate = DateTime.Now.AddDays(-18) });
            _jobs.Add(new Job { Title = "Window Replacement", Description = "Replace old windows with energy-efficient ones", Cost = 2500, JobDate = DateTime.Now.AddDays(-3) });
            _jobs.Add(new Job { Title = "Door Installation", Description = "Install front and back doors", Cost = 1800, JobDate = DateTime.Now.AddDays(-2) });
            _jobs.Add(new Job { Title = "Deck Construction", Description = "Build wooden deck in backyard", Cost = 4200, JobDate = DateTime.Now.AddDays(-14) });
            _jobs.Add(new Job { Title = "Fence Installation", Description = "Install wooden fence around property", Cost = 3200, JobDate = DateTime.Now.AddDays(-6) });
            _jobs.Add(new Job { Title = "Lighting Upgrade", Description = "Replace indoor lighting with LED fixtures", Cost = 1500, JobDate = DateTime.Now.AddDays(-1) });
            _jobs.Add(new Job { Title = "HVAC Maintenance", Description = "Service and repair heating/cooling system", Cost = 2200, JobDate = DateTime.Now.AddDays(-11) });
            _jobs.Add(new Job { Title = "Exterior Painting", Description = "Paint house exterior and trim", Cost = 3800, JobDate = DateTime.Now.AddDays(-9) });

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

            var c = new Contractor { FirstName = first, LastName = last, StartDate = startDate, HourlyWage = wage};

            _contractors.Add(c);

            MessageBox.Show("Contractor added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Clear inputs
            TxtFirstName.Clear();
            TxtLastName.Clear();
            TxtHourlyWage.Clear();
            DpStartDate.SelectedDate = DateTime.Now;

            RefreshAvailableContractors();
            RefreshUnassignedJobs();

        }

        private void RefreshAvailableContractors()
        {
            _availableContractors.Clear();

            // If checkbox is checked, filter only unassigned contractors
            var contractorsToShow = ChkShowAvailable.IsChecked == true ? _contractors.Where(c => !c.IsAssigned): _contractors;

            foreach (var c in contractorsToShow)
                _availableContractors.Add(c);
        }

        private void ChkShowAvailable_Changed(object sender, RoutedEventArgs e)
        {
            RefreshAvailableContractors();
        }

        private void RefreshUnassignedJobs()
        {
            _unassignedJobs.Clear();

            // If checkbox is checked, filter only unassigned contractors
            var jobsToShow = ChkShowUnassignedJobs.IsChecked == true ? _jobs.Where(j => !j.IsAssigned) : _jobs;

            foreach (var j in jobsToShow)
                _unassignedJobs.Add(j);
        }

        private void ChkShowUnassignedJobs_Changed(object sender, RoutedEventArgs e)
        {
            RefreshUnassignedJobs();
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

            if (MessageBox.Show($"Remove contractor {selected.DisplayName}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _contractors.Remove(selected);
            }

            RefreshAvailableContractors();
            RefreshUnassignedJobs();
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

            var job = new Job { Title = title, Description = desc, Cost = cost };
            _jobs.Add(job);

            // clear inputs
            TxtJobTitle.Text = "";
            TxtJobDescription.Text = "";
            TxtJobCost.Text = "";

            RefreshAvailableContractors();
            RefreshUnassignedJobs();

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

            if (selectedJob.IsAssigned)
            {
                MessageBox.Show("Selected job already has an assigned contractor.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // assign contractor to job
            selectedJob.AssignedContractor = selectedContractor;
            selectedContractor.IsAssigned = true;

            RefreshAvailableContractors();
            RefreshUnassignedJobs();
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
            selectedJob.CompletedAt = DateTime.Now;

            // if a contractor was assigned, unassign them
            if (selectedJob.AssignedContractor != null)
            {
                selectedJob.AssignedContractor.IsAssigned = false;
                selectedJob.AssignedContractor = null;
            }

            RefreshAvailableContractors();
            RefreshUnassignedJobs();
        }
        #endregion

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
        #endregion

    }
}