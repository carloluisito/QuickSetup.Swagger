# QuickSetup.Swagger ![.NET Core](https://github.com/carloluisito/QuickSetup.Swagger/workflows/.NET%20Core/badge.svg)

## Installation
- Install Nuget Package: https://www.nuget.org/packages/QuickSetup.Swagger/
- Create a configuration on your appsettings.json file
```
"SwaggerConfiguration": {
"Title": "YOUR_DOCUMENT_TITLE",
"Description": "YOUR_DOCUMENT_DESCRIPTION",
"ContactName": "Sample Name",
"ContactEmail": "yor_email@domain.com",
"AuthorizationType":  "Bearer" //Can be Basic or Bearer
}
```
- Startup.cs
```
using QuickSetup.Swagger.Configuration;
using QuickSetup.Swagger.Services;

public void ConfigureServices(IServiceCollection services)
{
      var swaggerConfiguration = configuration.GetSection("SwaggerConfiguration").Get<SwaggerConfiguration>();
      services.AddScoped(x => swaggerConfiguration);

      SwaggerService.Register(services, swaggerConfiguration);
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
{
    //other setup
    SwaggerService.UseSwagger(app, provider);
}
```
