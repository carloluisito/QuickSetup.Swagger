using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace QuickSetup.Swagger.Configurations
{
    public class SwaggerOptionConfiguration : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        private static SwaggerConfiguration _swaggerConfiguration;

        public SwaggerOptionConfiguration(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public static void AddSwaggerConfiguration(SwaggerConfiguration swaggerConfiguration)
        {
            _swaggerConfiguration = swaggerConfiguration;
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var api in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(api.GroupName, CreateInfoForApiVersion(_swaggerConfiguration, api));
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(SwaggerConfiguration swaggerConfiguration, ApiVersionDescription api)
        {
            var info = new OpenApiInfo()
            {
                Title = swaggerConfiguration.Title,
                Version = api.ApiVersion.ToString(),
                Description = swaggerConfiguration.Description,
                Contact = new OpenApiContact() { Name = swaggerConfiguration.ContactName, Email = swaggerConfiguration.ContactEmail },
            };

            if (api.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
