/*
 * Source: https://github.com/Microsoft/aspnet-api-versioning/blob/master/samples/aspnetcore/SwaggerSample/ConfigureSwaggerOptions.cs
 * Modified to support Swagger v5, and be extendable.
 */

using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CloudBrew.SaaS.Api.Infrastructure;

/// <summary>
/// Configures the Swagger generation options.
/// </summary>
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _versionDescriptionProvider;
    private readonly ApiInformation _apiInformation;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
    /// </summary>
    /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
    /// <param name="apiInformation">The <see cref="ApiInformation"/> config section</param>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IOptions<ApiInformation> apiInformation)
    {
        _versionDescriptionProvider = provider;
        _apiInformation = apiInformation?.Value;

        if (_apiInformation == null)
            throw new ArgumentNullException(nameof(apiInformation), "Missing config section");
    }

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        // add a swagger document for each discovered API version
        // note: you might choose to skip or document deprecated API versions differently
        foreach (var description in _versionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var primaryContact = _apiInformation.Contacts?.FirstOrDefault(p => p.IsPrimary) ?? new ApiInformationContact
        {
             Name = "Bart"
        };

        var info = new OpenApiInfo
        {
            Title = _apiInformation.Title,
            Version = description.ApiVersion.ToString(),
            Description = _apiInformation.Description,
            Contact = new OpenApiContact { Name = primaryContact?.Name, Email = primaryContact?.Email },
            TermsOfService = !string.IsNullOrWhiteSpace(_apiInformation.TermsOfServiceUri) ?
                new Uri(_apiInformation.TermsOfServiceUri) : null,
            License = !string.IsNullOrWhiteSpace(_apiInformation.LicenseUri) ?
                new OpenApiLicense { Name = _apiInformation.LicenseName, Url = new Uri(_apiInformation.LicenseUri) } : null
        };

        return info;
    }
}
