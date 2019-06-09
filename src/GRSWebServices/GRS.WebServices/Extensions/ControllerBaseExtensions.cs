using GRS.Core;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GRS.WebService.Extensions
{
   /// <summary>
   /// A set of correctly formatted ErrorModels
   /// </summary>
   public static class ControllerBaseExtensions
   {
      private const string NullDtoOrFieldMismatchError = "DTO is missing or {0} mismatch";
      private const string NullDtoOrIdMismatchError = "DTO is missing or Id mismatch";

      public static IActionResult ItemNotFound(this ControllerBase controller, string message)
      {
         if (string.IsNullOrEmpty(message)) throw new ArgumentNullException(nameof(message));

         return controller.NotFound(new ErrorModel
         {
            Error = new ErrorInfo(ErrorCode.ItemNotFound, message)
         });
      }

      public static IActionResult NullDtoOrFieldMismatchBadRequest(this ControllerBase controller, string fieldName, int? dtoField, int routeParam = default(int))
      {
         return NullDtoOrFieldMismatchBadRequest(controller, NullDtoOrFieldMismatchError, fieldName, dtoField?.ToString(), routeParam.ToString());
      }

      public static IActionResult NullDtoOrFieldMismatchBadRequest(this ControllerBase controller, string fieldName)
      {
         return NullDtoOrFieldMismatchBadRequest(controller, NullDtoOrFieldMismatchError, fieldName, null, null);
      }

      public static IActionResult NullDtoOrFieldMismatchBadRequest(this ControllerBase controller, string fieldName, string dtoField, string routeParam = null)
      {
         return NullDtoOrFieldMismatchBadRequest(controller, NullDtoOrFieldMismatchError, fieldName, dtoField, routeParam);
      }

      public static IActionResult NullDtoOrFieldMismatchBadRequest(this ControllerBase controller, string message, string fieldName, string dtoField, string routeParam)
      {
         if (string.IsNullOrWhiteSpace(message))
            message = string.Format(NullDtoOrFieldMismatchError, fieldName);

         if (routeParam != null)
         {
            message += $" {fieldName}:{dtoField} != Route {fieldName}: {routeParam}";
         }

         return controller.ValidationFailed(message, new ErrorDetail[] { });
      }

      public static IActionResult NullDtoOrIdMismatchBadRequest(this ControllerBase controller, int? dtoId = null, int routeId = default(int))
      {
         return NullDtoOrIdMismatchBadRequest(controller, NullDtoOrIdMismatchError, dtoId, routeId);
      }

      public static IActionResult NullDtoOrIdMismatchBadRequest(this ControllerBase controller, string message, int? dtoId = null, int routeId = default(int))
      {
         if (string.IsNullOrWhiteSpace(message))
            message = NullDtoOrIdMismatchError;

         if (routeId > 0)
         {
            var dtoIdValue = dtoId.HasValue ? dtoId.Value.ToString() : string.Empty;
            message += $" Id:{dtoIdValue} != routeId: {routeId}";
         }

         return controller.ValidationFailed(message, new ErrorDetail[] { });
      }

      public static IActionResult RequestFailed(this ControllerBase controller, string message)
      {
         return controller.RequestFailed(message, new ErrorDetail[] { });
      }

      public static IActionResult RequestFailed(this ControllerBase controller, string message, ErrorDetail errorDetail)
      {
         return controller.RequestFailed(message, new ErrorDetail[] { errorDetail });
      }

      public static IActionResult RequestFailed(this ControllerBase controller, params ErrorDetail[] errorDetails)
      {
         return controller.RequestFailed(null, errorDetails);
      }

      /// <summary>
      /// Creates a BadRequestObjectResult that produces a StatusCodes.Status400BadRequest response.
      /// Wraps error message within an ErrorModel
      /// </summary>
      /// <param name="controller">
      /// </param>
      /// <param name="message">
      /// </param>
      /// <param name="errorDetails">
      /// </param>
      /// <returns>
      /// </returns>
      public static IActionResult RequestFailed(this ControllerBase controller, string message, params ErrorDetail[] errorDetails)
      {
         return controller.BadRequest(
            new ErrorModel
            {
               Error = new ErrorInfo(ErrorCode.RequestFailed, message ?? DefaultErrorMessages.ValidationFailedErrorMessage, errorDetails),
            });
      }

      public static IActionResult ValidationFailed(this ControllerBase controller, string message)
      {
         return controller.ValidationFailed(message, new ErrorDetail[] { });
      }

      public static IActionResult ValidationFailed(this ControllerBase controller, string message, ErrorDetail errorDetail)
      {
         return controller.ValidationFailed(message, new ErrorDetail[] { errorDetail });
      }

      public static IActionResult ValidationFailed(this ControllerBase controller, params ErrorDetail[] errorDetails)
      {
         return controller.ValidationFailed(null, errorDetails);
      }

      /// <summary>
      /// Creates a BadRequestObjectResult that produces a StatusCodes.Status400BadRequest response.
      /// Wraps error message within an ErrorModel
      /// </summary>
      /// <param name="controller">
      /// </param>
      /// <param name="message">
      /// </param>
      /// <param name="errorDetails">
      /// </param>
      /// <returns>
      /// </returns>
      public static IActionResult ValidationFailed(this ControllerBase controller, string message, params ErrorDetail[] errorDetails)
      {
         return controller.BadRequest(
            new ErrorModel
            {
               Error = new ErrorInfo(ErrorCode.ValidationFailed, message ?? DefaultErrorMessages.ValidationFailedErrorMessage, errorDetails),
            });
      }
   }
}
