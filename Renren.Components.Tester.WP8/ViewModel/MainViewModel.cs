using GalaSoft.MvvmLight;
using Renren.Components.Tester.Model.TestCases;
using System.Collections.ObjectModel;

namespace Renren.Components.Tester.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly ITestCasesProvider _dataService;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        public TestCase CurrentCase { get; set; }

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }

            set
            {
                if (_welcomeTitle == value)
                {
                    return;
                }

                _welcomeTitle = value;
                RaisePropertyChanged(WelcomeTitlePropertyName);
            }
        }

        private ObservableCollection<TestCase> _items = new ObservableCollection<TestCase>();
        public ObservableCollection<TestCase> Items
        {
            get { return _items; }
            set { this.Set("Items", ref _items, value); }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ITestCasesProvider dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }
                    
                    WelcomeTitle = item.Title;
                    Items = item.Cases;
                });
        }

        public void HandleCaseRun(TestCase item, object owner)
        {
            if (item.NeedJumpPage)
            {
                
            }
            else
            {
                item.RunAction(item, this);
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}