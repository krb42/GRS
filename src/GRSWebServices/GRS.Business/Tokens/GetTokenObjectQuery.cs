using GRS.Dto;
using MediatR;

namespace GRS.Business.Tokens
{
   public class GetTokenObjectQuery : IRequest<TokenDto>
   {
      public GetTokenObjectQuery()
      {
      }
   }
}
