using AuctionService.Data;
using AuctionService.Dtos;
using AuctionService.Entites;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controller;
[ApiController]
[Route("api/auctions")]
public class AuctionController:ControllerBase
{
    private readonly AuctionDbContext _context;
    private IMapper _mapper;
public AuctionController(AuctionDbContext context,IMapper mapper)
{
_context=context;
_mapper=mapper;
}

[HttpGet]
public async Task<ActionResult<List<AuctionDto>>> GetAllAuction(string date)
{
     var query=_context.Auctions.OrderBy(x=>x.Item.Make).AsQueryable();
    if(!string.IsNullOrEmpty(date)){
        query=query.Where(x=>x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime())>0);
    }

   
    return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
}

[HttpGet("{Id}")]
public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid Id)
{
var auction=await _context.Auctions
.Include(x=>x.Item)
.FirstOrDefaultAsync(x=>x.Id==Id);
if(auction==null) return NotFound();
return _mapper.Map<AuctionDto>(auction);
} 

[HttpPost]
public async Task<ActionResult<AuctionDto>> Add(CreateAuctionDto entity)
{
var auction=_mapper.Map<Auction>(entity);
auction.Seller="Test";

_context.Auctions.Add(auction);
var result =await _context.SaveChangesAsync()>0;
if(!result){
    return BadRequest(result+"Couldunt Save changes to DB");
}
return CreatedAtAction(nameof(GetAuctionById),new {auction.Id},_mapper.Map<AuctionDto>(auction))
 ;
}

[HttpPut("{Id}")]
public async Task<ActionResult> update (Guid Id,UpdateAuctionDto entity){
    var auction=await _context.Auctions
    .Include(x=>x.Item)
    .FirstOrDefaultAsync(x=>x.Id==Id);
    if(auction==null){
        return NotFound();
    }
    auction.Item.Make=entity.Make??auction.Item.Make;
    auction.Item.Model=entity.Model??auction.Item.Model;
    auction.Item.Color=entity.Color??auction.Item.Color;
    auction.Item.Mileage=entity.Mileage ?? auction.Item.Mileage;
    auction.Item.Year=entity.Year ?? auction.Item.Year;
    _context.Auctions.Update(auction);
var result =await _context.SaveChangesAsync()>0;
if(!result){
    return BadRequest("problem saving changes");
}
return Ok();
}

[HttpDelete("{Id}")]
public async Task<ActionResult> Delete(Guid Id)
{
    var auction=await _context.Auctions.FindAsync(Id);

    if(auction==null){
        return NotFound();
    }
    _context.Auctions.Remove(auction);
    var result =await _context.SaveChangesAsync()>0;
if(!result){
    return BadRequest("Couldunt delete Auction");
}
return Ok();
}
}
