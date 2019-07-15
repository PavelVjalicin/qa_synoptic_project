using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstCatering.BackService.Data;
using FirstCatering.BackService.Models;
using System.Text.RegularExpressions;

namespace FirstCatering.BackService.Controllers
{

    [Route("api/[controller]")]
    public class IdCardController : Controller
    {

        private bool cardIdIsValid(string cardId)
        {
            Regex r = new Regex("^[a-zA-Z0-9]*$");
            return r.IsMatch(cardId);
        }

        DataBaseContext _context;
        public IdCardController(DataBaseContext context)
        {
            _context = context;
        }

        // Returns json array of all employees
        //GET api/IdCard
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Employees.ToList());
        }

        // Returns 200 Status code with employee data
        // Returns 400 status code with error message if employee not found
        // GET api/IdCard/:id
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var result = _context.Employees.Find(id);
            if(result != null)
            {
                return Ok(result);
            } else
            {
                return BadRequest("No employee with this card id");
            }
        }

        // Add an employee with credentials.
        // Returns 400 Status code if employee with this cardID already exists.
        // POST api/IdCard
        [HttpPost]
        public IActionResult Post([FromBody]EmployeeNoPin employeeNoPin)
        {   
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!cardIdIsValid(employeeNoPin.CardIdNumber)) {
                return BadRequest("Invalid card id. Card ID can only consist of numbers and letters");
            }

            var existingEmployee = _context.Employees.Find(employeeNoPin.CardIdNumber);

            if (existingEmployee != null) return BadRequest("Employee with this card id already exists");

            var cardId = employeeNoPin.CardIdNumber;

            var pin = cardId.Substring(cardId.Length - 4);

            var employee = new Employee(employeeNoPin,pin);

            _context.Employees.Add(employee);

            _context.SaveChanges();

            return Ok();
        }

        // Deletes ID card information and employee data by card ID
        // DELETE api/IdCard/:id
        [HttpDelete("{id}")]
        public IActionResult Delete(string id) {
            var employee = _context.Employees.Find(id);

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            return NoContent();
        }
	}
}
