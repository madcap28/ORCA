using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models.Consultation
{
    public class ConsultTicketEntryForTicketEntryList
    {
        [Key]
        public int TicketEntryID { get; set; }

        [Display(Name = "Ticket Number")]
        public int TicketID { get; set; }

        [Display(Name = "Posted By")]
        public string OrcaUserNameCreator { get; set; }

        public int OrcaUserID { get; set; }

        [Display(Name = "Date Created"), DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:g}", ConvertEmptyStringToNull = true, NullDisplayText = "MM-dd-yyyy hh:mm am/pm", ApplyFormatInEditMode = true)]
        public DateTime EntryDTStamp { get; set; }

        [Display(Name = "Message Posted")]
        public string EntryText { get; set; }

    }
}