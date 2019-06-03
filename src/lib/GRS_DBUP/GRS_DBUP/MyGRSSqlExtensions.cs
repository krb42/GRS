using DbUp.Builder;
using DbUp.Engine.Transactions;
using DbUp.SqlServer;
using System;

namespace GRS_DBUP
{
    internal static class MyGRSSqlExtensions
    {
        internal static string GetPartValue(string contentsToSearch, string beginPart, string endPart)
        {
            if (string.IsNullOrEmpty(contentsToSearch) || string.IsNullOrEmpty(beginPart) || string.IsNullOrEmpty(endPart))
            {
                // nothing to search or to search for
                return string.Empty;
            }

            var startPos = contentsToSearch.IndexOf(beginPart, StringComparison.OrdinalIgnoreCase);
            var endPos = contentsToSearch.IndexOf(endPart, StringComparison.OrdinalIgnoreCase);
            if (startPos < 0 || endPos < 0)
            {
                // begin part or endpart not found, invalid so return nothing
                return string.Empty;
            }

            var scriptPart = contentsToSearch.Substring(startPos + beginPart.Length, endPos - startPos - beginPart.Length);
            return scriptPart.Trim();
        }

        /// <summary>
        /// Creates an upgrader for GRS SQL Server databases.
        /// </summary>
        /// <param name="supported">
        /// Fluent helper type.
        /// </param>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <returns>
        /// A builder for a database upgrader designed for SQL Server databases.
        /// </returns>
        public static UpgradeEngineBuilder MyGRSSqlDatabase(this SupportedDatabases supported, string connectionString)
        {
            IConnectionManager connectionManager = new SqlConnectionManager(connectionString);
            string schema = "dbo";

            var builder = new UpgradeEngineBuilder();
            builder.Configure(c => c.ConnectionManager = connectionManager);
            builder.Configure(c => c.ScriptExecutor = new SqlScriptExecutor(() => c.ConnectionManager, () => c.Log, schema, () => c.VariablesEnabled, c.ScriptPreprocessors, () => c.Journal));
            builder.Configure(c => c.Journal = new MyGRSSqlTableJournal(() => c.ConnectionManager, () => c.Log, schema, "GRSSchemaVersions"));
            return builder;
        }
    }
}
