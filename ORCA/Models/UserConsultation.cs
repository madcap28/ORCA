using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models
{
    public enum TicketStatus { Open, Closed }

    public class UserConsultation
    {
        [Display(Name = "Ticket Number")]
        public int TicketID { get; set; }

        [Required, Display(Name = "Description")]
        public string DescriptionName { get; set; }

        [Required, Display(Name = "Date Created"), DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:g}", ConvertEmptyStringToNull = true, NullDisplayText = "MM-dd-yyyy hh:mm am/pm", ApplyFormatInEditMode = true)]
        public DateTime DTStamp { get; set; }

        public int OrcaUserIDLastReplied { get; set; }

        public string OrcaUserNameLastReplied { get; set; }
        
        [Required, Display(Name = "Status")]
        public TicketStatus Status { get; set; }
    }
}