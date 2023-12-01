using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CloudBrew.SaaS.Api.Infrastructure;

/// <summary>
/// Extension methods on <see cref="IApplicationBuilder"/> and <see cref="IServiceCollection"/>
/// for Swagger / OpenAPI documentation.
/// </summary>
internal static class SwaggerExtensions
{
    /// <summary>
    /// Add Swagger as OpenAPI documentation tool.
    /// Uses API versioning configured through <see cref="ApiVersionExtensions.AddApiVersionRegistration"/>.
    /// If called directly from Startup class, don't forget to call <code>services.AddOptions();</code> first.
    /// </summary>
    /// <param name="services">Reference for <see cref="IServiceCollection"/></param>
    /// <param name="configuration">Configuration</param>
    /// <param name="typesForDocumentation">
    /// Collection of types. The assemblies of these types are looked into for Swagger documentation files.
    /// Make sure that all xml documentation files are copied to the API project's output.
    /// Can be used to load documentation from models outside the API project.
    /// </param>
    /// <returns></returns>
    public static IServiceCollection AddVersionedSwaggerRegistration(this IServiceCollection services, IConfiguration configuration, params Type[] typesForDocumentation)
    {
        services.Configure<ApiInformation>(configuration.GetSection(nameof(ApiInformation)));

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(
            options =>
            {
                // integrate xml comments
                foreach (var type in typesForDocumentation)
                {
                    options.IncludeXmlComments(GetXmlCommentsFilePath(type));
                }

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "OAuth2.0 Auth Code with PKCE",
                    Name = "oauth2",
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(configuration["AuthorizationUrl"]),
                            TokenUrl = new Uri(configuration["TokenUrl"]),
                            Scopes = new Dictionary<string, string>
                            {
                                { configuration["ApiScope"], "read the api" }
                            }
                        }
                    }
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        new[] { configuration["ApiScope"] }
                    }
                });
            });

        return services;
    }

    /// <summary>
    /// Adds a versioned API description and Swagger UI.
    /// </summary>
    /// <param name="app">Reference for <see cref="IApplicationBuilder"/>.</param>
    /// <param name="swaggerRoutePrefix">Swagger options RoutePrefix, default string.Empty to serve Swagger at the application root.</param>
    /// <returns></returns>
    public static WebApplication UseVersionedSwaggerUI(this WebApplication app, string swaggerRoutePrefix = "")
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger();
        app.UseSwaggerUI(
            options =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    options.RoutePrefix = swaggerRoutePrefix;
                }

                options.OAuthClientId(app.Configuration["OpenIdClientId"]);
                options.OAuthUsePkce();
                options.OAuthScopeSeparator(" ");
            });

        return app;
    }

    private static string GetXmlCommentsFilePath(Type type)
    {
        var xmlFile = $"{type.Assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        return xmlPath;
    }
}
