using ORCA.DAL;
using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORCA.Models
{
    public enum ConsultationTicketSelection { All, Open, Closed, NewReply, None }
    
    public class UserConsultations
    {

        public int OrcaUserID { get; set; }// This is the OrcaUserID of the consultation ticket creator

        public string SearchField { get; set; }

        public List<UserConsultation> ConsultationTickets { get; set; }
        
        public UserConsultations(int ticketCreatorOrcaUserID, ConsultationTicketSelection selection)
        {
            OrcaContext db = new OrcaContext();

            // NOTE: I need to figure out how to do the following section properly. I believe it has to do with using join, but I don't know enough about it so this will suffice for now.
            List<Ticket> tickets = (from tic in db.Tickets
                                    where tic.OrcaUserID == OrcaUserID
                                    select tic).ToList();

            ConsultationTickets = new List<UserConsultation>();

            foreach (var tic in tickets)
            {
                UserConsultation consultation = new UserConsultation();

                consultation.TicketID = tic.TicketID;
                consultation.OrcaUserIDLastReplied = tic.OrcaUserIDLastReplied;
                consultation.DTStamp = tic.DTStamp;
                consultation.DescriptionName = tic.DescriptionName;

                if (tic.IsTicketOpen) { consultation.Status = TicketStatus.Open; }
                else { consultation.Status = TicketStatus.Closed; }

                //OrcaUser lastRepliedUser = db.OrcaUsers.Find(tic.OrcaUserIDLastReplied);
                //consultation.OrcaUserNameLastReplied = lastRepliedUser.OrcaUserName;
                consultation.OrcaUserNameLastReplied = db.OrcaUsers.Find(tic.OrcaUserIDLastReplied).OrcaUserName;

                switch (selection)
                {
                    case ConsultationTicketSelection.Open:
                        if (tic.IsTicketOpen) { ConsultationTickets.Add(consultation); }
                        break;
                    case ConsultationTicketSelection.Closed:
                        if (!tic.IsTicketOpen) { ConsultationTickets.Add(consultation); }
                        break;
                    case ConsultationTicketSelection.NewReply:
                        if (tic.OrcaUserIDLastReplied != OrcaUserID) { ConsultationTickets.Add(consultation); }
                        break;
                    default:// case ConsultationTicketSelection.All:
                        ConsultationTickets.Add(consultation);
                        break;
                }
            }
            // NOTE: Need to figure out how to do the above section properly. I believe it can and should be done with a join but I don't know enough about it and this will suffice for now.

            ConsultationTickets = ConsultationTickets.OrderBy(x => x.TicketID).ToList();
        }

        public UserConsultations SortListBy(SortBy sortBy, SortMethod sortMethod)
        {
            if (sortMethod == SortMethod.Ascending)
            {
                switch (sortBy)
                {
                    case SortBy.DTStamp:
                        ConsultationTickets = ConsultationTickets.OrderBy(x => x.DTStamp).ToList();
                        break;
                    case SortBy.DescriptionName:
                        ConsultationTickets = ConsultationTickets.OrderBy(x => x.DescriptionName).ToList();
                        break;
                    case SortBy.IsTicketOpen:
                        ConsultationTickets = ConsultationTickets.OrderByDescending(x => x.Status).ToList();
                        break;
                    default:// case SortBy.TicketID
                        ConsultationTickets = ConsultationTickets.OrderBy(x => x.TicketID).ToList();
                        break;
                }
            }
            else
            {
                switch (sortBy)
                {
                    case SortBy.DTStamp:
                        ConsultationTickets = ConsultationTickets.OrderByDescending(x => x.DTStamp).ToList();
                        break;
                    case SortBy.DescriptionName:
                        ConsultationTickets = ConsultationTickets.OrderByDescending(x => x.DescriptionName).ToList();
                        break;
                    case SortBy.IsTicketOpen:
                        ConsultationTickets = ConsultationTickets.OrderBy(x => x.Status).ToList();
                        break;
                    default:// case SortBy.TicketID:
                        ConsultationTickets = ConsultationTickets.OrderByDescending(x => x.TicketID).ToList();
                        break;
                }
            }

            return this;
        }
    }
}