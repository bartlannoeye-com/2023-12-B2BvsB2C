using CloudBrew.SaaS.Api.Infrastructure;
using CloudBrew.SaaS.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddKeyVaultIfConfigured(builder.Configuration);

builder.Services.AddScoped<IUser, CurrentUser>();

// register Application and Infrastructure assemblies

// Add AAD security
builder.Services.AddAadRegistration(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOptions();
builder.Services.Configure<ApiInformation>(builder.Configuration.GetSection(nameof(ApiInformation)));

// moved API version and Swagger config in separate classes to keep Program clean
builder.Services.AddApiVersionRegistration();
builder.Services.AddVersionedSwaggerRegistration(builder.Configuration, typeof(Program));

// Starting from Microsoft.ApplicationInsights.AspNetCore version 2.15.0, calling services.AddApplicationInsightsTelemetry() will automatically read the instrumentation key 
// from Microsoft.Extensions.Configuration.IConfiguration of the application. There is no need to explicitly provide the IConfiguration.
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // added for debugging purposes
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseVersionedSwaggerUI();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

namespace CloudBrew.SaaS.Api
{
    public partial class Program { }
}
