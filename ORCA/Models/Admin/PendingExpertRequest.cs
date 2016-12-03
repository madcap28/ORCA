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

        public ExpertStatus ExpertStatus { get; set; }

        public string OrcaUserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}