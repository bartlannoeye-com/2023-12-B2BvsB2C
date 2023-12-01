using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;

namespace CloudBrew.SaaS.Api.Infrastructure;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAadRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        services.AddMicrosoftIdentityWebApiAuthentication(configuration);
        services.Configure<OpenIdConnectOptions>(
            OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                // The claim in the Jwt token where App roles are available.
                options.TokenValidationParameters.RoleClaimType = "roles";
                options.TokenValidationParameters.NameClaimType = "name";
            });

        // The following lines code instruct the asp.net core middleware to use the data in the "roles" claim in the Authorize attribute and User.IsInrole()
        // See https://docs.microsoft.com/aspnet/core/security/authorization/roles?view=aspnetcore-2.2 for more info.
        services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            // The claim in the Jwt token where App roles are available.
            options.TokenValidationParameters.RoleClaimType = "roles";
        });

        services.AddCors(options =>
        {
            options.AddPolicy(name: "CORSPolicy",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200") // your Angular port
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });

        return services;
    }
}
