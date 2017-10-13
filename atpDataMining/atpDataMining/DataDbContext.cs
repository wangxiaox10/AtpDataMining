using System.Data.Entity;
using SQLite.CodeFirst;

namespace atpDataMining
{
    public class DataDbContext : DbContext
    {
        public DataDbContext() : base("DataDbContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<DataDbContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }

        public DbSet<Player> Players { get; set; }
    }
}
