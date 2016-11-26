﻿using ORCA.DAL;
using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models
{
    public class ActiveExperts
    {
        [Display(Name = "Search Experts")]
        public string SearchField { get; set; }

        public List<ActiveExpert> Experts { get; set; }

        public ActiveExperts()
        {
            PopulateList();
        }



        public ActiveExperts FilterList(string searchString)
        {
            if (!(String.IsNullOrEmpty(searchString) || String.IsNullOrWhiteSpace(searchString)))
            {
                // not the best way to do it, but it will work for now
                OrcaContext db = new OrcaContext();

                List<ActiveExpert> expertsToKeep = new List<ActiveExpert>();// will contain the filtered list of experts

                foreach (var expert in Experts)
                {
                    bool keep = false;// don't keep the expert unless we find matching substring

                    // check their expertises for a match
                    var checkExpertises = from expertise in db.ConsultantExpertises
                                          where expertise.OrcaUserID == expert.OrcaUserID
                                          select expertise;

                    foreach (var chk in checkExpertises)
                    {
                        if (!String.IsNullOrEmpty(chk.FieldOfExpertise))
                        {
                            if (chk.FieldOfExpertise.ToLower().Contains(searchString.ToLower()))
                            {
                                keep = true;
                                break;
                            }
                        }
                    }
                    if (keep)
                    {
                        expertsToKeep.Add(expert);
                        continue;
                    }
                    
                    // search the experts keyword list
                    var expertConsultant = from exp in db.ExpertConsultants
                                           where exp.OrcaUserID == exp.OrcaUserID
                                           select exp;
                    foreach (var exp in expertConsultant)
                    {
                        if (!String.IsNullOrEmpty(exp.KeyWordList))
                        {
                            if (exp.KeyWordList.ToLower().Contains(searchString.ToLower()))
                            {
                                keep = true;
                                break;
                            }
                        }
                        if (!String.IsNullOrEmpty(exp.TitleDegree))
                        {
                            if (exp.TitleDegree.ToLower().Contains(searchString.ToLower()))
                            {
                                keep = true;
                                break;
                            }
                        }
                    }
                    if (keep)
                    {
                        expertsToKeep.Add(expert);
                        continue;
                    }
                }
                // set new list
                Experts = expertsToKeep;
            }
            else PopulateList();

            return this;
        }

        void PopulateList()
        {
            OrcaContext db = new OrcaContext();

            // THIS IS A NASTY HACK BUT IT WILL WORK FOR NOW, it assumes that an expert consultant has added an expertise, otherwise the consultant can not be found in the list
            // NOTE: I need to figure out how to do the following section properly. I believe it has to do with using join, but I don't know enough about it so this will suffice for now.
            List<ConsultantExpertise> consultantExpertises = db.ConsultantExpertises.ToList();

            Experts = new List<ActiveExpert>();

            foreach (var exp in consultantExpertises)
            {
                ExpertConsultant expertConsultant = db.ExpertConsultants.Find(exp.OrcaUserID);

                if (expertConsultant != null && expertConsultant.IsActive)
                {
                    OrcaUser orcaUser = db.OrcaUsers.Find(exp.OrcaUserID);
                    ActiveExpert activeExpert = new ActiveExpert();

                    activeExpert.OrcaUserID = exp.OrcaUserID;
                    activeExpert.OrcaUserName = orcaUser.OrcaUserName;
                    activeExpert.FirstName = orcaUser.FirstName;
                    activeExpert.LastName = orcaUser.LastName;
                    activeExpert.TitleDegree = expertConsultant.TitleDegree;
                    activeExpert.FieldOfExpertise = exp.FieldOfExpertise;

                    Experts.Add(activeExpert);
                }
            }
            // NOTE: Need to figure out how to do the above section properly. I believe it can and should be done with a join but I don't know enough about it and this will suffice for now.

            Experts = Experts.OrderBy(x => x.FieldOfExpertise).ToList();
        }

        public ActiveExperts SortListBy(SortBy sortBy, SortMethod sortMethod)
        {
            if (sortMethod == SortMethod.Ascending)
            {
                switch (sortBy)
                {
                    case SortBy.OrcaUserID:
                        //Experts.Sort((y, x) => x.OrcaUserID.CompareTo(y.OrcaUserID));
                        Experts = Experts.OrderBy(x => x.OrcaUserID).ToList();
                        break;
                    case SortBy.OrcaUserName:
                        //Experts.Sort((y, x) => x.OrcaUserName.CompareTo(y.OrcaUserName));
                        Experts = Experts.OrderBy(x => x.OrcaUserName).ToList();
                        break;
                    case SortBy.FirstName:
                        //Experts.Sort((y, x) => x.FirstName.CompareTo(y.FirstName));
                        Experts = Experts.OrderBy(x => x.FirstName).ToList();
                        break;
                    case SortBy.LastName:
                        //Experts.Sort((y, x) => x.LastName.CompareTo(y.LastName));
                        Experts = Experts.OrderBy(x => x.LastName).ToList();
                        break;
                    case SortBy.TitleDegree:
                        //Experts.Sort((y, x) => x.TitleDegree.CompareTo(y.TitleDegree));
                        Experts = Experts.OrderBy(x => x.TitleDegree).ToList();
                        break;
                    case SortBy.FieldOfExpertise:
                        //Experts.Sort((y, x) => x.FieldOfExpertise.CompareTo(y.FieldOfExpertise));
                        Experts = Experts.OrderBy(x => x.FieldOfExpertise).ToList();
                        break;
                    default:
                        //Experts.Sort((y, x) => x.FieldOfExpertise.CompareTo(y.FieldOfExpertise));
                        Experts = Experts.OrderBy(x => x.FieldOfExpertise).ToList();
                        break;
                }
            }
            else
            {
                switch (sortBy)
                {
                    case SortBy.OrcaUserID:
                        //Experts.Sort((y, x) => x.OrcaUserID.CompareTo(y.OrcaUserID));
                        Experts = Experts.OrderByDescending(x => x.OrcaUserID).ToList();
                        break;
                    case SortBy.OrcaUserName:
                        //Experts.Sort((y, x) => x.OrcaUserName.CompareTo(y.OrcaUserName));
                        Experts = Experts.OrderByDescending(x => x.OrcaUserName).ToList();
                        break;
                    case SortBy.FirstName:
                        //Experts.Sort((y, x) => x.FirstName.CompareTo(y.FirstName));
                        Experts = Experts.OrderByDescending(x => x.FirstName).ToList();
                        break;
                    case SortBy.LastName:
                        //Experts.Sort((y, x) => x.LastName.CompareTo(y.LastName));
                        Experts = Experts.OrderByDescending(x => x.LastName).ToList();
                        break;
                    case SortBy.TitleDegree:
                        //Experts.Sort((y, x) => x.TitleDegree.CompareTo(y.TitleDegree));
                        Experts = Experts.OrderByDescending(x => x.TitleDegree).ToList();
                        break;
                    case SortBy.FieldOfExpertise:
                        //Experts.Sort((y, x) => x.FieldOfExpertise.CompareTo(y.FieldOfExpertise));
                        Experts = Experts.OrderByDescending(x => x.FieldOfExpertise).ToList();
                        break;
                    default:
                        //Experts.Sort((y, x) => x.FieldOfExpertise.CompareTo(y.FieldOfExpertise));
                        Experts = Experts.OrderByDescending(x => x.FieldOfExpertise).ToList();
                        break;
                }
            }
            
            return this;
        }


        //// WARNING: THE FOLLOWING IS A HACK AND A HORRIBLY BAD IDEA, but I didn't think earlier and I'm too tired to think about how to do it properly
        //public ActiveExperts AddInactiveExpertsThatAreStillActiveOnTicket(int ticketID)
        //{
        //    OrcaContext db = new OrcaContext();

        //    List<TicketExpert> ticExp = (from tick in db.TicketExperts
        //                                 where tick.TicketActivityState != ActivityState.Inactive
        //                                 select tick).ToList();

        //    //foreach (var exp in ticExp.ToList())


        //    //foreach (var exp in expertConsultantQuery)
        //    //{
        //    //    if (db.ExpertConsultants.)

        //    //    ActiveExpert activeExpert = new ActiveExpert();

        //    //    activeExpert.OrcaUserID = exp.OrcaUserID;
        //    //    activeExpert.OrcaUserName = exp.OrcaUser.OrcaUserName;
        //    //    activeExpert.TitleDegree = exp.TitleDegree;
        //    //    activeExpert.FirstName = exp.OrcaUser.FirstName;
        //    //    activeExpert.LastName = exp.OrcaUser.LastName;
        //    //    activeExpert.FieldOfExpertise = db.ConsultantExpertises.Where(x => x.OrcaUserID == exp.OrcaUserID).ToList().Find(x => x.OrcaUserID == exp.OrcaUserID).FieldOfExpertise;

        //    return this;
        //}
    }
}