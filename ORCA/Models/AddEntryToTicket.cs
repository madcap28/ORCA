using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models
{
    public class AddEntryToTicket
    {

        [Display(Name = "Ticket Number")]
        public int TicketID { get; set; }

        [Display(Name = "Created By")]
        public string OrcaUserName { get; set; }

        [Display(Name = "Date Created"), DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:g}", ConvertEmptyStringToNull = true, NullDisplayText = "MM-dd-yyyy hh:mm am/pm", ApplyFormatInEditMode = true)]
        public DateTime DTStamp { get; set; }

        [Required, Display(Name = "Description")]
        public string DescriptionName { get; set; }

        [Required, Display(Name = "Status")]
        public TicketStatus IsTicketOpen { get; set; }

        [Display(Name = "New Entry")]
        public string NewTicketEntry { get; set; }

    }
}