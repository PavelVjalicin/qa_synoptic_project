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

        public Employee(EmployeeNoPin employeeNoPin, string pin)
        {
            this.CardIdNumber = employeeNoPin.CardIdNumber;
            this.EmployeeId = employeeNoPin.EmployeeId;
            this.Name = employeeNoPin.Name;
            this.Email = employeeNoPin.Email;
            this.MobileNumber = employeeNoPin.MobileNumber;
            this.PinNumber = pin;
        }

        [Required]
        public string PinNumber { get; set; }

	}

    public class EmployeeNoPin
    {
        [Key]
        [Required]
        [MinLength(4)]
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
