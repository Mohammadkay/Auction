﻿using AutoMapper;
using Contract;
using MassTransit;
using MassTransit.Testing;
using MongoDB.Entities;

namespace SearchService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IMapper _mapper;
    public AuctionCreatedConsumer(IMapper mapper)
    {
        _mapper=mapper;
    }
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine("==> Consuming  Auction Created"+context.Message.Id);
        var item =_mapper.Map<Item>(context.Message);
        if (item.Model=="Foo") throw new ArgumentException("Can't sell car with name of foo");
        await item.SaveAsync(); 
    }
}
