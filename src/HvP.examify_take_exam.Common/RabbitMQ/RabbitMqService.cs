using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using HvP.Common.Config;
using HvP.examify_take_exam.Common.Logger;
using HvP.examify_take_exam.Common.RabbitMQ.Consumers;

namespace HvP.examify_take_exam.Common.RabbitMQ
{
    public sealed class RabbitMqService : IDisposable
    {
        private IConnection _connection;
        private readonly ILoggerService<RabbitMqService> _logger;
        private static RabbitMqService? _instance = null;

        public RabbitMqService(IConnection connection, ILoggerService<RabbitMqService> logger)
        {
            this._logger = logger;
            this._connection = connection;
        }

        // # Exchange Define
        public static string ExchangeTakeExam => "TakeExam_Exchange";
        public static string ExchangeNotification => "Notification_Exchange";

        // # Queue Define
        public static string QueueTakeExam => "TakeExam_Queue";
        public static string QueueNotification => "Notification_Queue";


        /// <summary>
        /// Consumer Config
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, IMessageConsumer> GetConsumerConfig()
        {
            return new Dictionary<string, IMessageConsumer>
            {
                {
                    RabbitMqService.QueueTakeExam,
                    new NotificationMessageConsumer(_connection, new LoggerService<NotificationMessageConsumer>())
                }
            };
        }

        /// <summary>
        /// RabbitMqService Instance
        /// </summary>
        public static RabbitMqService Instance
        {
            get
            {
                if (_instance == null)
                {
                    var connLogger = new LoggerService<RabbitMqConnection>();
                    IConnection connection = new RabbitMqConnection(EnvConfig.RabbitMQConfig, connLogger).GetConnection().Result;

                    var logger = new LoggerService<RabbitMqService>();
                    _instance = new RabbitMqService(connection, logger);
                }
                return _instance;
            }
        }

        /// <summary>
        /// DeclareExchangeAndQueueAsync()
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="queueName"></param>
        /// <param name="exchangeType"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        public async Task DeclareExchangeAndQueueAsync(string exchangeName, string queueName, string exchangeType = ExchangeType.Direct, string routingKey = "")
        {
            IChannel? channel = null;

            try
            {
                channel = await _connection.CreateChannelAsync();

                await channel.ExchangeDeclareAsync(
                    exchange: exchangeName,
                    type: exchangeType,
                    durable: true,
                    autoDelete: false);

                await channel.QueueDeclareAsync(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                await channel.QueueBindAsync(
                    queue: queueName,
                    exchange: exchangeName,
                    routingKey: routingKey);
            }
            catch (Exception ex)
            {
                this._logger.LogFatal($"*** TRY DECLARE RABBITMQ Fail ***: exchangeName = {exchangeName}, queueName = {queueName}");
            }
            finally
            {
                if (channel != null)
                {
                    await channel.DisposeAsync();
                }
            }
        }

        /// <summary>
        /// PushMessage()
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <param name="message"></param>
        /// <param name="isPersit"></param>
        /// <returns></returns>
        public async Task PushMessage(string exchangeName, string routingKey, object message, bool isPersit = true)
        {
            IChannel? channel = null;
            try
            {
                channel = await _connection.CreateChannelAsync();

                var msg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                var properties = new BasicProperties()
                {
                    Persistent = isPersit,
                };

                await channel.BasicPublishAsync(
                    exchange: exchangeName,
                    routingKey: routingKey,
                    body: msg,
                    mandatory: false,
                    basicProperties: properties);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Push Message Fail ***: exchangeName = {exchangeName}, msg: ", message);
            }
            finally
            {
                if (channel != null)
                {
                    await channel.DisposeAsync();
                }
            }
        }

        /// <summary>
        /// StartAllConsuming()
        /// </summary>
        public void StartAllConsuming()
        {
            var consumers = GetConsumerConfig();

            foreach (var consumer in consumers)
            {
                string queueName = consumer.Key;
                IMessageConsumer consumerHandler = consumer.Value;
                consumerHandler.StartConsuming(queueName);
            }
        }

        public async void Dispose()
        {
            if (_connection != null)
            {
                await _connection.CloseAsync();
            }
        }
    }
}
