namespace projektverwaltung;

using RabbitMQ.Client;

public class RabbitMQService
{
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQService()
    {
        _factory = new ConnectionFactory
        {
            HostName = "127.0.0.1",
            Port = 5672,
            UserName = "rabbit",
            Password = "rabbitpwd"
        };
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

  
    
    
    
    
}