using ORCA.DAL;
using System;
using System.ComponentModel.DataAnnotations;

namespace ORCA.Models
{
    public class ConsultationEntry
    {
        [Display(Name = "Posted By")]
        public string OrcaUserName { get; set; }

        [Display(Name = "Date Created"), DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:g}", ConvertEmptyStringToNull = true, NullDisplayText = "MM-dd-yyyy hh:mm am/pm", ApplyFormatInEditMode = true)]
        public DateTime EntryDTStamp { get; set; }

        [Display(Name = "Entry Text")]
        public string EntryText { get; set; }

        public ConsultationEntry(int ticketEntryId)
        {
            OrcaContext db = new OrcaContext();

            OrcaUserName = db.TicketEntries.Find(ticketEntryId).OrcaUser.OrcaUserName;
            EntryDTStamp = db.TicketEntries.Find(ticketEntryId).EntryDTStamp;
            EntryText = db.TicketEntries.Find(ticketEntryId).EntryText;
        }
    }

    
}