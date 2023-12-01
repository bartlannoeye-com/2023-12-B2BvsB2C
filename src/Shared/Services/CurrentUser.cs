using System.IdentityModel.Tokens.Jwt;

namespace Shared.Services;

public static class CurrentUserDetails
{
    public static dynamic DecodeJwtToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);

        return new
        {
            Header = jwtSecurityToken.Header,
            Payload = jwtSecurityToken.Payload
        };
    }
}
