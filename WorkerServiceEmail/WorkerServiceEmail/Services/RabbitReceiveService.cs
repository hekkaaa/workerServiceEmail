using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using WorkerServiceEmail.Email;
using WorkerServiceEmail.EntityMessage;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Services
{
    public class RabbitReceiveService : IRabbitReceiveService
    {
        private readonly IRunner _runner;
        private readonly IEmailService _emailService;

        public RabbitReceiveService(IRunner runner, IEmailService emailService)
        {
            _runner = runner;
            _emailService = emailService;
        }

        public async void Recevie()
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    channel.QueueDeclare(queue: "test-que-1",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);


                        MessageEmail infomessage = new MessageEmail
                        {
                            EmailFrom = "dogsitterclub2022@gmail.com",
                            NameFrom = "Daemon Start Service",
                            EmailTo = "silencemyalise@gmail.com",
                            NameTo = "Administrator Service",
                            Subject = "Service Email Alert!",
                            MessageText = "<b>message from rabbitMQ</b><br>" +
                             $"<b> {message} </b>"
                        };
                        _emailService.SendEmailAsync(infomessage);
                    };
                    channel.BasicConsume(queue: "test-que-1",
                                         autoAck: true,
                                         consumer: consumer);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
