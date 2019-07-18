using FirstCateringLtd.BackService.Controllers;
using FirstCateringLtd.BackService.Data;
using FirstCateringLtd.BackService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace FirstCateringLtd.Tests
{
    //Used for testing REST API Service
    //The descriptions of tests are provided in the Design word document "Test Documentation" section.
    public class FunctionalityTests
    {
        [Fact]
        public void EmployeeIsInitialisedProperlyFromEmployeeInputData()
        {
            //arange
            var employeeInputData = new EmployeeInputData
            {
                CardIdNumber = "KAS123ZKLX1234",
                EmployeeId = "AKJSDL91283",
                Name = "Pavel Vjalicin",
                Email = "email@email.com",
                MobileNumber = "+1234 1234 1234 1234"
            };
            var employee = new Employee(employeeInputData);
            //act and assert
            Assert.Equal(0, employee.Credit);
            Assert.Equal("1234", employee.PinNumber);
            Assert.Equal(employeeInputData.CardIdNumber, employee.CardIdNumber);
            Assert.Equal(employeeInputData.EmployeeId, employee.EmployeeId);
            Assert.Equal(employeeInputData.Name, employee.Name);
            Assert.Equal(employeeInputData.Email, employee.Email);
            Assert.Equal(employeeInputData.MobileNumber, employee.MobileNumber);
        }

        [Fact]
        public void CardIdMustOnlyCantainAlphaNumericCharacters()
        {
            //arrange
            var employeeInputData = new EmployeeInputData
            {
                CardIdNumber = "KAS12{}3Z KLX1234",
                EmployeeId = "AKJSDL91283",
                Name = "Pavel Vjalicin",
                Email = "email@email.com",
                MobileNumber = "+1234 1234 1234 1234"
            };

            var employee = new Employee(employeeInputData);

            //act
            //CardID Must only contain alphanumeric characters
            var modelError = GetModelError(employee);

            //assert
            Assert.True(modelError.ErrorMessage != null);
        }

        [Fact]
        public void GetIdCardMustReturnNotFoundIfCardIdIsNotRegistered()
        {
            //arrange
            var _dbContext = AllocateInMemoryDatabaseContext("testDB1");

            var controller = new EmployeeController(_dbContext);

            //act 
            var result = controller.Get("nonExistantCardID");

            //assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User must register this card before proceeding.", notFoundResult.Value);
        }

        [Fact]
        public void GetIdCardMustReturnWelcomeMessageIfCardIsRegistered()
        {
            //arrange
            var _dbContext = AllocateInMemoryDatabaseContext("testDB2");

            var employeeInputData = new EmployeeInputData
            {
                CardIdNumber = "KAS123ZKLX1234",
                EmployeeId = "AKJSDL91283",
                Name = "Pavel Vjalicin",
                Email = "email@email.com",
                MobileNumber = "+1234 1234 1234 1234"
            };
            var employee = new Employee(employeeInputData);

            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();

            var controller = new EmployeeController(_dbContext);
            //act

            var result = controller.Get(employee.CardIdNumber);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Welcome " + employee.Name,okResult.Value);
        }

        [Fact]
        public void EmployeeMustBeAbleToCheckCredit()
        {
            //arrange
            var _dbContext = AllocateInMemoryDatabaseContext("testDB3");

            var employeeInputData = new EmployeeInputData
            {
                CardIdNumber = "KAS123ZKLX1234",
                EmployeeId = "AKJSDL91283",
                Name = "Pavel Vjalicin",
                Email = "email@email.com",
                MobileNumber = "+1234 1234 1234 1234"
            };
            var employee = new Employee(employeeInputData);

            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();

            var controller = new EmployeeController(_dbContext);
            //act

            var result = controller.GetCredit(employee.CardIdNumber);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0m, okResult.Value);
        }
        [Fact]
        public void InValidCardIdGetCreditCallShouldReturnNotFound()
        {
            //arrange
            var _dbContext = AllocateInMemoryDatabaseContext("testDB4");
            var controller = new EmployeeController(_dbContext);

            //act
            var result = controller.GetCredit("invalidCardId");

            //assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User with this card id doesn't exist.", notFound.Value);
        }

        [Fact]
        public void EmployeeMustBeAbleToUpdateCredit()
        {
            //arrange
            var _dbContext = AllocateInMemoryDatabaseContext("testDB4");

            var employeeInputData = new EmployeeInputData
            {
                CardIdNumber = "KAS123ZKLX1234",
                EmployeeId = "AKJSDL91283",
                Name = "Pavel Vjalicin",
                Email = "email@email.com",
                MobileNumber = "+1234 1234 1234 1234"
            };
            var employee = new Employee(employeeInputData);

            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();

            var controller = new EmployeeController(_dbContext);

            //act
            var putResult = controller.PutCredit(employee.CardIdNumber,255.55m);
            var getResult = controller.GetCredit(employee.CardIdNumber);

            //assert
            Assert.IsType<OkResult>(putResult);
            var getOkResult = Assert.IsType<OkObjectResult>(getResult);
            Assert.Equal(255.55m, getOkResult.Value);
        }

        //Used to allocate an in-memory database context
        //dbName string is used to assign a specific database name that will remain persistant through the whole testing process
        private DatabaseContext AllocateInMemoryDatabaseContext(string dbName)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseInMemoryDatabase(dbName);
            return new DatabaseContext(optionsBuilder.Options);
        }

        //Validates Model with ORM based on DataAnnotations (Data type constraints) provided on the Model fields
        //Expected to return a validation error from Model provided for testing purposes. Returns null if validation passed, breaking the test.
        private ValidationResult GetModelError(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults[0];
        }
    }

    
}
