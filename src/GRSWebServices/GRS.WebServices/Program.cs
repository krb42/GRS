using CommandLine;
using CSharpx;
using GRS.Core.Extensions;
using GRS.WebServices.Configuration;
using log4net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace GRS.WebServices
{
   public class Program
   {
      private const int Failure = 1;

      private const int Success = 0;

      // Define a static logger variable so that it references the Logger instance
      private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      private static IWebHostBuilder CreateWebHostBuilder(GRSOptions options)
      {
         return WebHost.CreateDefaultBuilder()
                       .UseStartup<Startup>()
                       .ConfigureAppConfiguration((hostingContext, config) =>
                       {
                          var env = hostingContext.HostingEnvironment;
                          config.AddJsonFile("appsetting.json", optional: true, reloadOnChange: true)
                                .AddJsonFile($"appsetting.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                          config.AddEnvironmentVariables();
                          config.AddInMemoryCollection(options.ToDictionary());
                       })
                       .ConfigureLogging((hostingContext, logging) =>
                       {
                          logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));

                          // The ILoggingBuilder minimum level determines the lowest possible level
                          // for logging. The log4net level then sets the level that we actually log at.
                          logging.AddLog4Net();
                          logging.SetMinimumLevel(LogLevel.Debug);
                       })
                       .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                       .CaptureStartupErrors(true);
      }

      private static int ProcessOptions(GRSOptions options)
      {
         var result = Failure;
         try
         {
            log.Info("Program Main - Creating and Starting WebHost");
            CreateWebHostBuilder(options).Build().Run();
            result = Success;
         }
         catch (Exception ex)
         {
            log.Error("The GRS API web host terminated unexpectedly", ex);
            throw;
         }
         finally
         {
            log.Info("Stopping the GRS API web host");
         }
         return result;
      }

      private static int ReturnFailure(IEnumerable<Error> errs)
      {
         log.Error($"**************************************************{Environment.NewLine}{Environment.NewLine}");
         log.Error($"Failed to parse commandline{Environment.NewLine}{Environment.NewLine}");
         log.Error("Errors:");
         errs.ForEach(error => log.Error($"{error.Tag}"));

         return Failure;
      }

      public static int Main(string[] args)
      {
         // https://dotnetthoughts.net/how-to-use-log4net-with-aspnetcore-for-logging/
         XmlDocument log4netConfig = new XmlDocument();
         log4netConfig.Load(File.OpenRead("log4net.config"));
         var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
         log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

         log.Info("Program Main - Main has been invoked");

         return Parser.Default.ParseArguments<GRSOptions>(args)
            .MapResult(
               ProcessOptions,
               ReturnFailure);
      }
   }
}
