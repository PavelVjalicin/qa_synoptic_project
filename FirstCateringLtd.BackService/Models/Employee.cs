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

        //Function that updates the employee based on employee input data
        public void Update(EmployeeInputData employeeInputData)
        {
            this.CardIdNumber = employeeInputData.CardIdNumber;
            this.EmployeeId = employeeInputData.EmployeeId;
            this.Name = employeeInputData.Name;
            this.Email = employeeInputData.Email;
            this.MobileNumber = employeeInputData.MobileNumber;
            var cardId = employeeInputData.CardIdNumber;

            //Takes last 4 digits of cardID
            var pin = this.GetPinNumberFromCardId(cardId);
            this.PinNumber = pin;
        }

        [Required]
        public string PinNumber { get; private set; }

        private string GetPinNumberFromCardId(string cardIdNumber)
        {
            var pin = cardIdNumber.Substring(cardIdNumber.Length - 4);
            return pin;
        }

        [Required]
        public decimal Credit { get; set; }
    }

    

    public class EmployeeInputData
    {
        //Card id is varified by regular expression to contain only alphanumeric values.
        [Key]
        [Required]
        [MinLength(4)]
        [RegularExpression(@"^[a-zA-Z0-9]*$",
            ErrorMessage = "Invalid card id. Card ID can only consist of numbers and letters")]
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
