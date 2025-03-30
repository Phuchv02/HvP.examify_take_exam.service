using StackExchange.Redis;
using HvP.examify_take_exam.DB.Common.Models;

namespace HvP.DB.Common.Config
{
    public class RedisConnection
    {

        private RedisConfigModel configModel;

        public RedisConnection(RedisConfigModel configModel)
        {
            this.configModel = configModel;
        }

        public ConnectionMultiplexer GetConnection()
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

            return connection;
        }
    }
}
