using GRS.Dto;
using MediatR;

namespace GRS.Business.Meetings.Queries
{
   public class GetMeetingByIdQuery : IRequest<MeetingDto>
   {
      public int MeetingId { get; set; }
   }
}
