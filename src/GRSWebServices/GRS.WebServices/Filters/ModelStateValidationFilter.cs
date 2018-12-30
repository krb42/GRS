using GRS.WebService.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace GRS.WebService.Filters
{
   /// <summary>
   /// Prior to executing the controller action, we look into the ModelState dictionary, which
   /// contains a collection of all errors and if it's not valid we throw the status code 400 (bad
   /// request) back with the ModelState errors attached
   ///
   /// NOTE: FluentValidation places validation erros into the ModelState so our
   /// ActionFilterAttribute will need to check the ModelStat's status.
   /// </summary>
   public class ModelStateValidationFilter : ActionFilterAttribute
   {
      private readonly ILogger<ModelStateValidationFilter> _logger;

      public ModelStateValidationFilter(ILogger<ModelStateValidationFilter> logger)
      {
         _logger = logger;
      }

      public override void OnActionExecuting(ActionExecutingContext context)
      {
         if (!context.ModelState.IsValid)
         {
            var errorModel = context.ModelState.ToErrorModel();
            context.Result = new BadRequestObjectResult(context.ModelState.ToErrorModel());
            _logger.LogInformation($"{errorModel.Error.Message} {errorModel}");
         }

         base.OnActionExecuting(context);
      }
   }
}
