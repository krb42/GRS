using GRS.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace GRS.WebService.Extensions
{
   public static class ModelStateExtensions
   {
      public static ErrorModel ToErrorModel(this ModelStateDictionary modelState)
      {
         var errorDetails = modelState.SelectMany(keyValuePair =>
           keyValuePair.Value.Errors.Select(error => new ErrorDetail(keyValuePair.Key, error.ErrorMessage))
         ).ToArray();

         var errorModel = new ErrorModel
         {
            Error = new ErrorInfo(ErrorCode.ValidationFailed, DefaultErrorMessages.ValidationFailedErrorMessage, errorDetails),
         };

         return errorModel;
      }
   }
}
