using Renren.Components.Tester.Model.TestCases;
using System;

namespace Renren.Components.Tester.Design
{
    public class DesignDataService : ITestCasesProvider
    {
        public void GetData(Action<TestCasesModel, Exception> callback)
        {
            // Use this to create design time data

            var item = new TestCasesModel("Renren Comp. Test Cases [design]");
            callback(item, null);
        }
    }
}