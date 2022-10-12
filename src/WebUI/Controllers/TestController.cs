using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.WebUI.Controllers;


[Authorize("dataEventRecordsPolicy")]
public class TestController : ApiControllerBase
{
    [HttpGet("GetData")]
    public async Task<object> GetData()
    {
        //_userId = User.Claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.Subject)?.Value;
        // var data = User.Claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.Subject)?.Value;
        return "test data got";
    }
}