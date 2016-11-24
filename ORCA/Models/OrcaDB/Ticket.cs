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
        [Key]
        public int TicketID { get; set; }

        [ForeignKey("OrcaUser"), Required]
        public int OrcaUserID { get; set; }// This is the OrcaUserID of the creator of this Ticket. OrcaUserID = OrcaUser.OrcaUserID

        [Required]
        public DateTime DTStamp { get; set; }// This is the Date/Time Stamp when the Ticket is created

        [Required]
        public string DescriptionName { get; set; }// This is a short description of what the OrcaUser is seeking advice about on this ticket

        [Required]
        public bool IsTicketOpen { get; set; }// Indicates if Ticket is open or closed



        [ForeignKey("OrcaUserID")]
        public virtual OrcaUser OrcaUser { get; set; }// This points to the OrcaUser that created this Ticket.

        public virtual ICollection<TicketEntry> TicketEntries { get; set; }// This points to a list of all of the TicketEntries for this Ticket.
        public virtual ICollection<TicketExpert> TicketExperts { get; set; }// This points to a list of all of the TicketExperts for this Ticket.
    }
}