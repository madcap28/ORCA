using ORCA.DAL;
using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models.Consultation
{
    public enum ConsultationTicketStatus { Open, Closed }


    public class ConsultationTicket
    {
        /*
         * 
         * Fields to Display  (They were entered when ticket was created)
         * 
         */
         
        
        [Display(Name = "Ticket Number"),Key]
        public int TicketID { get; set; }

        [Display(Name = "Created By")]
        public string OrcaUserName { get; set; }

        [Display(Name = "Date Created"), DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:g}", ConvertEmptyStringToNull = true, NullDisplayText = "MM-dd-yyyy hh:mm am/pm", ApplyFormatInEditMode = true)]
        public DateTime DTStamp { get; set; }

        [Display(Name = "Description")]
        public string DescriptionName { get; set; }

        [Display(Name = "Last Reply")]
        public int OrcaUserIDLastReplied { get; set; }
        
        private bool _TicketStatus_IsTicketOpen { get; set; }
        [Display(Name = "Status")]
        public ConsultationTicketStatus TicketStatus
        {
            get { return _TicketStatus_IsTicketOpen ? ConsultationTicketStatus.Open : ConsultationTicketStatus.Closed; }
            set
            {
                if (value == ConsultationTicketStatus.Open)
                {
                    _TicketStatus_IsTicketOpen = true;
                }
            }
        }

        
        ///*
        // * 
        // * Consturtors and Initializers
        // * 
        // */
        

        public ConsultationTicket(int? ticketId = null)
        {
            if (ticketId != null)
            {
                int tmp = (int)ticketId;
                InitPopulateConsultationTicket((int)ticketId);
            }
        }

        public ConsultationTicket InitPopulateConsultationTicket(int? ticketId)
        {
            OrcaContext db = new OrcaContext();
            

            Ticket ticket = db.Tickets.Find(ticketId);

            if (ticket != null)
            {
                this.TicketID = ticket.TicketID;
                this.OrcaUserName = ticket.OrcaUserCreator.OrcaUserName;
                this.DTStamp = ticket.DTStamp;
                this.DescriptionName = ticket.DescriptionName;
                this.OrcaUserIDLastReplied = ticket.OrcaUserIDLastReplied;
                this.TicketStatus = ticket.IsTicketOpen ? ConsultationTicketStatus.Open : ConsultationTicketStatus.Closed;
            }
            return this;
        }
        
    }

}