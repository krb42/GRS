using AutoMapper;
using FluentValidation.AspNetCore;
using GRS.Business;
using GRS.Business.Behaviors;
using GRS.Business.Meetings;
using GRS.WebService.Filters;
using GRS.WebServices.Configuration;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GRS.WebServices
{
   public class Startup
   {
      private IConfiguration Configuration { get; }

      private IHostingEnvironment HostingEnvironment { get; }

      private ILogger<Startup> Logger { get; }

      public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILogger<Startup> logger)
      {
         HostingEnvironment = hostingEnvironment;
         Configuration = configuration;
         Logger = logger;
         Logger.LogInformation($"Hosting Environment: {hostingEnvironment.EnvironmentName}");
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
      {
         Logger.LogInformation("Configure ApplicationBuilder");

         loggerFactory.AddLog4Net();

         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();

            // Verify the automapper setup
            Mapper.AssertConfigurationIsValid();
         }
         else
         {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
         }

         app.UseHttpsRedirection();
         app.UseStaticFiles();
         app.UseCookiePolicy();

         Logger.LogDebug("Configure Swagger Documents for GRS");
         app.UseSwaggerDocsForGRS();

         //app.UseMvc(routes =>
         //{
         //   routes.MapRoute(
         //          name: "default",
         //          template: "{controller=Home}/{action=Index}/{id?}");
         //});
         app.UseMvc();

         app.Run(async (context) =>
         {
            await context.Response.WriteAsync("Hello World!");
         });
      }

      // This method gets called by the runtime. Use this method to add services to the container.
      // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
      public void ConfigureServices(IServiceCollection services)
      {
         Logger.LogInformation("Configuring Base Services");

         services.Configure<CookiePolicyOptions>(options =>
            {
               // This lambda determines whether user consent for non-essential cookies is needed
               // for a given request.
               options.CheckConsentNeeded = context => true;
               options.MinimumSameSitePolicy = SameSiteMode.None;
            });

         services.AddMvc(options =>
           {
              options.Filters.Add(typeof(ExceptionLogFilter));
              options.Filters.Add(typeof(ModelStateValidationFilter));
           })
           .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
           .AddFluentValidation(config =>
           {
              config.RegisterValidatorsFromAssemblyContaining(typeof(MediatRAssemblyMarker));
           });

         // Setup GRS Swagger documentation
         Logger.LogDebug("Add Swagger Services for GRS");
         services.AddSwaggerForGRS();

         // Setup AutoMapper object mapper
         Logger.LogDebug("Add AutoMapper Service");
         services.AddAutoMapper();

         // Add all profiles in the GRS.Business assembly
         Mapper.Initialize(cfg => cfg.AddProfiles(typeof(AutoMapperAssemblyMarker).Assembly));

         // Setup MediatR
         services.AddMediatR(typeof(MediatRAssemblyMarker));
         services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

         var grsConfiguration = new GRSServicesConfiguration(Configuration, HostingEnvironment, Logger);
         grsConfiguration.ConfigureGRSServices(services);
      }
   }
}
