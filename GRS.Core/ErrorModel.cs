namespace GRS.Core
{
   /// <summary>
   /// Class representing the error model
   /// </summary>
   /// Error model based on https://github.com/Microsoft/api-guidelines/blob/master/Guidelines.md#7102-error-condition-responses
   public class ErrorModel
   {
      public ErrorInfo Error { get; set; }
   }
}
