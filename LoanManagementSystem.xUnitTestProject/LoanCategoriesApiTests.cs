﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace LoanManagementSystem.xUnitTestProject
{
    // Add the following NuGet Packages:
    //      Microsoft.EntityFrameworkCore.InMemory      (same version as EF Core in WebApp)
    //      Moq                                         (latest version)
    //      FluentAssertions                            (latest version)
    // Also add Project Reference to the WebApp
    public partial class LoanCategoriesApiTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public LoanCategoriesApiTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

    }
}
