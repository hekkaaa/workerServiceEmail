using MassTransit;
using WorkerServiceEmail.EntityMessage;

namespace WorkerServiceEmail.Services.Consumer
{
    public class RecevieInfoConsumer : IConsumer<IMessageEmail>
    {
        public Task Consume(ConsumeContext<IMessageEmail> context)
        {
            Console.WriteLine(context.Message);
            Console.WriteLine(context.Message.EmailFrom);
            Console.WriteLine(context.Message.Subject);
            Console.WriteLine(context.Message.MessageText);
            return Task.CompletedTask;
        }
    }
}
