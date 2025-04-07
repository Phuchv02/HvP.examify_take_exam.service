using Npgsql;
using HvP.examify_take_exam.DB.Common.Models;
using Microsoft.EntityFrameworkCore;
using HvP.examify_take_exam.DB.Logger;

namespace HvP.DB.Common.Config
{
    public class PostgresDbConnection : IDBConnection
    {
        private NpgsqlConnection conn;
        private DBConfigModel configModel;
        private readonly ILoggerService<PostgresDbConnection> _logger;

        public PostgresDbConnection(DBConfigModel configModel, ILoggerService<PostgresDbConnection> logger)
        {
            this._logger = logger;
            this.configModel = configModel;
        }

        public string GetConnectionString()
        {
            if (configModel == null)
            {
                return null;
            }

            //return $"Host={configModel.Address};Port={configModel.Port};User ID={configModel.Username};Password={configModel.Password};Database={configModel.DatabaseName};";

            return $"Host={configModel.Address};Port={configModel.Port};User ID={configModel.Username};Password={configModel.Password};Database={configModel.DatabaseName};Pooling=true;MinPoolSize=5;MaxPoolSize=100;Timeout=20;CommandTimeout=60;";
        }

        public void SetOptionBuilder(ref DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                optionsBuilder.UseNpgsql(GetConnectionString());
            }
            catch (Exception ex)
            {
                this._logger.LogFatal($"*** TRY CONNECT POSTGRES Fail***: Address={configModel.Address}, Port = {configModel.Port}");
            }
        }

        public DBConfigModel GetDBConfig()
        {
            return configModel;
        }
    }
}
