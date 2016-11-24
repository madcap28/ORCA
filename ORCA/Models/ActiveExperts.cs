using ORCA.DAL;
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
        public SortBy CurrentSortBy { get; set; }
        public bool IsAscendingSort { get; set; }

        [Display(Name = "Search Experts")]
        public string SearchField { get; set; }

        public List<ActiveExpert> Experts { get; set; }

        public ActiveExperts()
        {
            OrcaContext db = new OrcaContext();


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
    }
}