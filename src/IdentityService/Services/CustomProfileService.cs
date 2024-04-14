using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService;

public class CustomProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManger;

    public CustomProfileService(UserManager<ApplicationUser> userManger)
    {
        _userManger=userManger;
    }
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user=await _userManger.GetUserAsync(context.Subject);
        var existingClaim=await _userManger.GetClaimsAsync(user);
        var claims=new List<Claim>
        {
            new Claim("username",user.UserName)
        };
        context.IssuedClaims.AddRange(claims);
        context.IssuedClaims.Add(existingClaim.FirstOrDefault(x=>x.Type==JwtClaimTypes.Name));
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        throw new NotImplementedException();
    }
}
