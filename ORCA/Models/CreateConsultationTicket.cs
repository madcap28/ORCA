using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models
{
    public class CreateConsultationTicket
    {
        [Required, Display(Name = "Short Description/Name"), MinLength(4), MaxLength(100)]
        public string DescriptionName { get; set; }
        
        [Required, Display(Name = "Details / Reason for Consultation")]
        public string DescriptionDetails { get; set; }
    }
}