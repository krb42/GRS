using GRS.Core;
using GRS.WebService.ActionResult;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace GRS.WebService.Filters
{
   public class ExceptionLogFilter : ExceptionFilterAttribute
   {
      private readonly IHostingEnvironment _env;

      public ExceptionLogFilter(IHostingEnvironment env)
      {
         _env = env;
      }

      public override void OnException(ExceptionContext context)
      {
         if (context == null)
            throw new ArgumentNullException(nameof(context));

         if (context.Exception.GetType() == typeof(GRSException))
         {
            var error = new ErrorModel
            {
               Error = new ErrorInfo(ErrorCode.ValidationFailed, context.Exception.Message)
               {
                  InnerError = new InnerErrorInfo(context.Exception),
               },
            };

            context.Result = new BadRequestObjectResult(error);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
         }
         else
         {
            InnerErrorInfo innerError = null;

            if (!_env.IsProduction())
            {
               innerError = new InnerErrorInfo(context.Exception);
            }

            var error = new ErrorModel
            {
               Error = new ErrorInfo(ErrorCode.UnhandledException, DefaultErrorMessages.InternalServerErrorMessage)
               {
                  InnerError = innerError,
               },
            };

            context.Result = new InternalServerErrorObjectResult(error);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
         }

         context.ExceptionHandled = true;
      }
   }
}
