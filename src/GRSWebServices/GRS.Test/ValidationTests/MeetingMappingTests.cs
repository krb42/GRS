using AutoMapper;
using GRS.Data.Model;
using GRS.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRS.Test.ValidationTests
{
   [TestClass]
   public class MeetingMappingTests
   {
      private TestDBContext _dbContext;
      private IMapper _mapper;

      [TestMethod]
      public void CheckAutoMap_Dto_to_Model()
      {
         var expect = _dbContext.Meeting.GetMeetingByID(1);
         var meetingDto = _dbContext.Mapper.Map<MeetingDto>(expect);

         var source = new[] { meetingDto };

         var destination = (Meeting[])_mapper.Map(source, typeof(MeetingDto[]), typeof(Meeting[]));

         Assert.AreEqual(1, destination.Length);
         Assert.AreEqual(expect.MeetingID, destination[0].MeetingID);
         Assert.AreEqual(expect.Name, destination[0].Name);
      }

      [TestMethod]
      public void CheckAutoMap_Model_to_Dto()
      {
         var expect = _dbContext.Meeting.GetMeetingByID(1);
         var meetingDto = _dbContext.Mapper.Map<MeetingDto>(expect);

         var source = new[] { expect };

         var destination = (MeetingDto[])_mapper.Map(source, typeof(Meeting[]), typeof(MeetingDto[]));

         Assert.AreEqual(1, destination.Length);
         Assert.AreEqual(expect.MeetingID, destination[0].MeetingID);
         Assert.AreEqual(expect.Name, destination[0].Name);
      }

      [TestInitialize]
      public void Setup()
      {
         _dbContext = new TestDBContext(new TestGRSDBContextOptions());

         var config = new MapperConfiguration(cfg => cfg.AddProfiles(typeof(Meeting).Assembly));
         _mapper = config.CreateMapper();
      }
   }
}
