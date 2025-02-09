using HvP.examify_take_exam.DB.Common.Models;

namespace HvP.DB.Common.Config
{
    public class EnvConfig
    {
        // get env by key
        public static string GetEnvData(string configKey)
        {
            var rs = Environment.GetEnvironmentVariable(configKey);
            if (rs == null)
            {
                DotNetEnv.Env.Load();
                rs = Environment.GetEnvironmentVariable(configKey);
            }
            if (rs == null)
            {
                throw new Exception($"Get config from ENV error, ConfigKey: {configKey}");
            }
            return rs;
        }

        // Common
        public static string ENV
        {
            get { return GetEnvData("ENV"); }
        }

        public static string VERSION
        {
            get { return GetEnvData("VERSION"); }
        }
        public static string ApiKey
        {
            get { return GetEnvData("API_KEY"); }
        }

        // Database
        public static DBConfigModel MasterDatabaseConfig
        {
            get
            {
                string Address = GetEnvData("MASTER_DB_CONFIG__ADDRESS");
                string Port = GetEnvData("MASTER_DB_CONFIG__PORT");
                string DatabaseName = GetEnvData("MASTER_DB_CONFIG__DATABASE_NAME");
                string Username = GetEnvData("MASTER_DB_CONFIG__USERNAME");
                string Password = GetEnvData("MASTER_DB_CONFIG__PASSWORD");

                return new DBConfigModel(
                    Address,
                    int.Parse(Port),
                    Username,
                    Password,
                    DatabaseName
                );
            }
        }

        // RabbitMq
        public static RabbitMqConfigModel RabbitMQConfig
        {
            get
            {
                string rabbitMQAddress = GetEnvData("RABBITMQ_CONFIG__ADDRESS");
                string rabbitMQPort = GetEnvData("RABBITMQ_CONFIG__PORT");
                string rabbitMQUsername = GetEnvData("RABBITMQ_CONFIG__USERNAME");
                string rabbitMQPassword = GetEnvData("RABBITMQ_CONFIG__PASSWORD");

                return new RabbitMqConfigModel(
                    rabbitMQAddress,
                    int.Parse(rabbitMQPort),
                    rabbitMQUsername,
                    rabbitMQPassword
                );
            }
        }

        // Elastic Search
        public static ElasticSeachConfigModel ElasticSearchConfig
        {
            get
            {
                string elasticSearchAddress = GetEnvData("ELASTICSEARCH_CONFIG__ADDRESS");
                string elasticSearchPort = GetEnvData("ELASTICSEARCH_CONFIG__PORT");
                string elasticSearchUsername = GetEnvData("ELASTICSEARCH_CONFIG__USERNAME");
                string elasticSearchPassword = GetEnvData("ELASTICSEARCH_CONFIG__PASSWORD");

                return new ElasticSeachConfigModel(
                    elasticSearchAddress,
                    int.Parse(elasticSearchPort),
                    elasticSearchUsername,
                    elasticSearchPassword
                );
            }
        }

        // Redis
        public static RedisConfigModel RedisConfig
        {
            get
            {
                string redisAddress = GetEnvData("REDIS_CONFIG__ADDRESS");
                string redisPort = GetEnvData("REDIS_CONFIG__PORT");
                string redisUsername = GetEnvData("REDIS_CONFIG__USERNAME");
                string redisPassword = GetEnvData("REDIS_CONFIG__PASSWORD");

                return new RedisConfigModel(
                    redisAddress,
                    int.Parse(redisPort),
                    redisUsername,
                    redisPassword
                );
            }
        }
    }
}
