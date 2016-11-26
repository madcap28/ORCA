using ORCA.DAL;
using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models
{
    public class EditConsultationTicket
    {
        public int TicketID { get; set; }
        
        public string OrcaUserName { get; set; }

        [Required, Display(Name = "Date Created"), DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:g}", ConvertEmptyStringToNull = true, NullDisplayText = "MM-dd-yyyy hh:mm am/pm", ApplyFormatInEditMode = true)]
        public DateTime DTStamp { get; set; }

        [Required, Display(Name = "Description")]
        public string DescriptionName { get; set; }

        [Required, Display(Name = "Status")]
        public TicketStatus IsTicketOpen { get; set; }

        public List<TicketEntry> TicketEntries { get; set; }

        public List<TicketExpert> CurrentTicketExperts { get; set; }


        public List<TicketExpert> ExpertsToAdd { get; set; }


        public EditConsultationTicket() { }
        public EditConsultationTicket(int ticketID)
        {
            OrcaContext db = new OrcaContext();

            Ticket ticket = db.Tickets.Find(ticketID);

            if (ticket != null)
            {
                TicketID = ticketID;
                OrcaUserName = db.OrcaUsers.Find().OrcaUserName;
                //OrcaUserName = ticket.OrcaUserCreator.OrcaUserName; // should test this to see if it is created
                DTStamp = ticket.DTStamp;

                if (ticket.IsTicketOpen) IsTicketOpen = TicketStatus.Open;
                else IsTicketOpen = TicketStatus.Closed;

                TicketEntries = (from entry in db.TicketEntries
                                 where entry.TicketID == ticketID
                                 orderby entry.EntryDTStamp descending
                                 select entry).ToList();

                CurrentTicketExperts = (from expert in db.TicketExperts
                                        where expert.TicketID == TicketID
                                        select expert).ToList();

                //ExpertsToAdd = 

            }
        }

        public EditConsultationTicket UpdateTicket()
        {

            return this;
        }

        
        

    }
}