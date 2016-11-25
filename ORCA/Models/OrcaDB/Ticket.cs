using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ORCA.Models.OrcaDB
{
    public class Ticket
    {
        [Key, Display(Name = "Ticket Number")]
        public int TicketID { get; set; }

        [ForeignKey("OrcaUserCreator"), Required]
        public int OrcaUserID { get; set; }// This is the OrcaUserID of the creator of this Ticket. OrcaUserID = OrcaUser.OrcaUserID

        [ForeignKey("OrcaUserLastReplied")]
        public int OrcaUserIDLastReplied { get; set; }// This is the OrcaUserID of the last person to reply on the ticket

        [Required, Display(Name = "Date Created"), DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:g}", ConvertEmptyStringToNull = true, NullDisplayText = "MM-dd-yyyy hh:mm am/pm", ApplyFormatInEditMode = true)]
        public DateTime DTStamp { get; set; }// This is the Date/Time Stamp when the Ticket is created

        [Required, Display(Name = "Description")]
        public string DescriptionName { get; set; }// This is a short description of what the OrcaUser is seeking advice about on this ticket

        [Required, Display(Name = "Status")]
        public bool IsTicketOpen { get; set; }// Indicates if Ticket is open or closed



        [ForeignKey("OrcaUserID")]
        public virtual OrcaUser OrcaUserCreator { get; set; }// This points to the OrcaUser that created this Ticket.

        [ForeignKey("OrcaUserIDLastReplied")]
        public virtual OrcaUser OrcaUserLastReplied { get; set; }// This points to the OrcaUser that last replied to this ticket.

        public virtual ICollection<TicketEntry> TicketEntries { get; set; }// This points to a list of all of the TicketEntries for this Ticket.
        public virtual ICollection<TicketExpert> TicketExperts { get; set; }// This points to a list of all of the TicketExperts for this Ticket.
    }
}