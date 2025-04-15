using HvP.examify_take_exam.Common.Logger;
using RabbitMQ.Client;

namespace HvP.examify_take_exam.Common.RabbitMQ.Consumers
{
    public class NotificationMessageConsumer : BaseMessageConsumer<NotificationMessageConsumer>
    {
        private readonly ILoggerService<NotificationMessageConsumer> _logger;

        public NotificationMessageConsumer(IConnection connection, ILoggerService<NotificationMessageConsumer> logger)
            : base(connection, logger)
        {
            _logger = logger;
        }

        public override void HandleMessageProcess(string message)
        {
            // TODO:  var message = JsonSerializer.Deserialize<TConsumer>(message);
            Console.WriteLine(message);
        }
    }

    public class NotifyMsgPaloadModel { }

}
