using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.IO;

namespace GRS.WebServices.Configuration
{
   // Library to aid the combined use of Swashbuckle and ASP NET API Versioning - https://github.com/rh072005/SwashbuckleAspNetVersioningShim
   //
   // ASP.NET API Versioning - https://github.com/Microsoft/aspnet-api-versioning
   public static class GRSSwaggerDocsConfiguration
   {
      private const string Description = "REST API for GRS";

      //COMPANY private const string Email = "krfirth@gmail.com";
      //COMPANY private const string Name = "GRS";
      private const string Title = "GRS REST API";

      //COMPANY private const string Url = "http://www.grs.com.au";
      private const string Version = "v1";

      private static List<string> GetXmlCommentsFilePath()
      {
         var app = PlatformServices.Default.Application;
         var basePath = app.ApplicationBasePath;
         var filename = app.ApplicationName + ".xml";
         var dtoAssembleXmFileName = "GRS.Dto.xml";

         var mainAppXmlPath = Path.Combine(basePath, filename);
         var dtoXmlPath = Path.Combine(basePath, dtoAssembleXmFileName);

         //TODO   return new List<string> { mainAppXmlPath, dtoXmlPath };
         return new List<string> { mainAppXmlPath };
      }

      public static void AddSwaggerForGRS(this IServiceCollection services)
      {
         //Swagger - Enable this line and the related lines in COnfigure method to enable Swagger UI
         services.AddSwaggerGen(options =>
         {
            options.SwaggerDoc(Version,
               new OpenApiInfo
               {
                  Title = $"{Title} {Version}",
                  Version = Version,
                  Description = Description,

                  //COMPANY Contact = new Contact { Email = Email, Name = Name, Url = Url, }
               });

            options.SchemaFilter<EnumDefinitionSchemaFilter>();
            options.SchemaFilter<DtoValueTypesNullabilitySchemaFilter>();

            var files = GetXmlCommentsFilePath();
            files.ForEach(f => options.IncludeXmlComments(f));
         });
      }

      public static void UseSwaggerDocsForGRS(this IApplicationBuilder app)
      {
         // Enable middleware to serve generated swagger as JSON endpoint
         app.UseSwagger();

         // Enable middleware to serve swagger-ui assets (HTML, JS, CSS, etc)
         // URL: /swagger
         app.UseSwaggerUI(options =>
         {
            //options.DocExpansion("none");
            options.SwaggerEndpoint($"/swagger/{Version}/swagger.json", $"{Title} {Version}");
         });
      }
   }
}
