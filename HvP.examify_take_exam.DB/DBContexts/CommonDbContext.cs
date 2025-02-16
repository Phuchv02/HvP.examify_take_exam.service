using HvP.DB.Common.Config;
using HvP.examify_take_exam.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HvP.Database.DBContexts
{
    public class CommonDBContext : DbContext
    {
        public IDBConnection dbconnection;

        // DB Set
        public virtual DbSet<TakeExamEntity> TakeExams { get; set; }

        public CommonDBContext() { }

        public CommonDBContext(IDBConnection dbConnection)
        {
            this.dbconnection = dbConnection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            dbconnection.SetOptionBuilder(ref optionsBuilder);

            // # debug mode
            // optionsBuilder.EnableSensitiveDataLogging();
        }

        public static async Task<bool> CreateDatabase(IDBConnection dbConnection)
        {
            using (var dbcontext = new CommonDBContext(dbConnection))
            {
                bool result = await dbcontext.Database.EnsureCreatedAsync();
                return result;
            }
        }

        public static async Task<bool> DeleteDatabase(IDBConnection dbConnection)
        {
            using (var dbcontext = new CommonDBContext(dbConnection))
            {
                bool result = await dbcontext.Database.EnsureDeletedAsync();
                return result;
            }
        }
    }
}
