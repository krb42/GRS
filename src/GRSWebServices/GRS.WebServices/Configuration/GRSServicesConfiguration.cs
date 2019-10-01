using GRS.Data.Model;
using GRS.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GRS.WebServices.Configuration
{
   public class GRSServicesConfiguration
   {
      private IConfiguration Configuration { get; }

      private IHostingEnvironment HostingEnvironment { get; }

      private ILogger<Startup> Logger { get; }

      private void AddGRSDbContext(IServiceCollection services)
      {
         Logger.LogInformation("Configuring GRS Services");

         // create DB Options and DB Context
         var dbOptions = new GRSDBContextOptions
         {
            DataSource = Configuration.GetValue<string>("DBContext:DataSource"),
            Catalog = Configuration.GetValue<string>("DBContext:Catalog"),
            TrustedConnection = Configuration.GetValue<bool>("DBContext:TrustedConnection"),
            Username = Configuration.GetValue<string>("DBContext:LoggedInUserName"),
            Password = Configuration.GetValue<string>("DBContext:LoggedInPassword"),
            AccessUsername = Configuration.GetValue<string>("DBContext:UserName"),
            AccessPassword = Configuration.GetValue<string>("DBContext:Password"),
         };
         services.AddSingleton<IGRSDBContext>(new GRSDBContext(dbOptions));

         if (HostingEnvironment.IsDevelopment())
         {
            Logger.LogInformation($"Connection string = '{dbOptions.ConnectionString}'");
         }

         //GRSDbContextProviderOptions options;

         //services.AddSingleton<IGRSDbContext>(new GRSDbContextProvider(options));

         //services.Configure<DbOptions>

         //DBContextOptionsBuilder
         ////services.Configure<GRSDbContext>(options =>
         ////{
         ////   options.SqlServer();
         ////};
      }

      public GRSServicesConfiguration(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILogger<Startup> logger)
      {
         HostingEnvironment = hostingEnvironment;
         Configuration = configuration;
         Logger = logger;
      }

      /// <summary>
      /// Configure the GRS services
      /// </summary>
      /// <param name="services">
      /// The Service Collection the GRS services are to be added to
      /// </param>
      public void ConfigureGRSServices(IServiceCollection services)
      {
         Logger.LogInformation("Configuring GRS Services");

         // create DB Options and DB Context
         AddGRSDbContext(services);

         // setup application services
         services.AddTransient<ITokenService, TokenService>();
         //services.AddTransient<IEncryptionService, EncryptionService>();
         services.AddTransient<IMeetingService, MeetingService>();
      }
   }
}
