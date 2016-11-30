using ORCA.DAL;
using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models.Consultation
{

    public class Consultation
    {

        public enum TicketFilterOption { All, Open, Closed, NewReply }
        public enum TicketSortOption { TicketID, OrcaUserName, OrcaUserIDLastReplied, DTStamp, DescriptionName, TicketStatus,
            TicketID_desc, OrcaUserName_desc, OrcaUserIDLastReplied_desc, DTStamp_desc, DescriptionName_desc, TicketStatus_desc }

        // id of user viewing ticket
        public int? OrcaUserID_or_TicketExpertID { get; set; }

        // type of user view (as a consultee or consultant)
        public OrcaUserType UserViewType { get; set; }
        
        [Display(Name = "Filter List"), EnumDataType(typeof(TicketFilterOption))]
        public TicketFilterOption FilterTicketsOption { get; set; }
        
        [Display(Name = "SortOptionSelection"), EnumDataType(typeof(TicketSortOption))]
        public TicketSortOption SortTicketsOption { get; set; }

        [Display(Name = "Search Tickets")]
        public string TicketSearchString { get; set; }

        [Display(Name = "Consultation Tickets")]
        public List<ConsultationTicket> ConsultationTickets { get; set; }


        ///*
        // * 
        // * Consturtors and Initializers
        // * 
        // */

        public Consultation() { }
        
        public Consultation(int orcaUserID_or_TicketExpertID, OrcaUserType userViewType, TicketFilterOption? FilterTicketsOption = TicketFilterOption.All, TicketSortOption? SortTicketsOption = TicketSortOption.DTStamp_desc, string TicketSearchString = "")
        {
            Init(orcaUserID_or_TicketExpertID, userViewType, FilterTicketsOption, SortTicketsOption, TicketSearchString);
        }

        public Consultation(Consultation consultationModel)
        {
            Init(consultationModel);
        }

        public Consultation(Consultation consultationModel, int? orcaUserID_or_TicketExpertID = null, OrcaUserType? userViewType = null, TicketFilterOption? FilterTicketsOption = TicketFilterOption.All, TicketSortOption? SortTicketsOption = TicketSortOption.DTStamp_desc, string TicketSearchString = "")
        {
            Init(consultationModel, orcaUserID_or_TicketExpertID, userViewType, FilterTicketsOption, SortTicketsOption, TicketSearchString);
        }

        public void Init(Consultation consultationModel, int? orcaUserID_or_TicketExpertID = null, OrcaUserType? userViewType = null, TicketFilterOption? FilterTicketsOption = TicketFilterOption.All, TicketSortOption? SortTicketsOption = TicketSortOption.DTStamp_desc, string TicketSearchString = "")
        {
            if (orcaUserID_or_TicketExpertID != null) this.OrcaUserID_or_TicketExpertID = orcaUserID_or_TicketExpertID;
            else this.OrcaUserID_or_TicketExpertID = this.OrcaUserID_or_TicketExpertID = consultationModel.OrcaUserID_or_TicketExpertID;

            if (userViewType != null) this.UserViewType = UserViewType;
            else this.UserViewType = consultationModel.UserViewType;

            if (FilterTicketsOption != null) this.FilterTicketsOption = (TicketFilterOption)FilterTicketsOption;
            else this.FilterTicketsOption = consultationModel.FilterTicketsOption;

            if (SortTicketsOption != null) this.SortTicketsOption = (TicketSortOption)SortTicketsOption;
            else this.SortTicketsOption = consultationModel.SortTicketsOption;

            if (!String.IsNullOrEmpty(TicketSearchString)) this.TicketSearchString = TicketSearchString;
            else this.TicketSearchString = consultationModel.TicketSearchString;
            
            RefreshConsultationTickets();
        }

        public void Init(int? orcaUserID_or_TicketExpertID = null, OrcaUserType? userViewType = null, TicketFilterOption? FilterTicketsOption = TicketFilterOption.All, TicketSortOption? SortTicketsOption = TicketSortOption.DTStamp_desc, string TicketSearchString = "")
        {
            if (orcaUserID_or_TicketExpertID != null) this.OrcaUserID_or_TicketExpertID = orcaUserID_or_TicketExpertID;
            if (userViewType != null) this.UserViewType = UserViewType;
            if (FilterTicketsOption != null) this.FilterTicketsOption = (TicketFilterOption)FilterTicketsOption;
            if (SortTicketsOption != null) this.SortTicketsOption = (TicketSortOption)SortTicketsOption;
            this.TicketSearchString = TicketSearchString;
            
            RefreshConsultationTickets();
        }





        public Consultation FilterConsultationTickets(TicketFilterOption? filterTicketOption)
        {
            if (filterTicketOption == null) this.FilterTicketsOption = TicketFilterOption.All;
            else this.FilterTicketsOption = (TicketFilterOption)filterTicketOption;
            
            RefreshConsultationTickets();

            return this;
        }

        public Consultation SortConsultationTickets(TicketSortOption? sortOption, bool? toggleIfSame = false)
        {
            if (sortOption == null) this.SortTicketsOption = TicketSortOption.DTStamp_desc;
            else this.SortTicketsOption = (TicketSortOption)sortOption;

            ConsultationTicket tic = new ConsultationTicket();
            List<ConsultationTicket> tmp = new List<ConsultationTicket>();


            if (toggleIfSame != null)
                if (this.SortTicketsOption == sortOption)// sorting if value passed is equal to current value
                {
                    if ((int)this.SortTicketsOption <= 6)// enum option is currently ascending
                        this.SortTicketsOption += 6;// change to descending
                    else this.SortTicketsOption -= 6;// otherwise it is descending so change to ascending
                }


            switch (SortTicketsOption)
            {
                case TicketSortOption.TicketID:
                    this.ConsultationTickets.OrderBy(x => x.TicketID).ToList();
                    break;
                case TicketSortOption.OrcaUserName:
                    this.ConsultationTickets.OrderBy(x => x.OrcaUserName).ToList();
                    break;
                case TicketSortOption.OrcaUserIDLastReplied:
                    this.ConsultationTickets.OrderBy(x => x.OrcaUserIDLastReplied).ToList();
                    break;
                case TicketSortOption.DTStamp:
                    this.ConsultationTickets.OrderBy(x => x.DTStamp).ToList();
                    break;
                case TicketSortOption.DescriptionName:
                    this.ConsultationTickets.OrderBy(x => x.DescriptionName).ToList();
                    break;
                case TicketSortOption.TicketStatus:
                    this.ConsultationTickets.OrderBy(x => x.TicketStatus).ToList();
                    break;
                case TicketSortOption.TicketID_desc:
                    this.ConsultationTickets.OrderByDescending(x => x.TicketID).ToList();
                    break;
                case TicketSortOption.OrcaUserName_desc:
                    this.ConsultationTickets.OrderByDescending(x => x.OrcaUserName).ToList();
                    break;
                case TicketSortOption.OrcaUserIDLastReplied_desc:
                    this.ConsultationTickets.OrderByDescending(x => x.OrcaUserIDLastReplied).ToList();
                    break;
                case TicketSortOption.DTStamp_desc:
                    this.ConsultationTickets.OrderByDescending(x => x.DTStamp).ToList();
                    break;
                case TicketSortOption.DescriptionName_desc:
                    this.ConsultationTickets.OrderByDescending(x => x.DescriptionName).ToList();
                    break;
                case TicketSortOption.TicketStatus_desc:
                    this.ConsultationTickets.OrderByDescending(x => x.TicketStatus).ToList();
                    break;
                default:
                    this.ConsultationTickets.OrderByDescending(x => x).ToList();
                    break;
            }

           
            return this;
        }

        public Consultation RefreshConsultationTickets()
        {
            OrcaContext db = new OrcaContext();
            
            
            List<Ticket> tickets = new List<Ticket>();// the list of tickets need to display, taken from database
            //System.Diagnostics.Debug.WriteLine("Inside RefreshConsultationTickets   (before switch)");
            ////////////////////////     SELECT TICKETS FROM DATABASE       /////////////////////////////
            switch (UserViewType)
            {
                /////////////////////////////////////////////////   Consultee   //////////////////////////////////////////
                case OrcaUserType.Consultee:
                    switch (FilterTicketsOption)
                    {
                        case TicketFilterOption.All:
                            tickets = db.Tickets.Where(x => 
                                    x.OrcaUserID == OrcaUserID_or_TicketExpertID).ToList();
                            break;
                        case TicketFilterOption.Open:
                            tickets = db.Tickets.Where(x => 
                                    x.OrcaUserID == OrcaUserID_or_TicketExpertID && 
                                    x.IsTicketOpen).ToList();
                            break;
                        case TicketFilterOption.Closed:
                            tickets = db.Tickets.Where(x => x.OrcaUserID == OrcaUserID_or_TicketExpertID && !(x.IsTicketOpen)).ToList();
                            break;
                        case TicketFilterOption.NewReply:
                            tickets = db.Tickets.Where(x => x.OrcaUserID == OrcaUserID_or_TicketExpertID && x.IsTicketOpen && x.OrcaUserIDLastReplied == OrcaUserID_or_TicketExpertID).ToList();
                            break;
                        default:
                            tickets = db.Tickets.Where(x => x.OrcaUserID == OrcaUserID_or_TicketExpertID).ToList();
                            break;
                    }
                    break;

                    ////////////////////////////////////////////////////   Consultants   ////////////////////////////////////////////////
                case OrcaUserType.Consultant:
                case OrcaUserType.ConsultantAdmin:
                    switch (FilterTicketsOption)
                    {
                        //case FilterOption.All:// this follows the path   Tickets -> TicketExperts.ITEM
                        //    tickets = db.Tickets.Where(x => 
                        //            x.TicketExperts.Where(y => y.ExpertForThisTicket == OrcaUserID_or_TicketExpertID).FirstOrDefault().ExpertForThisTicket == OrcaUserID_or_TicketExpertID).ToList();
                        //    break;
                        case TicketFilterOption.Open:// this follows the paths   Tickets.ITEM   &&   Tickets -> TicketExperts.ITEM
                            tickets = db.Tickets.Where(x => 
                                    x.IsTicketOpen && 
                                    x.TicketExperts.Where(y => y.ExpertForThisTicket == OrcaUserID_or_TicketExpertID).FirstOrDefault().ExpertForThisTicket == OrcaUserID_or_TicketExpertID).ToList();
                            break;
                        case TicketFilterOption.Closed:// this follows the paths   Tickets.ITEM   &&   Tickets -> TicketExperts.ITEM
                            tickets = db.Tickets.Where(x =>
                                    !x.IsTicketOpen &&
                                    x.TicketExperts.Where(y => y.ExpertForThisTicket == OrcaUserID_or_TicketExpertID).FirstOrDefault().ExpertForThisTicket == OrcaUserID_or_TicketExpertID).ToList();
                            break;
                        case TicketFilterOption.NewReply:// this follows the paths   Tickets.ITEM   &&   Tickets.ITEM   &&   Tickets -> TicketExperts.ITEM
                            tickets = db.Tickets.Where(x =>
                                    x.IsTicketOpen &&
                                    x.OrcaUserIDLastReplied != OrcaUserID_or_TicketExpertID &&
                                    x.TicketExperts.Where(y => y.ExpertForThisTicket == OrcaUserID_or_TicketExpertID).FirstOrDefault().ExpertForThisTicket == OrcaUserID_or_TicketExpertID).ToList();
                            break;
                        default:// this follows the path   Tickets -> TicketExperts.ITEM
                            tickets = db.Tickets.Where(x =>
                                    x.TicketExperts.Where(y => y.ExpertForThisTicket == OrcaUserID_or_TicketExpertID).FirstOrDefault().ExpertForThisTicket == OrcaUserID_or_TicketExpertID).ToList();
                            break;
                    }
                    break;
                default:
                    break;
            }
            //////////////////////////////////////////         END SELECT TICKETS FROM DATABASE  //////////////////////
            
            // Create and clear the list 

            this.ConsultationTickets = new List<ConsultationTicket>();
            
            // Now create ConsultationTicket objects for all of the Ticket objects in the list
            foreach (Ticket tic in tickets)
            {
                ConsultationTicket conTicToAdd = new ConsultationTicket(tic.TicketID);
                conTicToAdd.InitPopulateConsultationTicket(tic.TicketID);

                this.ConsultationTickets.Add(conTicToAdd);// add the ConsultationTicket to the ConsultationTickets list
            }
            
            return this;
        }
        


    }
}
























//public Consultation() { }
//public Consultation(int? orcaUserID_or_TicketExpertID = null, OrcaUserType? userViewType = null, TicketFilterOption? FilterTicketsOption = TicketFilterOption.All, TicketSortOption? SortTicketsOption = TicketSortOption.DTStamp_desc)
//{

//}

//public Consultation(string searchString, int? orcaUserID_or_TicketExpertID = null, OrcaUserType? userViewType = null, TicketFilterOption? FilterTicketsOption = TicketFilterOption.All, TicketSortOption? SortTicketsOption = TicketSortOption.DTStamp_desc)
//{

//}

//public Consultation(Consultation consultationModel, int? orcaUserID_or_TicketExpertID = null, OrcaUserType? userViewType = null, TicketFilterOption? FilterTicketsOption = TicketFilterOption.All, TicketSortOption? SortTicketsOption = TicketSortOption.DTStamp_desc)
//{

//}

//public Consultation(Consultation consultationModel, string searchString, int? orcaUserID_or_TicketExpertID = null, OrcaUserType? userViewType = null, TicketFilterOption? FilterTicketsOption = TicketFilterOption.All, TicketSortOption? SortTicketsOption = TicketSortOption.DTStamp_desc)
//{

//}


