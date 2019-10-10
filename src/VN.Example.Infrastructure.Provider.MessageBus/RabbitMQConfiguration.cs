namespace VN.Example.Infrastructure.Provider.MessageBus
{
    public class RabbitMQConfiguration
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
