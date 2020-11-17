using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using QuickSetup.Swagger.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace QuickSetup.Swagger.OperationFilters
{
    public class AuthResponsesOperationFilter : IOperationFilter
    {
        private static SwaggerConfiguration _swaggerConfiguration;

        public static void AddSwaggerConfiguration(SwaggerConfiguration swaggerConfiguration)
        {
            _swaggerConfiguration = swaggerConfiguration;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var anonymousAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AllowAnonymousAttribute>();

            var authorizeAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>();

            if (!anonymousAttribute.Any() && authorizeAttribute.Any())
            {
                var securityRequirement = new OpenApiSecurityRequirement()
                {
                    {
                        // Put here you own security scheme, this one is an example
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = _swaggerConfiguration.AuthorizationType
                            },
                            Name = _swaggerConfiguration.AuthorizationType,
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                };
                operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            }
        }
    }
}
