using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;

namespace CloudBrew.SaaS.Api.Infrastructure;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAadRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(jwtBearerOptions =>
                {
                    configuration.Bind("AzureAdB2C", jwtBearerOptions);

                    jwtBearerOptions.TokenValidationParameters.NameClaimType = "name";
                }, identityOptions =>
                {
                    configuration.Bind("AzureAdB2C", identityOptions);
                }
            );

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
