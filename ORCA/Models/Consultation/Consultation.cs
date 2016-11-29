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

        public enum FilterTicketOption { All, Open, Closed, NewReply }
        public enum SortTicketOption { TicketID, OrcaUserName, OrcaUserIDLastReplied, DTStamp, DescriptionName, TicketStatus, TicketID_desc, OrcaUserName_desc, OrcaUserIDLastReplied_desc, DTStamp_desc, DescriptionName_desc, TicketStatus_desc }
        //private enum _FilterTicketOptions { All, Open, Closed, NewReply }


        // id of user viewing ticket
        public int OrcaUserID_or_TicketExpertID { get; set; }

        // type of user viewing ticket
        public OrcaUserType UserViewType { get; set; }

        //private FilterTicketOption _ButtonPressFilter { get; set; }
        [Display(Name = "Filter List"), EnumDataType(typeof(FilterTicketOption))]
        public FilterTicketOption FilterTicketsOption { get; set; }
        //{
        //    get { return _ButtonPressFilter; }
        //    set
        //    {
        //        _ButtonPressFilter = value;
        //        FilterConsultationTickets(value);
        //    }
        //}

        //private SortTicketOption _SortOption { get; set; }
        [Display(Name = "SortOptionSelection"), EnumDataType(typeof(SortTicketOption))]
        public SortTicketOption SortTicketsOption { get; set; }
        //{
        //    get { return _SortOption; }
        //    set
        //    {
        //        _SortOption = value;
        //        SortConsultationTickets(value);
        //    }
        //}

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
        public Consultation(int orcaUserID, OrcaUserType userViewType = OrcaUserType.Consultee, FilterTicketOption buttonPressFilter = FilterTicketOption.All, SortTicketOption sortOptionSelection = SortTicketOption.DTStamp_desc, string ticketSearchString = "")
        {
            Init(orcaUserID, userViewType, buttonPressFilter, sortOptionSelection, ticketSearchString);
        }

        public Consultation Init(int orcaUserID, OrcaUserType userViewType = OrcaUserType.Consultee, FilterTicketOption buttonPressFilter = FilterTicketOption.All, SortTicketOption sortOptionSelection = SortTicketOption.DTStamp_desc, string ticketSearchString = "")
        {
            this.UserViewType = userViewType;
            this.OrcaUserID_or_TicketExpertID = orcaUserID;
            this.FilterTicketsOption = buttonPressFilter;
            this.SortTicketsOption = sortOptionSelection;
            this.TicketSearchString = ticketSearchString;
System.Diagnostics.Debug.WriteLine(" Consultation.Init  (before RefreshConsultationTickets");

            this.ConsultationTickets = new List<ConsultationTicket>();


            RefreshConsultationTickets();

            return this;
        }

        public Consultation RefreshConsultationTickets()
        {

            // should take the code from here and put it in FilterConsultationTickets instead

            OrcaContext db = new OrcaContext();
            
            List<Ticket> tickets = new List<Ticket>();// the list of tickets need to display, taken from database
System.Diagnostics.Debug.WriteLine("Inside RefreshConsultationTickets   (before switch)");
            ////////////////////////     SELECT TICKETS FROM DATABASE       /////////////////////////////
            switch (UserViewType)
            {
                /////////////////////////////////////////////////   Consultee   //////////////////////////////////////////
                case OrcaUserType.Consultee:
                    switch (FilterTicketsOption)
                    {
                        case FilterTicketOption.All:
                            tickets = db.Tickets.Where(x => 
                                    x.OrcaUserID == OrcaUserID_or_TicketExpertID).ToList();
                            break;
                        case FilterTicketOption.Open:
                            tickets = db.Tickets.Where(x => 
                                    x.OrcaUserID == OrcaUserID_or_TicketExpertID && 
                                    x.IsTicketOpen).ToList();
                            break;
                        case FilterTicketOption.Closed:
                            tickets = db.Tickets.Where(x => x.OrcaUserID == OrcaUserID_or_TicketExpertID && !(x.IsTicketOpen)).ToList();
                            break;
                        case FilterTicketOption.NewReply:
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
                        case FilterTicketOption.Open:// this follows the paths   Tickets.ITEM   &&   Tickets -> TicketExperts.ITEM
                            tickets = db.Tickets.Where(x => 
                                    x.IsTicketOpen && 
                                    x.TicketExperts.Where(y => y.ExpertForThisTicket == OrcaUserID_or_TicketExpertID).FirstOrDefault().ExpertForThisTicket == OrcaUserID_or_TicketExpertID).ToList();
                            break;
                        case FilterTicketOption.Closed:// this follows the paths   Tickets.ITEM   &&   Tickets -> TicketExperts.ITEM
                            tickets = db.Tickets.Where(x =>
                                    !x.IsTicketOpen &&
                                    x.TicketExperts.Where(y => y.ExpertForThisTicket == OrcaUserID_or_TicketExpertID).FirstOrDefault().ExpertForThisTicket == OrcaUserID_or_TicketExpertID).ToList();
                            break;
                        case FilterTicketOption.NewReply:// this follows the paths   Tickets.ITEM   &&   Tickets.ITEM   &&   Tickets -> TicketExperts.ITEM
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
System.Diagnostics.Debug.WriteLine("Inside RefreshConsultationTickets   (after switch)");


            // Create and clear the list 

            this.ConsultationTickets = new List<ConsultationTicket>();


System.Diagnostics.Debug.WriteLine("Inside RefreshConsultationTickets   (before foreach)");


            // Now create ConsultationTicket objects for all of the Ticket objects in the list
            foreach (Ticket tic in tickets)
            {
                //// Alternative ways to do it?
                //ConsultationTicket conTicToAdd = new ConsultationTicket(tic.TicketID);
                //ConsultationTicket conTicToAdd = new ConsultationTicket().InitPopulateConsultationTicket(tic.TicketID);
                ConsultationTicket conTicToAdd = new ConsultationTicket();
                conTicToAdd.InitPopulateConsultationTicket(tic.TicketID);

                this.ConsultationTickets.Add(conTicToAdd);// add the ConsultationTicket to the ConsultationTickets list
            }


System.Diagnostics.Debug.WriteLine("Inside RefreshConsultationTickets   (before foreach)");


            return this;
        }
        
        public Consultation SortConsultationTickets(SortTicketOption sortOption)
        {
            ConsultationTicket tic = new ConsultationTicket();
            List<ConsultationTicket> tmp = new List<ConsultationTicket>();
            
            switch (sortOption)
            {
                case SortTicketOption.TicketID:
                    this.ConsultationTickets.OrderBy(x => x.TicketID).ToList();
                    break;
                case SortTicketOption.OrcaUserName:
                    this.ConsultationTickets.OrderBy(x => x.OrcaUserName).ToList();
                    break;
                case SortTicketOption.OrcaUserIDLastReplied:
                    this.ConsultationTickets.OrderBy(x => x.OrcaUserIDLastReplied).ToList();
                    break;
                case SortTicketOption.DTStamp:
                    this.ConsultationTickets.OrderBy(x => x.DTStamp).ToList();
                    break;
                case SortTicketOption.DescriptionName:
                    this.ConsultationTickets.OrderBy(x => x.DescriptionName).ToList();
                    break;
                case SortTicketOption.TicketStatus:
                    this.ConsultationTickets.OrderBy(x => x.TicketStatus).ToList();
                    break;
                case SortTicketOption.TicketID_desc:
                    this.ConsultationTickets.OrderByDescending(x => x.TicketID).ToList();
                    break;
                case SortTicketOption.OrcaUserName_desc:
                    this.ConsultationTickets.OrderByDescending(x => x.OrcaUserName).ToList();
                    break;
                case SortTicketOption.OrcaUserIDLastReplied_desc:
                    this.ConsultationTickets.OrderByDescending(x => x.OrcaUserIDLastReplied).ToList();
                    break;
                case SortTicketOption.DTStamp_desc:
                    this.ConsultationTickets.OrderByDescending(x => x.DTStamp).ToList();
                    break;
                case SortTicketOption.DescriptionName_desc:
                    this.ConsultationTickets.OrderByDescending(x => x.DescriptionName).ToList();
                    break;
                case SortTicketOption.TicketStatus_desc:
                    this.ConsultationTickets.OrderByDescending(x => x.TicketStatus).ToList();
                    break;
                default:
                    this.ConsultationTickets.OrderByDescending(x => x).ToList();
                    break;
            }
            return this;
        }

        public Consultation FilterConsultationTickets(FilterTicketOption filterTicketOption)
        {
            // should take the code from RefreshConsultationTickets and put it here and call this from there instead

            this.FilterTicketsOption = filterTicketOption;

            RefreshConsultationTickets();

            return this;
        }

    }
}




