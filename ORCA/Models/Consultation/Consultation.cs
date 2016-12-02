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
        public enum UserAccessType { NonRegistered, Consultee, Consultant }




        public enum TicketFilterOption { All, Open, Closed, NewReply }




        // NOTE: If additional options are added, they must maintain order correspondence between ascending descending (all ascending are listed first, then all descending are listed in the same order) and TicketID always remain an the first position
        public enum TicketSortOption
        {
            TicketID, OrcaUserName, OrcaUserNameLastReplied, DTStamp, DescriptionName, TicketStatus,
            TicketID_desc, OrcaUserName_desc, OrcaUserNameLastReplied_desc, DTStamp_desc, DescriptionName_desc, TicketStatus_desc
        }




        // id of user viewing ticket
        public int OrcaUserID_or_TicketExpertID { get; set; }




        // type of user view (as a consultee or consultant)
        public UserAccessType UserViewAccess { get; set; }




        // FILTER :NOTE: when setting FilterTicketsOption, the ConsultationTickets list will be updated
        private TicketFilterOption _FilterTicketsOption;
        private bool _FilterTicketsOptionTripFlag;
        [Display(Name = "Filter List"), EnumDataType(typeof(TicketFilterOption))]
        public TicketFilterOption FilterTicketsOption
        {
            get { return _FilterTicketsOption; }
            set
            {
                if (_FilterTicketsOptionTripFlag) { _FilterTicketsOptionTripFlag = false; }// prevent infinite loop, 
                else
                {
                    _FilterTicketsOptionTripFlag = true;
                    _FilterTicketsOption = value;

                    RefreshConsultationTickets();
                }
            }
        }




        // SORT  :NOTE: when SortTicketsOption, the ConsultationTickets list will be updated
        private TicketSortOption _SortTicketsOption;
        [Display(Name = "SortOptionSelection"), EnumDataType(typeof(TicketSortOption))]
        public TicketSortOption SortTicketsOption
        {
            get { return _SortTicketsOption; }
            set
            {
                _SortTicketsOption = value;
                RefreshConsultationTickets();
            }
        }




        // SEARCH
        private string _TicketSearchString;
        [Display(Name = "Search Tickets")]
        public string TicketSearchString
        {

            get { return _TicketSearchString; }
            set
            {
                this._TicketSearchString = value;
                RefreshConsultationTickets();
            }

        }




        [Display(Name = "Consultation Tickets")]
        public List<ConsultationTicket> ConsultationTickets { get; set; }









        ////private void FilterConsultationTickets()
        ////{
        ////    OrcaContext db = new OrcaContext();

        ////    List<ConsultationTicket> newConsultationTickets = new List<ConsultationTicket>();// will replcase current list




        ////    // first create a list of consultation tickets depending on user view // this will become newConsultationTickets
        ////    switch (this.UserViewAccess)
        ////    {
        ////        case UserAccessType.Consultee:

        ////            // Tickets that have OrcaUserID = OrcOrcaUserID_or_TicketExpertID
        ////            //              db.Tickets.Where(x => x.OrcaUserID == OrcaUserID_or_TicketExpertID)
        ////            List<Ticket> ticketsCreatedByUser = new List<Ticket>();
        ////            List<Ticket> tickets = db.Tickets.Where(x => x.OrcaUserID == OrcaUserID_or_TicketExpertID).ToList();

        ////            // create a corresponding ConsultationTicket and add it to newConsulationTickets
        ////            foreach (Ticket tic in tickets)
        ////            {
        ////                ConsultationTicket consultationTicket = new ConsultationTicket(tic.TicketID);// create ConsulationTicket
        ////                if (!newConsultationTickets.Contains(consultationTicket))// make sure not to add duplicates
        ////                    newConsultationTickets.Add(consultationTicket);// add it to newConsulationTickets
        ////            }
        ////            break;
        ////        case UserAccessType.Consultant:
        ////            // TicketExperts that have ExpertForThisTicket = OrcOrcaUserID_or_TicketExpertID
        ////            //              db.TicketExperts.Where(x => x.ExpertForThisTicket == OrcaUserID_or_TicketExpertID)
        ////            List<TicketExpert> ticketExpertsWhereUserIsConsultingOnTicket = new List<TicketExpert>();
        ////            List<TicketExpert> experts = db.TicketExperts.Where(x => x.ExpertForThisTicket == OrcaUserID_or_TicketExpertID).ToList();

        ////            foreach (TicketExpert tex in experts)
        ////            {
        ////                ConsultationTicket consultationTicket = new ConsultationTicket(tex.TicketID);// create ConsultationTicket

        ////                if (!newConsultationTickets.Contains(consultationTicket))// make sure not to add duplicates
        ////                    newConsultationTickets.Add(consultationTicket);// add it to newConsultationTickets
        ////            }
        ////            break;
        ////    }

        ////    this.ConsultationTickets = newConsultationTickets;

        ////}

        public Consultation RefreshConsultationTickets()
        {
            //           FilterConsultationTickets();
            //           SortConsultationTickets();
            //           SearchConsultationTickets();


            OrcaContext db = new OrcaContext();

            List<Ticket> tickets = new List<Ticket>();// the list of tickets that will replace ConsultationTickets


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Now filter list
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            ////////////////////////     SELECT TICKETS FROM DATABASE       /////////////////////////////
            switch (UserViewAccess)
            {
                /////////////////////////////////////////////////   Consultee   //////////////////////////////////////////
                case UserAccessType.Consultee:
                    switch (FilterTicketsOption)
                    {
                        //case TicketFilterOption.All:// all tickets created by user
                        //    tickets = db.Tickets.Where(x =>
                        //            x.OrcaUserID == OrcaUserID_or_TicketExpertID).ToList();
                        //    break;

                        case TicketFilterOption.Open:// tickets created by user && tickets that are open
                            tickets = db.Tickets.Where(x =>
                                    x.OrcaUserID == OrcaUserID_or_TicketExpertID &&
                                    x.IsTicketOpen).ToList();
                            break;

                        case TicketFilterOption.Closed:// tickets created by user && tickets that are not open
                            tickets = db.Tickets.Where(x => x.OrcaUserID == OrcaUserID_or_TicketExpertID && !(x.IsTicketOpen)).ToList();
                            break;

                        case TicketFilterOption.NewReply:// tickets created by users && tickets that are open && tickets where last reply was not the creator
                            tickets = db.Tickets.Where(x => x.OrcaUserID == OrcaUserID_or_TicketExpertID && x.IsTicketOpen && x.OrcaUserIDLastReplied != OrcaUserID_or_TicketExpertID).ToList();
                            break;

                        default:// all tickets created by user
                            tickets = db.Tickets.Where(x => x.OrcaUserID == OrcaUserID_or_TicketExpertID).ToList();
                            break;
                    }
                    break;

                ////////////////////////////////////////////////////   Consultants   ////////////////////////////////////////////////
                case UserAccessType.Consultant:
                    switch (FilterTicketsOption)
                    {
                        case TicketFilterOption.All:// this follows the path   Tickets -> TicketExperts.ITEM
                            tickets = db.Tickets.Where(x =>
                                    x.TicketExperts.Where(y => y.ExpertForThisTicket == OrcaUserID_or_TicketExpertID).FirstOrDefault().ExpertForThisTicket == OrcaUserID_or_TicketExpertID).ToList();
                            break;

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

            //  We have the tickets now search them
            ConsultationTickets = new List<ConsultationTicket>();

            // Now create ConsultationTicket objects for all of the Ticket objects in the list
            foreach (Ticket tic in tickets)
            {
                ConsultationTicket conTicToAdd = new ConsultationTicket(tic.TicketID);
                conTicToAdd.InitPopulateConsultationTicket(tic.TicketID);

                ConsultationTickets.Add(conTicToAdd);// add the ConsultationTicket to the ConsultationTickets list
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // List is filtered
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Now sort list
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            switch (SortTicketsOption)
            {
                case TicketSortOption.TicketID:
                    this.ConsultationTickets.OrderBy(x => x.TicketID).ToList();
                    break;
                case TicketSortOption.OrcaUserName:
                    this.ConsultationTickets.OrderBy(x => x.OrcaUserName).ToList();
                    break;
                case TicketSortOption.OrcaUserNameLastReplied:
                    this.ConsultationTickets.OrderBy(x => x.OrcaUserNameLastReplied).ToList();
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
                case TicketSortOption.OrcaUserNameLastReplied_desc:
                    this.ConsultationTickets.OrderByDescending(x => x.OrcaUserNameLastReplied).ToList();
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

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // List is sorted
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Now search list and discard non-matches
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            List<ConsultationTicket> consultationTicketsToKeep = new List<ConsultationTicket>();// this will be the new ConsultationTickets list

            //if (this.UserViewAccess = UserAccessType.Consultee)
            foreach (ConsultationTicket ticket in ConsultationTickets)
            {
                int tid = ticket.TicketID;

                // search description name of ticket
                if (db.Tickets.Find(tid).DescriptionName.Contains(this.TicketSearchString))
                {
                    consultationTicketsToKeep.Add(ticket);
                    continue;
                }

                // search ticket entries
                var ticketEntriesQuery = from tick in db.TicketEntries
                                         where tick.TicketID == ticket.TicketID
                                         select tick;

                foreach (var tick in ticketEntriesQuery)
                {
                    consultationTicketsToKeep.Add(ticket);
                    break;
                }
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // List is searched and only contains matches
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            return this;
        }







        /////*
        //// * 
        //// * Consturtors and Initializers
        //// * 
        //// */

        public Consultation() { }

        public Consultation(int OrcaUserID_or_TicketExpertID, UserAccessType UserViewAccess)
        {
            this.OrcaUserID_or_TicketExpertID = OrcaUserID_or_TicketExpertID;
            this.UserViewAccess = UserViewAccess;
            this._FilterTicketsOption = TicketFilterOption.All;
            this._SortTicketsOption = TicketSortOption.DTStamp_desc;
            this._TicketSearchString = "";

            this.ConsultationTickets = new List<ConsultationTicket>();
            this.RefreshConsultationTickets();
        }

        













        //public void Init(int? orcaUserID_or_TicketExpertID = null, UserAccessType? userViewType = null, TicketFilterOption? FilterTicketsOption = TicketFilterOption.All, TicketSortOption? SortTicketsOption = TicketSortOption.DTStamp_desc, string TicketSearchString = "")
        //{
        //    if (orcaUserID_or_TicketExpertID != null) this.OrcaUserID_or_TicketExpertID = orcaUserID_or_TicketExpertID;
        //    if (userViewType != null) this.UserViewAccess = UserViewAccess;


        //    if (FilterTicketsOption != null)
        //        this.FilterTicketsOption = (TicketFilterOption)FilterTicketsOption;



        //    if (SortTicketsOption != null) this.SortTicketsOption = (TicketSortOption)SortTicketsOption;
        //    this.TicketSearchString = TicketSearchString;

        //    RefreshConsultationTickets();
        //}








        //public Consultation(int orcaUserID_or_TicketExpertID, UserAccessType userViewType, TicketFilterOption? FilterTicketsOption = TicketFilterOption.All, TicketSortOption? SortTicketsOption = TicketSortOption.DTStamp_desc, string TicketSearchString = "")
        //{
        //    Init(orcaUserID_or_TicketExpertID, userViewType, FilterTicketsOption, SortTicketsOption, TicketSearchString);
        //}

        //public Consultation(Consultation consultationModel)
        //{
        //    Init(consultationModel);
        //}

        //public Consultation(Consultation consultationModel, int? orcaUserID_or_TicketExpertID = null, OrcaUserType? userViewType = null, TicketFilterOption? FilterTicketsOption = TicketFilterOption.All, TicketSortOption? SortTicketsOption = TicketSortOption.DTStamp_desc, string TicketSearchString = "")
        //{
        //    Init(consultationModel, orcaUserID_or_TicketExpertID, userViewType, FilterTicketsOption, SortTicketsOption, TicketSearchString);
        //}




        //public void Init(Consultation consultationModel, int? orcaUserID_or_TicketExpertID = null, UserAccessType? userViewType = null, TicketFilterOption? FilterTicketsOption = TicketFilterOption.All, TicketSortOption? SortTicketsOption = TicketSortOption.DTStamp_desc, string TicketSearchString = "")
        //{
        //    if (orcaUserID_or_TicketExpertID != null) this.OrcaUserID_or_TicketExpertID = orcaUserID_or_TicketExpertID;
        //    else this.OrcaUserID_or_TicketExpertID = this.OrcaUserID_or_TicketExpertID = consultationModel.OrcaUserID_or_TicketExpertID;

        //    if (userViewType != null) this.UserViewAccess = UserViewAccess;
        //    else this.UserViewAccess = consultationModel.UserViewAccess;

        //    if (FilterTicketsOption != null) this.FilterTicketsOption = (TicketFilterOption)FilterTicketsOption;
        //    else this.FilterTicketsOption = consultationModel.FilterTicketsOption;

        //    if (SortTicketsOption != null) this.SortTicketsOption = (TicketSortOption)SortTicketsOption;
        //    else this.SortTicketsOption = consultationModel.SortTicketsOption;

        //    if (!String.IsNullOrEmpty(TicketSearchString)) this.TicketSearchString = TicketSearchString;
        //    else this.TicketSearchString = consultationModel.TicketSearchString;

        //    RefreshConsultationTickets();
        //}

        //public void Init(int? orcaUserID_or_TicketExpertID = null, UserAccessType? userViewType = null, TicketFilterOption? FilterTicketsOption = TicketFilterOption.All, TicketSortOption? SortTicketsOption = TicketSortOption.DTStamp_desc, string TicketSearchString = "")
        //{
        //    if (orcaUserID_or_TicketExpertID != null) this.OrcaUserID_or_TicketExpertID = orcaUserID_or_TicketExpertID;
        //    if (userViewType != null) this.UserViewAccess = UserViewAccess;


        //    if (FilterTicketsOption != null)
        //        this.FilterTicketsOption = (TicketFilterOption)FilterTicketsOption;



        //    if (SortTicketsOption != null) this.SortTicketsOption = (TicketSortOption)SortTicketsOption;
        //    this.TicketSearchString = TicketSearchString;

        //    RefreshConsultationTickets();
        //}

    }


}























//        public Consultation SortConsultationTickets(TicketSortOption? sortOption, bool? toggleIfSame = false)
//        {
//            if (sortOption == null) this.SortTicketsOption = TicketSortOption.DTStamp_desc;
//            else this.SortTicketsOption = (TicketSortOption)sortOption;

//            ConsultationTicket tic = new ConsultationTicket();
//            List<ConsultationTicket> tmp = new List<ConsultationTicket>();

//            if (toggleIfSame != null)
//                if (this.SortTicketsOption == sortOption)// sorting if value passed is equal to current value
//                {
//                    if ((int)this.SortTicketsOption <= 6)// enum option is currently ascending
//                        this.SortTicketsOption += 6;// change to descending
//                    else this.SortTicketsOption -= 6;// otherwise it is descending so change to ascending
//                }

//            switch (SortTicketsOption)
//            {
//                case TicketSortOption.TicketID:
//                    this.ConsultationTickets.OrderBy(x => x.TicketID).ToList();
//                    break;
//                case TicketSortOption.OrcaUserName:
//                    this.ConsultationTickets.OrderBy(x => x.OrcaUserName).ToList();
//                    break;
//                case TicketSortOption.OrcaUserIDLastReplied:
//                    this.ConsultationTickets.OrderBy(x => x.OrcaUserIDLastReplied).ToList();
//                    break;
//                case TicketSortOption.DTStamp:
//                    this.ConsultationTickets.OrderBy(x => x.DTStamp).ToList();
//                    break;
//                case TicketSortOption.DescriptionName:
//                    this.ConsultationTickets.OrderBy(x => x.DescriptionName).ToList();
//                    break;
//                case TicketSortOption.TicketStatus:
//                    this.ConsultationTickets.OrderBy(x => x.TicketStatus).ToList();
//                    break;
//                case TicketSortOption.TicketID_desc:
//                    this.ConsultationTickets.OrderByDescending(x => x.TicketID).ToList();
//                    break;
//                case TicketSortOption.OrcaUserName_desc:
//                    this.ConsultationTickets.OrderByDescending(x => x.OrcaUserName).ToList();
//                    break;
//                case TicketSortOption.OrcaUserIDLastReplied_desc:
//                    this.ConsultationTickets.OrderByDescending(x => x.OrcaUserIDLastReplied).ToList();
//                    break;
//                case TicketSortOption.DTStamp_desc:
//                    this.ConsultationTickets.OrderByDescending(x => x.DTStamp).ToList();
//                    break;
//                case TicketSortOption.DescriptionName_desc:
//                    this.ConsultationTickets.OrderByDescending(x => x.DescriptionName).ToList();
//                    break;
//                case TicketSortOption.TicketStatus_desc:
//                    this.ConsultationTickets.OrderByDescending(x => x.TicketStatus).ToList();
//                    break;
//                default:
//                    this.ConsultationTickets.OrderByDescending(x => x).ToList();
//                    break;
//            }

//            return this;
//        }
//    }
//}

    

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


