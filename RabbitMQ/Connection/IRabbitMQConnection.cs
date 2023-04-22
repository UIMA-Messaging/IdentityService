using RabbitMQ.Client;

namespace IdentityService.RabbitMQ.Connection
{
    public interface IRabbitMQConnection
    {
        public IConnection TryConnect();
    }
}