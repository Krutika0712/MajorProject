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
        public void InsertLoanCategory_BadRequestResult()
        {
            // ARRANGE
            var dbName = nameof(LoanCategoriesApiTests.InsertLoanCategory_BadRequestResult);
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
        public void InsertLoanCategory_OkResult()
        {
            // ARRANGE
            var dbName = nameof(LoanCategoriesApiTests.InsertLoanCategory_OkResult);
            var logger = Mock.Of<ILogger<LoanCategoriesController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var apiController = new LoanCategoriesController(dbContext, logger);
            LoanCategory loancategoryToAdd = new LoanCategory
            {
                LoanId = 5,
               LoanName = "New Loan Category"             // IF = null, then: INVALID!  LoanName is REQUIRED
            };

            // ACT
            IActionResult actionResultPost = apiController.PostLoanCategory(loancategoryToAdd).Result;

            // ASSERT - check if the IActionResult is Ok
            Assert.IsType<OkObjectResult>(actionResultPost);

            // ASSERT - check if the Status Code is (HTTP 200) "Ok", (HTTP 201 "Created")
            int expectedStatusCode = (int)System.Net.HttpStatusCode.OK;
            var actualStatusCode = (actionResultPost as OkObjectResult).StatusCode.Value;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);

            // Extract the result from the IActionResult object.
            var postResult = actionResultPost.Should().BeOfType<OkObjectResult>().Subject;

            // ASSERT - if the result is a CreatedAtActionResult
            Assert.IsType<CreatedAtActionResult>(postResult.Value);

            // Extract the inserted Category object
            LoanCategory actualLoanCategory = (postResult.Value as CreatedAtActionResult).Value
                                      .Should().BeAssignableTo<LoanCategory>().Subject;

            // ASSERT - if the inserted Loan Category object is NOT NULL
            Assert.NotNull(actualLoanCategory);

            Assert.Equal(loancategoryToAdd.LoanId, actualLoanCategory.LoanId);
            Assert.Equal(loancategoryToAdd.LoanName, actualLoanCategory.LoanName);
        }
    }
}
