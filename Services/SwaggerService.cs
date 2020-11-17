using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using QuickSetup.Swagger.Configurations;
using QuickSetup.Swagger.OperationFilters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace QuickSetup.Swagger.Services
{
    public class SwaggerService
    {
        public static void Register(IServiceCollection services, SwaggerConfiguration swaggerConfiguration)
        {
            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(options =>
            {
                AuthResponsesOperationFilter.AddSwaggerConfiguration(swaggerConfiguration);


                // add a custom operation filter which sets default values
                options.OperationFilter<ParameterOperationFilter>();
                options.OperationFilter<AuthResponsesOperationFilter>();


                options.AddSecurityDefinition(swaggerConfiguration.AuthorizationType, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = swaggerConfiguration.AuthorizationType == "Bearer" ? "JWT" : "",
                    Scheme = swaggerConfiguration.AuthorizationType.ToLower(),
                    In = ParameterLocation.Header,
                    Description = swaggerConfiguration.AuthorizationType == "Bearer" ? "JWT Authorization header using the Bearer scheme." : "Basic Authorization Header"
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

                options.EnableAnnotations();

            });

            SwaggerOptionConfiguration.AddSwaggerConfiguration(swaggerConfiguration);

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerOptionConfiguration>();
        }


        public static void UseSwagger(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();

            app.UseSwaggerUI(
                 options =>
                 {
                     foreach (var description in provider.ApiVersionDescriptions)
                     {
                         options.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                     }
                 });
        }
    }
}
