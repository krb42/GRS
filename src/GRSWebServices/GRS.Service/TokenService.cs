using GRS.Business.Tokens;
using GRS.Dto;
using MediatR;
using System;
using System.Threading.Tasks;

namespace GRS.Service
{
   public interface ITokenService
   {
      Task<string> GetToken();

      Task<TokenDto> GetTokenObject();
   }

   public class TokenService : ITokenService
   {
      private readonly IMediator _mediator;

      public TokenService(IMediator mediator)
      {
         _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
      }

      public async Task<string> GetToken()
      {
         return await _mediator.Send(new GetTokenQuery());
      }

      public async Task<TokenDto> GetTokenObject()
      {
         return await _mediator.Send(new GetTokenObjectQuery());
      }
   }
}
