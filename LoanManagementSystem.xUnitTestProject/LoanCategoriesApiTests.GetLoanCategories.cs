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
        public void GetLoanCategories_OkResult()
        {
            // ARRANGE
            var dbName = nameof(LoanCategoriesApiTests.GetLoanCategories_OkResult);
            var logger = Mock.Of<ILogger<LoanCategoriesController>>();
            var dbContext = DbContextMocker.GetApplicationDbContext(dbName);
            var apiController = new LoanCategoriesController(dbContext, logger);

            // ACT
            IActionResult actionResult = apiController.GetLoanCategories().Result;

            // ASSERT
            // --- Check if the ActionResult is of the type OkObjectResult
            Assert.IsType<OkObjectResult>(actionResult);

            // --- Check if the HTTP Response Code is 200 "Ok"
            int expectedStatusCode = (int)System.Net.HttpStatusCode.OK;
            int actualStatusCode = (actionResult as OkObjectResult).StatusCode.Value;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public void GetLoanCategories_CheckCorrectResult()
        {
            // ARRANGE
            var dbName = nameof(LoanCategoriesApiTests.GetLoanCategories_OkResult);
            var logger = Mock.Of<ILogger<LoanCategoriesController>>();
            var dbContext = DbContextMocker.GetApplicationDbContext(dbName);
            var apiController = new LoanCategoriesController(dbContext, logger);

            // ACT: Call the API action method
            IActionResult actionResult = apiController.GetLoanCategories().Result;

            // ASSERT: Check if the ActionResult is of the type OkObjectResult
            Assert.IsType<OkObjectResult>(actionResult);


            // ACT: Extract the OkResult result 
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;

            // ASSERT: if the OkResult contains the object of the Correct Type
            Assert.IsAssignableFrom<List<LoanCategory>>(okResult.Value);


            // ACT: Extract the Categories from the result of the action
            var loancategoriesFromApi = okResult.Value.Should().BeAssignableTo<List<LoanCategory>>().Subject;

            // ASSERT: if the categories is NOT NULL
            Assert.NotNull(loancategoriesFromApi);

            // ASSERT: if the number of categories in the DbContext seed data
            //         is the same as the number of categories returned in the API Result
            Assert.Equal<int>(expected: DbContextMocker.TestData_LoanCategories.Length,
                              actual: loancategoriesFromApi.Count);

            // ASSERT: Test the data received from the API against the Seed Data
            int ndx = 0;
            foreach (LoanCategory loancategory in DbContextMocker.TestData_LoanCategories)
            {
                // ASSERT: check if the Category ID is correct
                Assert.Equal<int>(expected: loancategory.LoanId,
                                  actual: loancategoriesFromApi[ndx].LoanId);

                // ASSERT: check if the Category Name is correct
                Assert.Equal(expected: loancategory.LoanName,
                             actual: loancategoriesFromApi[ndx].LoanName);

                _testOutputHelper.WriteLine($"Compared Row # {ndx} successfully");

                ndx++;          // now compare against the next element in the array
            }
        }
    }
}
