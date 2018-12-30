using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GRS.Core
{
   [JsonConverter(typeof(StringEnumConverter))]
   public enum ErrorCode
   {
      /// <summary>
      /// An unhandled exception occurred
      /// </summary>
      UnhandledException,

      /// <summary>
      /// API request validation failed
      /// </summary>
      ValidationFailed,

      /// <summary>
      /// Item request was not found
      /// </summary>
      ItemNotFound,
   }
}
