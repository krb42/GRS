using AutoMapper;
using GRS.Data.Model;
using GRS.Data.Model.Repositories;
using GRS.Dto;
using GRS.Test.Data;
using GRS.Test.Repositories;

namespace GRS.Test
{
   public class TestDBContext : TestBaseRepository, IGRSDBContext
   {
      public TestDBContext(GRSDBContextOptions options) : base(options)
      {
         var config = new MapperConfiguration(cfg =>
         {
            cfg.CreateMap<Meeting, MeetingDto>();
         });

         Mapper = config.CreateMapper();
      }

      public IMapper Mapper { get; set; }

      public IMeetingRepository Meeting => new TestMeetingRepository(this, MeetingTestData.Meetings);
   }
}
