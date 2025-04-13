using HvP.examify_take_exam.Common.Logger;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client.Events;
using HvP.Common.Config;
using System.Net.Http.Headers;

namespace HvP.examify_take_exam.Common.RabbitMQ
{
    public sealed class RabbitMqService
    {
        private IConnection _connection;
        private readonly ILoggerService<RabbitMqService> _logger;
        private static RabbitMqService? _instance = null;

        public RabbitMqService(IConnection connection, ILoggerService<RabbitMqService> logger)
        {
            this._logger = logger;
            this._connection = connection;
        }

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

        public async Task StartConsummerAsync<T>(string queueName, Func<T, Task> handleMessageFunc)
        {
            var channel = await _connection.CreateChannelAsync();
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (ch, args) =>
            {
                try
                {
                    var body = args.Body.ToArray();
                    var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

                    if (message != null)
                    {
                        await handleMessageFunc(message);
                    }

                    await channel.BasicAckAsync(args.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    this._logger.LogError($"Receive Message Fail ***: queueName = {queueName}");
                    await channel.BasicNackAsync(args.DeliveryTag, false, requeue: false);
                }
            };

            await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);

            this._logger.LogInformation($"*** STARTED CONSUMING Message from Queue: {queueName}");
        }

        // # Exchange Define
        public static string ExchangeTakeExam => "TakeExam_Exchange";
        public static string ExchangeNotification => "Notification_Exchange";

        public static string QueueTakeExam => "TakeExam_Queue";
        public static string QueueNotification => "Notification_Queue";
    }
}
