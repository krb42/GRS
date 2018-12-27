using System;
using System.Diagnostics;

namespace GRS.Core
{
   /// <summary>
   /// Class for providing details debugging information
   /// </summary>
   [DebuggerDisplay("Message")]
   public class InnerErrorInfo
   {
      public InnerErrorInfo(Exception exception)
      {
         Message = exception.Message ?? string.Empty;
         TypeNmae = exception.GetType().FullName;
         StackTrace = exception.StackTrace;

         if (exception.InnerException == null)
            return;

         InnerError = new InnerErrorInfo(exception.InnerException);
      }

      /// <summary>
      /// The nested eror information
      /// </summary>
      public InnerErrorInfo InnerError { get; set; }

      /// <summary>
      /// The error message
      /// </summary>
      public string Message { get; set; }

      /// <summary>
      /// The stack trace for this error
      /// </summary>
      public string StackTrace { get; set; }

      /// <summary>
      /// The type name of the error
      /// </summary>
      public string TypeNmae { get; set; }
   }
}
