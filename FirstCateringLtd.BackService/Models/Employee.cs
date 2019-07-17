using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCateringLtd.BackService.Models
{
	public class Employee : EmployeeInputData 
	{

        public Employee() { }

        public Employee(EmployeeInputData employeeNoPin)
        {
            Update(employeeNoPin);
            this.Credit = 0;
        }

        public void Update(EmployeeInputData employeeNoPin)
        {
            this.CardIdNumber = employeeNoPin.CardIdNumber;
            this.EmployeeId = employeeNoPin.EmployeeId;
            this.Name = employeeNoPin.Name;
            this.Email = employeeNoPin.Email;
            this.MobileNumber = employeeNoPin.MobileNumber;
            var cardId = employeeNoPin.CardIdNumber;

            //Takes last 4 digits of cardID
            var pin = this.GetPinNumber(cardId);
            this.PinNumber = pin;
        }

        [Required]
        public string PinNumber { get; private set; }

        private string GetPinNumber(string cardIdNumber)
        {
            var pin = cardIdNumber.Substring(cardIdNumber.Length - 4);
            return pin;
        }

        [Required]
        public decimal Credit { get; set; }
    }

    

    public class EmployeeInputData
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
