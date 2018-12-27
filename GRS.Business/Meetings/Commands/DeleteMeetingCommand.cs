using MediatR;

namespace GRS.Business.Meetings.Commands
{
   public class DeleteMeetingCommand : IRequest
   {
      public DeleteMeetingCommand(int meetingId)
      {
         MeetingId = meetingId;
      }

      public int MeetingId { get; set; }
   }
}
