namespace GRS.Data.Model
{
   public class GRSDBContextOptions
   {
      public string Catalog { get; set; }

      public string ConnectionString
      {
         get
         {
            var authorisation = "Trusted_connection=true";
            if (!TrustedConnection)
               authorisation = $"User ID={Username};Password={Password}";

            var connectionString = $"Data Source={DataSource};Initial Catalog={Catalog};{authorisation};Connection Timeout=20";

            return connectionString;
         }
      }

      public string DataSource { get; set; }

      public string Password { get; set; }

      public bool TrustedConnection { get; set; }

      public string Username { get; set; }
   }
}
