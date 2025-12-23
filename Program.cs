using AdvanceAPI;
using AdvanceAPI.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Serilog.Context;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
});

ServiceConfiguration.RegisterServices(builder.Services, builder.Configuration);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseForwardedHeaders();
}
app.Use(async (context, next) =>
{
    var forwardedHeader = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    var ipAddress = forwardedHeader ?? context.Connection.RemoteIpAddress?.ToString();
    using (LogContext.PushProperty("IPAddress", ipAddress))
    {
        await next.Invoke();
    }
});
app.UseMiddleware<TokenBlacklistMiddleware>();

app.UseDefaultFiles();
app.MapStaticAssets();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseCors("DefaultCorsPolicy");
var providerfile = new FileExtensionContentTypeProvider();
providerfile.Mappings.Clear();
providerfile.Mappings.Add(".pdf", "application/pdf");
providerfile.Mappings.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
// TODO: Move uploaded bill assets to a dedicated storage service with antivirus scanning and signed URLs.

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwaggerUI(options =>
    {
        for (int i = 0; i < provider.ApiVersionDescriptions.Count; i++)
        {
            ApiVersionDescription? description = provider.ApiVersionDescriptions[i];
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
        options.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });

    app.MapOpenApi();

    app.MapFallbackToFile("/swagger");
}

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/Upload_Bills", StringComparison.OrdinalIgnoreCase))
    {
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            await context.ChallengeAsync();
            return;
        }
    }

    await next();
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Upload_Bills")),
    RequestPath = "/Upload_Bills",
    ContentTypeProvider = providerfile
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Upload_Bills")),
    RequestPath = "/Upload_Bills/Approved",
    ContentTypeProvider = providerfile
});

app.MapControllers();

await app.RunAsync();
