using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ORCA.DAL
{
    public class OrcaContext : DbContext
    {
        public OrcaContext() : base("OrcaContext")
        {
        }

        public DbSet<OrcaUser> OrcaUsers { get; set; }
        public DbSet<ExpertConsultant> ExpertConsultants { get; set; }
        public DbSet<ConsultantExpertise> ConsultantExpertises { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketEntry> TicketEntries { get; set; }
        public DbSet<TicketExpert> TicketExperts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}