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


        [Display(Name = "Current Consultants")]
        public ActiveExperts CurrentTicketExperts { get; set; }

        public ActiveExperts ExpertsToAdd { get; set; }

        [Display(Name = "Entries & Replies")]
        public List<ConsultationEntry> TicketEntries { get; set; }

        



        public EditConsultationTicket() { }
        public EditConsultationTicket(int ticketID)
        {
            OrcaContext db = new OrcaContext();

            Ticket ticket = db.Tickets.Find(ticketID);

            if (ticket != null)
            {

                System.Diagnostics.Debug.WriteLine("EditContultationTicket.cs");
                System.Diagnostics.Debug.WriteLine("ticket is not null");

                TicketID = ticketID;
                OrcaUserName = db.OrcaUsers.Find(ticket.OrcaUserID).OrcaUserName;
                DTStamp = ticket.DTStamp;
                DescriptionName = ticket.DescriptionName;
                
                if (ticket.IsTicketOpen) IsTicketOpen = TicketStatus.Open;
                else IsTicketOpen = TicketStatus.Closed;




                CurrentTicketExperts = new ActiveExperts();
                
                CurrentTicketExperts = CurrentTicketExperts.RemoveExpertsNotActiveOnTicket(ticketID);

                System.Diagnostics.Debug.WriteLine("After RemoveExpertsNotActiveOnTicket  CurrentTicketExperts.Experts.Count = " + CurrentTicketExperts.Experts.Count);


                //ExpertsToAdd = new ActiveExperts();
                //ExpertsToAdd.AddInactiveExpertsThatAreStillActiveOnTicket(ticketID);

                TicketEntries = new List<ConsultationEntry>();

                foreach(var entry in db.Tickets.Find(ticketID).TicketEntries)
                {
                    TicketEntries.Add(new ConsultationEntry(entry.TicketEntryID));
                }
                TicketEntries.OrderByDescending(x => x.EntryDTStamp);
            }
        }
        

    }
}