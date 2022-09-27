using FluentAssertions;
using LoanManagementSystem.Controllers;
using LoanManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LoanManagementSystem.xUnitTestProject
{
    public partial class LoanCategoriesApiTests
    {
        [Fact]
        public void GetLoanCategoryById_NotFoundResult()
        {
            // ARRANGE
            var dbName = nameof(LoanCategoriesApiTests.GetLoanCategoryById_NotFoundResult);
            var logger = Mock.Of<ILogger<LoanCategoriesController>>();

            // using (var db = DbContextMocker.GetApplicationDbContext(dbName))
            // {
            // }           // db.Dispose(); implicitly called when you exit the USING Block

            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var apiController = new LoanCategoriesController(dbContext, logger);
            int findLoanID = 900;

            // ACT
            IActionResult actionResultGet = apiController.GetLoanCategory(findLoanID).Result;

            // ASSERT - check if the IActionResult is NotFound 
            Assert.IsType<NotFoundResult>(actionResultGet);

            // ASSERT - check if the Status Code is (HTTP 404) "NotFound"
            int expectedStatusCode = (int)System.Net.HttpStatusCode.NotFound;
            var actualStatusCode = (actionResultGet as NotFoundResult).StatusCode;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public void GetLoanCategoryById_BadRequestResult()
        {
            // ARRANGE
            var dbName = nameof(LoanCategoriesApiTests.GetLoanCategoryById_BadRequestResult);
            var logger = Mock.Of<ILogger<LoanCategoriesController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var controller = new LoanCategoriesController(dbContext, logger);
            int? findLoanID = null;

            // ACT
            IActionResult actionResultGet = controller.GetLoanCategory(findLoanID).Result;

            // ASSERT - check if the IActionResult is BadRequest
            Assert.IsType<BadRequestResult>(actionResultGet);

            // ASSERT - check if the Status Code is (HTTP 400) "BadRequest"
            int expectedStatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            var actualStatusCode = (actionResultGet as BadRequestResult).StatusCode;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public void GetLoanCategoryById_OkResult()
        {
            // ARRANGE
            var dbName = nameof(LoanCategoriesApiTests.GetLoanCategoryById_OkResult);
            var logger = Mock.Of<ILogger<LoanCategoriesController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var controller = new LoanCategoriesController(dbContext, logger);
            int findLoanID = 2;

            // ACT
            IActionResult actionResultGet = controller.GetLoanCategory(findLoanID).Result;

            // ASSERT - if IActionResult is Ok
            Assert.IsType<OkObjectResult>(actionResultGet);

            // ASSERT - if Status Code is HTTP 200 (Ok)
            int expectedStatusCode = (int)System.Net.HttpStatusCode.OK;
            var actualStatusCode = (actionResultGet as OkObjectResult).StatusCode.Value;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public void GetLoanCategoryById_CorrectResult()
        {
            // ARRANGE
            var dbName = nameof(LoanCategoriesApiTests.GetLoanCategoryById_OkResult);
            var logger = Mock.Of<ILogger<LoanCategoriesController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var controller = new LoanCategoriesController(dbContext, logger);
            int findLoanID = 2;
            LoanCategory expectedLoanCategory = DbContextMocker.TestData_LoanCategories
                                        .SingleOrDefault(c => c.LoanId == findLoanID);

            // ACT
            IActionResult actionResultGet = controller.GetLoanCategory(findLoanID).Result;

            // ASSERT - if IActionResult is Ok
            Assert.IsType<OkObjectResult>(actionResultGet);

            // ASSERT - if IActionResult (i.e., OkObjectResult) contains an object of the type Category
            OkObjectResult okResult = actionResultGet.Should().BeOfType<OkObjectResult>().Subject;
            Assert.IsType<LoanCategory>(okResult.Value);

            // Extract the category object from the result.
            LoanCategory actualLoanCategory = okResult.Value.Should().BeAssignableTo<LoanCategory>().Subject;
            _testOutputHelper.WriteLine($"Found: LoanID == {actualLoanCategory.LoanId}");

            // ASSERT - if category is NOT NULL
            Assert.NotNull(actualLoanCategory);

            // ASSERT - if the CategoryId is containing the expected data.
            Assert.Equal<int>(expected: expectedLoanCategory.LoanId,
                              actual: actualLoanCategory.LoanId);

            // ASSERT - if the CateogoryName is correct 
            Assert.Equal(expected: expectedLoanCategory.LoanName,
                         actual: actualLoanCategory.LoanName);
        }
    }
}
