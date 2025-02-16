using Npgsql;
using HvP.examify_take_exam.DB.Common.Models;
using Microsoft.EntityFrameworkCore;

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

        public string GetConnectionString()
        {
            if (configModel == null)
            {
                return null;
            }

            return $"Host={configModel.Address};Port={configModel.Port};User ID={configModel.Username};Password={configModel.Password};Database={configModel.DatabaseName};";
            //return $"Host={configModel.Address};Port={configModel.Port};User ID={configModel.Username};Password={configModel.Password};Database={configModel.DatabaseName};Pooling=true;MinPoolSize=5;MaxPoolSize=100;Timeout=20;CommandTimeout=60;";
        }

        public void SetOptionBuilder(ref DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(GetConnectionString());
        }

        public DBConfigModel GetDBConfig()
        {
            return configModel;
        }
    }
}
