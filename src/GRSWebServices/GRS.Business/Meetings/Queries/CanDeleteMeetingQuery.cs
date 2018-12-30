using MediatR;

namespace GRS.Business.Meetings.Queries
{
   public class CanDeleteMeetingQuery : IRequest<bool>
   {
      public CanDeleteMeetingQuery(int meetingId)
      {
         MeetingId = meetingId;
      }

      public int MeetingId { get; set; }
   }
}
