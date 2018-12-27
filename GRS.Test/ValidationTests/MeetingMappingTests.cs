using AutoMapper;
using GRS.Data.Model;
using GRS.Dto;
using GRS.Test.DBContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRS.Test.ValidationTests
{
   [TestClass]
   public class MeetingMappingTests
   {
      private IGRSDBContext _dbContext;
      private IMapper _mapper;
      private ITestMeetingRepository _repository;

      [TestMethod]
      public void CheckAutoMap_Dto_to_Model()
      {
         var source = new[] { _repository.GetTestMeetingDto(1) };
         var expect = _repository.GetTestMeeting(1);

         var destination = (Meeting[])_mapper.Map(source, typeof(MeetingDto[]), typeof(Meeting[]));

         Assert.AreEqual(1, destination.Length);
         Assert.AreEqual(expect.MeetingID, destination[0].MeetingID);
         Assert.AreEqual(expect.Name, destination[0].Name);
      }

      [TestMethod]
      public void CheckAutoMap_Model_to_Dto()
      {
         var source = new[] { _repository.GetTestMeeting(1) };
         var expect = _repository.GetTestMeetingDto(1);

         var destination = (MeetingDto[])_mapper.Map(source, typeof(Meeting[]), typeof(MeetingDto[]));

         Assert.AreEqual(1, destination.Length);
         Assert.AreEqual(expect.MeetingID, destination[0].MeetingID);
         Assert.AreEqual(expect.Name, destination[0].Name);
      }

      [TestInitialize]
      public void Setup()
      {
         var config = new MapperConfiguration(cfg => cfg.AddProfiles(typeof(Meeting).Assembly));
         _mapper = config.CreateMapper();

         _dbContext = new TestDBContext();
         _repository = (ITestMeetingRepository)_dbContext.Meeting;
      }
   }
}
