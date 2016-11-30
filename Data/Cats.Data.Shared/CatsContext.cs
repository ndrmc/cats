using Cats.Models.Shared.DashBoardModels;
using System.Data.Entity;

namespace Cats.Data
{
    public partial class CatsContext : DbContext
    {
        static CatsContext()
        {
            Database.SetInitializer<CatsContext>(null);
        }

        public CatsContext() : base("Name=CatsContext") { }
        public DbSet<DashboardDataEntry> DashboardWidgets { get; set; }

        // TODO: Add properties to access set of Poco classes

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DashboardDataEntryMap());
        }
    }
}
