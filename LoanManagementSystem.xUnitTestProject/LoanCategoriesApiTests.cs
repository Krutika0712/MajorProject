using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace LoanManagementSystem.xUnitTestProject
{
   
    public partial class LoanCategoriesApiTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public LoanCategoriesApiTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

    }
}
