using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Diagnostics;

namespace GRS.Core
{
   [DebuggerDisplay("{ErrorCode}: {Message}")]
   public class ErrorInfo
   {
      /// <summary>
      /// Initialises an instance of the ErrorInfo class
      /// </summary>
      public ErrorInfo()
      {
      }

      /// <summary>
      /// Initialises an instance of the ErrorInfo class
      /// </summary>
      /// <param name="code">
      /// The error code
      /// </param>
      /// <param name="message">
      /// The error message
      /// </param>
      /// <param name="errorDetails">
      /// The error details
      /// </param>
      public ErrorInfo(ErrorCode code, string message, ErrorDetail[] errorDetails = null)
      {
         Code = code;
         Message = message;
         Details = errorDetails;
      }

      /// <summary>
      /// The server defined error code
      /// </summary>
      [JsonProperty]
      [JsonConverter(typeof(StringEnumConverter))]
      public ErrorCode Code { get; set; }

      /// <summary>
      /// An array of details about specific errors that led to this reported error
      /// </summary>
      [DefaultValue(null)]
      [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
      public ErrorDetail[] Details { get; set; }

      /// <summary>
      /// An object containing more specific information than the current object abouot the error
      /// </summary>
      public InnerErrorInfo InnerError { get; set; }

      /// <summary>
      /// Human readable representation of the error
      /// </summary>
      public string Message { get; set; }
   }
}
