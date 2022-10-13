using System.Security.Claims;
using App.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using OpenIddict.Abstractions;

namespace App.WebUI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(OpenIddictConstants.Claims.Subject);
    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(OpenIddictConstants.Claims.Email);
}
