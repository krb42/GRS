using GRS.Dto;
using MediatR;

namespace GRS.Business.Tokens
{
   public class GetTokenQuery : IRequest<string>
   {
      public GetTokenQuery()
      {
      }
   }
}
