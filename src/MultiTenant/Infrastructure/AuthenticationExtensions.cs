using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace CloudBrew.SaaS.Api.Infrastructure;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAadRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(jwtBearerOptions =>
                {
                    // disable so we don't get issuer null error
                    jwtBearerOptions.TokenValidationParameters.ValidateIssuer = false; // or write own validation to check vs your customer list
                    
                    // make sure it comes from our app registration
                    jwtBearerOptions.TokenValidationParameters.ValidAudience = $"api://{configuration.GetSection("AzureAd")["ClientId"]}";
                }, identityOptions =>
                {
                    configuration.Bind("AzureAd", identityOptions);
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
