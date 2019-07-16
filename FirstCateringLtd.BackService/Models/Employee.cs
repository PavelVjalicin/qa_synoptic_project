using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCatering.BackService.Models
{
	public class Employee : EmployeeNoPin 
	{

        public Employee() { }

        public Employee(EmployeeNoPin employeeNoPin)
        {
            UpdateFromEmployeeNoPin(this, employeeNoPin);
        }

        public void Update(EmployeeNoPin employeeNoPin)
        {
            UpdateFromEmployeeNoPin(this, employeeNoPin);
        }

        [Required]
        public string PinNumber { get; set; }

        private static void UpdateFromEmployeeNoPin(Employee employee,EmployeeNoPin employeeNoPin)
        {
            employee.CardIdNumber = employeeNoPin.CardIdNumber;
            employee.EmployeeId = employeeNoPin.EmployeeId;
            employee.Name = employeeNoPin.Name;
            employee.Email = employeeNoPin.Email;
            employee.MobileNumber = employeeNoPin.MobileNumber;
            var cardId = employeeNoPin.CardIdNumber;

            //Takes last 4 digits of cardID
            var pin = cardId.Substring(cardId.Length - 4);
            employee.PinNumber = pin;
        }
	}

    public class EmployeeNoPin
    {
        [Key]
        [Required]
        [MinLength(4)]
        [RegularExpression(@"^[a-zA-Z0-9]*$",ErrorMessage = "Invalid card id. Card ID can only consist of numbers and letters")]
        public string CardIdNumber { get; set; }

        [Required]
        public string EmployeeId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string MobileNumber { get; set; }
    }
}
