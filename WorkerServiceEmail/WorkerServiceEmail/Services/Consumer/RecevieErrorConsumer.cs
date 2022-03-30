using MassTransit;
using WorkerServiceEmail.EntityMessage;

namespace WorkerServiceEmail.Services.Consumer
{
    public class RecevieErrorConsumer : IConsumer<IMessageErrorEmail>
    {
        public Task Consume(ConsumeContext<IMessageErrorEmail> context)
        {
            Console.WriteLine(context.Message);
            Console.WriteLine(context.Message.ServiceName);
            Console.WriteLine(context.Message.Message);

            return Task.CompletedTask;
        }
    }
}
