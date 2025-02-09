using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data.Common;
using System.Data;
using HvP.examify_take_exam.DB.Common.Models;

namespace HvP.DB.Common.Config
{
    public class PostgresDbConnection : IDBConnection
    {
        private NpgsqlConnection conn;

        private DBConfigModel configModel;

        public PostgresDbConnection(DBConfigModel configModel)
        {
            this.configModel = configModel;
        }

        public static string GetConnectionString(DBConfigModel configModel)
        {
            if (configModel == null)
            {
                return null;
            }

            return $"Host={configModel.Address};Port={configModel.Port};User ID={configModel.Username};Password={configModel.Password};Database={configModel.DatabaseName};";
        }

        public string GetConnectionString()
        {
            if (configModel == null)
            {
                return null;
            }

            return $"Host={configModel.Address};Port={configModel.Port};User ID={configModel.Username};Password={configModel.Password};Database={configModel.DatabaseName};";
        }

        public IDbConnection GetConnection()
        {
            conn = new NpgsqlConnection(GetConnectionString());
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", isEnabled: true);
            return conn;
        }

        public DbTransaction BeginTransaction()
        {
            return conn.BeginTransaction();
        }

        public void SetOptionBuilder(ref DbContextOptionsBuilder optionsBuilder)
        {
            GetConnectionString();
            optionsBuilder.UseNpgsql(GetConnectionString());
        }

        public DBConfigModel GetDBConfig()
        {
            return configModel;
        }
    }
}
