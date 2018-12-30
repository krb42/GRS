using AutoMapper;
using GRS.Business.Meetings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRS.Test
{
   [TestClass]
   public class AutoMapperConfigTests
   {
      [TestMethod]
      public void MapperShouldHaveValidConfiguration()
      {
         var config = new MapperConfiguration(cfg =>
        {
           // Add all profiles in the GRS.Business assembly
           cfg.AddProfiles(typeof(MeetingMappingProfile).Assembly);
        });

         config.AssertConfigurationIsValid();
      }
   }
}
