using DbUp.Engine;
using System;
using System.Text;

namespace GRS_DBUP
{
    /// <summary>
    /// Substitutes variables for values in SqlScripts
    /// </summary>
    internal class MyGRSScriptValidator : IScriptPreprocessor
    {
        /// <summary>
        /// Check the contents for the required Version number and Description tags
        /// </summary>
        /// <param name="contents">
        /// </param>
        public string Process(string contents)
        {
            StringBuilder errors = new StringBuilder();

            string scriptVersion = MyGRSSqlExtensions.GetPartValue(contents, "<version>", "</version>");
            if (string.IsNullOrEmpty(scriptVersion))
            {
                errors.Append("Script Version is required. Please add <version>n.n.n</version> information").AppendLine(); ;
            }

            string scriptDescription = MyGRSSqlExtensions.GetPartValue(contents, "<description>", "</description>");
            if (string.IsNullOrEmpty(scriptDescription))
            {
                errors.Append("Script Description is required. Please add <description>script description</description> information").AppendLine();
            }

            if (errors.Length > 0)
            {
                throw new ArgumentException(errors.ToString());
            }

            return contents;
        }
    }
}
