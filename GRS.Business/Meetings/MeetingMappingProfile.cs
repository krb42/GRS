using AutoMapper;
using GRS.Data.Model;
using GRS.Dto;

namespace GRS.Business.Meetings
{
   public class MeetingMappingProfile : Profile
   {
      public MeetingMappingProfile()
      {
         CreateMap<Meeting, MeetingDto>().ReverseMap();
         ////.ForMember(dest => dest.Deleted, opt => opt.Ignore());
      }
   }
}
