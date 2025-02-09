using HvP.DB.Common.Config;
using Microsoft.EntityFrameworkCore;

namespace HvP.Database.DBContexts
{
    public class CommonDBContext : DbContext
    {
        public IDBConnection dbconnection;

        // DB Set
        //public virtual DbSet<UserEntity> I_Users { get; set; }

        public CommonDBContext() { }

        public CommonDBContext(IDBConnection dbConnection)
        {
            this.dbconnection = dbConnection;
        }

        public CommonDBContext(DbContextOptions<CommonDBContext> options)
        : base(options) { }


        public IDBConnection GetConnection()
        {
            return (IDBConnection)dbconnection.GetConnection();
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
