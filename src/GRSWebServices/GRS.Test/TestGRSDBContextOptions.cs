using GRS.Data.Model;

namespace GRS.Test
{
   public class TestGRSDBContextOptions : GRSDBContextOptions
   {
      public TestGRSDBContextOptions()
      {
         AccessPassword = "xxx";
         AccessUsername = "Test Access User";
         Catalog = "Test Catalog";
         DataSource = "Test Data Source";
         Password = "Test Password";
         TrustedConnection = false;
         Username = "Test User";
      }
   }
}
