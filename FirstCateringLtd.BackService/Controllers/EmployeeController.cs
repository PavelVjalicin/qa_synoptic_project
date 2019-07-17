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

        DatabaseContext _context;
        public EmployeeController(DatabaseContext context)
        {
            _context = context;
        }

        private bool cardIdIsValid(string cardId)
        {
            Regex r = new Regex("^[a-zA-Z0-9]*$");
            return r.IsMatch(cardId);
        }

        // GET api/IdCard
        // Returns array of all employees
        [HttpGet]
        [ProducesResponseType(typeof(List<Employee>), 200)]
        public IActionResult Get()
        {
            return Ok(_context.Employees.ToList());
        }



        // Returns an employee from cardId

        // Returns 200 Status code with employee data
        // Returns 404 status code with error message if employee not found
        // GET api/IdCard/:id
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
        // Returns 400 Status code if employee with this cardID already exists.
        // POST api/IdCard/
        // Body must contain EmployeeNoPin class fields.
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody]EmployeeInputData employeeNoPin)
        {   
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingEmployee = _context.Employees.Find(employeeNoPin.CardIdNumber);

            if (existingEmployee != null) return BadRequest("Employee with this card id already exists");

            var employee = new Employee(employeeNoPin);

            _context.Employees.Add(employee);

            _context.SaveChanges();

            return Ok();
        }

        
        //Replaces the employee with new values
        //PUT api/IdCard
        //Body request to have fields of class EmployeeNoPin
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Put( [FromBody] EmployeeInputData employeeNoPin)
        { 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _context.Employees.Find(employeeNoPin.CardIdNumber);

            if(employee == null)
            {
                return NotFound("No employee found with this card id");
            }

            employee.Update(employeeNoPin);

            _context.Employees.Update(employee);
            _context.SaveChanges();

            return Ok();
        }

        // Deletes ID card information and employee data by card ID        
        // DELETE api/IdCard/:id
        [HttpDelete("{cardId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Delete(string cardId) {
            var employee = _context.Employees.Find(cardId);
            
            if(employee == null) { return NotFound("No employee found with this card id"); }

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            return NoContent();
        }

        //Credit functions

        [HttpGet("credit/{cardID}")]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetCredit(string cardID)
        {
            var employee = _context.Employees.Find(cardID);
            if (employee == null) return NotFound("User with this card id doesn't exist.");

            return Ok(employee.Credit);
        }

        [HttpPut("credit/{cardID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult PutCredit(string cardID, [FromBody]decimal newCreditAmount)
        {
            var employee = _context.Employees.Find(cardID);
            if (employee == null) return NotFound("User with this card id doesn't exist.");

            employee.Credit = newCreditAmount;

            _context.Employees.Update(employee);
            _context.SaveChanges();

            return Ok();
        }
    }
}
