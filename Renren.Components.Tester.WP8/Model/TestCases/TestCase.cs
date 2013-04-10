using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.Tester.Model.TestCases
{
    [DataContract]
    public class TestCase : ObservableObject
    {
        private string _category = string.Empty;
        public string Category
        {
            get { return _category;  }
            set {this.Set("Category", ref _category, value); }
        }

        private string _group = string.Empty;
        public string Group
        {
            get { return _group; }
            set { this.Set("Group", ref _group, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set { this.Set("Description", ref _description, value); }
        }

        private long _id = -1;
        public long Id
        {
            get { return _id; }
            set { this.Set("Id", ref _id, value); }
        }

        private RelayCommand _command = null;
        public RelayCommand Command
        {
            get { return _command; }
            set { this.Set("Command", ref _command, value); }
        }

        private Action<TestCase, object> _runAction = null;
        public Action<TestCase, object> RunAction
        {
            get { return _runAction; }
            set { this.Set("RunAction", ref _runAction, value); }
        }

        private bool _result = false;
        public bool Result
        {
            get { return _result; }
            set { this.Set("Result", ref _result, value); }
        }

        private bool _needJump = false;
        public bool NeedJumpPage
        {
            get { return _needJump; }
            set { this.Set("NeedJumpPage", ref _needJump, value); }
        }

        private string _jumpUri = string.Empty;
        public string JumpPageUri
        {
            get { return _jumpUri; }
            set { this.Set("JumpPageUri", ref _jumpUri, value); }
        }

        private Type _jumpType = null;
        public Type JumpPageType
        {
            get { return _jumpType; }
            set { this.Set("JumpPageType", ref _jumpType, value); }
        }
    }
}
