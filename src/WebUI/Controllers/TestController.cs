using App.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.WebUI.Controllers;


[Authorize("dataEventRecordsPolicy")]
public class TestController : ApiControllerBase
{
    private readonly ICurrentUserService _currentUserService;

    public TestController(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }
    
    [HttpGet("GetData")]
    public async Task<object> GetData()
    {
        var data = _currentUserService.UserId;
        var data2 = _currentUserService.Email;
        //_userId = User.Claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.Subject)?.Value;
        // var data = User.Claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.Subject)?.Value;
        return "test data got";
    }
}