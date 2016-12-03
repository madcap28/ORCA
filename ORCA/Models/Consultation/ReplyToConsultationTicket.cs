using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models.Consultation
{
    public class ReplyToConsultationTicket
    {
        public int TicketID { get; set; }

        [Display(Name = "Enter Reply"), Required]
        public string ReplyText { get; set; }
    }
}