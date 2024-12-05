using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArchitectureDDD.WebApi.Documentation;

public sealed class ConfigureSwaggerOptions: IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var documentation in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(documentation.GroupName, CreateDocumentation(documentation));
        }
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private static OpenApiInfo CreateDocumentation(ApiVersionDescription description)
    {
        var openApiInfo = new OpenApiInfo
        {
            Title = $"CleanArchitectureDDD.Api v{description.ApiVersion}",
            Version = description.ApiVersion.ToString(),
        };

        if (description.IsDeprecated)
        {
            openApiInfo.Description += " This API version has been deprecated.";
        }
        
        return openApiInfo;
    }
}