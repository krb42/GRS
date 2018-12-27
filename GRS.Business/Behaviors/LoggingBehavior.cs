using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace GRS.Business.Behaviors
{
   public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
   {
      private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

      public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
      {
         _logger = logger;
      }

      public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
      {
         _logger.LogInformation("Handling the {@Request}", request);

         var response = await next();

         _logger.LogInformation("Handled the {@Request} with {@Response}", typeof(TRequest).Name, typeof(TResponse).Name);
         _logger.LogTrace("Handled the {@Request} with {@Response}", request, response);

         return response;
      }
   }
}
