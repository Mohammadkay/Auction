using AuctionService.Dtos;
using AuctionService.Entites;
using AutoMapper;
using Contract;

namespace AuctionService.RequestHelpers;
public class MappingProfile:Profile
{
public MappingProfile()
{
    CreateMap<Auction,AuctionDto>().IncludeMembers(x=>x.Item);
    CreateMap<Item,AuctionDto>();
    CreateMap<CreateAuctionDto,Auction>()
    .ForMember(d=>d.Item,o=>o.MapFrom(s=>s));
    CreateMap<CreateAuctionDto,Item>();
    CreateMap<AuctionDto,AuctionCreated>();
}
}
