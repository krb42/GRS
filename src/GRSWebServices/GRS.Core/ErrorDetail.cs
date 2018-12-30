using System;

namespace GRS.Core
{
   /// <summary>
   /// A class representing the Error Details
   /// </summary>
   public class ErrorDetail
   {
      public ErrorDetail(string target, string message)
      {
         Target = target ?? throw new ArgumentNullException(nameof(Target));
         Message = message ?? throw new ArgumentNullException(nameof(Message));
      }

      /// <summary>
      /// The error message
      /// </summary>
      public string Message { get; protected set; }

      /// <summary>
      /// The Target of the error
      /// </summary>
      public string Target { get; protected set; }
   }
}
