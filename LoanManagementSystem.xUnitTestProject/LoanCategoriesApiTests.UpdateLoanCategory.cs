using FluentAssertions;
using LoanManagementSystem.Controllers;
using LoanManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LoanManagementSystem.xUnitTestProject
{
    public partial class LoanCategoriesApiTests
    {
        [Fact]
        public async void UpdateLoanCategory_OkResult01()
        {
            // ARRANGE
            var dbName = nameof(LoanCategoriesApiTests.UpdateLoanCategory_OkResult01);
            var logger = Mock.Of<ILogger<LoanCategoriesController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var apiController = new LoanCategoriesController(dbContext, logger);
            int editLoanId = 2;
            LoanCategory originalLoanCategory, changedLoanCategory;
            changedLoanCategory = new LoanCategory
            {
                LoanId = editLoanId,
                LoanName = "--Changed Loan Name"
            };

            // ACT #1: Get the Record to Edit.

            // (a) Get the Loan Category to edit (to ensure that the row exists before editing it)
            IActionResult actionResultGet = await apiController.GetLoanCategory(editLoanId);

            // (b) Check if HTTP 200 "Ok" 
            OkObjectResult OkResult = actionResultGet.Should().BeOfType<OkObjectResult>().Subject;

            // (c) Extract the Category Object from the OkObjectResult
            originalLoanCategory = OkResult.Value.Should().BeAssignableTo<LoanCategory>().Subject;

            // (d) Check if the data to be edited was received from the API
            Assert.NotNull(originalLoanCategory);

            _testOutputHelper.WriteLine("Retrieved the Data from the API.");

            try
            {
                // ACT #2: Try to update the data, using a completely new object
                //         NOTE: This will throw the InvalidOperationException!
                var actionResultPutAttempt1 = await apiController.PutLoanCategory(editLoanId, changedLoanCategory);

                // ASSERT - if the update was successfull.
                Assert.IsType<OkResult>(actionResultPutAttempt1);

                _testOutputHelper.WriteLine("Updated the changes back to the API - using a new object.");
            }
            catch (System.InvalidOperationException exp)
            {
                // The PUT operation should throw this exception,
                // because it is an object that does not carry change tracking information.

                _testOutputHelper.WriteLine("Failed to update the change back to the API - using a new object");
                _testOutputHelper.WriteLine($"-- Exception Type: {exp.GetType()}");
                _testOutputHelper.WriteLine($"-- Exception Message: {exp.Message}");
                _testOutputHelper.WriteLine($"-- Exception Source: {exp.Source}");
                _testOutputHelper.WriteLine($"-- Exception TargetSite: {exp.TargetSite}");
            }
        }

        [Fact]
        public async void UpdateCategory_OkResult02()
        {
            // ARRANGE
            var dbName = nameof(LoanCategoriesApiTests.UpdateCategory_OkResult02);
            var logger = Mock.Of<ILogger<LoanCategoriesController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var apiController = new LoanCategoriesController(dbContext, logger);
            int editLoanId = 2;
            LoanCategory originalLoanCategory;
            string changedLoanName = "--- CHANGED Loan Name";

            // ACT #1: Get the Record to Edit.

            // (a) Get the Loan Category to edit (to ensure that the row exists before editing it)
            IActionResult actionResultGet = await apiController.GetLoanCategory(editLoanId);

            // (b) Check if HTTP 200 "Ok" 
            OkObjectResult OkResult = actionResultGet.Should().BeOfType<OkObjectResult>().Subject;

            // (c) Extract the Loan Category Object from the OkObjectResult
            originalLoanCategory = OkResult.Value.Should().BeAssignableTo<LoanCategory>().Subject;

            // (d) Check if the data to be edited was received from the API
            Assert.NotNull(originalLoanCategory);

            _testOutputHelper.WriteLine("Retrieved the Data from the API.");

            // ACT #2: Now, try to update the data
            // SOLUTION: The following lines would work!
            //           Reason, we are modifying the data originally received.
            //           And then, calling the PUT operation.
            //           So, the API is able to find the ChangeTracking data associated to the object.

            // (a) Change the data of the object that was received from the API.
            originalLoanCategory.LoanName = changedLoanName;

            // (b) Call the HTTP PUT API to update the changes (with the updated object)
            var actionResultPutAttempt2 = await apiController.PutLoanCategory(editLoanId, originalLoanCategory);

            // ASSERT - if the update was successfull.
            Assert.IsType<NoContentResult>(actionResultPutAttempt2);

            _testOutputHelper.WriteLine("Updated the changes back to the API - using the object received");
        }
    }
}

