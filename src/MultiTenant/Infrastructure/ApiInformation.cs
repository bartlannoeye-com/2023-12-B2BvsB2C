namespace CloudBrew.SaaS.Api.Infrastructure;

/// <summary>
/// General API information that can be loaded from config or environment variables
/// </summary>
public class ApiInformation
{
    /// <summary>
    /// API id, should be short and will be used in a URI
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// API title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Full API description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// API contacts
    /// </summary>
    public List<ApiInformationContact> Contacts { get; set; }

    /// <summary>
    /// License name
    /// </summary>
    public string LicenseName { get; set; }

    /// <summary>
    /// License URI
    /// </summary>
    public string LicenseUri { get; set; }

    /// <summary>
    /// Terms of Service URI
    /// </summary>
    public string TermsOfServiceUri { get; set; }
}

/// <summary>
/// Contact information for API owners and creators
/// </summary>
public class ApiInformationContact
{
    /// <summary>
    /// Contact name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Contact emails
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Is this the primary contact for the API?
    /// Only the (first) primary contact is shown on OpenApi.
    /// </summary>
    public bool IsPrimary { get; set; }
}
