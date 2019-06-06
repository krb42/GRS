using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace GRS_DBUP.Configuration
{
    internal class DBUPOptions
    {
        private const string cstrConfigurationBase = "GRSData_BASE";

        [Usage(ApplicationAlias = "grsdbup")]
        public static IEnumerable<Example> Examples => new List<Example>() { new Example("GRS_DBUP", new DBUPOptions()) };

        [Option('c', "catalog", Default = "GRSData_Dev", HelpText = "Catalog/Databasew name")]
        public string Catalog { get; set; }

        public string ConnectionString
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings[cstrConfigurationBase].ToString();

                connectionString = connectionString.Replace("=DBSource", $"={DataSource}");
                connectionString = connectionString.Replace("=DBCatalog", $"={Catalog}");

                if (connectionString.IndexOf("=DBUserName", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    connectionString = connectionString.Replace("=DBUsername", $"={Username}");
                    connectionString = connectionString.Replace("=DBPassword", $"={Password}");
                }

                return connectionString;
            }
        }

        [Option('d', "datasource", Default = "localhost", HelpText = "Datasource name")]
        public string DataSource { get; set; }

        [Option('r', "GenerateReport", Default = false, HelpText = "Generate Report - no update")]
        public bool GenerateReport { get; set; }

        [Option('p', "password", Default = "grsuser", HelpText = "User Password")]
        public string Password { get; set; }

        [Option('t', "trustedconnection", Default = false, HelpText = "Use the current user to connect to the database")]
        public bool TrustedConnection { get; set; }

        [Option('u', "username", Default = "grsuser", HelpText = "Username to connect to the database")]
        public string Username { get; set; }

        [Option("WithTransaction", Default = false, HelpText = "Process scripts WithTransaction")]
        public bool WithTransaction { get; set; }

        [Option("WithTransactionPerScript", Default = false, HelpText = "Process scripts WithTransactionPerScript")]
        public bool WithTransactionPerScript { get; set; }
    }
}
