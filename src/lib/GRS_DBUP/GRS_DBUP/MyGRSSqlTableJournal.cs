using DbUp.Engine;
using DbUp.Engine.Output;
using DbUp.Engine.Transactions;
using DbUp.SqlServer;
using DbUp.Support;
using System;
using System.Data;
using System.Text;

namespace GRS_DBUP
{
    internal class MyGRSSqlTableJournal : TableJournal
    {
        protected override string CreateSchemaTableSql(string quotedPrimaryKeyName)
        {
            var createTableScript = new StringBuilder();
            createTableScript.AppendFormat($"create table {FqSchemaTableName} (").AppendLine();
            createTableScript.AppendFormat($"[ID] int identity(1,1) not null constraint {quotedPrimaryKeyName} primary key,").AppendLine();
            createTableScript.Append("[ScriptName] nvarchar(255) not null,").AppendLine();
            createTableScript.Append("[ScriptVersion] nvarchar(20) not null,").AppendLine();
            createTableScript.Append("[ScriptDescription] nvarchar(2048) not null,").AppendLine();
            createTableScript.Append("[DateApplied] datetime not null").AppendLine();
            createTableScript.Append(")");

            return createTableScript.ToString();
        }

        protected override string GetInsertJournalEntrySql(string scriptName, string applied)
        {
            var insertScript = new StringBuilder();
            insertScript.Append($"insert into {FqSchemaTableName} ");
            insertScript.Append("([ScriptName], [ScriptVersion], [ScriptDescription], [DateApplied]) ");
            insertScript.Append($"values ({@scriptName}, '<scriptVersion>', '<scriptDescription>', {@applied})");
            return insertScript.ToString();
        }

        protected new IDbCommand GetInsertScriptCommand(Func<IDbCommand> dbCommandFactory, SqlScript script)
        {
            string scriptVersion = MyGRSSqlExtensions.GetPartValue(script.Contents, "<version>", "</version>");
            string scriptDescription = MyGRSSqlExtensions.GetPartValue(script.Contents, "<description>", "</description>");

            var command = dbCommandFactory();

            var scriptNameParam = command.CreateParameter();
            scriptNameParam.ParameterName = "scriptName";
            scriptNameParam.DbType = DbType.String;
            scriptNameParam.Size = 255;
            scriptNameParam.Value = script.Name;
            command.Parameters.Add(scriptNameParam);

            var scriptVersionParam = command.CreateParameter();
            scriptVersionParam.ParameterName = "scriptVersion";
            scriptVersionParam.DbType = DbType.String;
            scriptVersionParam.Size = 20;
            scriptVersionParam.Value = scriptVersion;
            command.Parameters.Add(scriptVersionParam);

            var scriptDescriptionParam = command.CreateParameter();
            scriptDescriptionParam.ParameterName = "scriptDescription";
            scriptVersionParam.DbType = DbType.String;
            scriptVersionParam.Size = 2048;
            scriptDescriptionParam.Value = scriptDescription;
            command.Parameters.Add(scriptDescriptionParam);

            var appliedParam = command.CreateParameter();
            appliedParam.ParameterName = "applied";
            appliedParam.Value = DateTime.Now;
            command.Parameters.Add(appliedParam);

            var commandText = GetInsertJournalEntrySql("@sciptName", "@applied");
            commandText = commandText.Replace("<scriptVersion>", "@scriptVersion");
            commandText = commandText.Replace("<scriptDescription>", "@scriptDescription");

            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
            return command;
        }

        protected override string GetJournalEntriesSql()
        {
            return $"select [ScriptName] from {FqSchemaTableName} order by [ScriptName]";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyGRSSqlTableJournal" /> class.
        /// </summary>
        /// <param name="connectionManager">
        /// The connection Manager.
        /// </param>
        /// <param name="logger">
        /// The log.
        /// </param>
        /// <param name="schema">
        /// The schema that contains the table.
        /// </param>
        /// <param name="table">
        /// The table name.
        /// </param>
        /// <example>
        /// var journal = new TableJournal("Server=server;Database=database;Trusted_Connection=True",
        /// "dbo", "MyVersionTable");
        /// </example>
        public MyGRSSqlTableJournal(Func<IConnectionManager> connectionManager, Func<IUpgradeLog> logger, string schema, string table)
            : base(connectionManager, logger, new SqlServerObjectParser(), schema, table)
        {
        }
    }
}
