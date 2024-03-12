using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace SearchService.Controllers;
[ApiController]
[Route("api/Search")]
public class SearchController : ControllerBase
{
[HttpGet]
public async Task<ActionResult<List<Item>>> SearchItem([FromQuery]SearchParmse search){
     var query=DB.PagedSearch<Item,Item>();
     query.Sort(x=>x.Ascending(a=>a.Make));
     if(!string.IsNullOrEmpty(search.SearchTerm)){
        query.Match(Search.Full,search.SearchTerm).SortByTextScore();
     }
     query=search.OrderBy switch
     {
        "make"=>query.Sort(x=>x.Ascending(a=>a.Make)),
        "new"=>query.Sort(x=>x.Descending(a=>a.CreatedAt)),
        _=>query.Sort(x=>x.Ascending(a=>a.AuctionEnd))
     };
     query=search.FilterBy switch 
     {
        "finshed"=>query.Match(x=>x.AuctionEnd<DateTime.UtcNow),
        "endingSoon"=>query.Match(x=>x.AuctionEnd<DateTime.UtcNow.AddHours(6)
        &&x.AuctionEnd>DateTime.UtcNow),
        _ =>query.Match(x=>x.AuctionEnd>DateTime.UtcNow)
     };
     if(!string.IsNullOrEmpty(search.Seller)){
        query.Match(x=>x.Seller==search.Seller);
     }
          if(!string.IsNullOrEmpty(search.Winner))
     {
         query.Match(x=>x.Winner==search.Winner);
     }
     query.PageNumber(search.PageNumber);
     query.PageSize(search.PageSize);
     var results=await query.ExecuteAsync();
     return Ok( new {
        Result=results.Results,
        pageCount=results.PageCount,
        totalCount=results.TotalCount
     });
}
}
