using GRS.Business.Meetings.Commands;
using GRS.Business.Meetings.Queries;
using GRS.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GRS.Service
{
   public interface IMeetingService
   {
      Task<bool> CanDeleteMeeting(int meetingId);

      Task<MeetingDto> CreateMeeting(MeetingDto meetingDto);

      Task DeleteMeeting(int meetingId);

      Task<MeetingDto> GetMeetingById(int meetingId);

      Task<List<MeetingDto>> GetMeetings(QueryParameters parameters);

      Task UpdateMeeting(MeetingDto meetingDto);
   }

   public class MeetingService : IMeetingService
   {
      private readonly IMediator _mediator;

      public MeetingService(IMediator mediator)
      {
         _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
      }

      public async Task<bool> CanDeleteMeeting(int meetingId)
      {
         return await _mediator.Send(new CanDeleteMeetingQuery(meetingId));
      }

      public async Task<MeetingDto> CreateMeeting(MeetingDto meetingDto)
      {
         var meetingId = await _mediator.Send(new CreateMeetingCommand(meetingDto));
         return await GetMeetingById(meetingId);
      }

      public async Task DeleteMeeting(int meetingId)
      {
         await _mediator.Send(new DeleteMeetingCommand(meetingId));
      }

      public async Task<MeetingDto> GetMeetingById(int meetingId)
      {
         var request = new GetMeetingByIdQuery
         {
            MeetingId = meetingId
         };
         return await _mediator.Send(request);
      }

      public async Task<List<MeetingDto>> GetMeetings(QueryParameters parameters)
      {
         return await _mediator.Send(new GetMeetingsQuery(parameters));
      }

      public async Task UpdateMeeting(MeetingDto meetingDto)
      {
         await _mediator.Send(new UpdateMeetingCommand(meetingDto));
      }
   }
}
