using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models.Admin
{
    public class PendingExpertRequest
    {
        [Key]
        public int OrcaUserID { get; set; }

        [Display(Name = "Expert Status")]
        public ExpertStatus ExpertStatus { get; set; }

        [Display(Name = "User Name")]
        public string OrcaUserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress), Display(Name = "Email Address"), EmailAddress]
        public string Email { get; set; }

        [Phone, DataType(DataType.PhoneNumber), Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}