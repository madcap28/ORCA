using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models.Consultation
{
    public class ConsultsTicketForTicketList
    {

        // this is the ticket information to be displayed in a list



        [Key, Display(Name = "Ticket Number")]
        public int TicketID { get; set; }

        [Display(Name = "Created By")]
        public string OrcaUserName { get; set; }

        [Display(Name = "Date Created"), DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:g}", ConvertEmptyStringToNull = true, NullDisplayText = "MM-dd-yyyy hh:mm am/pm", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}