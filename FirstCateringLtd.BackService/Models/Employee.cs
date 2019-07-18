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

        //Constructor used to create an Employee instance from EmployeeInputData that we recieve from client side
        public Employee(EmployeeInputData employeeInputData)
        {
            Update(employeeInputData);
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
        [MinLength(4)]
        [MaxLength(4)]
        public string PinNumber { get; private set; }

        //Login behind converting card ID string to pin code
        private string GetPinNumberFromCardId(string cardIdNumber)
        {
            //Takes 4 last digits of card id
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
