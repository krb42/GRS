using AutoMapper;
using GRS.Data.Model;
using GRS.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GRS.Business.Tokens
{
   public class GetTokenObjectQueryHandler : IRequestHandler<GetTokenObjectQuery, TokenDto>
   {
      private readonly IGRSDBContext _dbContext;
      private readonly IMapper _mapper;
      private TokenDto _token;

      private TokenDto GetNewAccessToken()
      {
         var token = new TokenDto
         {
            ExpiresIn = 30,
            AccessToken = "xyzzy_token",
            Scope = "application",
            TokenType = "this app",
         };
         token.ExpiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn);

         return token;
      }

      public GetTokenObjectQueryHandler(IGRSDBContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
         _token = new TokenDto()
         {
            AccessToken = "Old Token",
         };
      }

      public Task<TokenDto> Handle(GetTokenObjectQuery request, CancellationToken cancellationToken)
      {
         if (!_token.IsValidAndNotExpiring)
         {
            // Get New Access Token
            _token = GetNewAccessToken();
         }

         return Task.FromResult(_token);
      }
   }
}
