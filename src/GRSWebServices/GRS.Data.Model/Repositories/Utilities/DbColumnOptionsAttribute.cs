using System;

namespace GRS.Data.Model.Repositories.Utilities
{
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
   public class DbColumnOptionsAttribute : Attribute
   {
      public DbColumnOptionsAttribute(bool isOptional = false, bool isImmutable = false)
      {
         IsOptional = isOptional;
         IsImmutable = isImmutable;
      }

      public bool IsImmutable { get; private set; }

      public bool IsOptional { get; private set; }
   }
}
