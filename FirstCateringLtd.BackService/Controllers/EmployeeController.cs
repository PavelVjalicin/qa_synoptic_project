using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstCateringLtd.BackService.Data;
using FirstCateringLtd.BackService.Models;
using System.Text.RegularExpressions;

namespace FirstCateringLtd.BackService.Controllers
{

    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        //Initialising database context to be used with the controller
        //DatabaseContext is injected using dependancy injection by ASP.NET Core Framework
        //DatabaseContext outlines the structure of the database without a specific database type implementation
        DatabaseContext _context;
        public EmployeeController(DatabaseContext context)
        {
            _context = context;
        }

        // GET api/Employee
        // Returns array of all employees
        [HttpGet]
        [ProducesResponseType(typeof(List<Employee>), 200)]
        public IActionResult Get()
        {
            return Ok(_context.Employees.ToList());
        }




        // Returns an employee from cardId
        // Returns 200 Status code with employee data
        // Returns 404 status code with error message if employee was not found
        // GET api/Employee/{cardId}
        [HttpGet("{cardId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Get(string cardId)
        {
            var result = _context.Employees.Find(cardId);
            if(result != null)
            {
                return Ok("Welcome " + result.Name);
            } else
            {
                return NotFound("User must register this card before proceeding.");
            }
        }

        // Adds an employee with credentials
        // Returns 200 Status code on completion.
        // Returns 400 Status code if employee with this cardId already exists.
        // Body must contain EmployeeInputData class fields.
        // POST api/Employee
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody]EmployeeInputData employeeInputData)
        {   
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Checking the databse if employee with cardId already exists.
            var existingEmployee = _context.Employees.Find(employeeInputData.CardIdNumber);

            if (existingEmployee != null) return BadRequest("Employee with this card id already exists");

            var employee = new Employee(employeeInputData);

            _context.Employees.Add(employee);

            _context.SaveChanges();

            return Ok();
        }


        //Replaces the employee with new values
        //Returns 200 if employee was successfully updated
        //Returns 400 if request body was filled in not in a right way
        //Returns 404 if employee was not found
        //Body request to have fields of class EmployeeInputData
        //PUT api/Employee
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Put( [FromBody] EmployeeInputData employeeInputData)
        { 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _context.Employees.Find(employeeInputData.CardIdNumber);

            if(employee == null)
            {
                return NotFound("No employee found with this card id");
            }

            employee.Update(employeeInputData);

            _context.Employees.Update(employee);
            _context.SaveChanges();

            return Ok();
        }

        // Deletes ID card information and employee data by card ID       
        //Returns 204 if evertyhing went well
        //Returns 400 if no employee was found by cardId
        // DELETE api/Employee/:id
        [HttpDelete("{cardId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Delete(string cardId) {
            var employee = _context.Employees.Find(cardId);
            
            if(employee == null) { return NotFound("No employee found with this card id"); }

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            return NoContent();
        }

        //Credit functions

        //Returns a current status of employees credit based on cardId
        //Returns 200 status code with decimal number of employees current credit
        //Returns 404 status code if cardId was not found in the database
        //GET api/Employee/credit/{cardId}
        [HttpGet("credit/{cardId}")]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetCredit(string cardID)
        {
            var employee = _context.Employees.Find(cardID);
            if (employee == null) return NotFound("User with this card id doesn't exist.");

            return Ok(employee.Credit);
        }

        //Replaces the employee's credit with new values
        //Returns 200 if employee's credit was successfully updated
        //Returns 400 if request body was filled in not in a right way
        //Returns 404 if employee was not found
        //Body request to have fields of class EmployeeInputData
        //PUT api/Employee
        [HttpPut("credit/{cardId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult PutCredit(string cardID, [FromBody]decimal newCreditAmount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = _context.Employees.Find(cardID);
            if (employee == null) return NotFound("User with this card id doesn't exist.");

            employee.Credit = newCreditAmount;

            _context.Employees.Update(employee);
            _context.SaveChanges();

            return Ok();
        }
    }
}
