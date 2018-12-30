using GRS.Dto;
using MediatR;

namespace GRS.Business.Meetings.Commands
{
   public class UpdateMeetingCommand : IRequest
   {
      public UpdateMeetingCommand(MeetingDto meetingDto)
      {
         MeetingDto = meetingDto;
      }

      public MeetingDto MeetingDto { get; set; }
   }
}
