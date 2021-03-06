﻿using CommandLine;
using DbUp;
using DbUp.Helpers;
using GRS_DBUP.Configuration;
using GRS_DBUP.Extensions;
using GRS_DBUP.Reports;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace GRS_DBUP
{
    internal class Program
    {
        private const int Failure = 1;

        private const int Success = 0;

        private const int SuccessReport = 2;

        // Define a static logger variable so that it references the Logger instance
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [STAThread]
        private static int Main(string[] args)
        {
            // https://dotnetthoughts.net/how-to-use-log4net-with-aspnetcore-for-logging/
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));
            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

            log.Info("Program Main - Main has been invoked");

            var result = Parser.Default.ParseArguments<DBUPOptions>(args)
               .MapResult(
                  ProcessOptions,
                  ReturnFailure);

            if (result != SuccessReport)
            {
                Console.Write("$> ");
                Console.ReadLine();
            }

            return result;
        }

        private static int ProcessOptions(DBUPOptions options)
        {
            var result = Failure;
            try
            {
                log.Info("Program Main - Creating and Starting DBUP");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{options.ConnectionString}");
                Console.ResetColor();

                var upgradeEngineBuilder = DeployChanges.To
                                    //.SqlDatabase(options.ConnectionString)
                                    .MyGRSSqlDatabase(options.ConnectionString)
                                    .WithVariable("DatabaseName", options.Catalog)
                                    .WithPreprocessor(new MyGRSScriptValidator())
                                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                                    .LogToConsole();

                if (options.WithTransaction) upgradeEngineBuilder = upgradeEngineBuilder.WithTransaction();
                if (options.WithTransactionPerScript) upgradeEngineBuilder = upgradeEngineBuilder.WithTransactionPerScript();

                var upgrader = upgradeEngineBuilder.Build();

                if (options.GenerateReport)
                {
                    var upgradeFileName = "UpgradeReport.html";
                    upgrader.GenerateUpgradeHtmlReport(upgradeFileName);

                    var allFiles = Directory.GetFiles(".", upgradeFileName, SearchOption.AllDirectories);

                    if (allFiles.Length > 0)
                    {
                        var file = allFiles[0];
                        var filePath = Path.GetFullPath(file);

                        var frm = new frmHTMLReport(filePath);
                        frm.ShowDialog();
                    }

                    result = SuccessReport;
                }
                else
                {
                    var upgradeResult = upgrader.PerformUpgrade();

                    if (upgradeResult.Successful)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Success!");
                        Console.ResetColor();
                        result = Success;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(upgradeResult.Error);
                        Console.ResetColor();
                        result = Failure;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("DBUP Failed", ex);
                throw;
            }
            finally
            {
                log.Info("DBUP Completed");
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
    }
}
