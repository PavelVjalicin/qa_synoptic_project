using FirstCatering.BackService.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCateringLtd.BackService.Controllers
{
    [Route("api/[controller]")]
    public class CreditController : Controller
    {
        DataBaseContext _context;
        public CreditController(DataBaseContext context)
        {
            _context = context;
        }

        [HttpGet("{cardID}")]
        [ProducesResponseType(typeof(decimal),200)]
        [ProducesResponseType(404)]
        public IActionResult Get(string cardID)
        {
            var employee = _context.Employees.Find(cardID);
            if (employee == null) return NotFound("User with this card id doesn't exist.");

            return Ok(employee.Credit);
        }

        [HttpPut("{cardID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Put(string cardID,[FromBody]decimal newCreditAmount)
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
