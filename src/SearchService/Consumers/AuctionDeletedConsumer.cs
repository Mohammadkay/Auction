using Contract;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;
public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
{
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        var result=await DB.DeleteAsync<Item>(context.Message.Id);
       if(!result.IsAcknowledged){
        throw new MessageException(typeof(AuctionDeleted),"problem Deleting MongoDb");
    }
    }
}
