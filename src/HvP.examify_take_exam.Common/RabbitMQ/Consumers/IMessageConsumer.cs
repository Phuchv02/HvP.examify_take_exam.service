namespace HvP.examify_take_exam.Common.RabbitMQ.Consumers
{
    public interface IMessageConsumer
    {
        public void StartConsuming(string queueName);
        public void HandleMessageProcess(string message);
    }
}
