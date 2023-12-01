namespace CloudBrew.SaaS.Api.Infrastructure;

/// <summary>
/// Extension methods on <see cref="IServiceCollection"/> for API versions.
/// </summary>
internal static class ApiVersionExtensions
{
    /// <summary>
    /// Add API version through URL.
    /// </summary>
    /// <remarks>
    /// Swagger setup is also tweaked for API versioning.
    /// </remarks>
    /// <param name="services">Reference for <see cref="IServiceCollection"/></param>
    /// <returns></returns>
    public static IServiceCollection AddApiVersionRegistration(this IServiceCollection services)
    {
        // Documentation: https://github.com/microsoft/aspnet-api-versioning/wiki/API-Versioning-Options
        services.AddApiVersioning(options =>
        {
            // Add the headers "api-supported-versions" and "api-deprecated-versions"
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            options.GroupNameFormat = "'v'VVV";

            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}
