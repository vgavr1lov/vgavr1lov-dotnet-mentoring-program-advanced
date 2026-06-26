using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;
using ECommerce.Identity.Configuration;
using System.Security.Claims;

namespace ECommerce.Identity.Services;

public class ProfileService : IProfileService
{
    private readonly TestUserStore _userStore;

    public ProfileService(TestUserStore userStore)
    {
        _userStore = userStore;
    }
    public Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        //var claim = context.Subject.FindFirst(SecurityConstants.ClaimTypes.Role);

        var user = _userStore.FindBySubjectId(context.Subject.GetSubjectId());
        if (user is null)
            return Task.CompletedTask;

        var claim = user.Claims.FirstOrDefault(c => c.Type == SecurityConstants.ClaimTypes.Role);


        if (claim is null)
            return Task.CompletedTask;

        var permissionClaims = new List<Claim>
        {
            claim
        };

        if (string.Equals(claim.Value, SecurityConstants.Roles.StoreCustomer))
        {
            permissionClaims.Add(new Claim(SecurityConstants.ClaimTypes.Permission, SecurityConstants.Permissions.CatalogRead));
        }
        else if (string.Equals(claim.Value, SecurityConstants.Roles.Manager))
        {
            permissionClaims.Add(new Claim(SecurityConstants.ClaimTypes.Permission, SecurityConstants.Permissions.CatalogRead));
            permissionClaims.Add(new Claim(SecurityConstants.ClaimTypes.Permission, SecurityConstants.Permissions.CatalogCreate));
            permissionClaims.Add(new Claim(SecurityConstants.ClaimTypes.Permission, SecurityConstants.Permissions.CatalogDelete));
            permissionClaims.Add(new Claim(SecurityConstants.ClaimTypes.Permission, SecurityConstants.Permissions.CatalogUpdate));
        }

        context.IssuedClaims.AddRange(permissionClaims);

        return Task.CompletedTask;
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;

        return Task.CompletedTask;
    }
}
