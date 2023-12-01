using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Shared.Services;

namespace CloudBrew.SaaS.Api.Controllers;

[Authorize]
[ApiVersion("1.0")]
[EnableCors("CORSPolicy")]
[ApiController]
[Route("v{version:apiVersion}/[controller]")]
public class UserController : ControllerBase
{
    public UserController()
    {
    }

    [HttpGet]
    public async Task<ActionResult<string>> GetTokenContent()
    {
        string token = await HttpContext.GetTokenAsync("access_token");
        return Ok(CurrentUserDetails.DecodeJwtToken(token));
    }
}
