using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace Renren.Components.Tester.Model.TestCases
{
    public class TestCasesModel : ObservableObject
    {
        public TestCasesModel(string title)
        {
            this._title = title;
        }

        private ObservableCollection<TestCase> _cases = new ObservableCollection<TestCase>();
        public ObservableCollection<TestCase> Cases
        {
            get { return _cases; }
            set { this.Set("Cases", ref _cases, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { this.Set("Title", ref _title, value); }
        }
    }
}
