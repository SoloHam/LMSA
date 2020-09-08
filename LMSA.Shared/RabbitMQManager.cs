using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace LMSA.Shared
{
    public class RabbitMQManager
    {
        private IConnection _conn;
        private IModel _channel;
        public IModel GetChannel { get => _channel; }
        public IConnection GetConnection { get => _conn; }
        public IConfiguration Configuration { get; }

        public RabbitMQManager(IConfiguration configuration)
        {
            Configuration = configuration;

            Initialize();
        }

        public void Initialize()
        {
            ConnectionFactory factory = new ConnectionFactory();

            factory.UserName = Configuration["MRConfig:user"];
            factory.Password = Configuration["MRConfig:pass"];
            factory.VirtualHost = Configuration["MRConfig:vhost"];
            factory.HostName = Configuration["MRConfig:hostName"];

            _conn = factory.CreateConnection();

            _channel = _conn.CreateModel();

            _channel.ExchangeDeclare("lmsa", ExchangeType.Direct);
            _channel.QueueDeclare("lmsa-projects", false, false, false, null);
            _channel.QueueBind("lmsa-projects", "lmsa", "", null);
        }
    }
}
