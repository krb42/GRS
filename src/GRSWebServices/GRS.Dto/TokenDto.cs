using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GRS.Dto
{
   public class TokenDto
   {
      [JsonProperty(PropertyName = "access_token")]
      public string AccessToken { get; set; }

      public DateTime ExpiresAt { get; set; }

      [JsonProperty(PropertyName = "expires_in")]
      public int ExpiresIn { get; set; }

      public bool IsValidAndNotExpiring
      {
         get
         {
            return !string.IsNullOrEmpty(AccessToken) && ExpiresAt > DateTime.UtcNow.AddSeconds(30);
         }
      }

      public string Scope { get; set; }

      [JsonProperty(PropertyName = "token_type")]
      public string TokenType { get; set; }
   }
}
