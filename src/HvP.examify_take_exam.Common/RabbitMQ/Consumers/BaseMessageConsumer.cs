using HvP.examify_take_exam.Common.Logger;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace HvP.examify_take_exam.Common.RabbitMQ.Consumers
{
    public abstract class BaseMessageConsumer<TConsumer> : IMessageConsumer, IDisposable
    {
        private IConnection _connection;
        private IChannel? _channel = null;
        private readonly ILoggerService<TConsumer> _logger;

        public BaseMessageConsumer(IConnection connection, ILoggerService<TConsumer> logger)
        {
            _logger = logger;
            _connection = connection;
        }
        /// <summary>
        /// HandleMessageProcess(TModel message)
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="message"></param>
        public abstract void HandleMessageProcess(string message);

        /// <summary>
        /// StartConsuming(string queueName)
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="queueName"></param>
        public async void StartConsuming(string queueName)
        {
            var channel = await _connection.CreateChannelAsync();
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (ch, args) =>
            {
                try
                {
                    var body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    if (!string.IsNullOrEmpty(message))
                    {
                        HandleMessageProcess(message);
                    }

                    await channel.BasicAckAsync(args.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    this._logger.LogError($"Receive Message Fail ***: queueName = {queueName}, Ex: {ex.Message}");
                    await channel.BasicNackAsync(args.DeliveryTag, false, requeue: false);
                }
            };

            await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);

            this._logger.LogInformation($"*** STARTED CONSUMING Message from Queue: {queueName}");
        }

        /// <summary>
        /// Dispose()
        /// </summary>
        public async void Dispose()
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
            }
        }
    }
}
