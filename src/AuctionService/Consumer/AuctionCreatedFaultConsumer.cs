using Contract;
using MassTransit;

namespace AuctionService.Consumer;
public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        Console.WriteLine("==> Consuming faulty creation");

        var exception=context.Message.Exceptions.First();
        if(exception.ExceptionType=="System.ArgumentException"){
            context.Message.Message.Model="FooBar";
            await context.Publish(context.Message.Message);
        }
        else
        {
            Console.WriteLine("No Argumet exception -update error in dashboard");
        }
    }
}
