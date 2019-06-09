using GRS.Dto;
using MediatR;

namespace GRS.Business.Meetings.Commands
{
   public class CreateMeetingCommand : IRequest<MeetingDto>
   {
      public CreateMeetingCommand(MeetingDto meetingDto)
      {
         MeetingDto = meetingDto;
      }

      public MeetingDto MeetingDto { get; set; }
   }
}
