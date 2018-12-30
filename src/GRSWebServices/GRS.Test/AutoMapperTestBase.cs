using AutoMapper;
using GRS.Business.Meetings;

namespace GRS.Test
{
   public abstract class AutoMapperTestBase
   {
      private readonly IMapper _mapper;

      protected AutoMapperTestBase()
      {
         var config = new MapperConfiguration(cfg =>
        {
           // Add all profiles in the GRS.Business assembly
           cfg.AddProfiles(typeof(MeetingMappingProfile).Assembly);
        });

         _mapper = config.CreateMapper();
      }

      protected IMapper GetMapper()
      {
         return _mapper;
      }
   }
}
