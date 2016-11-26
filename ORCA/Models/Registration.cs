using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models
{
    public class Registration
    {
        [Required, Display(Name = "User Name")]
        public string OrcaUserName { get; set; }
        
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress), Display(Name = "Email Address"), EmailAddress]
        public string Email { get; set; }

        // NOTE:
        // NOTE: Must take care of proper phone number formatting later
        // NOTE:
        [Phone, DataType(DataType.PhoneNumber), Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required,
            Display(Name = "Password"),
            DataType(DataType.Password),
            MinLength(8, ErrorMessage = "Please ensure that your password is at least 8 characters.")]
        public string Password { get; set; }

        [Required,
            Display(Name = "Confirm Password"),
            DataType(DataType.Password),
            Compare("Password", ErrorMessage = "This does not match the Password above. Please retype your password.")]
        public string ConfirmPassword { get; set; }

    }
}