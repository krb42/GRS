using GRS.Dto;
using GRS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GRS.WebServices.Controllers
{
   [Route("api/v1/[controller]")]
   public class TokenController : Controller
   {
      private readonly ILogger<TokenController> _logger;

      private readonly ITokenService _tokenService;

      public TokenController(ITokenService tokenService, ILogger<TokenController> logger)
      {
         _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
         _logger = logger;
      }

      /// <summary>
      /// Retrieve an access token
      /// </summary>
      /// <returns>
      /// An Access token
      /// </returns>
      [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
      [HttpGet]
      public async Task<IActionResult> GetToken()
      {
         return Ok(await _tokenService.GetToken());
      }

      /// <summary>
      /// Retrieve an access token
      /// </summary>
      /// <returns>
      /// An Access token
      /// </returns>
      [ProducesResponseType(typeof(TokenDto), (int)HttpStatusCode.OK)]
      [HttpGet("obj", Name = nameof(GetTokenObject))]
      public async Task<IActionResult> GetTokenObject()
      {
         return Ok(await _tokenService.GetTokenObject());
      }
   }
}
