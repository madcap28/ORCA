using ORCA.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models
{
    public class ActiveExpert
    {
        public int OrcaUserID { get; set; }

        [Display(Name = "Expert/Consultant")]
        public string OrcaUserName { get; set; }

        [Display(Name = "Title/Degree")]
        public string TitleDegree { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Display(Name = "Expertise")]
        public string FieldOfExpertise { get; set; }
        
    }
}