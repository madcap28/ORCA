using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ORCA.Models.OrcaDB
{
    public class TicketEntry
    {
        [Key]
        public int TicketEntryID { get; set; }

        [ForeignKey("Ticket"), Required]
        public int TicketID { get; set; }// This is the TicketID of the ticket this TicketEntry is on.  TicketID = Ticket.TicketID

        [ForeignKey("OrcaUser"), Required]
        public int OrcaUserID { get; set; }// This is the OrcaUserID of the creator of this TicketEntry.  OrcaUserID = OrcaUser.OrcaUserID

        [Required]
        public DateTime EntryDTStamp { get; set; }

        [Required]
        public string EntryText { get; set; }// This is the EntryText that the creator enters for the Ticket.




        [ForeignKey("TicketID")]
        public virtual Ticket Ticket { get; set; }// This points to the Ticket that this TicketEntry is on.

        [ForeignKey("OrcaUserID")]
        public virtual OrcaUser OrcaUser { get; set; }// This points to the OrcaUser that created this TicketEntry. 
    }
}