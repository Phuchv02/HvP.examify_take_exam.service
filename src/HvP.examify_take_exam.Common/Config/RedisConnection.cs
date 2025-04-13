using StackExchange.Redis;
using HvP.examify_take_exam.Common.Models;
using HvP.examify_take_exam.Common.Logger;

namespace HvP.examify_take_exam.Common.Config
{
    public class RedisConnection
    {
        private readonly ILoggerService<RedisConnection> _logger;
        private RedisConfigModel configModel;

        public RedisConnection(RedisConfigModel configModel, ILoggerService<RedisConnection> logger)
        {
            this._logger = logger;
            this.configModel = configModel;
        }

        public ConnectionMultiplexer GetConnection()
        {
            try
            {

                ConfigurationOptions option = new ConfigurationOptions()
                {
                    EndPoints = { { configModel.Address, configModel.Port } },
                    Password = configModel.Password,
                    User = configModel.Username,
                    DefaultDatabase = 0,
                    Ssl = false,
                };

                ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(option);

                this._logger.LogInformation($"*** TRY CONNECT REDIS Success ***: Address={configModel.Address}, Port = {configModel.Port}");

                return connection;
            }
            catch (RedisConnectionException ex)
            {
                this._logger.LogFatal($"*** TRY CONNECT REDIS Fail ***: Address={configModel.Address}, Port = {configModel.Port}");
                return null;
            }
        }
    }
}
