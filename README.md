# QuickSetup.Swagger ![.NET Core](https://github.com/carloluisito/QuickSetup.Swagger/workflows/.NET%20Core/badge.svg)

## Latest Version
* .Net 5.0
## Authors
* carloluisito
## Installation
- Install Nuget Package: https://www.nuget.org/packages/QuickSetup.Swagger/
- Create a configuration on your appsettings.json file
```
"SwaggerConfiguration": {
"Title": "YOUR_DOCUMENT_TITLE",
"Description": "YOUR_DOCUMENT_DESCRIPTION",
"ContactName": "YOUR_NAME",
"ContactEmail": "your_email@domain.com",
"AuthorizationType":  "Bearer" //Can be Basic or Bearer
}
```
- Startup.cs
```
using QuickSetup.Swagger.Configurations;
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
- API Project .csproj file
```
<PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
      <DocumentationFile>obj\Debug\netcoreapp3.1\{{PROJECT_NAME}}.xml</DocumentationFile>
      <OutputPath>obj\Debug\netcoreapp3.1\</OutputPath>
</PropertyGroup>
<PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
      <DocumentationFile>bin\Release\netcoreapp3.1\{{PROJECT_NAME}}.xml</DocumentationFile>
      <OutputPath>bin\Release\netcoreapp3.1\</OutputPath>
</PropertyGroup>
```
