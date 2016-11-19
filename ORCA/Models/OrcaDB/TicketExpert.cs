using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ORCA.Models.OrcaDB
{
    public class TicketExpert
    {
        [Key]
        public int TicketExpertID { get; set; }

        [Required]//[ForeignKey("OrcaUserID")] [ForeignKey("ExpertConsultantID")]
        public int ExpertForThisTicket { get; set; }// This is a ForeignKey for the OrcaUser and ExpertConsultant tables. ExpertForThisTicket = ExpertConsultant.ExpertConsultantID = OrcaUser.OrcaUserID

        [ForeignKey("Ticket"), Required]
        public int TicketID { get; set; }// This is the TicketID of the Ticket that this expert has been added to as a consultant.

        [Required]
        public bool IsActiveOnTicket { get; set; }// Indicates that the ExpertConsultant is active and still allowed to post to this Ticket.  This can be changed by the ticket creator.





        // NOTE: ExpertForThisTicket = ExpertConsultant.ExpertConsultantID = OrcaUser.OrcaUserID
        [ForeignKey("ExpertForThisTicket")]
        public virtual OrcaUser OrcaUser { get; set; }// This points to the OrcaUser

        [ForeignKey("ExpertForThisTicket")]
        public virtual ExpertConsultant ExpertConsultant { get; set; }// This points to the ExpertConsultant

        [ForeignKey("TicketID")]
        public virtual Ticket Ticket { get; set; }// This is the Ticket that has added the TicketExpert to its list.

    }
}