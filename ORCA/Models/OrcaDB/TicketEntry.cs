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
        [Key, Required]
        public int TicketEntryID { get; set; }

        [ForeignKey("OrcaUser"), Required]
        public int OrcaUserID { get; set; }// This is the OrcaUserID of the creator of this TicketEntry.  OrcaUserID = OrcaUser.OrcaUserID

        [Required]
        public DateTime DTStamp { get; set; }

        [Required]
        public string EntryText { get; set; }// This is the EntryText that the creator enters for the Ticket.





        [ForeignKey("OrcaUserID")]
        public OrcaUser OrcaUser { get; set; }// This points to the OrcaUser that created this TicketEntry. 
    }
}