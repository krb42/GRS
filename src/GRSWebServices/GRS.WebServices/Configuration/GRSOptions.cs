﻿using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

namespace GRS.WebServices.Configuration
{
   internal class GRSOptions
   {
      private string AccessPassword => "grsuser";

      private string AccessUsername => "grsuser";

      [Usage(ApplicationAlias = "grs")]
      public static IEnumerable<Example> Examples => new List<Example>() { new Example("GRS Webservices", new GRSOptions()) };

      [Option('c', "catalog", Default = "GRSData_Dev", HelpText = "Catalog/Databasew name")]
      public string Catalog { get; set; }

      [Option('d', "datasource", Default = "localhost", HelpText = "Datasource name")]
      public string DataSource { get; set; }

      [Option('p', "password", Default = "testuser", HelpText = "User Password")]
      public string Password { get; set; }

      [Option('t', "trustedconnection", Default = false, HelpText = "Use the current user to connect to the database")]
      public bool TrustedConnection { get; set; }

      [Option('u', "username", Default = "testuser", HelpText = "Username to connect to the database")]
      public string Username { get; set; }

      public IReadOnlyDictionary<string, string> ToDictionary()
      {
         return new Dictionary<string, string>
         {
            ["DBContext:DataSource"] = DataSource,
            ["DBContext:Catalog"] = Catalog,
            ["DBContext:TrustedConnection"] = TrustedConnection.ToString(),
            ["DBContext:UserName"] = AccessUsername,
            ["DBContext:Password"] = AccessPassword,
            ["DBContext:LoggedInUserName"] = Username,
            ["DBContext:LoggedInPassword"] = Password,
         };
      }
   }
}
