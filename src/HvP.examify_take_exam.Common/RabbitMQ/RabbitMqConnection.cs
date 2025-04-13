using RabbitMQ.Client;
using HvP.examify_take_exam.Common.Logger;
using HvP.examify_take_exam.Common.Models;

namespace HvP.examify_take_exam.Common.RabbitMQ
{
    public class RabbitMqConnection
    {
        private ILoggerService<RabbitMqConnection> _logger;
        private readonly RabbitMqConfigModel _configModel;

        public RabbitMqConnection(RabbitMqConfigModel configModel, ILoggerService<RabbitMqConnection> logger)
        {
            this._logger = logger;
            this._configModel = configModel;
        }

        public async Task<IConnection> GetConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configModel.Address,
                    Port = _configModel.Port,
                    UserName = _configModel.Username,
                    Password = _configModel.Password,
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
                };

                IConnection connetion = await factory.CreateConnectionAsync();
                return connetion;
            }
            catch (Exception ex)
            {

                this._logger.LogFatal($"*** TRY CONNECT RABBITMQ Fail ***: Address={_configModel.Address}, Port = {_configModel.Port}");
                return null;
            }
        }
    }
}
