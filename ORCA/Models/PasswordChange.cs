using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models
{
    public class PasswordChange
    {
        [Required,
            Display(Name = "Current Password"),
            DataType(DataType.Password),
            MinLength(8, ErrorMessage = "Please ensure that your password is at least 8 characters.")]
        public string CurrentPassword { get; set; }

        [Required,
            Display(Name = "New Password"),
            DataType(DataType.Password),
            MinLength(8, ErrorMessage = "Please ensure that your password is at least 8 characters.")]
        public string Password { get; set; }

        [Required,
            Display(Name = "Confirm New Password"),
            DataType(DataType.Password),
            Compare("Password", ErrorMessage = "This does not match the New Password you entered above.")]
        public string ConfirmPassword { get; set; }

    }
}