using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace GRS.WebServices.Configuration
{
   public class DtoValueTypesNullabilitySchemaFilter : ISchemaFilter
   {
      public void Apply(OpenApiSchema schema, SchemaFilterContext context)
      {
         if (schema.Type != "object" || schema.Properties == null)
            return;

         var propNames = context.Type.GetProperties()
            .Where(p =>

                        // is this a value type?
                        p.PropertyType.IsValueType &&

                        // is it not a nullable type?
                        Nullable.GetUnderlyingType(p.PropertyType) == null &&

                        // is it a read/write property?
                        p.CanRead && p.CanWrite)
            .Select(p => p.Name);

         ////schema.Required = schema.Properties.Keys.Intersect(propNames, StringComparer.OrdinalIgnoreCase).ToList();

         if (!schema.Required.Any())
            schema.Required = null;
      }
   }
}
