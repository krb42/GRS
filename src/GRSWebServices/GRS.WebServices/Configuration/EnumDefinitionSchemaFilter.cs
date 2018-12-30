using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace GRS.WebServices.Configuration
{
   public class EnumDefinitionSchemaFilter : ISchemaFilter
   {
      private readonly ILogger<EnumDefinitionSchemaFilter> _logger;

      public EnumDefinitionSchemaFilter(ILogger<EnumDefinitionSchemaFilter> logger = null)
      {
         _logger = logger ?? NullLogger<EnumDefinitionSchemaFilter>.Instance;
      }

      public void Apply(Schema schema, SchemaFilterContext context)
      {
         if (schema.Properties == null || !schema.Properties.Any())
            return;

         // Get all the enum properties in the swagger schema
         var enumProperties = schema.Properties.Where(p => p.Value.Enum != null)
            .Union(schema.Properties.Where(p => p.Value.Items?.Enum != null)).ToList();

         var enums = context.SystemType.GetProperties()
            .Where(p => p.CanRead && p.CanWrite)
            .Select(p => Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType.GetElementType()
                     ?? p.PropertyType.GetGenericArguments().FirstOrDefault() ?? p.PropertyType)
            .Where(p => p.IsEnum)
            .ToList();

         foreach (var enumProperty in enumProperties)
         {
            var enumPropertyValue = enumProperty.Value.Enum != null ? enumProperty.Value : enumProperty.Value.Items;

            var enumValues = enumPropertyValue.Enum.Select(value => value.ToString()).ToList();
            var enumType = enums.SingleOrDefault(type =>
                 {
                    var enumNames = Enum.GetNames(type);

                    if (enumNames.Except(enumValues, StringComparer.InvariantCultureIgnoreCase).Any())
                       return false;

                    if (enumValues.Except(enumNames, StringComparer.InvariantCultureIgnoreCase).Any())
                       return false;

                    return true;
                 });

            if (enumType == null)
               throw new Exception($"Property {enumProperty} not found in {context.SystemType.Name} Type.");

            var regSchema = context.SchemaRegistry.GetOrRegister(enumType);
            regSchema.Ref = $"#/definitions/{enumType.FullName}";

            if (context.SchemaRegistry.Definitions.ContainsKey(enumType.FullName) == false)
            {
               _logger.LogTrace($"Added $Ref {regSchema.Ref} to schema.");
               context.SchemaRegistry.Definitions.Add(enumType.FullName, enumPropertyValue);
            }

            if (enumProperty.Value.Enum != null)
               schema.Properties[enumProperty.Key] = regSchema;
            else if (enumProperty.Value.Items?.Enum != null)
               enumProperty.Value.Items = regSchema;
         }
      }
   }
}
